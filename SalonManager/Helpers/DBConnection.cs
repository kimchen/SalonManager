using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Reflection;
using SalonManager.Models;

namespace SalonManager.Helpers
{
    class DBConnection
    {
        private static DBConnection mInstance = null;
        private static int COMMAND_TIME_OUT = 30;

        private string path = "sqlDB.db";
        private string password = "1234";
        private SQLiteConnection connection = null;

        private Dictionary<string, Type> tableMap = new Dictionary<string, Type>() { { "customerTable", typeof(Customer) }, { "employeeTable", typeof(Employee) } };

        public static DBConnection ins()
        {
            if (mInstance == null)
            {
                mInstance = new DBConnection();
            }
            return mInstance;
        }
        private DBConnection(){
            initConnection();
            initTables();
        }
        private void initConnection(){
            if (File.Exists(path))
            {
                connection = new SQLiteConnection("Data Source = " + path);
                connection.SetPassword(password);
                connection.Open();
            }
            else
            {
                SQLiteConnection.CreateFile(path);
                connection = new SQLiteConnection("Data Source = " + path);
                connection.Open();
                connection.ChangePassword(password);
            }
        }
        private string typeToTableName(Type type)
        {
            foreach (KeyValuePair<string, Type> pair in tableMap)
            {
                if (pair.Value == type)
                    return pair.Key;
            }
            return "";
        }
        
        private void initTables()
        {
            foreach(KeyValuePair<string,Type> pair in tableMap)
            {
                initTable(pair.Key, pair.Value);
            }
        }
        private void initTable(string tableName, Type type)
        {
            if (connection == null)
                return;
            if(connection.GetSchema("Tables").Select("TABLE_NAME = '" + tableName + "'").Length <= 0){
                string createTxt = "create table " + tableName + "( dbid integer primary key ";
                FieldInfo[] infos = type.GetFields();
                foreach (FieldInfo info in infos)
                {
                    if (info.Name == "dbid")
                        continue;
                    if (info.FieldType == typeof(int))
                    {
                        createTxt += "," + info.Name + " INT";
                    }
                    else if (info.FieldType == typeof(string))
                    {
                        createTxt += "," + info.Name + " VARCHAR(256)";
                    }
                    else if (info.FieldType == typeof(float))
                    {
                        createTxt += "," + info.Name + " FLOAT";
                    }
                    else if (info.FieldType == typeof(double))
                    {
                        createTxt += "," + info.Name + " DOUBLE";
                    }
                }
                createTxt += ");";
                ExecuteNoQuery(createTxt, null);
            }
        }
        private SQLiteCommand MakeCommand(string cmd, params object[] p)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.Parameters.Clear();
            command.CommandText = cmd;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = COMMAND_TIME_OUT;
            if (p != null)
            {
                foreach (object obj in p)
                {
                    command.Parameters.AddWithValue(string.Empty, obj);
                }
            }
            return command;
        }
        private DataSet ExecuteQuery(string cmd, params object[] p)
        {
            SQLiteCommand command = MakeCommand(cmd,p);
            DataSet ds = new DataSet();
            SQLiteDataAdapter da = new SQLiteDataAdapter(command);
            da.Fill(ds);
            return ds;
        }
        private int ExecuteNoQuery(string cmd, params object[] p)
        {
            SQLiteCommand command = MakeCommand(cmd, p);
            return command.ExecuteNonQuery();
        }

        public void closeDb()
        {
            connection.Close();
        }
        public List<T> queryData<T>()
        {
            List<T> list = new List<T>();
            if (connection == null)
                return list;
            Type type = typeof(T);
            string tableName = typeToTableName(type);
            if (tableName == "")
                return list;
            FieldInfo[] infos = type.GetFields();
            string queryString = "select * from " + tableName + ";";
            DataSet dataSet = ExecuteQuery(queryString);
            DataTable table = dataSet.Tables[0];
            int dataCount = table.Rows.Count;
            for (int i = 0; i < dataCount; i++)
            {
                ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
                T data = (T)ci.Invoke(null);
                foreach (FieldInfo info in infos)
                {
                    
                    object fieldData = table.Rows[i][info.Name];
                    if (fieldData != null)
                    {
                        if (info.Name == "dbid")
                        {
                            int id = int.Parse(fieldData.ToString());
                            info.SetValue(data, id);
                        }
                        else
                        {
                            info.SetValue(data, fieldData);
                        }
                        
                    }
                }
                list.Add(data);
            }
            return list;
        }
        public int addData<T>(object obj)
        {
            if (connection == null)
                return -1;
            Type type = typeof(T);
            string tableName = typeToTableName(type);
            if (tableName == "")
                return -1;
            Dictionary<string, string> fieldData = new Dictionary<string, string>();
            FieldInfo[] infos = type.GetFields();
            foreach (FieldInfo info in infos)
            {
                if (info.Name == "dbid")
                    continue;
                string key = info.Name;
                string value = info.GetValue(obj).ToString();
                fieldData.Add(key, value);
            }
            string keyString = "";
            string valueString = "";
            foreach (KeyValuePair<string, string> pair in fieldData)
            {
                keyString += pair.Key + ",";
                valueString += "'" + pair.Value + "',";
            }
            if (keyString.EndsWith(","))
            {
                keyString = keyString.Remove(keyString.Length - 1);
            }
            if (valueString.EndsWith(","))
            {
                valueString = valueString.Remove(valueString.Length - 1);
            }
            string addString = "insert into " + tableName + "(" + keyString + ") values(" + valueString + ");";
            return ExecuteNoQuery(addString);
        }
        public int getDataId<T>(object obj)
        {
            if (connection == null)
                return -1;
            Type type = typeof(T);
            string tableName = typeToTableName(type);
            if (tableName == "")
                return -1;
            FieldInfo[] infos = type.GetFields();
            FieldInfo info = null;
            int index = 0;
            while (info == null || info.Name == "dbid")
            {
                info = infos[index++];
            }
            string idString = "select dbid from " + tableName + " where " + info.Name + "='" + info.GetValue(obj) + "';";
            DataSet dataSet = ExecuteQuery(idString);
            DataTable table = dataSet.Tables[0];
            string ss = table.Rows[0]["dbid"].ToString();
            return int.Parse(table.Rows[0]["dbid"].ToString());
        }
        public int deleteData<T>(object obj)
        {
            if (connection == null)
                return -1;
            Type type = typeof(T);
            string tableName = typeToTableName(type);
            if (tableName == "")
                return -1;
            FieldInfo info = type.GetField("dbid");
            string deleteString = "delete from " + tableName + " where " + info.Name + "='" + info.GetValue(obj) + "';";
            return ExecuteNoQuery(deleteString);
        }
        public int updateData<T>(object obj)
        {
            if (connection == null)
                return -1;
            Type type = typeof(T);
            string tableName = typeToTableName(type);
            if (tableName == "")
                return -1;

            int id = -1;
            string tempString = "";
            FieldInfo[] infos = type.GetFields();
            foreach (FieldInfo info in infos)
            {
                if (info.Name == "dbid")
                {
                    id = (int)info.GetValue(obj);
                    continue;
                }
                tempString += info.Name + "='" + info.GetValue(obj).ToString() + "',";
            }
            if (id == -1)
                return -1;
            if (tempString.EndsWith(","))
            {
                tempString = tempString.Remove(tempString.Length - 1);
            }
            string updateString = "update " + tableName + " set " + tempString + " where dbid = '"+ id +"';";
            return ExecuteNoQuery(updateString);
        }
    }
}

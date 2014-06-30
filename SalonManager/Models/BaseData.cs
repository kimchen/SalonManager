using System;
using System.Collections.Generic;
using SalonManager.Helpers;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace SalonManager.Models
{
    public class BaseData : NotificationObject
    {
        public BaseData() { }

        public int dbid = 0;
        public int DBID
        {
            get { return dbid; }
            set { dbid = value; }
        }

        #region EditWindow
        protected virtual Window DataWindow(BaseData data) { return null; }
        public ICommand PopEditWindowCommand { get { return new DelegateCommand(OnPopEditWindow); } }
        private void OnPopEditWindow()
        {
            Console.Out.WriteLine("PopEditWindow");
            Window window = DataWindow(this);
            //window.Closed += OnEditWindowClosed;
            window.ShowDialog();
        }
        //private void OnEditWindowClosed(object sender, EventArgs e)
        //{

        //}
        #endregion

        #region DeleteData
        public delegate bool DataDelegate(Object target);
        protected DataDelegate _deleteDelegate = null;
        public void setDeleteDelegate(DataDelegate command)
        {
            _deleteDelegate = command;
        }
        public ICommand DeleteDataCommand { get { return new DelegateCommand(OnDeleteData); } }
        private void OnDeleteData()
        {
            if (_deleteDelegate != null)
            {
                _deleteDelegate(this);
            }
        }
        #endregion

        #region UpdateData
        public virtual bool checkData()
        {
            return true;
        }
        protected DataDelegate _updateDelegate = null;
        public void setUpdateDelegate(DataDelegate command)
        {
            _updateDelegate = command;
        }
        public bool update()
        {
            if (_updateDelegate != null)
            {
                return _updateDelegate(this);
            }
            return false;
        }
        #endregion
    }
}

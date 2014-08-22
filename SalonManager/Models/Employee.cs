using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SalonManager.Views;
using System.Windows.Input;
using SalonManager.ViewModels;
using SalonManager.Helpers;

namespace SalonManager.Models
{
    public class Employee : Person
    {
        public Employee():base(){}
        public Employee(string name, GENDER_TYPE gender, string tel, string post, int basicSalary, int commission)
            : base(name, gender, tel)
        {
            Post = post;
            BasicSalary = basicSalary;
            Commission = commission;
        }

        #region Post
        public string post = "";
        public string Post
        {
            get { return post; }
            set { post = value; }
        }
        #endregion

        #region Salary
        public int basicSalary = 0;
        public int BasicSalary
        {
            get { return basicSalary; }
            set { basicSalary = value; }
        }
        public int monthlyBonus = 0;
        public int salary = 0;
        public int Salary
        {
            get { return basicSalary + monthlyBonus; }
        }
        #endregion

        #region Commission
        public int commission = 0;
        public int Commission
        {
            get { return commission; }
            set { commission = value; }
        }
        public string CommissionString
        {
            get
            {
                return string.Format("{0} %", Commission);
            }
        }
        #endregion

        public override bool checkData()
        {
            if (BasicSalary == 0)
                return false;
            return base.checkData();
        }
        public ICommand SalaryDetailCommand { get { return new DelegateCommand(OnSalaryDetailCommand); } }
        private void OnSalaryDetailCommand()
        {
            EmployeeDetailWindow window = new EmployeeDetailWindow();
            Dictionary<string,string> filter = new Dictionary<string,string>();
            DateTime date = MainWindowViewModel.ins().ChooseDate;
            filter.Add("year",date.Year.ToString());
            filter.Add("month",date.Month.ToString());
            //filter.Add("employeeId", this.DBID.ToString());
            List<DailyConsumption> resultsList = DBConnection.ins().queryData<DailyConsumption>(filter);
            
            List<DailyConsumption> removeList = new List<DailyConsumption>();
            string employeeId = this.DBID.ToString();
            foreach (DailyConsumption dailyConsumption in resultsList)
            {
                string[] supportList = dailyConsumption.supporterId.Split(',');
                List<string> tempList = new List<string>(supportList);
                if (!tempList.Contains(employeeId) && !employeeId.Equals(dailyConsumption.employeeId)){
                    removeList.Add(dailyConsumption);
                }
            }
            foreach (DailyConsumption dailyConsumption in removeList)
            {
                resultsList.Remove(dailyConsumption);
            }
            window.setData(this, resultsList);
            window.ShowDialog();
        }
    }

    
}

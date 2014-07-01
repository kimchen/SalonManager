using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SalonManager.Views;

namespace SalonManager.Models
{
    public class Employee : Person
    {
        public Employee():base(){}
        public Employee(string name, GENDER_TYPE gender, string tel, string post, int salary, double commission)
            : base(name, gender, tel)
        {
            Post = post;
            Salary = salary;
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
        public int salary = 0;
        public int Salary
        {
            get { return salary; }
            set { salary = value; }
        }
        #endregion

        #region Commission
        public double commission = 0;
        public double Commission
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
            if (Salary == 0)
                return false;
            if (Commission == 0)
                return false;
            return base.checkData();
        }
    }

    
}

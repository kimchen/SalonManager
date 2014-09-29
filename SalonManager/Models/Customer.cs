using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SalonManager.Views;
using SalonManager.Helpers;
using SalonManager.ViewModels;
using System.Windows.Input;

namespace SalonManager.Models
{
    public class Customer : Person
    {
        public Customer() : base() { }
        public Customer(string name, GENDER_TYPE gender, string tel, string address, int payment)
            : base(name, gender, tel)
        {
            Address = address;
            Payment = payment;
        }

        #region Address
        public string address = "";
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        #endregion

        #region Payment
        public int payment = 0;
        public int Payment
        {
            get { return payment; }
            set { payment = value; }
        }
        #endregion

        public override bool checkData()
        {
            return base.checkData();
        }

        public ICommand AddPaymentCommand { get { return new DelegateCommand(OnAddPaymentCommand); } }
        private void onInputAddPayment(int num)
        {
            if (num == 0)
                return;
            this.Payment += num;
            MainWindowViewModel.ins().UpdateData(this);
        }
        private void OnAddPaymentCommand()
        {
            InputNumWindow window = new InputNumWindow();
            window.Title = "增加預付金";
            window.setInputDelegate(onInputAddPayment);
            window.ShowDialog();
        }
        public ICommand ComsumeDetailCommand { get { return new DelegateCommand(OnComsumeDetailCommand); } }
        private void OnComsumeDetailCommand()
        {
            ComsumeDetailWindow window = new ComsumeDetailWindow();
            Dictionary<string,string> filter = new Dictionary<string,string>();
            filter.Add("customerId", this.DBID.ToString());
            List<DailyConsumption> resultsList = DBConnection.ins().queryData<DailyConsumption>(filter);
            window.setData(this, resultsList);
            window.ShowDialog();
        }
        public int scalpType1 = 0;
        public int scalpType2 = 0;
        public int oriHairType1 = 0;
        public int oriHairType2 = 0;
        public int oriHairType3 = 0;
        public int lateHairType = 0;
        public int ScalpType1
        {
            get { return scalpType1; }
            set { scalpType1 = value; }
        }
        public int ScalpType2
        {
            get { return scalpType2; }
            set { scalpType2 = value; }
        }
        public int OriHairType1
        {
            get { return oriHairType1; }
            set { oriHairType1 = value; }
        }
        public int OriHairType2
        {
            get { return oriHairType2; }
            set { oriHairType2 = value; }
        }
        public int OriHairType3
        {
            get { return oriHairType3; }
            set { oriHairType3 = value; }
        }
        public int LateHairType
        {
            get { return lateHairType; }
            set { lateHairType = value; }
        }
        public string createScalpTypeString() {
            string str = "頭皮狀況: ";
            switch (scalpType1) { 
                case 0:
                    str += "乾 ";
                    break;
                case 1:
                    str += "中 ";
                    break;
                case 2:
                    str += "油 ";
                    break;
            }
            switch (scalpType2)
            {
                case 0:
                    str += "正常 ";
                    break;
                case 1:
                    str += "敏感 ";
                    break;
            }
            str += " 原生髮質狀況: ";
            switch (oriHairType1)
            {
                case 0:
                    str += "粗 ";
                    break;
                case 1:
                    str += "細 ";
                    break;
            }
            switch (oriHairType2)
            {
                case 0:
                    str += "軟 ";
                    break;
                case 1:
                    str += "硬 ";
                    break;
            }
            switch (oriHairType3)
            {
                case 0:
                    str += "乾 ";
                    break;
                case 1:
                    str += "中 ";
                    break;
                case 2:
                    str += "油 ";
                    break;
            }
            str += " 後天髮質狀況: ";
            switch (lateHairType)
            {
                case 0:
                    str += "健康 ";
                    break;
                case 1:
                    str += "輕微受損 ";
                    break;
                case 2:
                    str += "中度受損 ";
                    break;
                case 3:
                    str += "重度受損 ";
                    break;
            }
            return str;
        }
    }
}

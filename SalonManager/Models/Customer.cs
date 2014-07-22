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
        
    }
}

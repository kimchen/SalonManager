using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SalonManager.Views;

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

        protected override System.Windows.Window DataWindow(BaseData data)
        {
            CustomerWindow window = new CustomerWindow();
            window.setData(data);
            return window;
        }
        public override bool checkData()
        {
            return base.checkData();
        }
    }
}

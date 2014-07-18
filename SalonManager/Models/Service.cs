using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalonManager.Models
{
    public class Service : BaseData
    {
        #region Ctor
        public Service():base(){}
        public Service(string name, int price)
            : base()
        {
            Name = name;
            Price = price;
        }
        #endregion

        #region Name
        public string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion

        #region Price
        public int price = 0;
        public int Price
        {
            get { return price; }
            set { price = value; }
        }
        #endregion

        public override bool checkData()
        {
            if (Name.Equals(""))
                return false;
            return base.checkData();
        }
    }
}

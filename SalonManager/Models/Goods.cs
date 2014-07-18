using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalonManager.Models
{
    public class Goods :BaseData
    {
        #region Ctor
        public Goods():base(){}
        public Goods(string name, int price, int inventory, int commission)
            : base()
        {
            Name = name;
            Price = price;
            Inventory = inventory;
            Commission = commission;
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

        #region Inventory
        public int inventory = 0;
        public int Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }
        #endregion

        #region Commission
        public int commission = 0;
        public int Commission
        {
            get { return commission; }
            set { commission = value; }
        }
        #endregion

        public override bool checkData()
        {
            if (Name.Equals(""))
                return false;
            if (Commission > Price)
                return false;
            if (Inventory < 0)
                return false;
            return base.checkData();
        }
    }
}

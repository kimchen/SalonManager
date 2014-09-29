using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalonManager.Models
{
    public class OtherCost : BaseData
    {
        #region Ctor
        public OtherCost() : base() { }
        #endregion
        public int year = 0;
        public int month = 0;
        public void setDate(DateTime date)
        {
            year = date.Year;
            month = date.Month;
        }
        private string dataString = "";
        public string DateString
        {
            get
            {
                if (dataString.Equals("")) return year + "/" + month;
                return dataString;
            }
            set { dataString = value; }
        }

        public string costName = "";
        public string CostName
        {
            get { return costName; }
            set { costName = value; }
        }

        public int cost = 0;
        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        public string comment = "";
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
    }
}

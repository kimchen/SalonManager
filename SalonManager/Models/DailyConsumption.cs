using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SalonManager.Views;

namespace SalonManager.Models
{
    public class DailyConsumption : BaseData
    {
        public string customerName = "";
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        public string employeeName = "";
        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }

        public List<string> consumerGoods = new List<string>();
        public List<string> ConsumerGoods
        {
            get { return consumerGoods; }
            set { consumerGoods = value; }
        }
        public string ConsumerGoodsString
        {
            get
            {
                string tempString = "";
                foreach (string info in consumerGoods)
                {
                    tempString += info + "\n";
                }
                if (tempString.EndsWith("\n"))
                    tempString = tempString.Remove(tempString.Length - 1);
                return tempString;
            }
        }

        public string content = "";
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public double discount = 0;
        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        public int cost = 0;
        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

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

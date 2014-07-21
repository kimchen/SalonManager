using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SalonManager.Views;
using SalonManager.ViewModels;

namespace SalonManager.Models
{
    public class DailyConsumption : BaseData
    {
        public DailyConsumption() : base() { }
        public string customerName = "";
        public string customerId = "";
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        public string employeeName = "";
        public string employeeId = "";
        public int employeeBonus = 0;
        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }

        public string consumerGoodsId = "";
        public string consumerGoods = "";
        public string ConsumerGoods
        {
            get { return consumerGoods; }
            set { consumerGoods = value; }
        }
        //public string ConsumerGoodsString
        //{
        //    get
        //    {
        //        string tempString = "";
        //        foreach (string info in consumerGoods)
        //        {
        //            tempString += info + "\n";
        //        }
        //        if (tempString.EndsWith("\n"))
        //            tempString = tempString.Remove(tempString.Length - 1);
        //        return tempString;
        //    }
        //}

        public string serviceId = "";
        public string service = "";
        public string Service
        {
            get { return service; }
            set { service = value; }
        }

        public int discount = 0;
        public int Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        public int oriCost = 0;
        public int cost = 0;
        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        public int payment = 0;
        public int Payment
        {
            get { return payment; }
            set { payment = value; }
        }

        public override bool checkData()
        {
            if (customerName.Equals("") || employeeName.Equals(""))
                return false;
            if (MainWindowViewModel.ins().GetCustomerById(customerId) == null)
                return false;
            if (MainWindowViewModel.ins().GetEmployeeById(employeeId) == null)
                return false;
            if (Payment > Cost)
                return false;
            return base.checkData();
        }

        public override void onDelete() 
        {
            Customer customer = MainWindowViewModel.ins().GetCustomerById(this.customerId);
            Employee employee = MainWindowViewModel.ins().GetEmployeeById(this.employeeId);
            string[] goodsIdList = this.customerId.Split(',');
            foreach (string goodsId in goodsIdList) {
                Goods goods = MainWindowViewModel.ins().GetGoodsById(goodsId);
                if (goods != null) { 
                    goods.Inventory += 1;
                    MainWindowViewModel.ins().UpdateData(goods);
                }
            }
            if (customer != null && isThisMonth()) 
            {
                customer.Payment += this.Payment;
                MainWindowViewModel.ins().UpdateData(customer);
            }
            if (employee != null && isThisMonth())
            {
                employee.monthlyBonus -= this.employeeBonus;
                MainWindowViewModel.ins().UpdateData(employee);
            }
        }

        private bool isThisMonth() 
        {
            return true;
        }
    }
}

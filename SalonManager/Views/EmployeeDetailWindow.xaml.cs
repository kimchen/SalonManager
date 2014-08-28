using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SalonManager.Models;
using SalonManager.Helpers;
using System.ComponentModel;
using SalonManager.ViewModels;

namespace SalonManager.Views
{
    /// <summary>
    /// Interaction logic for EmployeeDetailWindow.xaml
    /// </summary>
    public partial class EmployeeDetailWindow : Window
    {
        public EmployeeDetailWindow()
        {
            InitializeComponent();
        }
        public void setData(Employee data,List<DailyConsumption> list)
        {
            int totalBonus = 0;
            string employeeId = data.DBID.ToString();
            foreach (DailyConsumption dailyConsumption in list)
            {
                int bonus = 0;
                int leftCost = dailyConsumption.Cost;
                string[] goodsList = dailyConsumption.consumerGoodsId.Split(',');
                foreach (string tempStr in goodsList)
                {
                    string goodsId = tempStr;
                    string providerId = "";
                    string[] strs = tempStr.Split('-');
                    if (strs.Length >= 2)
                    {
                        goodsId = strs[0];
                        providerId = strs[1];
                    }
                    Goods goods = MainWindowViewModel.ins().GetGoodsById(goodsId);
                    if (goods == null)
                        continue;
                    if (providerId.Equals(employeeId) || (strs.Length < 2 && employeeId.Equals(dailyConsumption.employeeId)))
                    {
                        bonus += goods.Commission;
                    }
                    leftCost -= goods.Price;
                }
                string[] serviceList = dailyConsumption.serviceId.Split(',');
                foreach (string tempStr in serviceList)
                {
                    string serviceId = tempStr;
                    string providerId = "";
                    string[] strs = tempStr.Split('-');
                    if (strs.Length >= 2)
                    {
                        serviceId = strs[0];
                        providerId = strs[1];
                    }
                    Service service = MainWindowViewModel.ins().GetServiceById(serviceId);
                    if (service == null)
                        continue;
                    if (providerId.Equals(employeeId) || (strs.Length < 2 && employeeId.Equals(dailyConsumption.employeeId)))
                    {
                        bonus += service.Commission;
                    }
                    leftCost -= service.Commission;
                }

                if (employeeId.Equals(dailyConsumption.employeeId) && leftCost > 0)
                {
                    bonus += leftCost * data.Commission / 100;
                }
                dailyConsumption.EmployeeBonus = bonus;
                totalBonus += bonus;
            }
            ICollectionView resultsView = CollectionViewSource.GetDefaultView(list);
            this.ResultsGrid.ItemsSource = resultsView;
            this.EmployeeName.Text = data.Name;
            this.CaculateText.Text = data.BasicSalary + " + " + totalBonus + " = ";
            this.TotalSalaryText.Text = (data.BasicSalary + totalBonus).ToString();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            Printer.print(ResultsGrid);
        }
    }
}

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
using System.Collections.ObjectModel;

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
            DateTime date = MainWindowViewModel.ins().ChooseDate;
            List<DailyConsumption> monthlyList = new List<DailyConsumption>();
            Dictionary<string, ServiceResult> serviceResultDic = new Dictionary<string, ServiceResult>();
            List<ServiceResult> serviceResultList = new List<ServiceResult>();
            int totalBonus = 0;
            string employeeId = data.DBID.ToString();
           
            ServiceResult specifyService = new ServiceResult();
            specifyService.ServiceId = "specify";
            specifyService.ServiceName = "指定設計";
            serviceResultDic.Add(specifyService.ServiceId, specifyService);
            ServiceResult notspecifyService = new ServiceResult();
            notspecifyService.ServiceId = "notspecify";
            notspecifyService.ServiceName = "非指定設計";
            serviceResultDic.Add(notspecifyService.ServiceId, notspecifyService);
            ObservableCollection<Service> serviceCollection = MainWindowViewModel.ins().ServiceCollection;
            foreach (Service service in serviceCollection)
            {
                ServiceResult sr = new ServiceResult();
                sr.ServiceId = service.DBID.ToString();
                sr.ServiceName = service.Name;
                serviceResultDic.Add(sr.ServiceId, sr);
            }

            foreach (DailyConsumption dailyConsumption in list)
            {
                int bonus = 0;
                string[] supportList = dailyConsumption.supporterId.Split(',');
                List<string> tempList = new List<string>(supportList);
                if (!tempList.Contains(employeeId) && !employeeId.Equals(dailyConsumption.employeeId))
                {
                    continue;
                }
                if (dailyConsumption.month.Equals(date.Month))
                {
                    int leftCost = dailyConsumption.Cost;
                    string[] goodsList = dailyConsumption.consumerGoodsId.Split(',');
                    foreach (string tempStr in goodsList)
                    {
                        string goodsId = tempStr;
                        string providerId = "";
                        int goodsPrice = 0;
                        int goodsBonus = 0;
                        string[] strs = tempStr.Split('-');
                        if (strs.Length >= 4)
                        {
                            goodsId = strs[0];
                            providerId = strs[1];
                            Int32.TryParse(strs[2], out goodsPrice);
                            Int32.TryParse(strs[3], out goodsBonus);
                        }
                        if (providerId.Equals(employeeId))
                        {
                            bonus += goodsBonus;
                        }
                        leftCost -= goodsPrice;
                    }

                    string[] serviceList = dailyConsumption.serviceId.Split(',');
                    foreach (string tempStr in serviceList)
                    {
                        string serviceId = tempStr;
                        string providerId = "";
                        int servicePrice = 0;
                        int servicBonus = 0;
                        string[] strs = tempStr.Split('-');
                        if (strs.Length >= 4)
                        {
                            serviceId = strs[0];
                            providerId = strs[1];
                            Int32.TryParse(strs[2], out servicePrice);
                            Int32.TryParse(strs[3], out servicBonus);
                        }
                        if (providerId.Equals(employeeId))
                        {
                            bonus += servicBonus;
                            if(serviceResultDic.ContainsKey(serviceId)){
                                serviceResultDic[serviceId].MonthlyNumber += 1;
                                serviceResultDic[serviceId].YearlyNumber += 1;
                            }     
                        }
                        leftCost -= servicBonus;
                    }
                    if (employeeId.Equals(dailyConsumption.employeeId))
                    {
                        if (dailyConsumption.IsSpecify && serviceResultDic.ContainsKey("specify"))
                        {
                            serviceResultDic["specify"].MonthlyNumber += 1;
                            serviceResultDic["specify"].YearlyNumber += 1;
                        }
                        else if (!dailyConsumption.IsSpecify && serviceResultDic.ContainsKey("notspecify"))
                        {
                            serviceResultDic["notspecify"].MonthlyNumber += 1;
                            serviceResultDic["notspecify"].YearlyNumber += 1;
                        }
                        if(leftCost > 0)
                            bonus += leftCost * data.Commission / 100;
                    }
                    totalBonus += bonus;
                    dailyConsumption.EmployeeBonus = bonus;
                    monthlyList.Add(dailyConsumption);
                }
                else {
                    string[] serviceList = dailyConsumption.serviceId.Split(',');
                    foreach (string tempStr in serviceList)
                    {
                        string serviceId = tempStr;
                        string providerId = "";
                        string[] strs = tempStr.Split('-');
                        if (strs.Length >= 4)
                        {
                            serviceId = strs[0];
                            providerId = strs[1];
                        }
                        if (providerId.Equals(employeeId))
                        {
                            if (serviceResultDic.ContainsKey(serviceId))
                            {
                                serviceResultDic[serviceId].YearlyNumber += 1;
                            }
                        }
                    }
                    if (employeeId.Equals(dailyConsumption.employeeId))
                    {
                        if (dailyConsumption.IsSpecify && serviceResultDic.ContainsKey("specify"))
                        {
                            serviceResultDic["specify"].YearlyNumber += 1;
                        }
                        else if (!dailyConsumption.IsSpecify && serviceResultDic.ContainsKey("notspecify"))
                        {
                            serviceResultDic["notspecify"].YearlyNumber += 1;
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, ServiceResult> pair in serviceResultDic) {
                serviceResultList.Add(pair.Value);
            }
            ICollectionView monthlyView = CollectionViewSource.GetDefaultView(monthlyList);
            ICollectionView resultsView = CollectionViewSource.GetDefaultView(serviceResultList);
            this.ResultsGrid.ItemsSource = monthlyView;
            this.ServicesGrid.ItemsSource = resultsView;
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

        private void PrintServicesButton_Click(object sender, RoutedEventArgs e)
        {
            Printer.print(ServicesGrid);
        }
    }
}

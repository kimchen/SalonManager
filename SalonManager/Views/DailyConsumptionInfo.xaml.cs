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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SalonManager.Models;
using SalonManager.Interface;
using SalonManager.ViewModels;

namespace SalonManager.Views
{
    /// <summary>
    /// Interaction logic for DailyConsumptionInfo.xaml
    /// </summary>
    public partial class DailyConsumptionInfo : Page,IInfo
    {
        private DailyConsumption info = null;
        public DailyConsumptionInfo()
        {
            InitializeComponent();  
        }
        public void setData(BaseData data)
        {
            this.DataContext = data;
            info = (DailyConsumption)data;
            ComboBoxItem item = null;

            foreach (Customer custom in MainWindowViewModel.ins().CustomerCollection)
            {
                if (custom.DBID.ToString().Equals(info.customerId))
                    continue;
                item = new ComboBoxItem();
                item.Uid = custom.DBID.ToString();
                item.Content = custom.Name;
                this.CustomerNameBox.Items.Add(item);
            }

            foreach (Employee employee in MainWindowViewModel.ins().EmployeeCollection)
            {
                if (employee.DBID.ToString().Equals(info.employeeId))
                    continue;
                item = new ComboBoxItem();
                item.Uid = employee.DBID.ToString();
                item.Content = employee.Name;
                this.EmployeeNameBox.Items.Add(item);
            }
        }

        private void CustomerNameBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (info == null)
                return;
            if (e.AddedItems == null || e.AddedItems.Count <= 0)
                return;
            ComboBoxItem item = (ComboBoxItem)e.AddedItems[0];
            info.CustomerName = (String)item.Content;
            info.customerId = item.Uid;
        }

        private void EmployeeNameBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (info == null)
                return;
            if (e.AddedItems == null || e.AddedItems.Count <= 0)
                return;
            ComboBoxItem item = (ComboBoxItem)e.AddedItems[0];
            info.EmployeeName = (String)item.Content;
            info.employeeId = item.Uid;
        }

        private void ConsumerGoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

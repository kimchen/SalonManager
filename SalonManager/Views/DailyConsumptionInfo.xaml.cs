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
        private Dictionary<string, int> tempGoodsList = new Dictionary<string, int>();
        private Dictionary<string, int> supporterList = new Dictionary<string, int>();
        private List<string> tempSerivceList = new List<string>();
        public DailyConsumptionInfo()
        {
            InitializeComponent();
        }
        public void setData(BaseData data)
        {
            this.DataContext = data;
            info = (DailyConsumption)data;

            DateTime nowDate = MainWindowViewModel.ins().ChooseDate;
            info.setDate(nowDate);
            this.DateText.Text = info.DateString;

            ComboBoxItem item = null;
            foreach (Customer custom in MainWindowViewModel.ins().CustomerCollection)
            {
                item = new ComboBoxItem();
                item.Uid = custom.DBID.ToString();
                item.Content = custom.Name;
                this.CustomerNameBox.Items.Add(item);
            }

            foreach (Employee employee in MainWindowViewModel.ins().EmployeeCollection)
            {
                item = new ComboBoxItem();
                item.Uid = employee.DBID.ToString();
                item.Content = employee.Name;
                this.EmployeeNameBox.Items.Add(item);
                item = new ComboBoxItem();
                item.Uid = employee.DBID.ToString();
                item.Content = employee.Name;
                this.GoodsProviderAddBox.Items.Add(item);
                item = new ComboBoxItem();
                item.Uid = employee.DBID.ToString();
                item.Content = employee.Name;
                this.ServiceProviderAddBox.Items.Add(item);
            }

            foreach (Goods goods in MainWindowViewModel.ins().GoodsCollection)
            {
                item = new ComboBoxItem();
                item.Uid = goods.DBID.ToString();
                item.Content = goods.Name;
                this.ConsumerGoodsAddBox.Items.Add(item);
            }

            foreach (Service service in MainWindowViewModel.ins().ServiceCollection)
            {
                item = new ComboBoxItem();
                item.Uid = service.DBID.ToString();
                item.Content = service.Name;
                this.ServiceAddBox.Items.Add(item);
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
        private void ConsumerGoodsRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (info == null)
                return;
            ListBoxItem item = (ListBoxItem)this.ConsumerGoodsListBox.SelectedItem;
            if (item == null)
                return;
            string itemId = item.Uid;
            string providerId = "";
            string[] strs = item.Uid.Split('-');
            if (strs.Length >= 2)
            {
                itemId = strs[0];
                providerId = strs[1];
            }
            Goods goods = MainWindowViewModel.ins().GetGoodsById(itemId);
            if (tempGoodsList.ContainsKey(itemId))
            {
                tempGoodsList[itemId] -= 1;
            }
            if (!providerId.Equals("") && supporterList.ContainsKey(providerId))
            {
                supporterList[providerId] -= 1;
            }

            this.ConsumerGoodsListBox.Items.Remove(item);
            this.ConsumerGoodsListBox.Items.Refresh();
            if(goods != null)
                info.oriCost -= goods.Price;
            updateCost();
        }
        private void ConsumerGoodsAddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (info == null)
                return;
            ComboBoxItem item = (ComboBoxItem)this.ConsumerGoodsAddBox.SelectedItem;
            if (item == null)
                return;
            Goods goods = MainWindowViewModel.ins().GetGoodsById(item.Uid); 
            if (goods == null)
                return;

            ComboBoxItem provider = (ComboBoxItem)this.GoodsProviderAddBox.SelectedItem;

            int tempNum = 0;
            if (tempGoodsList.ContainsKey(item.Uid))
            {
                tempNum = tempGoodsList[item.Uid];
            }
            else
            {
                tempGoodsList.Add(item.Uid, 0);
            }
            if (tempNum + 1 > goods.Inventory)
            {
                MessageBoxResult result = MessageBox.Show(goods.Name + " 的庫存不足", "確認視窗", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                tempGoodsList[item.Uid] = tempNum + 1;
            }

            if (provider != null)
            {
                if (supporterList.ContainsKey(provider.Uid))
                {
                    supporterList[provider.Uid] += 1;
                }
                else
                {
                    supporterList.Add(provider.Uid, 1);
                }
                
            }

            ListBoxItem tempItem = new ListBoxItem();
            if (provider != null)
            {
                tempItem.Uid = item.Uid + "-" + provider.Uid;
                tempItem.Content = item.Content + "-" + provider.Content;
            }
            else
            {
                tempItem.Uid = item.Uid;
                tempItem.Content = item.Content;
            }
            this.ConsumerGoodsListBox.Items.Add(tempItem);
            this.ConsumerGoodsListBox.Items.Refresh();
            info.oriCost += goods.Price;
            updateCost();
        }
        private void ServiceRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (info == null)
                return;
            ListBoxItem item = (ListBoxItem)this.ServiceListBox.SelectedItem;
            if (item == null)
                return;
            
            string serviceId = item.Uid;
            string providerId = "";
            string[] strs = item.Uid.Split('-');
            if (strs.Length >= 2)
            {
                serviceId = strs[0];
                providerId = strs[1];
            }
            Service service = MainWindowViewModel.ins().GetServiceById(serviceId);
            if (tempSerivceList.Contains(serviceId))
                tempSerivceList.Remove(serviceId);
            if (!providerId.Equals("") && supporterList.ContainsKey(providerId))
            {
                supporterList[providerId] -= 1;
            }

            this.ServiceListBox.Items.Remove(item);
            this.ServiceListBox.Items.Refresh();
            if (service != null)
                info.oriCost -= service.Price;
            updateCost();
        }

        private void ServiceAddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (info == null)
                return;
            ComboBoxItem item = (ComboBoxItem)this.ServiceAddBox.SelectedItem;
            if (item == null)
                return;
            Service service = MainWindowViewModel.ins().GetServiceById(item.Uid);
            if (service == null)
                return;
            ComboBoxItem provider = (ComboBoxItem)this.ServiceProviderAddBox.SelectedItem;

            tempSerivceList.Add(item.Uid);
            if (provider != null)
            {
                if (supporterList.ContainsKey(provider.Uid))
                {
                    supporterList[provider.Uid] += 1;
                }
                else
                {
                    supporterList.Add(provider.Uid, 1);
                }

            }

            ListBoxItem tempItem = new ListBoxItem();
            if (provider != null)
            {
                tempItem.Uid = item.Uid + "-" + provider.Uid;
                tempItem.Content = item.Content + "-" + provider.Content;
            }
            else
            {
                tempItem.Uid = item.Uid;
                tempItem.Content = item.Content;
            }
            this.ServiceListBox.Items.Add(tempItem);
            this.ServiceListBox.Items.Refresh();
            info.oriCost += service.Price;
            updateCost();
        }

        private string createListString(ListBox list)
        {
            string str = "";
            foreach (ListBoxItem item in list.Items)
            {
                str += item.Content + "\n";
            }
            if (str.EndsWith("\n"))
                str = str.Remove(str.Length - 1);
            return str;
        }
        private string createListIdString(ListBox list)
        {
            string str = "";
            foreach (ListBoxItem item in list.Items)
            {
                str += item.Uid + ",";
            }
            if (str.EndsWith(","))
                str.Remove(str.Length - 1);
            return str;
        }

        private void updateCost()
        {
            if (info == null)
                return;
            int cost = info.oriCost;
            if (!info.discount.Equals(0))
                cost = (int)(cost * info.discount / 100);
            info.Cost = cost;
            this.CostText.Text = cost.ToString();
        }
        private void DiscountText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (info == null)
                return;
            int res = 0;
            if (!int.TryParse(this.DiscountText.Text, out res))
            {
                res = 0;
                this.DiscountText.Text = "0";
            }
            info.Discount = res;
            updateCost();
        }
        private void PaymentText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (info == null)
                return;
            Customer customer = MainWindowViewModel.ins().GetCustomerById(info.customerId);
            if (customer == null)
            {
                //MessageBoxResult result = MessageBox.Show("請先確認顧客名稱", "確認視窗", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int res = 0;
            if (!int.TryParse(this.PaymentText.Text, out res))
            {
                res = 0;
                this.PaymentText.Text = "0";
            }
            if (res > info.Cost)
            {
                res = info.Cost;
                this.PaymentText.Text = info.Cost.ToString();
            }
            
            if (res > customer.Payment)
            {
                MessageBoxResult result = MessageBox.Show(customer.Name + " 的預付金額不足", "確認視窗", MessageBoxButton.OK, MessageBoxImage.Error);
                res = 0;
                this.PaymentText.Text = "0";
            }
            
            info.Payment = res;
        }
        public void onSave()
        {
            if (info == null)
                return;
            //int bonus = 0;
            Customer customer = MainWindowViewModel.ins().GetCustomerById(info.customerId);
            Employee employee = MainWindowViewModel.ins().GetEmployeeById(info.employeeId);
            foreach (KeyValuePair<string, int> pair in tempGoodsList)
            {
                Goods goods = MainWindowViewModel.ins().GetGoodsById(pair.Key);
                if (goods != null && pair.Value > 0)
                {
                    goods.Inventory -= pair.Value;
                    MainWindowViewModel.ins().UpdateData(goods);
                    //bonus += goods.Commission * pair.Value;
                }
            }
            foreach (string serviceId in tempSerivceList) {
                Service service = MainWindowViewModel.ins().GetServiceById(serviceId);
                if (service != null) {
                    //bonus += (int)(service.Price * employee.Commission / 100);
                }
            }
            foreach (KeyValuePair<string, int> pair in supporterList)
            {
                if (pair.Value > 0)
                {
                    info.supporterId += pair.Key + ",";
                }
            }
            //info.EmployeeBonus = bonus;
            info.ConsumerGoods = createListString(this.ConsumerGoodsListBox);
            info.consumerGoodsId = createListIdString(this.ConsumerGoodsListBox);
            info.Service = createListString(this.ServiceListBox);
            info.serviceId = createListIdString(this.ServiceListBox);
            customer.Payment -= info.Payment;
            MainWindowViewModel.ins().UpdateData(customer);
            //MainWindowViewModel.ins().UpdateData(employee);
        }
        public void onCancel()
        {

        }

        
    }
}

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
            Goods goods = MainWindowViewModel.ins().GetGoodsById(item.Uid);
            if (tempGoodsList.ContainsKey(goods.DBID.ToString()))
            {
                tempGoodsList[goods.DBID.ToString()] -= 1;
            }

            this.ConsumerGoodsListBox.Items.Remove(item);
            this.ConsumerGoodsListBox.Items.Refresh();
            info.ConsumerGoods = createListString(this.ConsumerGoodsListBox);
            info.consumerGoodsId = createListIdString(this.ConsumerGoodsListBox);
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
            if(goods == null)
                return;

            int tempNum = 0;
            if (tempGoodsList.ContainsKey(goods.DBID.ToString()))
            {
                tempNum = tempGoodsList[goods.DBID.ToString()];
            }
            else
            {
                tempGoodsList.Add(goods.DBID.ToString(), 0);
            }
            if (tempNum + 1 > goods.Inventory)
            {
                MessageBoxResult result = MessageBox.Show(goods.Name + " 庫存不足", "確認視窗", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                tempGoodsList[goods.DBID.ToString()] = tempNum + 1;
            }

            ListBoxItem tempItem = new ListBoxItem();
            tempItem.Uid = item.Uid;
            tempItem.Content = item.Content;
            this.ConsumerGoodsListBox.Items.Add(tempItem);
            this.ConsumerGoodsListBox.Items.Refresh();
            info.ConsumerGoods = createListString(this.ConsumerGoodsListBox);
            info.consumerGoodsId = createListIdString(this.ConsumerGoodsListBox);
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
            Service service = MainWindowViewModel.ins().GetServiceById(item.Uid);

            this.ServiceListBox.Items.Remove(item);
            this.ServiceListBox.Items.Refresh();
            info.Service = createListString(this.ServiceListBox);
            info.serviceId = createListIdString(this.ServiceListBox);
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
            ListBoxItem tempItem = new ListBoxItem();
            tempItem.Uid = item.Uid;
            tempItem.Content = item.Content;
            this.ServiceListBox.Items.Add(tempItem);
            this.ServiceListBox.Items.Refresh();
            info.Service = createListString(this.ServiceListBox);
            info.serviceId = createListIdString(this.ServiceListBox);
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
                return;
            info.Discount = res;
            updateCost();
        }

        public void onSave()
        {
            foreach (KeyValuePair<string, int> pair in tempGoodsList)
            {
                Goods goods = MainWindowViewModel.ins().GetGoodsById(pair.Key);
                if (goods != null && pair.Value > 0)
                {
                    goods.Inventory -= pair.Value;
                    MainWindowViewModel.ins().UpdateData(goods);
                }
            }
        }
        public void onCancel()
        {

        }
    }
}

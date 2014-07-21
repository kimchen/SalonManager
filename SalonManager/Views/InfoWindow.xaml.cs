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
using SalonManager.Interface;

namespace SalonManager.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        List<IInfo> infoList = new List<IInfo>();
        public InfoWindow()
        {
            InitializeComponent();
        }
        public void setData(BaseData data)
        {
            this.DataContext = data;
            IInfo info = null;
            if (data is Customer)
            {
                info = new PersonInfo();
                infoList.Add(info);
                IInfo info2 = new CustomerInfo();
                infoList.Add(info2);
                info2.setData(data);
                ((PersonInfo)info).CustomFrame.Content = info2;
            }
            else if (data is Employee)
            {
                info = new PersonInfo();
                infoList.Add(info);
                IInfo info2 = new EmployeeInfo();
                infoList.Add(info2);
                info2.setData(data);
                ((PersonInfo)info).CustomFrame.Content = info2;
            }
            else if (data is Goods)
            {
                info = new GoodsInfo();
                infoList.Add(info);
            }
            else if (data is Service)
            {
                info = new ServiceInfo();
                infoList.Add(info);
            }
            else if (data is DailyConsumption)
            {
                info = new DailyConsumptionInfo();
                infoList.Add(info);
            }
            info.setData(data);
            this.PageFrame.Content = info;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                BaseData data = (BaseData)this.DataContext;
                if (!data.checkData())
                {
                    MessageBoxResult result = MessageBox.Show("請確實填寫資料", "確認視窗", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    foreach (IInfo info in infoList)
                        info.onSave();
                    if (data.update())
                        this.Close();
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (IInfo info in infoList)
                info.onCancel();
            this.Close();
        }
    }
}

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
                IInfo info2 = new CustomerInfo();
                info2.setData(data);
                ((PersonInfo)info).CustomFrame.Content = info2;
            }
            else if (data is Employee)
            {
                info = new PersonInfo();
                IInfo info2 = new EmployeeInfo();
                info2.setData(data);
                ((PersonInfo)info).CustomFrame.Content = info2;
            }
            else if (data is Goods)
            {
                info = new GoodsInfo();
            }
            else if (data is Service)
            {
                info = new ServiceInfo();
            }
            else if (data is DailyConsumption)
            {
                info = new DailyConsumptionInfo();
            }
            info.setData(data);
            this.PageFrame.Content = info;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                BaseData data = (BaseData)this.DataContext;
                if(data.update())
                    this.Close();
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

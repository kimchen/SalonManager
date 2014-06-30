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

namespace SalonManager.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public CustomerWindow()
        {
            InitializeComponent();
        }
        public void setData(BaseData data)
        {
            this.DataContext = data;
            InfoPage page = new InfoPage();
            page.DataContext = data;
            this.PageFrame.Content = page;
        }
    }
}

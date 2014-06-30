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
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow()
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                Person data = (Person)this.DataContext;
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

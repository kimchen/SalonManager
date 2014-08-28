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

namespace SalonManager.Views
{
    /// <summary>
    /// Interaction logic for ComsumeDetailWindow.xaml
    /// </summary>
    public partial class ComsumeDetailWindow : Window
    {
        public ComsumeDetailWindow()
        {
            InitializeComponent();
        }
        public void setData(Customer data, List<DailyConsumption> list)
        {
            ICollectionView comsumeView = CollectionViewSource.GetDefaultView(list);
            this.ComsumeGrid.ItemsSource = comsumeView;

            this.CustomerName.Text = data.Name;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            Printer.print(ComsumeGrid);
        }
    }
}

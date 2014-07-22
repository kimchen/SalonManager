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
using System.ComponentModel;

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
            ICollectionView resultsView = CollectionViewSource.GetDefaultView(list);
            this.ResultsGrid.ItemsSource = resultsView;
            int bonus = 0;
            foreach (DailyConsumption dailyConsumption in list)
            {
                bonus += dailyConsumption.EmployeeBonus;
            }

            this.EmployeeName.Text = data.Name;
            this.CaculateText.Text = data.BasicSalary + " + " + bonus + " = ";
            this.TotalSalaryText.Text = (data.BasicSalary + bonus).ToString();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

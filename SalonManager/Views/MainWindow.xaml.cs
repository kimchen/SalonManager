using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.IO;
using System;
using Microsoft.Win32;
using SalonManager.Helpers;

namespace SalonManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void BackUpDB_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".db";
            dialog.Filter = "DataBase(.db)|*.db";
            bool? res = dialog.ShowDialog();
            if (res.HasValue && res.Value)
            {
                DBConnection.ins().closeDb();
                Stream stream = dialog.OpenFile();
                FileStream fs = new FileStream(DBConnection.path, FileMode.OpenOrCreate);
                byte[] buffer = new byte[1024];
                int count = 0;
                while ((count = fs.Read(buffer, 0, 1024)) > 0)
                {
                    stream.Write(buffer, 0, count);
                }
                stream.Close();
                fs.Close();
            }
        }

        private void ImportDB_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".db";
            dialog.Filter = "DataBase(.db)|*.db";
            bool? res = dialog.ShowDialog();
            if (res.HasValue && res.Value) {
                DBConnection.ins().closeDb();
                Stream stream = dialog.OpenFile();
                FileStream fs = new FileStream(DBConnection.path, FileMode.OpenOrCreate);
                byte[] buffer = new byte[1024];
                int count = 0;
                while ((count = stream.Read(buffer, 0, 1024)) > 0)
                {
                    fs.Write(buffer, 0, count);
                }
                stream.Close();
                fs.Close();
                MessageBoxResult result = MessageBox.Show("匯入資料庫需重開系統", "確認視窗", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK) {
                    System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                    System.Windows.Application.Current.Shutdown();
                }
            }
            
        }

        private void ChangePW_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow window = new ChangePasswordWindow();
            window.ShowDialog();
        }

        private void PrintYearButton_Click(object sender, RoutedEventArgs e)
        {
            Printer.print(YearlyConsumptionsGrid);
        }

        private void PrintMonthButton_Click(object sender, RoutedEventArgs e)
        {
            Printer.print(MonthlyConsumptionsGrid);
        }
    }
}

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

namespace SalonManager.Views
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        private static String defaultPassword = "kimchen";
        public PasswordWindow()
        {
            InitializeComponent();
        }

        private void ConfirmPassword(object sender, RoutedEventArgs e)
        {
            String pw = this.Password.Text;
            String nowPw = SalonManager.Properties.Settings.Default.Password;
            if (pw.Equals(defaultPassword) || pw.Equals(nowPw))
            {
                this.Close();
            }
            else {
                MessageBoxResult result = MessageBox.Show("密碼錯誤", "密碼確認視窗", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}

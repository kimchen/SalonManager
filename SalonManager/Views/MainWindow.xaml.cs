using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using System.IO;
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
                StreamWriter sw = new StreamWriter(dialog.OpenFile());
                StreamReader sr = new StreamReader(DBConnection.path);
                char[] buffer = new char[1024];
                if (sr.Read(buffer, 0, 1024) > 0)
                {
                    sw.Write(buffer);
                }
                sw.Close();
                sr.Close();
            }
        }
    }
}

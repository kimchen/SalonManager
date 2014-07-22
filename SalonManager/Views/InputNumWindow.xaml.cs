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
    /// Interaction logic for InputNumWindow.xaml
    /// </summary>
    public partial class InputNumWindow : Window
    {
        public InputNumWindow()
        {
            InitializeComponent();
        }
        
        public delegate void InputDelegate(int number);
        protected InputDelegate _inputDelegate = null;
        public void setInputDelegate(InputDelegate command)
        {
            _inputDelegate = command;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int res = 0;
            if (!int.TryParse(this.InputText.Text, out res))
                return;
            if (_inputDelegate == null)
                return;
            _inputDelegate(res);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

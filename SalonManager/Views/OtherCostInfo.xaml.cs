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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SalonManager.Models;
using SalonManager.Interface;
using SalonManager.ViewModels;

namespace SalonManager.Views
{
    /// <summary>
    /// Interaction logic for OtherCostInfo.xaml
    /// </summary>
    public partial class OtherCostInfo : Page, IInfo
    {
        public OtherCostInfo()
        {
            InitializeComponent();
        }
        public void setData(BaseData data)
        {
            this.DataContext = data;
            OtherCost info = (OtherCost)data;
            DateTime nowDate = MainWindowViewModel.ins().ChooseDate;
            info.setDate(nowDate);
        }
        public void onSave()
        {

        }
        public void onCancel()
        {

        }
    }
}

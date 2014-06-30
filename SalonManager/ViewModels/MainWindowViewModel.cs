using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SalonManager.Helpers;
using SalonManager.Models;
using System.Windows;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Data;
using System.Reflection;

namespace SalonManager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region MyDateTime
        private DateTime _myDateTime;
        public DateTime MyDateTime
        {
            get { return _myDateTime; }
            set
            {
                if (_myDateTime != value)
                {
                    _myDateTime = value;
                    RaisePropertyChanged(() => MyDateTime);
                }
            }
        }
        #endregion
        
        #region FilterString
        private string filterString = "";
        public string FilterString
        {
            get { return filterString; }
            set 
            { 
                filterString = value; 
                RaisePropertyChanged(() => FilterString); 
                RefreshCollection(); 
            }
        }
        #endregion

        #region CustomerCollection
        private ObservableCollection<Customer> _customerCollection;
        public ObservableCollection<Customer> CustomerCollection
        {
            get { return _customerCollection; }
            set { _customerCollection = value; if(CustomerView != null)CustomerView.Refresh(); }
        }
        public ICollectionView customerView;
        public ICollectionView CustomerView
        {
            get { return customerView; }
            set { customerView = value; RaisePropertyChanged(() => CustomerView); }
        }
        #endregion

        #region EmployeeCollection
        private ObservableCollection<Employee> _employeeCollection;
        public ObservableCollection<Employee> EmployeeCollection
        {
            get { return _employeeCollection; }
            set { _employeeCollection = value; if(EmployeeView!=null)EmployeeView.Refresh(); }
        }
        public ICollectionView employeeView;
        public ICollectionView EmployeeView
        {
            get { return employeeView; }
            set { employeeView = value; RaisePropertyChanged(() => EmployeeView); }
        }
        #endregion

        #region DailyConsumptionCollection
        private ObservableCollection<DailyConsumption> _dailyConsumptionCollection;
        public ObservableCollection<DailyConsumption> DailyConsumptionCollection
        {
            get { return _dailyConsumptionCollection; }
            set { _dailyConsumptionCollection = value; if (DailyConsumptionView != null)DailyConsumptionView.Refresh(); }
        }
        public ICollectionView dailyConsumptionView;
        public ICollectionView DailyConsumptionView
        {
            get { return dailyConsumptionView; }
            set { dailyConsumptionView = value; RaisePropertyChanged(() => DailyConsumptionView); }
        }
        #endregion
        

        #region Commands
        public ICommand AddCustomerDataCommand { get { return new DelegateCommand(OnAddCustomerData); } }
        public ICommand AddEmployeeDataCommand { get { return new DelegateCommand(OnAddEmployeeData); } }
        public ICommand ClearFilter { get { return new DelegateCommand(OnClearFilter); } }
        public ICommand CloseProgram { get { return new DelegateCommand(OnCloseProgram); } }
        #endregion

        #region Ctor
        public MainWindowViewModel()
        {
            initCollection();
            LoadDBData();
            //RandomizeData();
        }
        #endregion

        private void OnAddCustomerData()
        {
            Customer customer = new Customer();
            customer.setUpdateDelegate(UpdateData);
            customer.setDeleteDelegate(DeleteData);
            customer.PopEditWindowCommand.Execute(null);
        }
        private void OnAddEmployeeData()
        {
            Employee employee = new Employee();
            employee.setUpdateDelegate(UpdateData);
            employee.setDeleteDelegate(DeleteData);
            employee.PopEditWindowCommand.Execute(null);
        }
        private void OnClearFilter()
        {
            FilterString = "";
        }
        private void OnCloseProgram()
        {
            Application.Current.Shutdown();
        }

        private bool SearchFilter(object obj)
        {
            Type type = obj.GetType();
            FieldInfo[] infos = type.GetFields();
            foreach (FieldInfo info in infos)
            {
                object infoObj = info.GetValue(obj);
                if (!string.IsNullOrEmpty(infoObj.ToString()))
                {
                    if (infoObj.ToString().Contains(filterString))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void RefreshCollection()
        {
            if (EmployeeView != null) EmployeeView.Refresh();
            if (CustomerView != null) CustomerView.Refresh();
            if (DailyConsumptionView != null) DailyConsumptionView.Refresh();
            
        }
        private void initCollection()
        {
            EmployeeCollection = new ObservableCollection<Employee>();
            CustomerCollection = new ObservableCollection<Customer>();
            DailyConsumptionCollection = new ObservableCollection<DailyConsumption>();
            EmployeeView = CollectionViewSource.GetDefaultView(EmployeeCollection);
            CustomerView = CollectionViewSource.GetDefaultView(CustomerCollection);
            DailyConsumptionView = CollectionViewSource.GetDefaultView(DailyConsumptionCollection);
            EmployeeView.Filter = new Predicate<object>(SearchFilter);
            CustomerView.Filter = new Predicate<object>(SearchFilter);
            DailyConsumptionView.Filter = new Predicate<object>(SearchFilter);

            DailyConsumption dc = new DailyConsumption();
            dc.ConsumerGoods.Add("111");
            dc.ConsumerGoods.Add("222");
            DailyConsumptionCollection.Add(dc);
        }
        private void LoadDBData()
        {
            List<Employee> employeeList = DBConnection.ins().queryData<Employee>();
            foreach (Employee employee in employeeList)
            {
                employee.setDeleteDelegate(DeleteData);
                employee.setUpdateDelegate(UpdateData);
                EmployeeCollection.Add(employee);
            }
            List<Customer> customerList = DBConnection.ins().queryData<Customer>();
            foreach (Customer customer in customerList)
            {
                customer.setDeleteDelegate(DeleteData);
                customer.setUpdateDelegate(UpdateData);
                CustomerCollection.Add(customer);
            }
        }
        private void RandomizeData()
        {
            for (var i = 0; i < 20; i++)
            {
                Employee employee = new Employee(
                    //RandomHelper.RandomString(10, true),
                    RandomHelper.RandomString(10, true),
                    (GENDER_TYPE)RandomHelper.RandomInt(1,3),
                    RandomHelper.RandomString(10, true),
                    RandomHelper.RandomString(10, true),
                    RandomHelper.RandomInt(19000,50000),
                    RandomHelper.RandomNumber(0,100,2)
                    );
                employee.setDeleteDelegate(DeleteData);
                employee.setUpdateDelegate(UpdateData);
                EmployeeCollection.Add(employee);
            }

            for (var i = 0; i < 10; i++)
            {
                Customer customer = new Customer(
                    //RandomHelper.RandomString(10, true),
                    RandomHelper.RandomString(10, true),
                    (GENDER_TYPE)RandomHelper.RandomInt(1, 3),
                    RandomHelper.RandomString(10, true),
                    RandomHelper.RandomString(10, true),
                    RandomHelper.RandomInt(1000, 10000)
                    );
                customer.setDeleteDelegate(DeleteData);
                customer.setUpdateDelegate(UpdateData);
                CustomerCollection.Add(customer);
            }
        }

        private bool DeleteData(Object target)
        {
            MessageBoxResult result = MessageBox.Show("是否確定刪除?", "確認視窗", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return false;
            if (target is Customer)
            {
                CustomerCollection.Remove((Customer)target);
                DBConnection.ins().deleteData<Customer>(target);
                return true;
            }
            else if (target is Employee)
            {
                EmployeeCollection.Remove((Employee)target);
                DBConnection.ins().deleteData<Employee>(target);
                return true;
            }
            return false;
        }
        private bool UpdateData(Object target)
        {
            if (!(((Person)target).checkData()))
            {
                MessageBoxResult result = MessageBox.Show("請確實填寫資料", "確認視窗", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (target is Customer)
            {
                if (CustomerCollection.Contains((Customer)target))
                {
                    int index = CustomerCollection.IndexOf((Customer)target);
                    CustomerCollection.RemoveAt(index);
                    CustomerCollection.Insert(index, (Customer)target);
                    DBConnection.ins().updateData<Customer>(target);
                }
                else{
                    CustomerCollection.Add((Customer)target);
                    DBConnection.ins().addData<Customer>(target);
                    ((Customer)target).dbid = DBConnection.ins().getDataId<Customer>(target);
                }
                return true;
            }
            else if (target is Employee)
            {
                if (EmployeeCollection.Contains((Employee)target))
                {
                    int index = EmployeeCollection.IndexOf((Employee)target);
                    EmployeeCollection.RemoveAt(index);
                    EmployeeCollection.Insert(index, (Employee)target);
                    DBConnection.ins().updateData<Employee>(target);
                }
                else
                {
                    EmployeeCollection.Add((Employee)target);
                    DBConnection.ins().addData<Employee>(target);
                    ((Employee)target).dbid = DBConnection.ins().getDataId<Employee>(target);
                }
                return true;
            }
            return false;
        }
    }
}
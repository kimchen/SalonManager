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

namespace SalonManager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private static MainWindowViewModel instance = null;
        public static MainWindowViewModel ins()
        {
            return instance;
        }

        #region ChooseDate
        private DateTime _chooseDate;
        public DateTime ChooseDate
        {
            get { return _chooseDate; }
            set
            {
                if (_chooseDate != value)
                {
                    bool hasChangeMonth = false;
                    bool hasChangeYear = false;
                    if(_chooseDate.Month != value.Month || _chooseDate.Year != value.Year)
                      hasChangeMonth = true;
                    if (_chooseDate.Year != value.Year)
                        hasChangeYear = true;
                    _chooseDate = value;
                    RaisePropertyChanged(() => ChooseDate);
                    LoadDaliyConsumption();
                    if (hasChangeMonth)
                    {
                        LoadMonthlyConsumption();
                        LoadOtherCost();
                    }
                    if (hasChangeYear)
                        LoadYearlyConsumption();
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

        #region GoodsCollection
        private ObservableCollection<Goods> _goodsCollection;
        public ObservableCollection<Goods> GoodsCollection
        {
            get { return _goodsCollection; }
            set { _goodsCollection = value; if (GoodsView != null)GoodsView.Refresh(); }
        }
        public ICollectionView goodsView;
        public ICollectionView GoodsView
        {
            get { return goodsView; }
            set { goodsView = value; RaisePropertyChanged(() => GoodsView); }
        }
        #endregion

        #region ServiceCollection
        private ObservableCollection<Service> _serviceCollection;
        public ObservableCollection<Service> ServiceCollection
        {
            get { return _serviceCollection; }
            set { _serviceCollection = value; if (ServiceView != null)ServiceView.Refresh(); }
        }
        public ICollectionView serviceView;
        public ICollectionView ServiceView
        {
            get { return serviceView; }
            set { serviceView = value; RaisePropertyChanged(() => ServiceView); }
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

        #region MonthlyConsumptionCollection
        private ObservableCollection<DailyConsumption> _monthlyConsumptionCollection;
        public ObservableCollection<DailyConsumption> MonthlyConsumptionCollection
        {
            get { return _monthlyConsumptionCollection; }
            set { _monthlyConsumptionCollection = value; if (MonthlyConsumptionView != null)MonthlyConsumptionView.Refresh(); }
        }
        public ICollectionView monthlyConsumptionView;
        public ICollectionView MonthlyConsumptionView
        {
            get { return monthlyConsumptionView; }
            set { monthlyConsumptionView = value; RaisePropertyChanged(() => MonthlyConsumptionView); }
        }
        #endregion

        #region YearlyConsumptionCollection
        private ObservableCollection<DailyConsumption> _yearlyConsumptionCollection;
        public ObservableCollection<DailyConsumption> YearlyConsumptionCollection
        {
            get { return _yearlyConsumptionCollection; }
            set { _yearlyConsumptionCollection = value; if (YearlyConsumptionView != null)YearlyConsumptionView.Refresh(); }
        }
        public ICollectionView yearlyConsumptionView;
        public ICollectionView YearlyConsumptionView
        {
            get { return yearlyConsumptionView; }
            set { yearlyConsumptionView = value; RaisePropertyChanged(() => YearlyConsumptionView); }
        }
        #endregion

        #region OtherCostCollection
        private ObservableCollection<OtherCost> _otherCostCollection;
        public ObservableCollection<OtherCost> OtherCostCollection
        {
            get { return _otherCostCollection; }
            set { _otherCostCollection = value; if (OtherCostView != null)OtherCostView.Refresh(); }
        }
        public ICollectionView otherCostView;
        public ICollectionView OtherCostView
        {
            get { return otherCostView; }
            set { otherCostView = value; RaisePropertyChanged(() => OtherCostView); }
        }
        private int _totalOtherCost = 0;
        public int TotalOtherCost 
        {
            get { return _totalOtherCost; }
            set { _totalOtherCost = value; }
        }
        #endregion

        public Goods GetGoodsById(String id)
        {
            foreach (Goods goods in GoodsCollection)
            {
                string itemId = goods.DBID.ToString();
                if (itemId.Equals(id))
                    return goods;
            }
            return null;
        }
        public Service GetServiceById(String id)
        {
            foreach (Service service in ServiceCollection)
            {
                string itemId = service.DBID.ToString();
                if (itemId.Equals(id))
                    return service;
            }
            return null;
        }
        public Customer GetCustomerById(String id)
        {
            foreach (Customer customer in CustomerCollection)
            {
                string itemId = customer.DBID.ToString();
                if (itemId.Equals(id))
                    return customer;
            }
            return null;
        }
        public Employee GetEmployeeById(String id)
        {
            foreach (Employee employee in EmployeeCollection)
            {
                string itemId = employee.DBID.ToString();
                if (itemId.Equals(id))
                    return employee;
            }
            return null;
        }
        #region Commands
        public ICommand AddCustomerDataCommand { get { return new DelegateCommand(OnAddCustomerData); } }
        public ICommand AddEmployeeDataCommand { get { return new DelegateCommand(OnAddEmployeeData); } }
        public ICommand AddGoodsDataCommand { get { return new DelegateCommand(OnAddGoodsData); } }
        public ICommand AddServiceDataCommand { get { return new DelegateCommand(OnAddServiceData); } }
        public ICommand AddDailyConsumptionDataCommand { get { return new DelegateCommand(OnAddDailyConsumptionData); } }
        public ICommand AddOtherCostCommand { get { return new DelegateCommand(OnAddOtherCostData); } }
        
        public ICommand ClearFilter { get { return new DelegateCommand(OnClearFilter); } }
        public ICommand CloseProgram { get { return new DelegateCommand(OnCloseProgram); } }
        #endregion

        #region Ctor
        public MainWindowViewModel()
        {
            initCollection();
            LoadDBData();
            ChooseDate = System.DateTime.Now;
            //RandomizeData();
            instance = this;
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
        private void OnAddGoodsData()
        {
            Goods goods = new Goods();
            goods.setUpdateDelegate(UpdateData);
            goods.setDeleteDelegate(DeleteData);
            goods.PopEditWindowCommand.Execute(null);
        }
        private void OnAddServiceData()
        {
            Service service = new Service();
            service.setUpdateDelegate(UpdateData);
            service.setDeleteDelegate(DeleteData);
            service.PopEditWindowCommand.Execute(null);
        }
        private void OnAddDailyConsumptionData()
        {
            DailyConsumption dailyConsumption = new DailyConsumption();
            dailyConsumption.setUpdateDelegate(UpdateData);
            dailyConsumption.setDeleteDelegate(DeleteData);
            dailyConsumption.PopEditWindowCommand.Execute(null);
        }
        private void OnAddOtherCostData() {
            OtherCost otherCost = new OtherCost();
            otherCost.setUpdateDelegate(UpdateData);
            otherCost.setDeleteDelegate(DeleteData);
            otherCost.PopEditWindowCommand.Execute(null);
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
            if (MonthlyConsumptionView != null) MonthlyConsumptionView.Refresh();
            if (YearlyConsumptionView != null) YearlyConsumptionView.Refresh();
            if (GoodsView != null) GoodsView.Refresh();
            if (ServiceView != null) ServiceView.Refresh();
            if (OtherCostView != null) OtherCostView.Refresh();
        }
        private void initCollection()
        {
            EmployeeCollection = new ObservableCollection<Employee>();
            CustomerCollection = new ObservableCollection<Customer>();
            GoodsCollection = new ObservableCollection<Goods>();
            ServiceCollection = new ObservableCollection<Service>();
            DailyConsumptionCollection = new ObservableCollection<DailyConsumption>();
            MonthlyConsumptionCollection = new ObservableCollection<DailyConsumption>();
            YearlyConsumptionCollection = new ObservableCollection<DailyConsumption>();
            OtherCostCollection = new ObservableCollection<OtherCost>();

            EmployeeView = CollectionViewSource.GetDefaultView(EmployeeCollection);
            CustomerView = CollectionViewSource.GetDefaultView(CustomerCollection);
            GoodsView = CollectionViewSource.GetDefaultView(GoodsCollection);
            ServiceView = CollectionViewSource.GetDefaultView(ServiceCollection);
            DailyConsumptionView = CollectionViewSource.GetDefaultView(DailyConsumptionCollection);
            MonthlyConsumptionView = CollectionViewSource.GetDefaultView(MonthlyConsumptionCollection);
            YearlyConsumptionView = CollectionViewSource.GetDefaultView(YearlyConsumptionCollection);
            OtherCostView = CollectionViewSource.GetDefaultView(OtherCostCollection);

            EmployeeView.Filter = new Predicate<object>(SearchFilter);
            CustomerView.Filter = new Predicate<object>(SearchFilter);
            GoodsView.Filter = new Predicate<object>(SearchFilter);
            ServiceView.Filter = new Predicate<object>(SearchFilter);
            DailyConsumptionView.Filter = new Predicate<object>(SearchFilter);
            MonthlyConsumptionView.Filter = new Predicate<object>(SearchFilter);
            YearlyConsumptionView.Filter = new Predicate<object>(SearchFilter);
            OtherCostView.Filter = new Predicate<object>(SearchFilter);
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
            List<Goods> goodsList = DBConnection.ins().queryData<Goods>();
            foreach (Goods goods in goodsList)
            {
                goods.setDeleteDelegate(DeleteData);
                goods.setUpdateDelegate(UpdateData);
                GoodsCollection.Add(goods);
            }
            List<Service> serviceList = DBConnection.ins().queryData<Service>();
            foreach (Service service in serviceList)
            {
                service.setDeleteDelegate(DeleteData);
                service.setUpdateDelegate(UpdateData);
                ServiceCollection.Add(service);
            }
        }
        private void LoadDaliyConsumption()
        {
            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("year", ChooseDate.Year.ToString());
            filter.Add("month", ChooseDate.Month.ToString());
            filter.Add("day", ChooseDate.Day.ToString());
            DailyConsumptionCollection = new ObservableCollection<DailyConsumption>();
            List<DailyConsumption> dailyConsumptionList = DBConnection.ins().queryData<DailyConsumption>(filter);
            foreach (DailyConsumption dailyConsumption in dailyConsumptionList)
            {
                dailyConsumption.setDeleteDelegate(DeleteData);
                dailyConsumption.setUpdateDelegate(UpdateData);
                DailyConsumptionCollection.Add(dailyConsumption);
            }
            DailyConsumptionView = CollectionViewSource.GetDefaultView(DailyConsumptionCollection);
            DailyConsumptionView.Filter = new Predicate<object>(SearchFilter);
        }
        private void LoadMonthlyConsumption()
        {
            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("year", ChooseDate.Year.ToString());
            filter.Add("month", ChooseDate.Month.ToString());
            MonthlyConsumptionCollection = new ObservableCollection<DailyConsumption>();
            List<DailyConsumption> dailyConsumptionList = DBConnection.ins().queryData<DailyConsumption>(filter);
            int totalCost = 0;
            foreach (DailyConsumption dailyConsumption in dailyConsumptionList)
            {
                totalCost += dailyConsumption.Cost;
                MonthlyConsumptionCollection.Add(dailyConsumption);
            }
            DailyConsumption tatolConsumption = new DailyConsumption();
            tatolConsumption.DateString = "本月累計";
            tatolConsumption.Cost = totalCost;
            MonthlyConsumptionCollection.Add(tatolConsumption);
            MonthlyConsumptionView = CollectionViewSource.GetDefaultView(MonthlyConsumptionCollection);
            MonthlyConsumptionView.Filter = new Predicate<object>(SearchFilter); 
        }

        private void LoadOtherCost() {
            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("year", ChooseDate.Year.ToString());
            filter.Add("month", ChooseDate.Month.ToString());
            OtherCostCollection = new ObservableCollection<OtherCost>();
            List<OtherCost> otherCostList = DBConnection.ins().queryData<OtherCost>(filter);
            TotalOtherCost = 0;
            foreach (OtherCost otherCost in otherCostList)
            {
                TotalOtherCost += otherCost.Cost;
                otherCost.setDeleteDelegate(DeleteData);
                otherCost.setUpdateDelegate(UpdateData);
                OtherCostCollection.Add(otherCost);
            }
            OtherCostView = CollectionViewSource.GetDefaultView(OtherCostCollection);
            OtherCostView.Filter = new Predicate<object>(SearchFilter);
            RaisePropertyChanged(() => TotalOtherCost);
        }
        private void LoadYearlyConsumption()
        {
            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("year", ChooseDate.Year.ToString());
            YearlyConsumptionCollection = new ObservableCollection<DailyConsumption>();
            List<DailyConsumption> dailyConsumptionList = DBConnection.ins().queryData<DailyConsumption>(filter);
            int totalCost = 0;
            foreach (DailyConsumption dailyConsumption in dailyConsumptionList)
            {
                totalCost += dailyConsumption.Cost;
                YearlyConsumptionCollection.Add(dailyConsumption);
            }
            DailyConsumption tatolConsumption = new DailyConsumption();
            tatolConsumption.DateString = "今年累計";
            tatolConsumption.Cost = totalCost;
            YearlyConsumptionCollection.Add(tatolConsumption);
            YearlyConsumptionView = CollectionViewSource.GetDefaultView(YearlyConsumptionCollection);
            YearlyConsumptionView.Filter = new Predicate<object>(SearchFilter);
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
                    RandomHelper.RandomInt(0, 100)
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

        public bool DeleteData(Object target)
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
            }else if(target is Goods){
                GoodsCollection.Remove((Goods)target);
                DBConnection.ins().deleteData<Goods>(target);
                return true;
            }
            else if (target is Service)
            {
                ServiceCollection.Remove((Service)target);
                DBConnection.ins().deleteData<Service>(target);
                return true;
            }
            else if (target is DailyConsumption)
            {
                DailyConsumptionCollection.Remove((DailyConsumption)target);
                DBConnection.ins().deleteData<DailyConsumption>(target);
                LoadMonthlyConsumption();
                LoadYearlyConsumption();
                return true;
            }
            else if (target is OtherCost)
            {
                OtherCostCollection.Remove((OtherCost)target);
                DBConnection.ins().deleteData<OtherCost>(target);
                LoadOtherCost();
                return true;
            }
            return false;
        }
        public bool UpdateData(Object target)
        {
            if (target is Customer)
            {
                if (CustomerCollection.Contains((Customer)target))
                {
                    //int index = CustomerCollection.IndexOf((Customer)target);
                    //CustomerCollection.RemoveAt(index);
                    //CustomerCollection.Insert(index, (Customer)target);
                    CustomerView.Refresh();
                    DBConnection.ins().updateData<Customer>(target);
                }
                else{
                    CustomerCollection.Add((Customer)target);
                    int id = (int)DBConnection.ins().addData<Customer>(target);
                    ((Customer)target).dbid = id;
                }
                return true;
            }
            else if (target is Employee)
            {
                if (EmployeeCollection.Contains((Employee)target))
                {
                    //int index = EmployeeCollection.IndexOf((Employee)target);
                    //EmployeeCollection.RemoveAt(index);
                    //EmployeeCollection.Insert(index, (Employee)target);
                    EmployeeView.Refresh();
                    DBConnection.ins().updateData<Employee>(target);
                }
                else
                {
                    EmployeeCollection.Add((Employee)target);
                    int id = (int)DBConnection.ins().addData<Employee>(target);
                    ((Employee)target).dbid = id;
                }
                return true;
            }
            else if (target is Goods)
            {
                if (GoodsCollection.Contains((Goods)target))
                {
                    //int index = GoodsCollection.IndexOf((Goods)target);
                    //GoodsCollection.RemoveAt(index);
                    //GoodsCollection.Insert(index, (Goods)target);
                    GoodsView.Refresh();
                    DBConnection.ins().updateData<Goods>(target);
                }
                else
                {
                    GoodsCollection.Add((Goods)target);
                    int id = (int)DBConnection.ins().addData<Goods>(target);
                    ((Goods)target).dbid = id;
                }
                return true;
            }
            else if (target is Service)
            {
                if (ServiceCollection.Contains((Service)target))
                {
                    //int index = ServiceCollection.IndexOf((Service)target);
                    //ServiceCollection.RemoveAt(index);
                    //ServiceCollection.Insert(index, (Service)target);
                    ServiceView.Refresh();
                    DBConnection.ins().updateData<Service>(target);
                }
                else
                {
                    ServiceCollection.Add((Service)target);
                    int id = (int)DBConnection.ins().addData<Service>(target);
                    ((Service)target).dbid = id;
                }
                return true;
            }
            else if (target is DailyConsumption)
            {
                if (DailyConsumptionCollection.Contains((DailyConsumption)target))
                {
                    //int index = DailyConsumptionCollection.IndexOf((DailyConsumption)target);
                    //DailyConsumptionCollection.RemoveAt(index);
                    //DailyConsumptionCollection.Insert(index, (DailyConsumption)target);
                    DailyConsumptionView.Refresh();
                    DBConnection.ins().updateData<DailyConsumption>(target);
                }
                else
                {
                    DailyConsumptionCollection.Add((DailyConsumption)target);
                    int id = (int)DBConnection.ins().addData<DailyConsumption>(target);
                    ((DailyConsumption)target).dbid = id;
                }
                LoadMonthlyConsumption();
                LoadYearlyConsumption();
                return true;
            }
            else if (target is OtherCost)
            {
                if (OtherCostCollection.Contains((OtherCost)target))
                {
                    OtherCostView.Refresh();
                    DBConnection.ins().updateData<OtherCost>(target);
                }
                else
                {
                    OtherCostCollection.Add((OtherCost)target);
                    int id = (int)DBConnection.ins().addData<OtherCost>(target);
                    ((OtherCost)target).dbid = id;
                }
                LoadOtherCost();
                return true;
            }
            return false;
        }
    }
}
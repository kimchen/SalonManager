using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalonManager.Models
{
    class ServiceResult
    {
        public string ServiceId;
        private string serviceName = "";
        public string ServiceName
        {
            get
            {
                return serviceName;
            }
            set
            {
                serviceName = value;
            }
        }
        private int monthlyNumber = 0;
        public int MonthlyNumber
        {
            get {
                return monthlyNumber;
            }
            set {
                monthlyNumber = value;
            }
        }
        private int yearlyNumber = 0;
        public int YearlyNumber
        {
            get
            {
                return yearlyNumber;
            }
            set
            {
                yearlyNumber = value;
            }
        }
    }
}

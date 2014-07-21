using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SalonManager.Models;

namespace SalonManager.Interface
{
    interface IInfo
    {
        void setData(BaseData data);
        void onSave();
        void onCancel();
    }
}

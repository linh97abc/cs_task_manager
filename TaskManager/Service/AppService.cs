using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Base;

namespace TaskManager.Service
{
    public class AppService : BindableBase
    {
        bool isSerialOpen;

        public bool IsSerialOpen
        {
            get => isSerialOpen;
            set { isSerialOpen = value; OnPropertyChanged("IsSerialOpen"); }
        }

        static AppService Instance = new AppService();

        public static AppService GetInst() => Instance;
    }
}

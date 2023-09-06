using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TaskManager.Base;
using System.Windows;

namespace TaskManager.Model
{
    public class RTOSTaskInfo : BindableBase
    {
        string taskName;
        int prio;
        int stkUsed;
        int stkSize;
        bool isRuning;

        uint swCnt = 0;

        public uint SwCnt
        {
            set 
            {

                if (swCnt != value)
                {
                    this.IsRuning = true;
                }
                else
                {
                    this.IsRuning = false;
                }

                swCnt = value;
            }
        }

        string taskStatus;
        SolidColorBrush taskColor;
        public string TaskStatus { get => taskStatus; }
        public SolidColorBrush TaskColor { get => taskColor; }
        public string TaskName
        {
            get => taskName;
            set
            {
                taskName = value;
                OnPropertyChanged("TaskName");
            }
        }

        public int Priority
        {
            get => prio;
            set
            {
                prio = value;
                OnPropertyChanged("Priority");
            }
        }

        public int StkUsed
        {
            get => stkUsed;
            set
            {
                stkUsed = value;
                OnPropertyChanged("StkUsed");
            }
        }

        public int StkSize
        {
            get => stkSize;
            set
            {
                stkSize = value;
                OnPropertyChanged("StkSize");
            }
        }

        public bool IsRuning
        {
            get => isRuning;
            set
            {
                isRuning = value;

                if (isRuning)
                {
                    taskStatus = "Running...";
                    taskColor = Application.Current.Resources["PaletteAmberBrush"] as SolidColorBrush;
                }
                else
                {
                    taskStatus = "Idle";
                    taskColor = new SolidColorBrush();
                }

                OnPropertyChanged("TaskStatus");
                OnPropertyChanged("TaskColor");
            }
        }


    }

}

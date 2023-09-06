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
    public class SerialInfo : BindableBase
    {
        string name;
        int baudarte;
        int stkUsed;
        int stkSize;
        bool isRuning;

        string taskStatus;
        SolidColorBrush taskColor;
        public string TaskStatus { get => taskStatus; }
        public SolidColorBrush TaskColor { get => taskColor; }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Baudrate
        {
            get => baudarte;
            set
            {
                baudarte = value;
                OnPropertyChanged("Baudrate");
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

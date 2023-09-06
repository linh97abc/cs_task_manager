using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TaskManager.CustomControl;
using TaskManager.Service;
using TaskManager.ViewModel;
using Wpf.Ui.Mvvm.Interfaces;

namespace TaskManager.Pages
{
    /// <summary>
    /// Interaction logic for HardwareMonitor.xaml
    /// </summary>
    public partial class HardwareMonitor
    {
        HardwareMonitorViewModel viewModel;
        LiveChart[] errChart;
        PV_SP_LiveChart[] posChart;

        float[] mockPV = new float[4];

        int packetSeq = 0;
        public HardwareMonitor()
        {
            this.viewModel = new HardwareMonitorViewModel();
            this.DataContext = this.viewModel;
            InitializeComponent();

            errChart = new LiveChart[]
            {
                this.errServo1,
                this.errServo2,
                this.errServo3,
                this.errServo4,
            };

            posChart = new PV_SP_LiveChart[]
            {
                this.posServo1,
                this.posServo2,
                this.posServo3,
                this.posServo4,
            };

            this.errServo1.YOffset = 50;
            this.errServo2.YOffset = 50;
            this.errServo3.YOffset = 50;
            this.errServo4.YOffset = 50;

            foreach (var item in this.viewModel.SetPointView)
            {
                item.PropertyChanged += OnViewModelPropertyChanged;
            }

        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "PV") return;

            TaskManager.Model.ServoProp servoProp = sender as TaskManager.Model.ServoProp;

            var index = servoProp.ServoID - 1;
            errChart[index].AddValue((int)(servoProp.PV - servoProp.SP));
            posChart[index].AddValue((int)servoProp.SP - 50, (int)servoProp.PV - 50);
            
        }

        private void Button_Run_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.IsBtnRunning = !this.viewModel.IsBtnRunning;
        }
    }
}

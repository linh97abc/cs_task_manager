using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Interop;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        ObservableCollection<string> _serialPorts;
        List<int> _listBaudrate;
        TaskManager.Service.AppService _appservice;
        public ObservableCollection<string> SerialPorts { get => _serialPorts; }
        public List<int> ListBaudrate { get => _listBaudrate; }

        string comport;
        public string ComPort
        {
            get => comport;
            set
            {
                comport = value;


                if (this.btnConnect != null)
                {
                    
                    this.btnConnect.IsEnabled = ((comport != null) && (comport != string.Empty));
                }
            }
        }
        public int Baudrate { get; set; }

        //ThemeType theme;
        public MainWindow()
        {
            this._serialPorts = new ObservableCollection<string>();
            this._listBaudrate = new List<int>();
            this._listBaudrate.Add(921600);
            this._listBaudrate.Add(115200);

            this._appservice = TaskManager.Service.AppService.GetInst();


            this.DataContext = this;
            InitializeComponent();

            Wpf.Ui.Appearance.Watcher.Watch(this);

            var listPort = System.IO.Ports.SerialPort.GetPortNames();

            foreach (var port in listPort)
            {

                this._serialPorts.Add(port);
            }

            //RootNavigation.SelectedPageIndex = 0;
        }

        private void RootNavigation_OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(
                $"DEBUG | WPF UI Navigated to: {sender?.Current ?? null}",
                "Wpf.Ui.Demo"
            );


        }

        private void NavigationButtonTheme_OnClick(object sender, RoutedEventArgs e)
        {
            var theme = Wpf.Ui.Appearance.Theme.GetAppTheme();
            Wpf.Ui.Appearance.Theme.Apply( (theme == ThemeType.Dark)? ThemeType.Light: ThemeType.Dark);
        }

        private void UiWindow_Closed(object sender, EventArgs e)
        {
            Service.MavlinkService.GetInst().ClosePort();
            //base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(ComPort);

            var service = Service.MavlinkService.GetInst();

            if (this.btnConnect.IsChecked == true)
            {
                service.ClosePort();
                this.btnConnect.Content = "Connect";
                this._appservice.IsSerialOpen = false;
            }
            else
            {
                service.OpenPort(ComPort, Baudrate);
                this.btnConnect.Content = "Disconnect";
                this._appservice.IsSerialOpen = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TaskManager.ViewModel;

namespace TaskManager.Pages
{

    /// <summary>
    /// Interaction logic for Configure.xaml
    /// </summary>
    public partial class Configure
    {
        ConfigureViewModel ViewModel { get; }
        public Configure()
        {
            this.ViewModel = new ConfigureViewModel();
            this.DataContext = this.ViewModel;

            InitializeComponent();

        }

        private void Button_Load_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.LoadConfig();
        }

        private void Button_Config_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.SaveConfig();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using TaskManager.ViewModel;
using TaskManager.Model;

namespace TaskManager.Pages
{
    
    /// <summary>
    /// Interaction logic for TaskManager.xaml
    /// </summary>
    public partial class TaskManagerPage
    {
        TaskManagerViewModel viewModel;

        public TaskManagerPage()
        {
            viewModel = new TaskManagerViewModel();
            this.DataContext = viewModel;
            InitializeComponent();

            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
            
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CpuUsage")
            {
                this.cpuUsageChart.AddValue(viewModel.CpuUsage);
            }
        }


    }
}

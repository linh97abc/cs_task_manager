using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using TaskManager.Base;
using TaskManager.Model;

namespace TaskManager.ViewModel
{
    public class SeialManagerViewModel
    {

        List<SerialInfo> serials;

        public List<SerialInfo> Serials { get => serials; set => serials = value; }

        public SeialManagerViewModel()
        {
            this.serials = new List<SerialInfo>();
            this.serials.Add(new SerialInfo()
            {
                Name = "RS485",
                Baudrate = 921600
            });
        }
    }
}

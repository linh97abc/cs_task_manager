using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TaskManager.Base;
using TaskManager.Service;
using TaskManager.Model;
using Wpf.Ui.Mvvm.Interfaces;

namespace TaskManager.ViewModel
{
    public class HardwareMonitorViewModel : BindableBase
    {


        ObservableCollection<ServoProp> servoProp;
        public ObservableCollection<ServoProp> SetPointView { get => servoProp; }

        bool isBtnRunningChecked = false;
        bool isBtnRunningEnable = false;

        public string BtnRunningStatus
        {
            get
            {
                if (isBtnRunningChecked)
                {
                    return "Stop";
                }
                else
                { return "Start"; }
            }
        }

        public string BtnRunningApperance
        {
            get
            {
                if (!isBtnRunningChecked)
                {
                    return "Primary";
                }
                else
                {
                    return "Caution";
                }
            }
        }

        public bool IsBtnRunning
        {
            get => isBtnRunningChecked;
            set
            {
                isBtnRunningChecked = value;

                if (value)
                {
                    timer.Start();

                }
                else
                {
                    timer.Stop();
                }

                OnPropertyChanged("BtnRunningStatus");
                OnPropertyChanged("BtnRunningApperance");
            }
        }

        public bool IsBtnRunningEnable
        {
            get => isBtnRunningEnable;
            set
            {
                isBtnRunningEnable = value;

                OnPropertyChanged("IsBtnRunningEnable");
            }
        }

        DispatcherTimer timer;

        public HardwareMonitorViewModel()
        {

            this.servoProp = new ObservableCollection<ServoProp>(new ServoProp[]
            {
                new ServoProp() {ServoID = 1},
                new ServoProp() {ServoID = 2},
                new ServoProp() {ServoID = 3},
                new ServoProp() {ServoID = 4},
            });

            IsBtnRunningEnable = Service.AppService.GetInst().IsSerialOpen;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;

            Service.MavlinkService.GetInst().OnMavlinkReceived += OnMavlinkReceived;
            Service.AppService.GetInst().PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "IsSerialOpen")
                {
                    var _appservice = sender as Service.AppService;
                    IsBtnRunningEnable = _appservice.IsSerialOpen;
                }
            };

        }

        Byte pos_mon_seq = 0;

        void timer_Tick(object sender, EventArgs e)
        {

            uint[] sp = new uint[4];


            int i = 0;
            foreach (var item in servoProp)
            {
                sp[i] = (uint)item.SP;
                i++;
            }


            var msg = new MAVLink.mavlink_pos_mon_t(
                sp
                , pos_mon_seq, 1);

            pos_mon_seq++;

            MavlinkService.GetInst().SendMsg(MAVLink.MAVLINK_MSG_ID.POS_MON, msg);

        }

        int packetSeq = 0;
        void OnMavlinkReceived(object sender, MAVLink.MAVLinkMessage msg)
        {
            if (msg.msgid == (uint)MAVLink.MAVLINK_MSG_ID.POS_MON)
            {
                var packet = msg.ToStructure<MAVLink.mavlink_pos_mon_t>();

                App.Current.Dispatcher.Invoke(() =>
                {

                    while (this.packetSeq != packet.seq)
                    {
                        int i = 0;

                        foreach (var item in servoProp)
                        {
                            item.PV = packet.pos[i];
                            i++;

                        }

                        this.packetSeq++;
                        this.packetSeq &= 0xFF;
                    }
                });

            }
        }
    }

}

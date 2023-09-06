using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using TaskManager.Base;
using TaskManager.Model;
using System.Windows.Threading;
using Wpf.Ui.Mvvm.Interfaces;
using System.Collections;

namespace TaskManager.ViewModel
{
    public class TaskManagerViewModel : BindableBase
    {

        int cpuUsage = 0;
        ObservableCollection<RTOSTaskInfo> tasks = new ObservableCollection<RTOSTaskInfo>();

        public int CpuUsage
        {
            get => cpuUsage;
            set
            {
                cpuUsage = value;
                OnPropertyChanged("CpuUsage");
            }
        }

        public ObservableCollection<RTOSTaskInfo> TaskInfos { get => tasks; set => tasks = value; }

        public TaskManagerViewModel()
        {
            Service.MavlinkService.GetInst().OnMavlinkReceived += OnMavlinkReceived;
        }

        int packetSeq = 0;
        void OnMavlinkReceived(object sender, MAVLink.MAVLinkMessage msg)
        {
            if (msg.msgid == (uint)MAVLink.MAVLINK_MSG_ID.CPU_USAGE)
            {
                var packet = msg.ToStructure<MAVLink.mavlink_cpu_usage_t>();

                App.Current.Dispatcher.Invoke(() =>
                {

                    while (this.packetSeq != packet.seq)
                    {
                        this.CpuUsage = packet.cpu_usage;

                        this.packetSeq++;
                        this.packetSeq &= 0xFF;
                    }
                });

            }
            else if (msg.msgid == (uint)MAVLink.MAVLINK_MSG_ID.TASK_MANAGER)
            {
                var packet = msg.ToStructure<MAVLink.mavlink_task_manager_t>();
                
                App.Current.Dispatcher.Invoke(() =>
                {
                    bool hasTask = false;
                    var taskName = System.Text.Encoding.UTF8.GetString(packet.task_name);

                    foreach (var item in this.TaskInfos)
                    {
                        
                        if (item.TaskName == taskName)
                        {
                            item.Priority = packet.prio;
                            item.SwCnt = packet.sw_cnt;
                            item.StkUsed = (int)packet.stk_used;
                            item.StkSize = (int)packet.stk_size;

                            hasTask = true;
                            break;
                        }
                    }

                    if (!hasTask)
                    {
                        this.TaskInfos.Add(new RTOSTaskInfo()
                        {
                            TaskName = taskName,
                            Priority = packet.prio,
                            StkSize = (int)packet.stk_size,
                            StkUsed = (int)packet.stk_used,
                        });
                    }
                });
            }
        }
    }

}

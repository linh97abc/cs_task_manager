using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using TaskManager.Base;
using TaskManager.Model;
using TaskManager.Service;

namespace TaskManager.ViewModel
{
    internal class ConfigureViewModel
    {

        List<ServoConfig> servoConfigs;

        public List<ServoConfig> ServoConfigs
        {
            get => servoConfigs;
        }

        public ConfigureViewModel()
        {
            this.servoConfigs = new List<ServoConfig>();
            this.servoConfigs.Add(new ServoConfig() { ServoID = 1 });
            this.servoConfigs.Add(new ServoConfig() { ServoID = 2 });
            this.servoConfigs.Add(new ServoConfig() { ServoID = 3 });
            this.servoConfigs.Add(new ServoConfig() { ServoID = 4 });

            _mavlinkService = Service.MavlinkService.GetInst();
            _mavlinkService.OnMavlinkReceived += OnMavlinkReceived;
        }

        private void OnMavlinkReceived(object sender, MAVLink.MAVLinkMessage msg)
        {
            if (msg.msgid == (uint)MAVLink.MAVLINK_MSG_ID.CONFIG)
            {
                var packet = msg.ToStructure<MAVLink.mavlink_config_t>();

                App.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var item in servoConfigs)
                    {
                        var index = item.ServoID - 1;

                        item.Kp = packet.Kp[index];
                        item.Ki = packet.Ki[index];
                        item.Kd = packet.Kd[index];
                    }
                });

            }
        }

        Service.MavlinkService _mavlinkService;
        public void LoadConfig()
        {
            var seq = _mavlinkService.MakeACKSeq();
            var msg = new MAVLink.mavlink_get_conf_t(seq);

            _mavlinkService.SendMsgWithACK(MAVLink.MAVLINK_MSG_ID.GET_CONF, seq, msg);
        }

        public void SaveConfig()
        {
            var seq = _mavlinkService.MakeACKSeq();
            float[] kp = new float[4];
            float[] ki = new float[4]; ;
            float[] kd = new float[4]; ;

            foreach (var item in servoConfigs)
            {
                kp[item.ServoID - 1] = item.Kp;
                ki[item.ServoID - 1] = item.Ki;
                kd[item.ServoID - 1] = item.Kd;
            }

            var msg = new MAVLink.mavlink_config_t(kp, ki, kd, seq);

            _mavlinkService.SendMsgWithACK(MAVLink.MAVLINK_MSG_ID.CONFIG, seq, msg);
        }
    }
}

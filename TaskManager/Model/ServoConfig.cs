using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Base;

namespace TaskManager.Model
{
    public class ServoConfig : BindableBase
    {
        int servoID;
        float kp;
        float ki;
        float kd;

        public int ServoID
        {
            get { return servoID; }
            set { servoID = value; OnPropertyChanged("ServoName"); }
        }

        public string ServoName
        {
            get { return "Servo " + servoID; }
        }

        public float Kp
        {
            get { return kp; }
            set { kp = value; OnPropertyChanged("Kp"); }
        }

        public float Ki
        {
            get { return ki; }
            set { ki = value; OnPropertyChanged("Ki"); }
        }

        public float Kd
        {
            get { return kd; }
            set { kd = value; OnPropertyChanged("Kd"); }
        }

        public ServoConfig()
        {
            kp = 0; ki = 0; kd = 0;
        }
    }

}

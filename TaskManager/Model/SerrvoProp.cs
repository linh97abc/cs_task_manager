using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Base;

namespace TaskManager.Model
{
    public class ServoProp : BindableBase
    {
        int servoID;
        float pv;
        float sp = 50;

        public float SP
        {
            get { return sp; }
            set { sp = value; OnPropertyChanged("SP"); }
        }

        public float PV
        {
            get { return pv; }
            set { pv = value; OnPropertyChanged("PV"); }
        }

        public string ServoName
        {
            get
            {
                return "Servo " + servoID;
            }
        }

        public int ServoID
        {
            get { return servoID; }
            set { servoID = value; OnPropertyChanged("ServoName"); }
        }

    }
}

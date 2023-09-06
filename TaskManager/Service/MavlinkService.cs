using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Windows.Interop;

namespace TaskManager.Service
{
    internal class MavlinkService
    {
        static MavlinkService Instance = new MavlinkService();
        MAVLink.MavlinkParse mavlinkParse;
        CancellationTokenSource tokenMavlinkService = new CancellationTokenSource();



        public delegate void MavlinkParseEventHandler(object sender, MAVLink.MAVLinkMessage msg);
        public delegate void MavlinkACKEventHandler(object sender, uint seq);

        public MavlinkParseEventHandler OnMavlinkReceived { get; set; }
        public MavlinkACKEventHandler OnACKReceived { get; set; }

        SerialPort ser;
        MavlinkService()
        {
            this.ser = new SerialPort();
            mavlinkParse = new MAVLink.MavlinkParse();
            this.rawSendMsg = new Queue<RawSendMsg_t> { };
            OnMavlinkReceived += new MavlinkParseEventHandler((object sender, MAVLink.MAVLinkMessage msg) => { });
            OnACKReceived += new MavlinkACKEventHandler((object sender, uint seq) => {
                try
                {
                    var rawMsg = this.rawSendMsg.Peek();
                    if (rawMsg.seq == seq)
                    {
                        this.rawSendMsg.Dequeue();
                    }

                }
                catch (Exception)
                {
                }
            });
        }



        static public MavlinkService GetInst()
        {
            return Instance;
        }

        bool IsOpen()
        {
            return this.ser.IsOpen;
        }
        public void OpenPort(string portName, int baudrate)
        {
            if (this.ser.IsOpen)
            {
                this.ser.Close();
            }

            this.ser.BaudRate = baudrate;
            this.ser.PortName = portName;

            this.ser.Open();


            tokenMavlinkService = new CancellationTokenSource();
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {

                while (true)
                {
                    MAVLink.MAVLinkMessage mavlinkMsg = null;

                    try
                    {
                        mavlinkMsg = mavlinkParse.ReadPacket(ser.BaseStream);
                    }
                    catch {
                        break;

                    }

                    if (mavlinkMsg != null)
                    {
                        if (mavlinkMsg.msgid == (uint)MAVLink.MAVLINK_MSG_ID.ACK)
                        {
                            var packet = mavlinkMsg.ToStructure<MAVLink.mavlink_ack_t>();
                            OnACKReceived(this, packet.seq);
                        }

                        OnMavlinkReceived(this, mavlinkMsg);


                    }

                }




            }, tokenMavlinkService.Token);
        }

        public void ClosePort()
        {
            if (this.ser != null && this.ser.IsOpen)
            {
                tokenMavlinkService.Cancel();
                this.ser.Close();
                tokenMavlinkService.Dispose();

            }
        }

        public void SendMsg(MAVLink.MAVLINK_MSG_ID msgID, object indata)
        {
            byte[] raw_bytes = mavlinkParse.GenerateMAVLinkPacket10(msgID, indata);
            this.ser.Write(raw_bytes, 0, raw_bytes.Length);
        }


        struct RawSendMsg_t
        {
            public byte[] rawSendMsg;
            public uint seq;
        }

        Queue<RawSendMsg_t> rawSendMsg;

        byte _ackSeq = 0;
        public byte MakeACKSeq()
        {
            return _ackSeq++;
        }
        public async void SendMsgWithACK(MAVLink.MAVLINK_MSG_ID msgID, byte seq, object indata)
        {

            var raw_bytes = new RawSendMsg_t()
            {
                rawSendMsg = mavlinkParse.GenerateMAVLinkPacket10(msgID, indata),
                seq = seq,
            };


            rawSendMsg.Enqueue(raw_bytes);

            RawSendMsg_t rawMsg;

            do
            {
                try
                {
                    rawMsg = this.rawSendMsg.Peek();
                    this.ser.Write(rawMsg.rawSendMsg, 0, rawMsg.rawSendMsg.Length);

                }
                catch (Exception)
                {
                    this.rawSendMsg.Clear();
                    return;
                }

                await Task.Delay(100);

            } while (rawMsg.seq == seq);


        }
    }
}

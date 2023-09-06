using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

public partial class MAVLink
{
    public const string MAVLINK_BUILD_DATE = "Thu Sep 07 2023";
    public const string MAVLINK_WIRE_PROTOCOL_VERSION = "1.0";
    public const int MAVLINK_MAX_PAYLOAD_LEN = 49;

    public const byte MAVLINK_CORE_HEADER_LEN = 9;///< Length of core header (of the comm. layer)
    public const byte MAVLINK_CORE_HEADER_MAVLINK1_LEN = 5;///< Length of MAVLink1 core header (of the comm. layer)
    public const byte MAVLINK_NUM_HEADER_BYTES = (MAVLINK_CORE_HEADER_LEN + 1);///< Length of all header bytes, including core and stx
    public const byte MAVLINK_NUM_CHECKSUM_BYTES = 2;
    public const byte MAVLINK_NUM_NON_PAYLOAD_BYTES = (MAVLINK_NUM_HEADER_BYTES + MAVLINK_NUM_CHECKSUM_BYTES);

    public const int MAVLINK_MAX_PACKET_LEN = (MAVLINK_MAX_PAYLOAD_LEN + MAVLINK_NUM_NON_PAYLOAD_BYTES + MAVLINK_SIGNATURE_BLOCK_LEN);///< Maximum packet length
    public const byte MAVLINK_SIGNATURE_BLOCK_LEN = 13;

    public const int MAVLINK_LITTLE_ENDIAN = 1;
    public const int MAVLINK_BIG_ENDIAN = 0;

    public const byte MAVLINK_STX = 0xFD;

    public const byte MAVLINK_STX_MAVLINK1 = 0xFE;

    public const byte MAVLINK_ENDIAN = MAVLINK_LITTLE_ENDIAN;

    public const bool MAVLINK_ALIGNED_FIELDS = (1 == 1);

    public const byte MAVLINK_CRC_EXTRA = 1;
    
    public const byte MAVLINK_COMMAND_24BIT = 0;
        
    public const bool MAVLINK_NEED_BYTE_SWAP = (MAVLINK_ENDIAN == MAVLINK_LITTLE_ENDIAN);
        
    // msgid, name, crc, minlength, length, type
    public static message_info[] MAVLINK_MESSAGE_INFOS = new message_info[] {
        new message_info(0, "ACK", 72, 1, 1, typeof( mavlink_ack_t )), // none 24 bit
        new message_info(1, "POS_MON", 155, 18, 18, typeof( mavlink_pos_mon_t )), // none 24 bit
        new message_info(2, "CPU_USAGE", 160, 2, 2, typeof( mavlink_cpu_usage_t )), // none 24 bit
        new message_info(3, "TASK_MANAGER", 61, 45, 45, typeof( mavlink_task_manager_t )), // none 24 bit
        new message_info(4, "COM_MANAGER", 239, 12, 12, typeof( mavlink_com_manager_t )), // none 24 bit
        new message_info(32, "CONFIG", 107, 49, 49, typeof( mavlink_config_t )), // none 24 bit
        new message_info(33, "GET_CONF", 149, 1, 1, typeof( mavlink_get_conf_t )), // none 24 bit
        new message_info(34, "CONTROL", 99, 2, 2, typeof( mavlink_control_t )), // none 24 bit

    };

    public const byte MAVLINK_VERSION = 2;

    public const byte MAVLINK_IFLAG_SIGNED=  0x01;
    public const byte MAVLINK_IFLAG_MASK   = 0x01;

    public struct message_info
    {
        public uint msgid { get; internal set; }
        public string name { get; internal set; }
        public byte crc { get; internal set; }
        public uint minlength { get; internal set; }
        public uint length { get; internal set; }
        public Type type { get; internal set; }

        public message_info(uint msgid, string name, byte crc, uint minlength, uint length, Type type)
        {
            this.msgid = msgid;
            this.name = name;
            this.crc = crc;
            this.minlength = minlength;
            this.length = length;
            this.type = type;
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}",name,msgid);
        }
    }   

    public enum MAVLINK_MSG_ID 
    {

        ACK = 0,
        POS_MON = 1,
        CPU_USAGE = 2,
        TASK_MANAGER = 3,
        COM_MANAGER = 4,
        CONFIG = 32,
        GET_CONF = 33,
        CONTROL = 34,
    }
    
    
    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=18)]
    ///<summary>  </summary>
    public struct mavlink_pos_mon_t
    {
        public mavlink_pos_mon_t(uint[] pos,byte seq,byte isRunning) 
        {
              this.pos = pos;
              this.seq = seq;
              this.isRunning = isRunning;
            
        }
        /// <summary>   </summary>
        [Units("")]
        [Description("")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=4)]
		public uint[] pos;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  byte seq;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  byte isRunning;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=2)]
    ///<summary>  </summary>
    public struct mavlink_cpu_usage_t
    {
        public mavlink_cpu_usage_t(byte seq,byte cpu_usage) 
        {
              this.seq = seq;
              this.cpu_usage = cpu_usage;
            
        }
        /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  byte seq;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  byte cpu_usage;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=45)]
    ///<summary>  </summary>
    public struct mavlink_task_manager_t
    {
        public mavlink_task_manager_t(uint stk_used,uint stk_size,uint sw_cnt,byte[] task_name,byte prio) 
        {
              this.stk_used = stk_used;
              this.stk_size = stk_size;
              this.sw_cnt = sw_cnt;
              this.task_name = task_name;
              this.prio = prio;
            
        }
        /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  uint stk_used;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  uint stk_size;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  uint sw_cnt;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=32)]
		public byte[] task_name;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  byte prio;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=12)]
    ///<summary>  </summary>
    public struct mavlink_com_manager_t
    {
        public mavlink_com_manager_t(uint baudrate,uint send,uint recv) 
        {
              this.baudrate = baudrate;
              this.send = send;
              this.recv = recv;
            
        }
        /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  uint baudrate;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  uint send;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  uint recv;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=49)]
    ///<summary>  </summary>
    public struct mavlink_config_t
    {
        public mavlink_config_t(float[] Kp,float[] Ki,float[] Kd,byte seq) 
        {
              this.Kp = Kp;
              this.Ki = Ki;
              this.Kd = Kd;
              this.seq = seq;
            
        }
        /// <summary>   </summary>
        [Units("")]
        [Description("")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=4)]
		public float[] Kp;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=4)]
		public float[] Ki;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=4)]
		public float[] Kd;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  byte seq;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=1)]
    ///<summary>  </summary>
    public struct mavlink_get_conf_t
    {
        public mavlink_get_conf_t(byte seq) 
        {
              this.seq = seq;
            
        }
        /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  byte seq;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=2)]
    ///<summary>  </summary>
    public struct mavlink_control_t
    {
        public mavlink_control_t(byte seq,byte start) 
        {
              this.seq = seq;
              this.start = start;
            
        }
        /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  byte seq;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  byte start;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=1)]
    ///<summary>  </summary>
    public struct mavlink_ack_t
    {
        public mavlink_ack_t(byte seq) 
        {
              this.seq = seq;
            
        }
        /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  byte seq;
    
    };

}

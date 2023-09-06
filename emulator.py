import mavlink
import serial
import time
import threading
from typing import Any, Callable, Dict, Iterable, List, Mapping, Optional, Sequence, Tuple, Type, Union, cast
import random

class MavlinkFile():
    def __init__(self, ser: serial.Serial):
        self.ser = ser

    def write(self, buff):
        print([i for i in buff])
        self.ser.write(buff)
    
    def read(self):
        return self.ser.read_all()

mavlinkFile = MavlinkFile(serial.Serial("COM2", baudrate=115200))
# ser1 = serial.Serial("COM1", baudrate=115200)

mav = mavlink.MAVLink(mavlinkFile)


def OnMavlinkReceived(msgs: List[mavlink.MAVLink_message]):
    for msg in msgs:
        if msg.get_msgId() == mavlink.MAVLINK_MSG_ID_POS_MON:
            return True
        
        if msg.get_msgId() >= 32:
            msgACK = mavlink.MAVLink_ack_message(msg.to_dict()['seq'])
            mav.send(msgACK)

        if msg.get_msgId() == mavlink.MAVLINK_MSG_ID_GET_CONF:
            sendmsg = mavlink.MAVLink_config_message(seq=0, Kp=(12, 1, 2, 3), Ki=(3, 4, 5, 3), Kd=(8, 1, 2.3, 4))
            mav.send(sendmsg)

        print(msg.get_msgId())
        # print(msg.to_dict())
    
    return False

while True:
    buff = mav.file.read()
    msgs = mav.parse_buffer(buff)

    if msgs is None: continue

    if OnMavlinkReceived(msgs):  break
        
    time.sleep(0.01)


seq = 0
while True:
    seq += 1
    seq &= 0xFF
    msg = mavlink.MAVLink_pos_mon_message(seq, 1, (random.randint(0, 100), random.randint(0, 100), random.randint(0, 100), random.randint(0, 100)))
    mav.send(msg)

    msg = mavlink.MAVLink_cpu_usage_message(seq, random.randint(0, 100))
    mav.send(msg)

    msg = mavlink.MAVLink_task_manager_message(b"Task_Idle", 10, random.randint(0, 100), 1024, 5)
    mav.send(msg)

    for taskid in range(5):

        msg = mavlink.MAVLink_task_manager_message(f"Task {taskid}".encode(), taskid, random.randint(0, 100), 1024, random.randint(0, 100))
        mav.send(msg)
    time.sleep(0.1)
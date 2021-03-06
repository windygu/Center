﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteInterface.MFCC
{
    public   interface I_MFCC_MAS : RemoteInterface.MFCC.I_MFCC_Base
    {
    //    void SendDisplay(string devName, byte boardid,  string mesg, byte color);
    //    //g_code_id =0 文字模式, g_code_id=1 圖形模式
       
    //    void setDisplayOff(string devName);
    //    void setDisplayOff(string devName,byte boardid);
    //    void GetCurrentTCDisplay(string devName,byte boardid,  ref string mesg, ref byte color);
    //        //color length= mesg length execept '\r'
        //g_code_id =0 文字模式, g_code_id=1 圖形模式
        void SetDisplayOff(string devName);
        void SetDisplayOff(string devName,byte laneid);
        void SendDisplay(string devName,int laneid, int g_code_id, int hor_space, string mesgs, byte[] colors, byte[] vspaces);
        void SendDisplay(string devName, int laneid, int speedlimit);
        void SendDisplay(string devName, int laneid, int g_code_id, int hor_space, string mesgs, byte[] colors);

    }
}

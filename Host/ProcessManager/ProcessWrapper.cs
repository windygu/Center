﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessManager
{


   public  class ProcessWrapper
    {

     public   string PName;
     public   int ConsolePort;
     //  public int pid;
     public   string ExecutingStr;
     public string argument;
     public string Pdesc;
     public   System.Diagnostics.Process Process;
     public int Startcnt = 0;
       public int state = 1;  //1:start,0:stop,2:pause
       public bool bManual = false;
     

    public   ProcessWrapper(string pname, int consolePort, string executingStr,string argument, string pdesc)
       {
         this.PName=pname;
         this.ConsolePort = consolePort;
         this.ExecutingStr = executingStr;
         this.argument = argument.Trim();
         this.Pdesc = pdesc;
         
       }

       public int pid
       {
           get
           {
               return Process.Id;
           }
       }






    }
}

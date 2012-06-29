using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monitor.WSS.Console
{
    class Program
    {
        //static delegate void DelegateTimerTicked(TimerTicked);

        static int timerAndPingTimeout  = 3000;

        static void Log(string s)
        {
            Log(s, false);
        }
        static void Log(string s, bool isError)
        {
            if (isError)
                System.Console.ForegroundColor = System.ConsoleColor.Red;
            else
                System.Console.ForegroundColor = System.ConsoleColor.Yellow;

            System.Console.WriteLine(s);
        }
        static void Main(string[] args)
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);


            TimerCallback tcb = TimerTicked;

            Timer t = new Timer(tcb, autoEvent, 0, timerAndPingTimeout);

            //keep app open
            System.Console.ReadLine();

        }

        protected static void TimerTicked(object state)
        {
            MonitorStorageServer("192.168.1.116");
        }

        protected static void MonitorStorageServer(string address)
        {          
            Ping pingSender = new Ping();

            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            //set options
            PingOptions options = new PingOptions(64, true);

            //send ping (async)
            pingSender.SendAsync(address, timerAndPingTimeout, buffer, options, address);



        }
        public static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Log("Ping canceled.");
                return;
            }

            
            if (e.Error != null)
            {
                Log("Ping failed:");
                Log(e.Error.ToString());
                return;
            }
            
            PingReply reply = e.Reply;

            DisplayReply(reply,e.UserState.ToString());
        }

        public static void DisplayReply(PingReply reply, string address)
        {
            if (reply == null)
                return;

            if (reply.Status == IPStatus.Success)
            {
                Log(String.Format("Reply from {0} with resonse time {1}", 
                    reply.Address.ToString(), 
                    reply.RoundtripTime.ToString()
                    ));
            }
            else
            {
                string add = address;
                string stat = reply.Status.ToString();
                string fail = String.Format("Failed Response to {0} - {1}", add, stat);
                Log(fail, true);
            }
        }

    }
}

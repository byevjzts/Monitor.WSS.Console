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
        static void Log(string s)
        {
            
            System.Console.WriteLine(s);
        }
        static void Main(string[] args)
        {
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
            MonitorStorageServer("192.168.0.1");
        }

        protected static void MonitorStorageServer(string address)
        {
            AutoResetEvent waiter = new AutoResetEvent(false);
            Ping pingSender = new Ping();

            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            // Wait 12 seconds for a reply.
            int timeout = 12000;

            // Set options for transmission:
            // The data can go through 64 gateways or routers
            // before it is destroyed, and the data packet
            // cannot be fragmented.
            PingOptions options = new PingOptions(64, true);

            // Send the ping asynchronously.
            // Use the waiter as the user token.
            // When the callback completes, it can wake up this thread.
            pingSender.SendAsync(address, timeout, buffer, options, waiter);

            // Prevent this example application from ending.
            // A real application should do something useful
            // when possible.
            waiter.WaitOne();
            Log("Ping example completed.");
            System.Console.ReadLine();
        }
        public static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            // If the operation was canceled, display a message to the user.
            if (e.Cancelled)
            {
                Log("Ping canceled.");

                // Let the main thread resume. 
                // UserToken is the AutoResetEvent object that the main thread 
                // is waiting for.
                ((AutoResetEvent)e.UserState).Set();
            }

            // If an error occurred, display the exception to the user.
            if (e.Error != null)
            {
                Log("Ping failed:");
                Log(e.Error.ToString());

                // Let the main thread resume. 
                ((AutoResetEvent)e.UserState).Set();
            }

            PingReply reply = e.Reply;

            DisplayReply(reply);

            // Let the main thread resume.
            ((AutoResetEvent)e.UserState).Set();
        }

        public static void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            Log(String.Format("ping status: {0}", reply.Status));
            if (reply.Status == IPStatus.Success)
            {
                Log(String.Format("Address: {0}", reply.Address.ToString()));
                Log(String.Format("RoundTrip time: {0}", reply.RoundtripTime));
                Log(String.Format("Time to live: {0}", reply.Options.Ttl));
                Log(String.Format("Don't fragment: {0}", reply.Options.DontFragment));
                Log(String.Format("Buffer size: {0}", reply.Buffer.Length));
            }
            else
            {
                //failed
            }
        }

    }
}

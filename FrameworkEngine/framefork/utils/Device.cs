

using System.IO.Ports;
using System.Threading;
using System;

namespace Bubla
{
    public class Device
    {
        private static SerialPort serialPort;
        private static Thread threadRead = null;
        private static bool connectNow = false;
        private static bool connect = false;
        private static bool read = false;
        private static string readLine = "";

        private static string com = "";

        public static string[] GetComs()
        {
            return SerialPort.GetPortNames();
        }

        public static string Com
        {
            get { return com; }
            set { com = value; }
        }

        public static bool IsConnect()
        {
            return connect;
        }

        public static void Connect()
        {
            if (connectNow) return;
            if (serialPort != null) serialPort.Close();
            read = false;

            Console.WriteLine("wait connect");

            connectNow = true;
            Thread threadWait = null;
            threadWait = new Thread(() =>
            {
                while (com.Equals("")) ;
                threadWait.Join(1000);
                Game.EventConnectDevice();
                serialPort = new SerialPort(com, 9600, Parity.None, 8, StopBits.One);
                try
                {
                    serialPort.Open();
                }
                catch { };

                connect = true;
            });
            threadWait.Start();


            threadRead = new Thread(() => {
                try
                {
                    while (true)
                    {
                        try {
                        if (!connect) continue;
                        readLine = serialPort.ReadLine();
                        Game.EventReadDevice(readLine);
                        read = true;
                        }
                        catch (Exception ex) when (ex is FormatException || ex is IndexOutOfRangeException) { };
                }
                }
                catch(Exception e)
                {
                    Console.WriteLine("disconnect");
                    Console.WriteLine(e.StackTrace);
                    connectNow = false;
                    connect = false;
                    threadWait = null;
                    Game.EventDisconnectDevice();
                    Connect();
                };
            });
            threadRead.Start();
        }

        public static void Write(string text)
        {
            new Thread(() => {
                while (true) {
                    Console.WriteLine("write " + text);
                    if (!read) continue;
                    else */if (connectNow) serialPort.Write(text + "\n");
                    break;
                }
            }).Start();
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ParadoxIp.Managers
{
    public class ParadoxNotificationListener
    {
        readonly TcpClient client;
        readonly StreamReader reader;
        readonly StreamWriter writer;
        private static bool stopRunner = false;
        private static TcpListener listener;

        public ParadoxNotificationListener(TcpClient client)
        {
            this.client = client;
            this.client.ReceiveTimeout = 5000;
            var stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true };
        }

        public static void RequestStop()
        {
            stopRunner = true;
            listener.Stop();
        }

        public static void Main()
        {
            //todo: load as config option
            listener = new TcpListener(IPAddress.Parse("192.168.20.148"), 25);
            listener.Start();
            while (!stopRunner)
            {
                try
                {
                    var client = listener.AcceptTcpClient();
                    if (stopRunner)
                        break;

                    ParadoxNotificationListener handler = new ParadoxNotificationListener(client);
                    Thread thread = new Thread(handler.Run);
                    thread.Start();
                }
                catch (Exception)
                {
                    // catches as exception from calling listner.Stop outside of current thread.    
                }

            }
        }

        public void Run()
        {
            writer.WriteLine("220 localhost -- Fake proxy server");
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                Console.Error.WriteLine("Read line {0}", line);
                switch (line)
                {
                    case "DATA":
                        writer.WriteLine("354 Start input, end data with <CRLF>.<CRLF>");
                        StringBuilder data = new StringBuilder();
                        String subject = "";
                        line = reader.ReadLine();
                        if (line != null && line != ".")
                        {
                            const string SUBJECT = "Subject: ";
                            if (line.StartsWith(SUBJECT))
                                subject = line.Substring(SUBJECT.Length);
                            else data.AppendLine(line);
                            for (line = reader.ReadLine();
                                line != null && line != ".";
                                line = reader.ReadLine())
                            {
                                data.AppendLine(line);
                            }
                        }
                        String message = data.ToString();
                        Console.Error.WriteLine("Received ­ email with subject: {0} and message: {1}",
                            subject, message);
                        writer.WriteLine("250 OK");
                        client.Close();
                        return;
                    default:
                        writer.WriteLine("250 OK");
                        break;
                }
            }
        }
    }
}

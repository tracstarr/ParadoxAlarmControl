using System;
using System.Runtime.InteropServices;
using System.Threading;
using Paradox.WebServices.Services;

namespace Paradox.WinService
{
    static class Program
    {
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }
        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            ServiceStartup.Stop();
            return false;
        }

        static void Main()
        {
#if DEBUG
            try
            {
                ServiceStartup.Run();
                Console.WriteLine("Press <CTRL>+C to stop.");
                SetConsoleCtrlHandler(ConsoleCtrlCheck, true);
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}: {1}", ex.GetType().Name, ex.Message);
                throw;
            }
            finally
            {
                ServiceStartup.Stop();
            }
#else
            var appHost = ServiceStartup.GetAppHostListner();

            var servicesToRun = new ServiceBase[] 
            { 
                new WinService(appHost, ServiceStartup.ListeningOn) 
            };
            ServiceBase.Run(servicesToRun);
#endif

        }
    }
}

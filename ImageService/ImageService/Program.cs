using System;
using System.ServiceProcess;

namespace ImageService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                ImageService service1 = new ImageService(args);
                service1.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] ServicesToRun = new ServiceBase[] { new ImageService(args) };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}

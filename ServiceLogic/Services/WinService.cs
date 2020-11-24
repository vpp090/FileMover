using ServiceLogic.Contracts;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using Topshelf;

namespace ServiceLogic.Services
{
    public class WinService : ServiceControl
    {
        
        private Timer Timer = new Timer();
        private IMovingService MovingService { get; set; }

        public WinService(IMovingService movingService)
        {
            MovingService = movingService;
        }

        public bool Start(HostControl hostControl)
        {
            try
            {
                Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
                Timer.Interval = double.Parse(ConfigurationManager.AppSettings["MoveInterval"]);
                Timer.Enabled = true;
                Timer.Start();
                return true;
            }
            catch(Exception ex)
            {
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Application";
                    log.WriteEntry(ex.Message + " " + ex.StackTrace, EventLogEntryType.Error);
                }

                return false;
            }
        }

        public bool Stop(HostControl hostControl)
        {
            try
            {
                Timer.Enabled = false;
                return true;
            }
            catch(Exception ex)
            {
                using(EventLog log = new EventLog("Application"))
                {
                    log.Source = "Application";
                    log.WriteEntry(ex.Message + " " + ex.StackTrace, EventLogEntryType.Error);    
                }
                return false;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var sourceLocation = ConfigurationManager.AppSettings["TestFolderSrc"];
            var destinationLocation = ConfigurationManager.AppSettings["TestFolderDest"];

            MovingService.Move(sourceLocation, destinationLocation);
        }

    }
}

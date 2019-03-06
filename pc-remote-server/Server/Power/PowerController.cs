using System.Diagnostics;
using System.Runtime.InteropServices;

namespace pc_remote_server.Server.Power
{
    public class PowerController
    {
        private static PowerController _instance;

        private PowerController()
        {
        }

        public static PowerController Instance => _instance ?? (_instance = new PowerController());

        public void Shutdown()
        {
            Process.Start("shutdown", "/s /t 0");
        }

        public void Restart()
        {
            Process.Start("shutdown", "/r /t 0");
        }

        public void Sleep()
        {
            SetSuspendState(false, true, true);
        }

        public void LogOff()
        {
            ExitWindowsEx(0, 0);
        }

        public void Lock()
        {
            LockWorkStation();
        }

        public void Hibernate()
        {
            SetSuspendState(true, true, true);
        }

        //Log off
        [DllImport("user32")]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        //Lock
        [DllImport("user32")]
        private static extern void LockWorkStation();

        //Hibernate + sleep
        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);
    }
}
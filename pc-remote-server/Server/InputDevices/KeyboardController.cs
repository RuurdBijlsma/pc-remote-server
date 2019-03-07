using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace pc_remote_server.Server.Mouse
{
    public class KeyboardController
    {
        private static KeyboardController _instance;

        private KeyboardController()
        {
        }

        public static KeyboardController Instance => _instance ?? (_instance = new KeyboardController());

        public void PressKey(string key)
        {
            SendKeys.SendWait(key);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        private const int VK_MEDIA_NEXT_TRACK = 0xB0;
        private const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        private const int VK_MEDIA_PREV_TRACK = 0xB1;
        private const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        private const int KEYEVENTF_KEYUP = 0x0002; //Key up flag

        public void MediaPlayPause()
        {
            SendMediaKey(VK_MEDIA_PLAY_PAUSE);
        }

        public void MediaNext()
        {
            SendMediaKey(VK_MEDIA_NEXT_TRACK);
        }

        public void MediaPrevious()
        {
            SendMediaKey(VK_MEDIA_PREV_TRACK);
        }

        private void SendMediaKey(byte key)
        {
            keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            keybd_event(key, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }
    }
}
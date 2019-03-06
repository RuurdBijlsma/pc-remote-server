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
            SendKeys.Send(key);
        }
    }
}
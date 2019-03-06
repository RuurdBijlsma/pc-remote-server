using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace pc_remote_server.Server.Mouse
{
    public class MouseController
    {
        //Mouse actions
        private const int MouseEventLeftDown = 0x02;
        private const int MouseEventLeftUp = 0x04;
        private const int MouseEventRightDown = 0x08;
        private const int MouseEventRightUp = 0x10;
        private const int MouseEventWheel = 0x0800;
        private static MouseController _instance;

        private MouseController()
        {
        }

        public static MouseController Instance => _instance ?? (_instance = new MouseController());

        public void MoveCursor(int x, int y)
        {
            var startPos = Cursor.Position;
            Cursor.Position = new Point(startPos.X + x, startPos.Y + y);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);


        public void LeftClick()
        {
            ClickMouse(MouseEventLeftDown | MouseEventLeftUp);
        }

        public void RightClick()
        {
            ClickMouse(MouseEventRightDown | MouseEventRightUp);
        }

        private void ClickMouse(uint buttons)
        {
            var x = (uint) Cursor.Position.X;
            var y = (uint) Cursor.Position.Y;
            mouse_event(buttons, x, y, 0, 0);
        }

        public void Scroll(int value = 120)
        {
            mouse_event(MouseEventWheel, 0, 0, (uint) value, 0);
        }
    }
}
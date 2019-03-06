using System.Linq;
using System.Net;
using System.Threading.Tasks;
using pc_remote_server.Server.Mouse;
using pc_remote_server.Server.Power;

namespace pc_remote_server.Server
{
    public class RemoteServer : Server
    {
        public RemoteServer(IPEndPoint endpoint) : base(endpoint)
        {
        }

        protected override async Task<CommandResponse> HandleMessage(Command command)
        {
            var result = new CommandResponse {Id = command.Id, Data = "Success"};
            switch (command.Action)
            {
                case "ping":
                    result.Data = "ping";
                    break;

                case "getVolume":
                    result.Data = VolumeController.Instance.GetGlobalVolume();
                    break;

                case "setVolume":
                    await VolumeController.Instance.SetGlobalVolume(double.Parse(command.Value));
                    break;

                case "getProcesses":
                    result.Data = VolumeController.Instance.GetVolumeProcesses();
                    break;

                case "moveMouse":
                    var (x, y) = command.Value.Split(',').Select(int.Parse);
                    MouseController.Instance.MoveCursor(x, y);
                    break;

                case "leftClick":
                    MouseController.Instance.LeftClick();
                    break;

                case "rightClick":
                    MouseController.Instance.RightClick();
                    break;

                case "scroll":
                    MouseController.Instance.Scroll(int.Parse(command.Value));
                    break;

                case "pressKey":
                    KeyboardController.Instance.PressKey(command.Value);
                    break;

                case "shutdown":
                    PowerController.Instance.Shutdown();
                    break;

                case "restart":
                    PowerController.Instance.Restart();
                    break;

                case "sleep":
                    PowerController.Instance.Sleep();
                    break;

                default:
                    result.Data = "Error: command does not exist";
                    break;
            }

            return result;
        }
    }
}
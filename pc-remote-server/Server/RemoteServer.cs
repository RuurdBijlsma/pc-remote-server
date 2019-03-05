using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace pc_remote_server.Server
{
    public class RemoteServer : Server
    {
        protected override async Task<CommandResponse> HandleMessage(Command command)
        {
            switch (command.Action)
            {
                case "ping":
                    return new CommandResponse
                    {
                        Id = command.Id,
                        Data = "ping"
                    };
                
                case "getVolume":
                    return new CommandResponse
                    {
                        Id = command.Id,
                        Data = VolumeController.Instance.GetVolume().ToString(CultureInfo.InvariantCulture)
                    };
                
                case "setVolume":
                    await VolumeController.Instance.SetVolume(double.Parse(command.Value));
                    break;
                
                default:
                    return new CommandResponse
                    {
                        Id = command.Id,
                        Data = "Error: command does not exist"
                    };
            }

            return new CommandResponse
            {
                Id = command.Id,
                Data = "Success"
            };
        }

        public RemoteServer(IPEndPoint endpoint) : base(endpoint)
        {
        }
    }
}
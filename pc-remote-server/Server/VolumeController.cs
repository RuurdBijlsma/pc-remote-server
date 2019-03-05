using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;

namespace pc_remote_server.Server
{
    public class VolumeController
    {
        private VolumeController()
        {
            _playbackDevice = new CoreAudioController().DefaultPlaybackDevice;
        }

        private readonly CoreAudioDevice _playbackDevice;
        private static VolumeController _instance;
        public static VolumeController Instance => _instance ?? (_instance = new VolumeController());

        public async Task SetVolume(double value)
        {
            await _playbackDevice.SetVolumeAsync(value);
        }

        public double GetVolume()
        {
            return _playbackDevice.Volume;
        }
    }
}
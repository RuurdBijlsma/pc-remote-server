using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using AudioSwitcher.AudioApi.CoreAudio;

namespace pc_remote_server.Server
{
    public class VolumeController
    {
        private static VolumeController _instance;
        public readonly CoreAudioDevice PlaybackDevice;
        private List<VolumeMixerController.VolumeControl> _controls;
        private bool _mixerInitiated;

        private List<Process> _volumeProcesses;

        private VolumeController()
        {
            Debug.WriteLine("Instantiating volume controller");
            // Retrieve default playback device
            PlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
        }

        public static VolumeController Instance => _instance ?? (_instance = new VolumeController());


        private void InitiateMixer()
        {
            if (_mixerInitiated) return;
            // Get processes making sound every 2 seconds
            RefreshVolumeProcesses();
            _mixerInitiated = true;

            var timer = new Timer(2000) {Enabled = true, AutoReset = true};
            timer.Elapsed += (sender, args) => RefreshVolumeProcesses();
            timer.Start();
            Debug.WriteLine("Instantiated volume controller");
        }

        private void RefreshVolumeProcesses()
        {
            var processes = Process.GetProcesses()
                .Where(p => p.MainWindowTitle != "")
                .ToList();

            _controls = processes.Select(process => new HashSet<int> {process.Id})
                .Select(hs =>
                {
                    try
                    {
                        return VolumeMixerController.GetVolumeControl(hs);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        return null;
                    }
                })
                .Where(v => v != null).ToList();

            var volumeIds = _controls.Select(c => c.ProcessId);
            _volumeProcesses = processes
                .Where(p => volumeIds.Contains(p.Id)).ToList();
        }

        public async Task SetGlobalVolume(double value)
        {
            await PlaybackDevice.SetVolumeAsync(value);
        }

        public double GetGlobalVolume()
        {
            return PlaybackDevice.Volume;
        }

        public IEnumerable<object> GetVolumeProcesses()
        {
            InitiateMixer();
            return _volumeProcesses.Select(p => new
            {
                name = p.MainWindowTitle,
                icon = Icon.ExtractAssociatedIcon(p.MainModule.FileName)
            });
        }
    }
}
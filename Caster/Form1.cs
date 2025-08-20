using Sharpcaster;
using Sharpcaster.Interfaces;
using Sharpcaster.Models;
using Sharpcaster.Models.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caster
{
    public partial class Form1 : Form
    {
        private List<ChromeCastDevice> _devices = new List<ChromeCastDevice>();
        private ChromeCastDevice _selectedDevice;
        private ChromecastClient _chromecastClient;
        private CastingService _castingService;
        public Form1()
        {
            _castingService = new CastingService();
            InitializeComponent();
            Load += Form1_Load;
            comboBoxDevices.SelectedIndexChanged += ComboBoxDevices_SelectedIndexChanged;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await DiscoverDevicesAsync();
        }

        private async Task DiscoverDevicesAsync()
        {
            comboBoxDevices.Items.Clear();
            _devices.Clear();

            await foreach (var device in _castingService.DiscoverReceiversAsync())
            {
                comboBoxDevices.Items.Add(device.Name);
                _devices.Add(device);
            }
            if (comboBoxDevices.Items.Count > 0)
                comboBoxDevices.SelectedIndex = 0;
        }

        private async void ComboBoxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = comboBoxDevices.SelectedIndex;
            if (idx >= 0 && idx < _devices.Count)
            {
                _selectedDevice = _devices[idx];
                await UpdateDeviceStatus();
                // Subscribe to status changes
                _selectedDevice.StatusChanged -= SelectedDevice_StatusChanged;
                _selectedDevice.StatusChanged += SelectedDevice_StatusChanged;
                propertyGridDevice.SelectedObject = _selectedDevice;
            }
        }

        private async Task UpdateDeviceStatus()
        {
            if (!_selectedDevice.Connected)
            {
                await _selectedDevice.Connect();
            }
            if (_selectedDevice?.Status != null)
            {
                var status = _selectedDevice.Status;
                labelStatus.Text = $"Status: {status.PlayerState} | Volume: {status.Volume?.Level}";
            }
            else
            {
                labelStatus.Text = "Status: N/A";
            }
        }

        private async void SelectedDevice_StatusChanged(object sender, EventArgs e)
        {
            // Ensure UI update on main thread
            if (InvokeRequired)
                await Invoke(async () => { await UpdateDeviceStatus(); });
            else
                await UpdateDeviceStatus();
        }

        private async void buttonTTS_Click(object sender, EventArgs e)
        {
            if (_selectedDevice == null)
            {
                MessageBox.Show("Please select a Chromecast device.");
                return;
            }
            _selectedDevice.LoadDefautlMedia(RadioCastingHelper.GetKinkMedia());
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await DiscoverDevicesAsync();
        }

        private async void buttonPlay_Click(object sender, EventArgs e)
        {
            if (_selectedDevice != null)
                await _selectedDevice.PlayAsync();
        }

        private async void buttonPause_Click(object sender, EventArgs e)
        {
            if (_selectedDevice != null)
                await _selectedDevice.PauseAsync();
        }

        private async void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            if (_selectedDevice != null)
            {
                double volume = trackBarVolume.Value / 100.0;
                await _selectedDevice.SetVolumeAsync(volume);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_selectedDevice == null)
            {
                MessageBox.Show("Please select a Chromecast device.");
                return;
            }
            _selectedDevice.LoadDefautlMedia(RadioCastingHelper.GetQMusicNonStopMedia());
        }
    }
}

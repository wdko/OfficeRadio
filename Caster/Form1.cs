using Sharpcaster;
using Sharpcaster.Interfaces;
using Sharpcaster.Models;
using Sharpcaster.Models.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        private Settings _settings;
        public Form1()
        {
            _castingService = new CastingService();
            _settings = Settings.Load();
            InitializeComponent();
            Load += Form1_Load;
            comboBoxDevices.SelectedIndexChanged += ComboBoxDevices_SelectedIndexChanged;
            SetupTrayIcon();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await DiscoverDevicesAsync();
        }

        private async Task DiscoverDevicesAsync()
        {
            comboBoxDevices.Items.Clear();
            _devices.Clear();

            ChromeCastDevice? favoriteDevice = null;
            var otherDevices = new List<ChromeCastDevice>();

            await foreach (var device in _castingService.DiscoverReceiversAsync())
            {
                comboBoxDevices.Items.Add(device.Name);
                _devices.Add(device);

                // Check if this is the favorite device
                if (!string.IsNullOrEmpty(_settings.FavoriteDeviceName) && 
                    device.Name == _settings.FavoriteDeviceName)
                {
                    favoriteDevice = device;
                }
                else
                {
                    otherDevices.Add(device);
                }
            }

            // Reorder devices to prioritize favorite
            if (favoriteDevice != null)
            {
                comboBoxDevices.Items.Clear();
                _devices.Clear();
                
                // Add favorite device first
                comboBoxDevices.Items.Add(favoriteDevice.Name);
                _devices.Add(favoriteDevice);
                
                // Add other devices
                foreach (var device in otherDevices)
                {
                    comboBoxDevices.Items.Add(device.Name);
                    _devices.Add(device);
                }
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
                
                // Update favorite device if it's different
                if (_settings.FavoriteDeviceName != _selectedDevice.Name)
                {
                    _settings.FavoriteDeviceName = _selectedDevice.Name;
                    _settings.Save();
                }
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

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void SetupTrayIcon()
        {
            ni_TrayIcon.Icon = SystemIcons.Application;
            ni_TrayIcon.Text = "OfficeRadio";
            ni_TrayIcon.Visible = true;
            ni_TrayIcon.DoubleClick += (s, e) => ShowForm();

            // Create context menu for tray icon
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Show", null, (s, e) => ShowForm());
            contextMenu.Items.Add("Settings", null, (s, e) => ShowSettings());
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());
            ni_TrayIcon.ContextMenuStrip = contextMenu;
        }

        private void ShowForm()
        {
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
        }

        private void ShowSettings()
        {
            using (var settingsForm = new SettingsForm(_settings))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    _settings.Save();
                }
            }
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);
            if (!value && _settings.MinimizeToTray)
            {
                Hide();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (WindowState == FormWindowState.Minimized && _settings.MinimizeToTray)
            {
                Hide();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_settings.ExitToTray)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                _settings.Save();
                base.OnFormClosing(e);
            }
        }
    }
}

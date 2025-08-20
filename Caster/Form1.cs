using Sharpcaster;
using Sharpcaster.Interfaces;
using Sharpcaster.Models;
using Sharpcaster.Models.Media;
using Sharpcaster.Models.MediaStatus;
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
        private bool _isMinimizedToTray = false;
        public Form1()
        {
            _castingService = new CastingService();
            InitializeComponent();
            Load += Form1_Load;
            comboBoxDevices.SelectedIndexChanged += ComboBoxDevices_SelectedIndexChanged;
            
            // Setup tray icon
            SetupTrayIcon();
            
            // Handle form events for tray functionality
            Resize += Form1_Resize;
            FormClosing += Form1_FormClosing;
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
                
            // Update tray menu when devices change
            BuildTrayMenu();
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
                
                // Update tray menu and tooltip
                BuildTrayMenu();
                UpdateTrayTooltip();
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
                
            // Update tray tooltip when status changes
            UpdateTrayTooltip();
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

        #region Tray Functionality

        private void SetupTrayIcon()
        {
            // Set tray icon (using default icon for now)
            ni_TrayIcon.Icon = SystemIcons.Application;
            ni_TrayIcon.Text = "OfficeRadio - No device selected";
            
            // Setup context menu
            BuildTrayMenu();
            
            // Handle tray icon events
            ni_TrayIcon.DoubleClick += ni_TrayIcon_DoubleClick;
        }

        private void BuildTrayMenu()
        {
            contextMenuTray.Items.Clear();
            
            // Device selection submenu
            var devicesMenu = new ToolStripMenuItem("Select Device");
            if (_devices.Count > 0)
            {
                foreach (var device in _devices)
                {
                    var deviceItem = new ToolStripMenuItem(device.Name);
                    deviceItem.Tag = device;
                    deviceItem.Checked = (_selectedDevice == device);
                    deviceItem.Click += DeviceMenuItem_Click;
                    devicesMenu.DropDownItems.Add(deviceItem);
                }
            }
            else
            {
                var noDevicesItem = new ToolStripMenuItem("No devices found");
                noDevicesItem.Enabled = false;
                devicesMenu.DropDownItems.Add(noDevicesItem);
            }
            contextMenuTray.Items.Add(devicesMenu);
            
            contextMenuTray.Items.Add(new ToolStripSeparator());
            
            // Volume controls
            var volumeUpItem = new ToolStripMenuItem("Volume Up");
            volumeUpItem.Click += VolumeUp_Click;
            contextMenuTray.Items.Add(volumeUpItem);
            
            var volumeDownItem = new ToolStripMenuItem("Volume Down");
            volumeDownItem.Click += VolumeDown_Click;
            contextMenuTray.Items.Add(volumeDownItem);
            
            contextMenuTray.Items.Add(new ToolStripSeparator());
            
            // Play/Pause control
            var playPauseItem = new ToolStripMenuItem("Play/Pause");
            playPauseItem.Click += PlayPause_Click;
            contextMenuTray.Items.Add(playPauseItem);
            
            contextMenuTray.Items.Add(new ToolStripSeparator());
            
            // Radio submenu
            var radioMenu = new ToolStripMenuItem("Play Radio");
            
            var kinkItem = new ToolStripMenuItem("KINK");
            kinkItem.Click += PlayKink_Click;
            radioMenu.DropDownItems.Add(kinkItem);
            
            var qmusicItem = new ToolStripMenuItem("Qmusic");
            qmusicItem.Click += PlayQmusic_Click;
            radioMenu.DropDownItems.Add(qmusicItem);
            
            contextMenuTray.Items.Add(radioMenu);
            
            contextMenuTray.Items.Add(new ToolStripSeparator());
            
            // Show/Hide application
            var showHideItem = new ToolStripMenuItem(_isMinimizedToTray ? "Show Application" : "Hide to Tray");
            showHideItem.Click += ShowHide_Click;
            contextMenuTray.Items.Add(showHideItem);
            
            // Exit application
            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += TrayExit_Click;
            contextMenuTray.Items.Add(exitItem);
        }

        private void UpdateTrayTooltip()
        {
            string deviceText = _selectedDevice?.Name ?? "No device selected";
            string statusText = _selectedDevice?.Status?.PlayerState.ToString() ?? "Unknown";
            ni_TrayIcon.Text = $"OfficeRadio - {deviceText} ({statusText})";
        }

        #endregion

        #region Tray Event Handlers

        private async void DeviceMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is ChromeCastDevice device)
            {
                _selectedDevice = device;
                // Find the device index and select it in the combobox
                int deviceIndex = _devices.IndexOf(device);
                if (deviceIndex >= 0)
                {
                    comboBoxDevices.SelectedIndex = deviceIndex;
                }
                
                await UpdateDeviceStatus();
                BuildTrayMenu(); // Rebuild to update checkmarks
                UpdateTrayTooltip();
            }
        }

        private async void VolumeUp_Click(object sender, EventArgs e)
        {
            if (_selectedDevice != null)
            {
                var currentVolume = _selectedDevice.Status?.Volume?.Level ?? 0.5;
                var newVolume = Math.Min(1.0, currentVolume + 0.2);
                await _selectedDevice.SetVolumeAsync(newVolume);
                
                // Update trackbar if form is visible
                if (WindowState != FormWindowState.Minimized && Visible)
                {
                    trackBarVolume.Value = (int)(newVolume * 100);
                }
            }
        }

        private async void VolumeDown_Click(object sender, EventArgs e)
        {
            if (_selectedDevice != null)
            {
                var currentVolume = _selectedDevice.Status?.Volume?.Level ?? 0.5;
                var newVolume = Math.Max(0.0, currentVolume - 0.2);
                await _selectedDevice.SetVolumeAsync(newVolume);
                
                // Update trackbar if form is visible
                if (WindowState != FormWindowState.Minimized && Visible)
                {
                    trackBarVolume.Value = (int)(newVolume * 100);
                }
            }
        }

        private async void PlayPause_Click(object sender, EventArgs e)
        {
            if (_selectedDevice != null)
            {
                var status = _selectedDevice.Status;
                if (status != null)
                {
                    if (status.PlayerState == PlayerState.Playing)
                    {
                        await _selectedDevice.PauseAsync();
                    }
                    else
                    {
                        await _selectedDevice.PlayAsync();
                    }
                }
                else
                {
                    // If no status, default to play
                    await _selectedDevice.PlayAsync();
                }
            }
        }

        private void PlayKink_Click(object sender, EventArgs e)
        {
            if (_selectedDevice == null)
            {
                MessageBox.Show("Please select a Chromecast device.");
                return;
            }
            _selectedDevice.LoadDefautlMedia(RadioCastingHelper.GetKinkMedia());
        }

        private void PlayQmusic_Click(object sender, EventArgs e)
        {
            if (_selectedDevice == null)
            {
                MessageBox.Show("Please select a Chromecast device.");
                return;
            }
            _selectedDevice.LoadDefautlMedia(RadioCastingHelper.GetQMusicNonStopMedia());
        }

        private void ShowHide_Click(object sender, EventArgs e)
        {
            if (_isMinimizedToTray)
            {
                ShowFromTray();
            }
            else
            {
                MinimizeToTray();
            }
        }

        private void TrayExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ni_TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            if (_isMinimizedToTray)
            {
                ShowFromTray();
            }
        }

        #endregion

        #region Form Event Handlers

        private void buttonMinimizeToTray_Click(object sender, EventArgs e)
        {
            MinimizeToTray();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && _isMinimizedToTray)
            {
                Hide();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Allow application to close normally
            ni_TrayIcon.Visible = false;
        }

        private void MinimizeToTray()
        {
            _isMinimizedToTray = true;
            Hide();
            BuildTrayMenu(); // Update menu text
        }

        private void ShowFromTray()
        {
            _isMinimizedToTray = false;
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
            BuildTrayMenu(); // Update menu text
        }

        #endregion
    }
}

using System;
using System.Windows.Forms;

namespace Caster
{
    public partial class SettingsForm : Form
    {
        private Settings _settings;
        private CheckBox _minimizeToTrayCheckBox;
        private CheckBox _exitToTrayCheckBox;
        private TextBox _favoriteDeviceTextBox;
        private Button _okButton;
        private Button _cancelButton;

        public SettingsForm(Settings settings)
        {
            _settings = settings;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Settings";
            Size = new System.Drawing.Size(350, 200);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(10)
            };

            // Minimize to tray checkbox
            _minimizeToTrayCheckBox = new CheckBox
            {
                Text = "Minimize to tray",
                Checked = _settings.MinimizeToTray,
                AutoSize = true,
                Dock = DockStyle.Fill
            };
            panel.Controls.Add(_minimizeToTrayCheckBox, 0, 0);
            panel.SetColumnSpan(_minimizeToTrayCheckBox, 2);

            // Exit to tray checkbox
            _exitToTrayCheckBox = new CheckBox
            {
                Text = "Exit to tray (close button minimizes)",
                Checked = _settings.ExitToTray,
                AutoSize = true,
                Dock = DockStyle.Fill
            };
            panel.Controls.Add(_exitToTrayCheckBox, 0, 1);
            panel.SetColumnSpan(_exitToTrayCheckBox, 2);

            // Favorite device
            var favoriteLabel = new Label
            {
                Text = "Favorite Device:",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            };
            panel.Controls.Add(favoriteLabel, 0, 2);

            _favoriteDeviceTextBox = new TextBox
            {
                Text = _settings.FavoriteDeviceName ?? "",
                Dock = DockStyle.Fill
            };
            panel.Controls.Add(_favoriteDeviceTextBox, 1, 2);

            // Buttons
            var buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            _cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel
            };
            _cancelButton.Click += (s, e) => Close();

            _okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK
            };
            _okButton.Click += OkButton_Click;

            buttonPanel.Controls.Add(_cancelButton);
            buttonPanel.Controls.Add(_okButton);
            panel.Controls.Add(buttonPanel, 0, 3);
            panel.SetColumnSpan(buttonPanel, 2);

            // Configure column styles
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            
            // Configure row styles
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Controls.Add(panel);
            AcceptButton = _okButton;
            CancelButton = _cancelButton;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            _settings.MinimizeToTray = _minimizeToTrayCheckBox.Checked;
            _settings.ExitToTray = _exitToTrayCheckBox.Checked;
            _settings.FavoriteDeviceName = string.IsNullOrWhiteSpace(_favoriteDeviceTextBox.Text) 
                ? null 
                : _favoriteDeviceTextBox.Text.Trim();
            Close();
        }
    }
}
namespace Caster
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            comboBoxDevices = new ComboBox();
            buttonTTS = new Button();
            ni_TrayIcon = new NotifyIcon(components);
            labelStatus = new Label();
            button1 = new Button();
            propertyGridDevice = new PropertyGrid();
            buttonPlay = new Button();
            buttonPause = new Button();
            trackBarVolume = new TrackBar();
            tableLayoutPanelMain = new TableLayoutPanel();
            flowLayoutPanelControls = new FlowLayoutPanel();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).BeginInit();
            tableLayoutPanelMain.SuspendLayout();
            flowLayoutPanelControls.SuspendLayout();
            SuspendLayout();
            // 
            // comboBoxDevices
            // 
            comboBoxDevices.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDevices.Location = new Point(13, 13);
            comboBoxDevices.Name = "comboBoxDevices";
            comboBoxDevices.Size = new Size(180, 23);
            comboBoxDevices.TabIndex = 0;
            // 
            // buttonTTS
            // 
            buttonTTS.Location = new Point(656, 13);
            buttonTTS.Name = "buttonTTS";
            buttonTTS.Size = new Size(100, 24);
            buttonTTS.TabIndex = 1;
            buttonTTS.Text = "Start Radio Kink";
            buttonTTS.Click += buttonTTS_Click;
            // 
            // ni_TrayIcon
            // 
            ni_TrayIcon.Text = "OfficeRadio";
            ni_TrayIcon.Visible = true;
            // 
            // labelStatus
            // 
            labelStatus.Location = new Point(199, 10);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(173, 23);
            labelStatus.TabIndex = 2;
            labelStatus.Text = "Status: N/A";
            // 
            // button1
            // 
            button1.Location = new Point(378, 13);
            button1.Name = "button1";
            button1.Size = new Size(100, 24);
            button1.TabIndex = 3;
            button1.Text = "Refresh";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // propertyGridDevice
            // 
            propertyGridDevice.BackColor = SystemColors.Control;
            propertyGridDevice.Dock = DockStyle.Fill;
            propertyGridDevice.Location = new Point(3, 110);
            propertyGridDevice.Name = "propertyGridDevice";
            propertyGridDevice.Size = new Size(802, 337);
            propertyGridDevice.TabIndex = 3;
            // 
            // buttonPlay
            // 
            buttonPlay.Location = new Point(484, 13);
            buttonPlay.Name = "buttonPlay";
            buttonPlay.Size = new Size(80, 24);
            buttonPlay.TabIndex = 4;
            buttonPlay.Text = "Play";
            buttonPlay.Click += buttonPlay_Click;
            // 
            // buttonPause
            // 
            buttonPause.Location = new Point(570, 13);
            buttonPause.Name = "buttonPause";
            buttonPause.Size = new Size(80, 24);
            buttonPause.TabIndex = 5;
            buttonPause.Text = "Pause";
            buttonPause.Click += buttonPause_Click;
            // 
            // trackBarVolume
            // 
            trackBarVolume.Location = new Point(148, 43);
            trackBarVolume.Maximum = 100;
            trackBarVolume.Name = "trackBarVolume";
            trackBarVolume.Size = new Size(120, 45);
            trackBarVolume.TabIndex = 6;
            trackBarVolume.TickFrequency = 10;
            trackBarVolume.Value = 50;
            trackBarVolume.Scroll += trackBarVolume_Scroll;
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 1;
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Controls.Add(flowLayoutPanelControls, 0, 0);
            tableLayoutPanelMain.Controls.Add(propertyGridDevice, 0, 1);
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelMain.Location = new Point(0, 0);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 2;
            tableLayoutPanelMain.RowStyles.Add(new RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Size = new Size(808, 450);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // flowLayoutPanelControls
            // 
            flowLayoutPanelControls.AutoSize = true;
            flowLayoutPanelControls.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanelControls.Controls.Add(comboBoxDevices);
            flowLayoutPanelControls.Controls.Add(labelStatus);
            flowLayoutPanelControls.Controls.Add(button1);
            flowLayoutPanelControls.Controls.Add(buttonPlay);
            flowLayoutPanelControls.Controls.Add(buttonPause);
            flowLayoutPanelControls.Controls.Add(buttonTTS);
            flowLayoutPanelControls.Controls.Add(button2);
            flowLayoutPanelControls.Controls.Add(trackBarVolume);
            flowLayoutPanelControls.Dock = DockStyle.Top;
            flowLayoutPanelControls.Location = new Point(3, 3);
            flowLayoutPanelControls.Name = "flowLayoutPanelControls";
            flowLayoutPanelControls.Padding = new Padding(10);
            flowLayoutPanelControls.Size = new Size(802, 101);
            flowLayoutPanelControls.TabIndex = 0;
            // 
            // button2
            // 
            button2.Location = new Point(13, 43);
            button2.Name = "button2";
            button2.Size = new Size(129, 24);
            button2.TabIndex = 7;
            button2.Text = "Start Radio Qmusic";
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(808, 450);
            Controls.Add(tableLayoutPanelMain);
            Name = "Form1";
            Text = "Caster";
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).EndInit();
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            flowLayoutPanelControls.ResumeLayout(false);
            flowLayoutPanelControls.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxDevices;
        private System.Windows.Forms.Button buttonTTS;
        private NotifyIcon ni_TrayIcon;
        private System.Windows.Forms.Label labelStatus;
        private Button button1;
        private System.Windows.Forms.PropertyGrid propertyGridDevice;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.TrackBar trackBarVolume;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelControls;
        private Button button2;
    }
}

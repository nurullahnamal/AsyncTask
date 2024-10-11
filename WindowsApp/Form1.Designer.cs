namespace WindowsApp
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
            btnManagedThreadId = new Button();
            lbLogs = new ListBox();
            btnWait = new Button();
            btnSet = new Button();
            btnStartTimer = new Button();
            lblClock = new Label();
            btnFakeApi = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // btnManagedThreadId
            // 
            btnManagedThreadId.BackColor = Color.FromArgb(192, 192, 255);
            btnManagedThreadId.Location = new Point(832, 12);
            btnManagedThreadId.Name = "btnManagedThreadId";
            btnManagedThreadId.Size = new Size(144, 50);
            btnManagedThreadId.TabIndex = 0;
            btnManagedThreadId.Text = "Print Managed Thread Id";
            btnManagedThreadId.UseVisualStyleBackColor = false;
            btnManagedThreadId.Click += btnManagedThreadId_Click;
            // 
            // lbLogs
            // 
            lbLogs.BackColor = SystemColors.ScrollBar;
            lbLogs.Dock = DockStyle.Bottom;
            lbLogs.Font = new Font("Segoe UI", 15F);
            lbLogs.FormattingEnabled = true;
            lbLogs.ItemHeight = 35;
            lbLogs.Location = new Point(0, 244);
            lbLogs.Name = "lbLogs";
            lbLogs.Size = new Size(988, 214);
            lbLogs.TabIndex = 1;
            // 
            // btnWait
            // 
            btnWait.BackColor = Color.FromArgb(255, 128, 128);
            btnWait.FlatStyle = FlatStyle.Flat;
            btnWait.Location = new Point(21, 28);
            btnWait.Name = "btnWait";
            btnWait.Size = new Size(144, 64);
            btnWait.TabIndex = 2;
            btnWait.Text = "WAIT 5 Sec";
            btnWait.UseVisualStyleBackColor = false;
            btnWait.Click += btnWait_Click;
            // 
            // btnSet
            // 
            btnSet.BackColor = Color.FromArgb(0, 192, 0);
            btnSet.FlatStyle = FlatStyle.Flat;
            btnSet.Location = new Point(201, 28);
            btnSet.Name = "btnSet";
            btnSet.Size = new Size(144, 64);
            btnSet.TabIndex = 3;
            btnSet.Text = "Continue";
            btnSet.UseVisualStyleBackColor = false;
            btnSet.Click += btnSet_Click;
            // 
            // btnStartTimer
            // 
            btnStartTimer.BackColor = Color.FromArgb(192, 192, 255);
            btnStartTimer.Location = new Point(832, 80);
            btnStartTimer.Name = "btnStartTimer";
            btnStartTimer.Size = new Size(144, 50);
            btnStartTimer.TabIndex = 4;
            btnStartTimer.Text = "Start Timer";
            btnStartTimer.UseVisualStyleBackColor = false;
            btnStartTimer.Click += btnStartTimer_Click;
            // 
            // lblClock
            // 
            lblClock.AutoSize = true;
            lblClock.Font = new Font("Segoe UI", 20F);
            lblClock.Location = new Point(429, 131);
            lblClock.Name = "lblClock";
            lblClock.Size = new Size(203, 46);
            lblClock.TabIndex = 5;
            lblClock.Text = "00:00:00.000";
            // 
            // btnFakeApi
            // 
            btnFakeApi.BackColor = Color.SlateBlue;
            btnFakeApi.Location = new Point(21, 130);
            btnFakeApi.Name = "btnFakeApi";
            btnFakeApi.Size = new Size(324, 63);
            btnFakeApi.TabIndex = 6;
            btnFakeApi.Text = "Fake Api Call";
            btnFakeApi.UseVisualStyleBackColor = false;
            btnFakeApi.Click += btnFakeApi_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(0, 192, 0);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(453, 37);
            button1.Name = "button1";
            button1.Size = new Size(144, 64);
            button1.TabIndex = 7;
            button1.Text = "TEST";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(988, 458);
            Controls.Add(button1);
            Controls.Add(btnFakeApi);
            Controls.Add(lblClock);
            Controls.Add(btnStartTimer);
            Controls.Add(btnSet);
            Controls.Add(btnWait);
            Controls.Add(lbLogs);
            Controls.Add(btnManagedThreadId);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Async Tester";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnManagedThreadId;
        private ListBox lbLogs;
        private Button btnWait;
        private Button btnSet;
        private Button btnStartTimer;
        private Label lblClock;
        private Button btnFakeApi;
        private Button button1;
    }
}

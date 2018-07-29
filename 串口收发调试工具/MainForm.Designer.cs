namespace 串口收发调试工具
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.serialPortDebugGroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SerialPortNum = new System.Windows.Forms.ComboBox();
            this.BaudRate = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.serialPortDebug = new System.Windows.Forms.Button();
            this.rxSetGroupBox = new System.Windows.Forms.GroupBox();
            this.clearRxLabel = new System.Windows.Forms.Label();
            this.RxFileCheckBox = new System.Windows.Forms.CheckBox();
            this.sendSetGroupBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.timeTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxLoopCheckBox = new System.Windows.Forms.CheckBox();
            this.TxFileCheckBox = new System.Windows.Forms.CheckBox();
            this.clearSendLabel = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.rxFrameCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.txFrameCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.clearNumButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.rxLocalFrameCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.txLocalFrameCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.SendTimer = new System.Windows.Forms.Timer(this.components);
            this.RxDataTextBox = new System.Windows.Forms.TextBox();
            this.RxDataGroupBox = new System.Windows.Forms.GroupBox();
            this.SendDataGroupBox = new System.Windows.Forms.GroupBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.sendTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataBagCheckBox = new System.Windows.Forms.CheckBox();
            this.nodeTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.frameLinkCheckBox = new System.Windows.Forms.CheckBox();
            this.frameLinkNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.frameNumCheckBox = new System.Windows.Forms.CheckBox();
            this.multicatGroupBox = new System.Windows.Forms.GroupBox();
            this.mulTextBox = new System.Windows.Forms.TextBox();
            this.mulAddButton = new System.Windows.Forms.Button();
            this.mulClrButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.mulNodeTextBox = new System.Windows.Forms.TextBox();
            this.mulCheckBox = new System.Windows.Forms.CheckBox();
            this.serialPortDebugGroupBox.SuspendLayout();
            this.rxSetGroupBox.SuspendLayout();
            this.sendSetGroupBox.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.RxDataGroupBox.SuspendLayout();
            this.SendDataGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameLinkNumericUpDown)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.multicatGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPortDebugGroupBox
            // 
            this.serialPortDebugGroupBox.Controls.Add(this.label2);
            this.serialPortDebugGroupBox.Controls.Add(this.SerialPortNum);
            this.serialPortDebugGroupBox.Controls.Add(this.BaudRate);
            this.serialPortDebugGroupBox.Controls.Add(this.label1);
            this.serialPortDebugGroupBox.Controls.Add(this.serialPortDebug);
            this.serialPortDebugGroupBox.Location = new System.Drawing.Point(12, 12);
            this.serialPortDebugGroupBox.Name = "serialPortDebugGroupBox";
            this.serialPortDebugGroupBox.Size = new System.Drawing.Size(145, 139);
            this.serialPortDebugGroupBox.TabIndex = 11;
            this.serialPortDebugGroupBox.TabStop = false;
            this.serialPortDebugGroupBox.Text = "通过串口调试";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "波特率";
            // 
            // SerialPortNum
            // 
            this.SerialPortNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SerialPortNum.FormattingEnabled = true;
            this.SerialPortNum.Location = new System.Drawing.Point(53, 24);
            this.SerialPortNum.Name = "SerialPortNum";
            this.SerialPortNum.Size = new System.Drawing.Size(79, 20);
            this.SerialPortNum.TabIndex = 4;
            // 
            // BaudRate
            // 
            this.BaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BaudRate.FormattingEnabled = true;
            this.BaudRate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.BaudRate.Location = new System.Drawing.Point(53, 51);
            this.BaudRate.Name = "BaudRate";
            this.BaudRate.Size = new System.Drawing.Size(79, 20);
            this.BaudRate.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "串口号";
            // 
            // serialPortDebug
            // 
            this.serialPortDebug.Location = new System.Drawing.Point(33, 89);
            this.serialPortDebug.Name = "serialPortDebug";
            this.serialPortDebug.Size = new System.Drawing.Size(75, 34);
            this.serialPortDebug.TabIndex = 11;
            this.serialPortDebug.Text = "连接";
            this.serialPortDebug.UseVisualStyleBackColor = true;
            this.serialPortDebug.Click += new System.EventHandler(this.serialPortDebug_Click);
            // 
            // rxSetGroupBox
            // 
            this.rxSetGroupBox.Controls.Add(this.clearRxLabel);
            this.rxSetGroupBox.Controls.Add(this.RxFileCheckBox);
            this.rxSetGroupBox.Location = new System.Drawing.Point(12, 157);
            this.rxSetGroupBox.Name = "rxSetGroupBox";
            this.rxSetGroupBox.Size = new System.Drawing.Size(145, 137);
            this.rxSetGroupBox.TabIndex = 12;
            this.rxSetGroupBox.TabStop = false;
            this.rxSetGroupBox.Text = "接收区设置";
            // 
            // clearRxLabel
            // 
            this.clearRxLabel.AutoSize = true;
            this.clearRxLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clearRxLabel.Location = new System.Drawing.Point(10, 105);
            this.clearRxLabel.Name = "clearRxLabel";
            this.clearRxLabel.Size = new System.Drawing.Size(53, 12);
            this.clearRxLabel.TabIndex = 1;
            this.clearRxLabel.Text = "清除显示";
            this.clearRxLabel.Click += new System.EventHandler(this.clearRxLabel_Click);
            // 
            // RxFileCheckBox
            // 
            this.RxFileCheckBox.AutoSize = true;
            this.RxFileCheckBox.Location = new System.Drawing.Point(12, 31);
            this.RxFileCheckBox.Name = "RxFileCheckBox";
            this.RxFileCheckBox.Size = new System.Drawing.Size(96, 16);
            this.RxFileCheckBox.TabIndex = 0;
            this.RxFileCheckBox.Text = "接收存储文件";
            this.RxFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // sendSetGroupBox
            // 
            this.sendSetGroupBox.Controls.Add(this.label5);
            this.sendSetGroupBox.Controls.Add(this.timeTextBox);
            this.sendSetGroupBox.Controls.Add(this.label4);
            this.sendSetGroupBox.Controls.Add(this.TxLoopCheckBox);
            this.sendSetGroupBox.Controls.Add(this.TxFileCheckBox);
            this.sendSetGroupBox.Controls.Add(this.clearSendLabel);
            this.sendSetGroupBox.Location = new System.Drawing.Point(12, 300);
            this.sendSetGroupBox.Name = "sendSetGroupBox";
            this.sendSetGroupBox.Size = new System.Drawing.Size(145, 125);
            this.sendSetGroupBox.TabIndex = 13;
            this.sendSetGroupBox.TabStop = false;
            this.sendSetGroupBox.Text = "发送区设置";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(123, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "ms";
            // 
            // timeTextBox
            // 
            this.timeTextBox.Location = new System.Drawing.Point(69, 81);
            this.timeTextBox.Name = "timeTextBox";
            this.timeTextBox.Size = new System.Drawing.Size(48, 21);
            this.timeTextBox.TabIndex = 4;
            this.timeTextBox.Text = "500";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "发送间隔";
            // 
            // TxLoopCheckBox
            // 
            this.TxLoopCheckBox.AutoSize = true;
            this.TxLoopCheckBox.Location = new System.Drawing.Point(12, 58);
            this.TxLoopCheckBox.Name = "TxLoopCheckBox";
            this.TxLoopCheckBox.Size = new System.Drawing.Size(96, 16);
            this.TxLoopCheckBox.TabIndex = 2;
            this.TxLoopCheckBox.Text = "数据循环发送";
            this.TxLoopCheckBox.UseVisualStyleBackColor = true;
            // 
            // TxFileCheckBox
            // 
            this.TxFileCheckBox.AutoSize = true;
            this.TxFileCheckBox.Location = new System.Drawing.Point(12, 35);
            this.TxFileCheckBox.Name = "TxFileCheckBox";
            this.TxFileCheckBox.Size = new System.Drawing.Size(96, 16);
            this.TxFileCheckBox.TabIndex = 1;
            this.TxFileCheckBox.Text = "发送存储文件";
            this.TxFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // clearSendLabel
            // 
            this.clearSendLabel.AutoSize = true;
            this.clearSendLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clearSendLabel.Location = new System.Drawing.Point(10, 107);
            this.clearSendLabel.Name = "clearSendLabel";
            this.clearSendLabel.Size = new System.Drawing.Size(53, 12);
            this.clearSendLabel.TabIndex = 0;
            this.clearSendLabel.Text = "清除输入";
            this.clearSendLabel.Click += new System.EventHandler(this.clearSendLabel_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.rxFrameCountLabel,
            this.txFrameCountLabel,
            this.clearNumButton,
            this.rxLocalFrameCountLabel,
            this.txLocalFrameCountLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 435);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1015, 23);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(56, 18);
            this.toolStripStatusLabel1.Text = "串口状态";
            // 
            // rxFrameCountLabel
            // 
            this.rxFrameCountLabel.Name = "rxFrameCountLabel";
            this.rxFrameCountLabel.Size = new System.Drawing.Size(75, 18);
            this.rxFrameCountLabel.Text = "已接收0字节";
            // 
            // txFrameCountLabel
            // 
            this.txFrameCountLabel.Name = "txFrameCountLabel";
            this.txFrameCountLabel.Size = new System.Drawing.Size(75, 18);
            this.txFrameCountLabel.Text = "已发送0字节";
            // 
            // clearNumButton
            // 
            this.clearNumButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.clearNumButton.Image = ((System.Drawing.Image)(resources.GetObject("clearNumButton.Image")));
            this.clearNumButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearNumButton.Name = "clearNumButton";
            this.clearNumButton.ShowDropDownArrow = false;
            this.clearNumButton.Size = new System.Drawing.Size(60, 21);
            this.clearNumButton.Text = "复位计数";
            this.clearNumButton.Click += new System.EventHandler(this.clearNumButton_Click);
            // 
            // rxLocalFrameCountLabel
            // 
            this.rxLocalFrameCountLabel.Name = "rxLocalFrameCountLabel";
            this.rxLocalFrameCountLabel.Size = new System.Drawing.Size(87, 18);
            this.rxLocalFrameCountLabel.Text = "原已接收0字节";
            this.rxLocalFrameCountLabel.Visible = false;
            // 
            // txLocalFrameCountLabel
            // 
            this.txLocalFrameCountLabel.Name = "txLocalFrameCountLabel";
            this.txLocalFrameCountLabel.Size = new System.Drawing.Size(87, 18);
            this.txLocalFrameCountLabel.Text = "原已发送0字节";
            this.txLocalFrameCountLabel.Visible = false;
            // 
            // SendTimer
            // 
            this.SendTimer.Tick += new System.EventHandler(this.SendTimer_Tick);
            // 
            // RxDataTextBox
            // 
            this.RxDataTextBox.Location = new System.Drawing.Point(6, 20);
            this.RxDataTextBox.Multiline = true;
            this.RxDataTextBox.Name = "RxDataTextBox";
            this.RxDataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RxDataTextBox.Size = new System.Drawing.Size(511, 262);
            this.RxDataTextBox.TabIndex = 15;
            // 
            // RxDataGroupBox
            // 
            this.RxDataGroupBox.Controls.Add(this.RxDataTextBox);
            this.RxDataGroupBox.Location = new System.Drawing.Point(163, 12);
            this.RxDataGroupBox.Name = "RxDataGroupBox";
            this.RxDataGroupBox.Size = new System.Drawing.Size(523, 291);
            this.RxDataGroupBox.TabIndex = 16;
            this.RxDataGroupBox.TabStop = false;
            this.RxDataGroupBox.Text = "串口数据接收区";
            // 
            // SendDataGroupBox
            // 
            this.SendDataGroupBox.Controls.Add(this.sendButton);
            this.SendDataGroupBox.Controls.Add(this.sendTextBox);
            this.SendDataGroupBox.Location = new System.Drawing.Point(163, 309);
            this.SendDataGroupBox.Name = "SendDataGroupBox";
            this.SendDataGroupBox.Size = new System.Drawing.Size(523, 116);
            this.SendDataGroupBox.TabIndex = 17;
            this.SendDataGroupBox.TabStop = false;
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(434, 14);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(83, 96);
            this.sendButton.TabIndex = 1;
            this.sendButton.Text = "发送";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // sendTextBox
            // 
            this.sendTextBox.Location = new System.Drawing.Point(6, 14);
            this.sendTextBox.Multiline = true;
            this.sendTextBox.Name = "sendTextBox";
            this.sendTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.sendTextBox.Size = new System.Drawing.Size(422, 96);
            this.sendTextBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataBagCheckBox);
            this.groupBox1.Controls.Add(this.nodeTextBox);
            this.groupBox1.Location = new System.Drawing.Point(692, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(156, 123);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "节点地址设置";
            // 
            // dataBagCheckBox
            // 
            this.dataBagCheckBox.AutoSize = true;
            this.dataBagCheckBox.Location = new System.Drawing.Point(32, 28);
            this.dataBagCheckBox.Name = "dataBagCheckBox";
            this.dataBagCheckBox.Size = new System.Drawing.Size(84, 16);
            this.dataBagCheckBox.TabIndex = 1;
            this.dataBagCheckBox.Text = "打包数据帧";
            this.dataBagCheckBox.UseVisualStyleBackColor = true;
            // 
            // nodeTextBox
            // 
            this.nodeTextBox.Location = new System.Drawing.Point(16, 69);
            this.nodeTextBox.Multiline = true;
            this.nodeTextBox.Name = "nodeTextBox";
            this.nodeTextBox.Size = new System.Drawing.Size(100, 21);
            this.nodeTextBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.frameLinkCheckBox);
            this.groupBox2.Controls.Add(this.frameLinkNumericUpDown);
            this.groupBox2.Location = new System.Drawing.Point(692, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(156, 133);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "连帧设置";
            // 
            // frameLinkCheckBox
            // 
            this.frameLinkCheckBox.AutoSize = true;
            this.frameLinkCheckBox.Checked = true;
            this.frameLinkCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.frameLinkCheckBox.Location = new System.Drawing.Point(32, 47);
            this.frameLinkCheckBox.Name = "frameLinkCheckBox";
            this.frameLinkCheckBox.Size = new System.Drawing.Size(96, 16);
            this.frameLinkCheckBox.TabIndex = 1;
            this.frameLinkCheckBox.Text = "连帧个数选择";
            this.frameLinkCheckBox.UseVisualStyleBackColor = true;
            // 
            // frameLinkNumericUpDown
            // 
            this.frameLinkNumericUpDown.Location = new System.Drawing.Point(36, 74);
            this.frameLinkNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frameLinkNumericUpDown.Name = "frameLinkNumericUpDown";
            this.frameLinkNumericUpDown.Size = new System.Drawing.Size(80, 21);
            this.frameLinkNumericUpDown.TabIndex = 0;
            this.frameLinkNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.frameNumCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(692, 309);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(156, 110);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "序号设置";
            // 
            // frameNumCheckBox
            // 
            this.frameNumCheckBox.AutoSize = true;
            this.frameNumCheckBox.Location = new System.Drawing.Point(38, 49);
            this.frameNumCheckBox.Name = "frameNumCheckBox";
            this.frameNumCheckBox.Size = new System.Drawing.Size(78, 16);
            this.frameNumCheckBox.TabIndex = 0;
            this.frameNumCheckBox.Text = "固定序号6";
            this.frameNumCheckBox.UseVisualStyleBackColor = true;
            // 
            // multicatGroupBox
            // 
            this.multicatGroupBox.Controls.Add(this.mulCheckBox);
            this.multicatGroupBox.Controls.Add(this.mulNodeTextBox);
            this.multicatGroupBox.Controls.Add(this.label3);
            this.multicatGroupBox.Controls.Add(this.mulClrButton);
            this.multicatGroupBox.Controls.Add(this.mulAddButton);
            this.multicatGroupBox.Controls.Add(this.mulTextBox);
            this.multicatGroupBox.Location = new System.Drawing.Point(854, 12);
            this.multicatGroupBox.Name = "multicatGroupBox";
            this.multicatGroupBox.Size = new System.Drawing.Size(149, 262);
            this.multicatGroupBox.TabIndex = 21;
            this.multicatGroupBox.TabStop = false;
            this.multicatGroupBox.Text = "组播数据帧";
            // 
            // mulTextBox
            // 
            this.mulTextBox.Location = new System.Drawing.Point(25, 69);
            this.mulTextBox.Name = "mulTextBox";
            this.mulTextBox.Size = new System.Drawing.Size(100, 21);
            this.mulTextBox.TabIndex = 0;
            // 
            // mulAddButton
            // 
            this.mulAddButton.Location = new System.Drawing.Point(10, 108);
            this.mulAddButton.Name = "mulAddButton";
            this.mulAddButton.Size = new System.Drawing.Size(56, 23);
            this.mulAddButton.TabIndex = 1;
            this.mulAddButton.Text = "添加";
            this.mulAddButton.UseVisualStyleBackColor = true;
            this.mulAddButton.Click += new System.EventHandler(this.mulAddButton_Click);
            // 
            // mulClrButton
            // 
            this.mulClrButton.Location = new System.Drawing.Point(81, 108);
            this.mulClrButton.Name = "mulClrButton";
            this.mulClrButton.Size = new System.Drawing.Size(56, 23);
            this.mulClrButton.TabIndex = 2;
            this.mulClrButton.Text = "清空";
            this.mulClrButton.UseVisualStyleBackColor = true;
            this.mulClrButton.Click += new System.EventHandler(this.mulClrButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "需要发送的节点地址：";
            // 
            // mulNodeTextBox
            // 
            this.mulNodeTextBox.Location = new System.Drawing.Point(14, 170);
            this.mulNodeTextBox.Multiline = true;
            this.mulNodeTextBox.Name = "mulNodeTextBox";
            this.mulNodeTextBox.ReadOnly = true;
            this.mulNodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mulNodeTextBox.Size = new System.Drawing.Size(123, 86);
            this.mulNodeTextBox.TabIndex = 4;
            // 
            // mulCheckBox
            // 
            this.mulCheckBox.AutoSize = true;
            this.mulCheckBox.Location = new System.Drawing.Point(34, 28);
            this.mulCheckBox.Name = "mulCheckBox";
            this.mulCheckBox.Size = new System.Drawing.Size(84, 16);
            this.mulCheckBox.TabIndex = 5;
            this.mulCheckBox.Text = "发送组播帧";
            this.mulCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 458);
            this.Controls.Add(this.multicatGroupBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SendDataGroupBox);
            this.Controls.Add(this.RxDataGroupBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.sendSetGroupBox);
            this.Controls.Add(this.rxSetGroupBox);
            this.Controls.Add(this.serialPortDebugGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "串口收发调试工具（集中器）";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.serialPortDebugGroupBox.ResumeLayout(false);
            this.serialPortDebugGroupBox.PerformLayout();
            this.rxSetGroupBox.ResumeLayout(false);
            this.rxSetGroupBox.PerformLayout();
            this.sendSetGroupBox.ResumeLayout(false);
            this.sendSetGroupBox.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.RxDataGroupBox.ResumeLayout(false);
            this.RxDataGroupBox.PerformLayout();
            this.SendDataGroupBox.ResumeLayout(false);
            this.SendDataGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameLinkNumericUpDown)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.multicatGroupBox.ResumeLayout(false);
            this.multicatGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox serialPortDebugGroupBox;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox SerialPortNum;
        public System.Windows.Forms.ComboBox BaudRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button serialPortDebug;
        private System.Windows.Forms.GroupBox rxSetGroupBox;
        private System.Windows.Forms.Label clearRxLabel;
        private System.Windows.Forms.CheckBox RxFileCheckBox;
        private System.Windows.Forms.GroupBox sendSetGroupBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel rxFrameCountLabel;
        private System.Windows.Forms.ToolStripStatusLabel txFrameCountLabel;
        private System.Windows.Forms.ToolStripDropDownButton clearNumButton;
        public System.Windows.Forms.Timer SendTimer;
        private System.Windows.Forms.TextBox RxDataTextBox;
        private System.Windows.Forms.GroupBox RxDataGroupBox;
        private System.Windows.Forms.GroupBox SendDataGroupBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.TextBox sendTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox timeTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox TxLoopCheckBox;
        private System.Windows.Forms.CheckBox TxFileCheckBox;
        private System.Windows.Forms.Label clearSendLabel;
        public System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox dataBagCheckBox;
        private System.Windows.Forms.TextBox nodeTextBox;
        private System.Windows.Forms.ToolStripStatusLabel rxLocalFrameCountLabel;
        private System.Windows.Forms.ToolStripStatusLabel txLocalFrameCountLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown frameLinkNumericUpDown;
        private System.Windows.Forms.CheckBox frameLinkCheckBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox frameNumCheckBox;
        private System.Windows.Forms.GroupBox multicatGroupBox;
        private System.Windows.Forms.Button mulClrButton;
        private System.Windows.Forms.Button mulAddButton;
        private System.Windows.Forms.TextBox mulTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox mulCheckBox;
        private System.Windows.Forms.TextBox mulNodeTextBox;
    }
}


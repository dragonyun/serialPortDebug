using AMS.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 串口收发调试工具
{
    public partial class MainForm : Form
    {
        #region 公共属性
        public Profile TxDataFile;     //记录发送数据信息
        public Profile RxDataFile;     //记录接收数据信息
        public int sendNum;
        public int recNum;
        public int frameNum6968;
        //public string[] framegroup;
        public List<string> framegroup = new List<string>();
        #endregion

        #region 私有属性
        private int _rxFrameCount;
        private int _txFrameCount;
        private string _rxDataText;
        private int _rxLocalFrameCount;//去除68,69帧的长度


        private int _txLocalFrameCount;


        #endregion


        #region 私有属性的公共存取方法
        public int RxFrameCount
        {
            get { return _rxFrameCount; }
            set
            {
                _rxFrameCount = value;
                if (_rxFrameCount == 0)
                {
                    rxFrameCountLabel.Text = "已接收0字节";
                }
                else
                {
                    rxFrameCountLabel.Text = "已接收" + _rxFrameCount.ToString() + "字节";
                }
            }
        }
        public int TxFrameCount
        {
            get { return _txFrameCount; }
            set
            {
                _txFrameCount = value;
                if (_txFrameCount == 0)
                {
                    txFrameCountLabel.Text = "已发送0字节";
                }
                else
                {
                    txFrameCountLabel.Text = "已发送" + _txFrameCount.ToString() + "字节";
                }
            }
        }
        public string RxDataText
        {
            get { return _rxDataText; }
            set 
            { 
                _rxDataText = value;
                RxDataTextBox.AppendText("\r\n" + _rxDataText);
                //RxDataTextBox.Text = RxDataTextBox.Text + "\r\n" + _rxDataText;
            }
        }
        public int RxLocalFrameCount
        {
            get { return _rxLocalFrameCount; }
            set 
            { 
                _rxLocalFrameCount = value;
                if (_rxLocalFrameCount == 0)
                {
                    rxLocalFrameCountLabel.Text = "原已接收0字节";
                }
                else
                {
                    rxLocalFrameCountLabel.Text = "原已接收" + _rxLocalFrameCount.ToString() + "字节";
                }
            }
        }
        public int TxLocalFrameCount
        {
            get { return _txLocalFrameCount; }
            set 
            { 
                _txLocalFrameCount = value;
                if (_txLocalFrameCount == 0)
                {
                    txLocalFrameCountLabel.Text = "原已发送0字节";
                }
                else
                {
                    txLocalFrameCountLabel.Text = "原已发送" + _txLocalFrameCount.ToString() + "字节";
                }
            }
        }
        #endregion


        /// <summary>
        /// 构造方法
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            string[] ports = SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                this.SerialPortNum.Items.AddRange(ports);
                this.SerialPortNum.Items.Add("自动");
                this.SerialPortNum.SelectedIndex = this.SerialPortNum.Items.Count - 1;
                this.toolStripStatusLabel1.Text = "发现串口";
            }
            else
            {
                this.toolStripStatusLabel1.Text = "未发现串口";
                this.serialPortDebug.Enabled = false;
            }

            Daemon.SerialPortDebugEnable = false;
            Daemon.SocketDebugEnable = false;

            this.sendButton.Enabled = false;

            this.BaudRate.SelectedIndex = this.BaudRate.Items.Count - 1;//默认选择最高波特率

            framegroup.Clear();
        }

        /// <summary>
        /// 加载窗体时运行方法，配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                AnalysisProcess.ReceivedTestDataEvent += AnalysisProcess_ReceivedTestDataEvent;
                
                //File.Delete(".\\xlsfile.ini");
                TxDataFile = new Ini(".\\TxDataFile.ini");//在当前目录下创建配置文件

                RxDataFile = new Ini(".\\RxDataFile.ini");//在当前目录下创建配置文件

                /* 载入配置文件中的值，若不存在则使用控件的默认值 */
                //发送数据编号
                string Number = "0";
                Number = TxDataFile.GetValue("send", "Number", Number);
                if (Number != "")
                {
                    sendNum = Convert.ToInt32(Number);
                }
                //接收数据编号
                Number = "0";
                Number = RxDataFile.GetValue("receive", "Number", Number);
                if (Number != "")
                {
                    recNum = Convert.ToInt32(Number);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取配置文件时出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 关闭窗体时运行方法，关闭委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnalysisProcess.ReceivedTestDataEvent -= AnalysisProcess_ReceivedTestDataEvent;
        }

        #region 主窗体控件初始化
        /// <summary>
        /// 串口初始化.
        /// </summary>
        /// <param name="portName">端口名.</param>
        /// <param name="baudRate">波特率.</param>
        private void serialPortInit(string portName, int baudRate)
        {
            serialPort1 = new SerialPort();
            serialPort1.PortName = portName;
            serialPort1.BaudRate = baudRate;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = StopBits.One;
            serialPort1.Parity = Parity.None;
            serialPort1.ReadTimeout = 1000;
            serialPort1.WriteTimeout = 1000;
        }

        /// <summary>
        ///  连接成功后主窗体控件状态.
        /// </summary>
        private void widgetStateWhileConnected()
        {
            this.SerialPortNum.Enabled = false;
            this.BaudRate.Enabled = false;
            this.sendButton.Enabled = true;
            //sendNum = 0;
            //recNum = 0;
        }

        /// <summary>
        ///  连接断开成功后主窗体控件状态.
        /// </summary>
        private void widgetStateWhileClosed()
        {
            this.SerialPortNum.Enabled = true;
            this.BaudRate.Enabled = true;
            this.sendButton.Enabled = false;
            RxFrameCount = 0;
            TxFrameCount = 0;
            RxLocalFrameCount = 0;
            TxLocalFrameCount = 0;
        }

        #endregion

        /// <summary>
        /// 串口连接方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialPortDebug_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(SerialPortNum.Text) && !String.IsNullOrEmpty(BaudRate.Text))
            {
                if (Daemon.SerialPortDebugEnable == false)
                {
                    try
                    {
                        /* 选择帧格式 */
                        PacketProcess.packetFrameType = PacketProcess.FrameType.Serial;

                        int baudRate = Convert.ToInt32(this.BaudRate.Text);//波特率是手动设置

                        if (String.Equals(this.SerialPortNum.Text, "自动"))
                        {
                            //自动搜索串口无效
                            return;
                        }
                        else//手动连接串口
                        {
                            serialPort1.Close();
                            serialPortInit(this.SerialPortNum.Text, baudRate);
                        }

                        serialPort1.Open();
                        Daemon.SerialPortDebugEnable = true;
                        Daemon.DaemonFrameType = Daemon.FrameType.Serial;
                        serialPortDebug.Text = "断开";
                        toolStripStatusLabel1.Text = "串口" + serialPort1.PortName + "连接成功";
                        
                        widgetStateWhileConnected();

                        ThreadPool.QueueUserWorkItem(new WaitCallback(Daemon.BackSend), null);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(Daemon.BackReceive), null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("串口连接时出错: " + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        if (serialPort1.IsOpen)
                        {
                            serialPort1.Close();
                            serialPort1.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("串口断开时出错: " + ex.Message + "请重新连接!");
                    }
                    finally
                    {
                        Daemon.SerialPortDebugEnable = false;
                        Daemon.DaemonFrameType = Daemon.FrameType.Unknown;
                        serialPortDebug.Text = "连接";
                        toolStripStatusLabel1.Text = "串口连接已断开";

                        //清除数据
                        PacketProcess.GatewayType = NetCommunicationFrame.GatewayType.UnknowType;
                        
                        widgetStateWhileClosed();
                        Queue.Clear();
                    }

                }
            }
            else
            {
                MessageBox.Show("请选择串口名称,如COM1.");
                return;
            }
        }
        #region 发送方法、接收方法、定时器方法
        /// <summary>
        /// 发送按键方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendButton_Click(object sender, EventArgs e)
        {
            //发送不能为空
            if (this.sendTextBox.Text == "")
            {
                return;
            }
            try
            {
                if (this.sendButton.Text == "发送")
                {
                    if (this.TxLoopCheckBox.Checked)
                    {
                        this.SendTimer.Interval = Convert.ToInt32(this.timeTextBox.Text);
                        this.SendTimer.Start();
                        this.sendButton.Text = "停止发送";
                    }

                    if (string.IsNullOrEmpty(sendTextBox.Text))
                    {
                        MessageBox.Show("请填写要发送的数据！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    //将接收到的字符串数据转换为字节数据数据
                    byte[] UserData;
                    byte[] linkData;
                    string[] data = sendTextBox.Text.Split(new char[] { ' ' });
                    data = data.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                    UserData = new byte[data.Length];
                    UserData = Array.ConvertAll(data, s => Convert.ToByte(s, 16));

                    linkData = UserData;

                    if (mulCheckBox.Checked)
                    {
                        if (framegroup.Count < 1)
                        {
                            MessageBox.Show("节点地址不存在");
                            return;
                        }
                        int num = framegroup.Count;
                        for (int i = 0; i < num; i++)
                        {
                            byte[] nodemac = Character.StringToBytes(framegroup[i]);
                            frameNum6968++;
                            if (frameNum6968 % 256 == 0)
                            {
                                frameNum6968++;
                            }
                            UserData = PacketProcess.SendCommand_68_serialtest(linkData, (byte)linkData.Length, framegroup[i], frameNum6968, false);
                            Queue.Push(UserData);
                        }

                        return;
                    }

                    //打包数据帧
                    if (dataBagCheckBox.Checked)
                    {
                        byte[] nodeMac = Character.StringToBytes(nodeTextBox.Text);
                        if (nodeMac.Length != 2)
                        {
                            MessageBox.Show("节点地址不正确");
                            return;
                        }
                       
                        frameNum6968++;
                        if (frameNum6968 % 256 == 0)
                        {
                            frameNum6968++;
                        }
                       
                        //固定帧序号
                        if (frameNumCheckBox.Checked)
                        {
                            //frame.FrameCount = (ushort)6;
                        }
                        

                        UserData = PacketProcess.SendCommand_68_serialtest(UserData, (byte)UserData.Length, nodeTextBox.Text, frameNum6968, frameNumCheckBox.Checked);
                    }

                    //发送数据
                    if (frameLinkCheckBox.Checked)
                    {

                        int num = Convert.ToInt32(frameLinkNumericUpDown.Value);
                        byte[] array = UserData;

                        UserData = new byte[array.Length * num];
                        for (int i = 0; i < num; i++)
                        {
                            if (dataBagCheckBox.Checked)
                            {
                               
                                frameNum6968++;
                                if (frameNum6968 % 256 == 0)
                                {
                                    frameNum6968++;
                                }

                                
                                array = PacketProcess.SendCommand_68_serialtest(linkData, (byte)linkData.Length, nodeTextBox.Text, frameNum6968, frameNumCheckBox.Checked);
                                

                            }
                            array.CopyTo(UserData, array.Length * i);    
                        }
                        Queue.Push(UserData);
                    }
                    else
                    {
                        Queue.Push(UserData);
                    }
                    //获取时间
                    DateTime dt = System.DateTime.Now;
                    string date = dt.ToString("HH:mm:ss fff");
                    Console.WriteLine(date);
                    //string leng = Character.BytesToString(sendByte, 0, sendByte.Length - 1, true);
                    //写入文件
                    if (this.TxFileCheckBox.Checked)
                    {
                        TxDataFile.SetValue("send", "Number", sendNum.ToString());
                        TxDataFile.SetValue("send", "data" + sendNum.ToString(), Character.BytesToString(UserData,0,UserData.Length-1,true) + " + " + date);
                        sendNum++;
                    }

                }
                else if (this.sendButton.Text == "停止发送")
                {
                    this.SendTimer.Stop();
                    this.sendButton.Text = "发送";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(" send error"+ex.ToString());
            }
        }


        /// <summary>
        /// 定时器方法，用于循环发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendTimer_Tick(object sender, EventArgs e)
        {
            //将接收到的字符串数据转换为字节数据数据
            byte[] UserData;
            byte[] sourData;
            
            string[] data = sendTextBox.Text.Split(new char[] { ' ' });
            data = data.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            UserData = new byte[data.Length];
            UserData = Array.ConvertAll(data, s => Convert.ToByte(s, 16));
            sourData = UserData;

            if (mulCheckBox.Checked)
            {
                if (framegroup.Count < 1)
                {
                    MessageBox.Show("节点地址不存在");
                    return;
                }
                int num = framegroup.Count;
                for (int i = 0; i < num; i++)
                {
                    byte[] nodemac = Character.StringToBytes(framegroup[i]);
                    frameNum6968++;
                    if (frameNum6968 % 256 == 0)
                    {
                        frameNum6968++;
                    }
                    UserData = PacketProcess.SendCommand_68_serialtest(sourData, (byte)sourData.Length, framegroup[i], frameNum6968, false);
                    Queue.Push(UserData);
                }
                return;
            }
            //打包数据帧
            if (dataBagCheckBox.Checked)
            {
                
                frameNum6968++;
                if (frameNum6968 % 256 == 0)
                {
                    frameNum6968++;
                }
               
                UserData = PacketProcess.SendCommand_68_serialtest(sourData, (byte)sourData.Length, nodeTextBox.Text, frameNum6968, frameNumCheckBox.Checked);
            }

            //发送数据
            Queue.Push(UserData);
            

            //获取时间
            DateTime dt = System.DateTime.Now;
            string date = dt.ToString("HH:mm:ss fff");
            Console.WriteLine(date);

            //写入文件
            if (this.TxFileCheckBox.Checked)
            {
                TxDataFile.SetValue("send", "Number", sendNum.ToString());
                TxDataFile.SetValue("send", "data" + sendNum.ToString(), Character.BytesToString(UserData, 0, UserData.Length - 1, true) + " + " + date);
                sendNum++;
            }
        }

        /// <summary>
        /// 接收数据处理方法
        /// </summary>
        /// <param name="e"></param>
        void AnalysisProcess_ReceivedTestDataEvent(AnalysisProcess.FrameEventArgs e)
        {
            byte[] test = e.Payload;
            
            string leng = Character.BytesToString(test, 0, test.Length - 1, true);
            
            //获取时间
            DateTime dt = System.DateTime.Now;
            string date = dt.ToString("HH:mm:ss fff");
            //Console.WriteLine(date);

            //窗体显示
            Program.mainForm.RxDataText = leng + date;
            Console.WriteLine(leng);

            //写入文件
            if (this.RxFileCheckBox.Checked)
            {
                RxDataFile.SetValue("receive", "Number", recNum.ToString());
                RxDataFile.SetValue("receive", "data" + recNum.ToString(), leng + " + " + date);
                recNum++;
            }
        }

        #endregion

        #region 清空 与 复位

        //清空接收区
        private void clearRxLabel_Click(object sender, EventArgs e)
        {
            this.RxDataTextBox.Text = "";
        }

        //清空发送区
        private void clearSendLabel_Click(object sender, EventArgs e)
        {
            this.sendTextBox.Text = "";
        }

        //复位计数
        private void clearNumButton_Click(object sender, EventArgs e)
        {
            RxFrameCount = 0;
            TxFrameCount = 0;
            RxLocalFrameCount = 0;
            TxLocalFrameCount = 0;
        }

        #endregion

        private void mulAddButton_Click(object sender, EventArgs e)
        {
            byte[] nodeMac = Character.StringToBytes(mulTextBox.Text);
            if (nodeMac.Length != 2)
            {
                MessageBox.Show("节点地址不正确");
                return;
            }
            framegroup.Add(mulTextBox.Text);
            mulNodeTextBox.AppendText(" " + mulTextBox.Text);
        }

        private void mulClrButton_Click(object sender, EventArgs e)
        {
            framegroup.Clear();
            mulNodeTextBox.Text = "";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 串口收发调试工具
{
    /// <summary>
    /// 在后台运行的方法
    /// </summary>
    class Daemon
    {
        #region 公共属性
        public static FrameType DaemonFrameType = FrameType.Unknown;//标识当前是什么模式下的通信（外网，内网，串口，未知）
        public enum FrameType
        {
            OutterNet,
            InnerNet,
            Serial,
            Unknown,
        }
        #endregion

        public static bool exploxe = false;
        public static byte frame_order_num_68 = 0x00;     //命令帧序号，发送数据帧时，需要携带帧序号

        #region 私有属性
        private static bool _serialPortDebugEnable;
        private static bool _socketDebugEnable;
        private static bool _sendFileActivity;
        private static bool _rxHoldUp = false;
        private static int _txSerialFrameIntervalTime = 50;
        private static int _txSocketFrameIntervalTime = 200;
        private static int _txTempExtraIntervalTime = 0;
        #endregion

        #region 私有属性的公共存取方法
        /// <summary>
        /// Gets or sets a value indicating whether [串口调试状态标志].
        /// </summary>
        /// <value>
        /// <c>true</c> if [串口调试状态标志]; otherwise, <c>false</c>.
        /// </value>
        public static bool SerialPortDebugEnable
        {
            get { return _serialPortDebugEnable; }
            set { _serialPortDebugEnable = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [网络调试状态标志].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [网络调试状态标志]; otherwise, <c>false</c>.
        /// </value>
        public static bool SocketDebugEnable
        {
            get { return _socketDebugEnable; }
            set { _socketDebugEnable = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [发送文件的状态标志].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [发送文件的状态标志]; otherwise, <c>false</c>.
        /// </value>
        public static bool SendFileActivity
        {
            get { return _sendFileActivity; }
            set { _sendFileActivity = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [拦截接收数据].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [rx hold up]; otherwise, <c>false</c>.
        /// </value>
        public static bool RxHoldUp
        {
            get { return _rxHoldUp; }
            set { _rxHoldUp = value; }
        }

        /// <summary>
        /// Gets or sets [串口帧发送间隔时间].
        /// </summary>
        /// <value>
        /// The tx frame interval time.
        /// </value>
        public static int TxSerialFrameIntervalTime
        {
            get { return _txSerialFrameIntervalTime; }
            set { _txSerialFrameIntervalTime = value; }
        }

        /// <summary>
        /// Gets or sets [Socket帧发送间隔时间].
        /// </summary>
        /// <value>
        /// The tx frame interval time.
        /// </value>
        public static int TxSocketFrameIntervalTime
        {
            get { return _txSocketFrameIntervalTime; }
            set { _txSocketFrameIntervalTime = value; }
        }

        /// <summary>
        /// 设置发送帧的额外间隔时间。用于发送某些需要下位机较长时间处理的帧，仅对下一帧有效。
        /// </summary>
        /// <value>
        /// The tx temporary extra interval time.
        /// </value>
        public static int TxTempExtraIntervalTime
        {
            set { _txTempExtraIntervalTime = value; }
        }
        #endregion

        #region 委托与事件
        delegate void DataAnalysisInvoke(byte[] receiveByte);//声明委托 对应的是接收检查方法
        public delegate void FrameByte(FrameEventArgs e);//声明委托
        public static event FrameByte SendedAFrameEvent; //声明事件
        public static event FrameByte ReceivedFrameEvent; //声明事件

        public class FrameEventArgs : EventArgs
        {
            public readonly byte[] Payload;
            public FrameEventArgs(byte[] payload)
            {
                this.Payload = payload;
            }
        }
        #endregion

        #region 后台运行的接收或发送方法
        /// <summary>
        /// 发送帧探测集中器与节点信息.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void BackConnectConcentrator(object sender)
        {
            PacketProcess.OnLineEdCount = 0;//节点个数清零

            Queue.Push(PacketProcess.SendCommand_77(0x04, PacketProcess.readNodeState, new byte[] { 0x00 }));
            
            Queue.Push(PacketProcess.SendCommand_68(PacketProcess.ConcentratorMac, 0x04, PacketProcess.readNetState, new byte[] { 0x00 }, frame_order_num_68));//查询网络当前状态,查询当前节点数
            frame_order_num_68++;
        }

        /// <summary>
        /// 发送堆栈里的所有帧.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void BackSend(Object sender)
        {
            while (DaemonFrameType != FrameType.Unknown)//当连接存在时循环运行
            {
                try
                {
                    while (Queue.length > 0)//当堆栈不为空时循环运行
                    {
                        byte[] sendByte = Queue.Pop();

                        
                        /* 发送一帧事件 */
                        //FrameEventArgs e = new FrameEventArgs(sendByte); //建立FrameEventArgs 对象
                        //if (SendedAFrameEvent != null)// 如果有对象注册
                        //{
                        //    SendedAFrameEvent(e); // 调用所有注册对象的方法
                        //}

                        //打印信息（测试）
                        //string leng = Character.BytesToString(sendByte, 0, sendByte.Length - 1, true);
                        //Console.WriteLine("发送完整的帧内容是：" + leng);
                        //Console.WriteLine();

                        if (_serialPortDebugEnable)//通过串口连接
                        {
                            Program.mainForm.serialPort1.Write(sendByte, 0, sendByte.Length);
                            //Thread.Sleep(_txSerialFrameIntervalTime + _txTempExtraIntervalTime);//发送帧的间隔时间
                            Thread.Sleep(10);
                            frame_order_num_68++;
                        }
                        else
                        {
                            Thread.Sleep(20);
                            break;
                        }

                        Program.mainForm.TxFrameCount += sendByte.Length;//统计发送的帧数量
                        Program.mainForm.TxLocalFrameCount += sendByte.Length;
                        if (sendByte.Length > 35 && sendByte[0] == 0x69)
                        {
                            Program.mainForm.TxFrameCount -= 36;
                            Program.mainForm.TxLocalFrameCount -= 36;
                        }
                        

                    }
                    Thread.Sleep(30);//每50ms读取一次堆栈：减少while循环次数，降低CPU使用时间
                }
                catch (Exception ex)
                {
                    //MainForm.WriteLog(ex, @".\\1.log");
                    Queue.Clear();
                    return;
                }
            }
        }

        /// <summary>
        /// 接收来自串口或网络的所有帧.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void BackReceive(Object sender)
        {
            int len = 0;
            while (DaemonFrameType != FrameType.Unknown)//当连接存在时循环运行
            {
                try
                {
                    if (_serialPortDebugEnable)//通过串口连接
                    {
                        len = 0;
                        
                        len = Program.mainForm.serialPort1.BytesToRead;
                        if (len > 0)
                        {
                            
                            Thread.Sleep(10);
                            len = Program.mainForm.serialPort1.BytesToRead;
                            byte[] receiveByte = new byte[len];
                            Program.mainForm.serialPort1.Read(receiveByte, 0, receiveByte.Length);//同步模式，线程被阻塞直到缓冲区有相应的数据

                            if (!_rxHoldUp)
                            {
                                Program.mainForm.Invoke(new DataAnalysisInvoke(AnalysisProcess.delegtReceiveData), receiveByte);
                            }
                            /* 接收帧事件 */
                            FrameEventArgs e = new FrameEventArgs(receiveByte); //建立FrameEventArgs 对象
                            if (ReceivedFrameEvent != null)// 如果有对象注册
                            {
                                ReceivedFrameEvent(e); // 调用所有注册对象的方法
                            }

                            Program.mainForm.RxFrameCount += receiveByte.Length;//统计发送的帧数量
                            Program.mainForm.RxLocalFrameCount += receiveByte.Length;
                            if (receiveByte.Length > 35 && receiveByte[0] == 0x69)
                            {
                                Program.mainForm.RxLocalFrameCount -= 36;
                            }

                        }
                        Thread.Sleep(5);
                    }

                }
                catch (Exception ex)//此异常可能由用户主动断开串口或Socket连接引发
                {
                    //MainForm.WriteLog(ex, @".\\1.log");
                    Queue.Clear();
                    return;
                }
            }
        }
       
        #endregion

     

      
    }
}

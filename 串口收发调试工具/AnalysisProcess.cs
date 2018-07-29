using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 串口收发调试工具
{
    class AnalysisProcess
    {
        public static int recframenum = 0;//未用
        public static int Node_frame;//未用

        #region 委托与事件
        /*声明委托*/
        public delegate void FrameByte(FrameEventArgs e);
        public delegate void CommandFrame(CommandFrameEventArgs e);
        public delegate void ReplyFrame(ReplyFrameEventArgs e);
        public delegate void AlarmFrame(AlarmFrameEventArgs e);
        public delegate void DataFrame(DataFrameEventArgs e);
        /*声明事件*/
        public static event FrameByte ReceivedACorrectFrameEvent;
        public static event FrameByte ReceivedTestDataEvent;
        public static event CommandFrame ReadAllNodesStatusEvent;
        public static event CommandFrame ReadLqiEvent;
        public static event CommandFrame ReadRssiEvent;
        public static event CommandFrame WriteEdBeginUpdateEvent;
        public static event ReplyFrame ReceiveReplyFrameEvent;
        public static event AlarmFrame ReceiveAlarmFrameEventEvent;
        public static event DataFrame ReceiveDataFrameEvent;

        // 定义FrameEventArgs类，传递给Observer所感兴趣的信息,继承EventArgs
        public class FrameEventArgs : EventArgs
        {
            public readonly byte[] Payload;
            public FrameEventArgs(byte[] payload)
            {
                this.Payload = payload;
            }
        }

        // 定义CommandFrameEventArgs类，传递给Observer所感兴趣的信息,继承EventArgs
        public class CommandFrameEventArgs : EventArgs
        {
            public readonly string NodeMac;
            public readonly byte[] CommandPayload;
            public readonly SerialPortFrame.Communication CommunicationWay;
            public CommandFrameEventArgs(byte[] commandPayload)
            {
                this.CommandPayload = commandPayload;
            }
            public CommandFrameEventArgs(SerialPortFrame.Communication communicationWay, byte[] commandPayload)
            {
                this.CommandPayload = commandPayload;
                this.CommunicationWay = communicationWay;
            }
            public CommandFrameEventArgs(SerialPortFrame.Communication communicationWay, byte[] commandPayload,string nodeMac)
            {
                this.CommandPayload = commandPayload;
                this.CommunicationWay = communicationWay;
                this.NodeMac = nodeMac;
            }
        }

        // 定义ReplyFrameEventArgs类，传递给Observer所感兴趣的信息,继承EventArgs
        public class ReplyFrameEventArgs : EventArgs
        {
            public readonly ushort FrameCounter;
            public ReplyFrameEventArgs(ushort frameCounter)
            {
                this.FrameCounter = frameCounter;
            }
        }

        // 定义AlarmFrameEventArgs类，传递给Observer所感兴趣的信息,继承EventArgs
        public class AlarmFrameEventArgs : EventArgs
        {
            public readonly byte[] AlarmInfo;
            public AlarmFrameEventArgs(byte[] alarmInfo)
            {
                this.AlarmInfo = alarmInfo;
            }
        }

        // 定义DataFrameEventArgs类，传递给Observer所感兴趣的信息,继承EventArgs
        public class DataFrameEventArgs : EventArgs
        {
            public readonly bool NeedACK;
            public readonly SerialPortFrame.FrameState FrameState;
            public readonly string NodeMAC;
            public readonly ushort FrameCounter;
            public readonly byte[] DataByte;
            public readonly SerialPortFrame.Type type;

            public DataFrameEventArgs(bool NeedACK, SerialPortFrame.FrameState FrameState, string NodeMAC, ushort FrameCounter, byte[] DataByte, SerialPortFrame.Type Type)
            {
                this.NeedACK = NeedACK;
                this.FrameState = FrameState;
                this.NodeMAC = NodeMAC;
                this.FrameCounter = FrameCounter;
                this.DataByte = DataByte;
                this.type = Type;
            }
        }
        #endregion

        #region 识别收到的帧
        //用来存储断帧
        public static byte[] interframe = new byte[0];

        /// <summary>
        /// 接收收到的数据，通过事件将其显示在窗体中以及写入文件中
        /// </summary>
        /// <param name="receiveByte"></param>
        public static void delegtReceiveData(byte[] receiveByte)
        {
            FrameEventArgs e = new FrameEventArgs(receiveByte); //建立FrameEventArgs 对象
            if (ReceivedTestDataEvent != null)// 如果有对象注册
            {
                ReceivedTestDataEvent(e); // 调用所有注册对象的方法
            }
        }

        /// <summary>
        /// 从字节流中拆分出正确的帧.
        /// </summary>
        /// <param name="receiveByte">接收到的字节流.</param>
        public static void CheckFrame(byte[] receiveByte)
        {
            try
            {
                Program.mainForm.RxDataText = "num";
                //打印数据（测试用）
                //string zframe = Character.BytesToString(receiveByte, 0, receiveByte.Length - 1, true);
                //Console.WriteLine(zframe);

                if (interframe.Length > 0)
                {
                    byte[] result = new byte[interframe.Length + receiveByte.Length];
                    interframe.CopyTo(result, 0);
                    receiveByte.CopyTo(result, interframe.Length);
                    receiveByte = new byte[result.Length];
                    receiveByte = result;
                }
                interframe = new byte[0];

                if (PacketProcess.packetFrameType == PacketProcess.FrameType.Serial)//处理串口通信帧
                {
                    for (int i = 0; i < receiveByte.Length; i++)
                    {
                        if (receiveByte[i] == (byte)NetCommunicationFrame.FrameHeadAndTail.Head)//找帧头
                        {
                            /* 保证receiveByte至少包含一帧 */
                            //第1个条件保证第二个条件中的receiveByte数组不越界;第2个条件保证后文中的receiveByte数组不越界
                            if ((i + 24 < receiveByte.Length) && (i + 24 + receiveByte[i + 23] + 2 <= receiveByte.Length))
                            {
                                int datalength = Convert.ToInt32(receiveByte[i + 22] << 8) + Convert.ToInt32(receiveByte[i + 23]);//数据域长度

                                if (receiveByte[i + 23 + datalength + 2] == (byte)NetCommunicationFrame.FrameHeadAndTail.Tail)//找帧尾
                                {
                                    PacketProcess.GatewayMAC = "000000" + Character.BytesToString(receiveByte, i + 9, i + 9, false);
                                    //Program.mainForm.GatewayMAC = PacketProcess.GatewayMAC;

                                    byte[] oneFrame = new byte[24 + datalength + 2];
                                    for (int j = 0; j < oneFrame.Length; j++)//抽取完整的一帧
                                    {
                                        oneFrame[j] = receiveByte[i + j];
                                    }
                                    if (CheckSumCorrect(oneFrame))//校验成功
                                    {
                                        if (PacketProcess.EnableAnalysisFrame)
                                        {
                                            FrameAnalysis(oneFrame);//处理该帧
                                        }

                                        //打印数据（测试用）
                                        string leng = Character.BytesToString(oneFrame, 0, oneFrame.Length - 1, true);
                                        Console.WriteLine("输出完整的帧内容是：" + leng);

                                        /* 接收一正确帧的事件 */
                                        FrameEventArgs e = new FrameEventArgs(oneFrame); //建立FrameEventArgs 对象
                                        if (ReceivedACorrectFrameEvent != null)// 如果有对象注册
                                        {
                                            ReceivedACorrectFrameEvent(e); // 调用所有注册对象的方法
                                        }

                                        Program.mainForm.RxFrameCount++;//更新主窗体接收到的帧数量统计

                                        if (i + 24 + datalength + 2 == receiveByte.Length)//receiveByte已经处理完
                                        {
                                            return;
                                        }
                                        else//receiveByte还没处理完，可能是连帧
                                        {
                                            i = i + 23 + datalength + 2;//处理下一帧数据
                                            continue;
                                        }
                                    }
                                    else//校验失败
                                    {
                                        //Program.mainForm.CheckFailedFrameCount++; //更新主窗体校验失败帧统计
                                        continue;//继续找下一个帧头
                                    }
                                }
                            }
                            else//断帧
                            {
                                //存储断帧
                                interframe = new byte[receiveByte.Length - i];
                                for (int nu = 0; i < receiveByte.Length; i++)
                                {
                                    interframe[nu] = receiveByte[i];
                                    nu++;
                                }
                                //不应在此处产生断帧
                                //Program.mainForm.UnknownFrameCount++; //更新主窗体未识别帧统计
                            }
                        }

                    }
                }
                else//处理网络通信帧
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("帧接收异常，异常信息" + ex.ToString());
                //MainForm.WriteLog(ex, @".\\1.log");
            }

        }

        /// <summary>
        /// 解析收到的帧.
        /// </summary>
        /// <param name="receiveByte">接收的字节流.</param>
        public static void FrameAnalysis(byte[] receiveByte)
        {

            #region 第一次负载提取
            byte[] receive;//帧负载

            //处理网络通信帧
            NetCommunicationFrame frame_first = new NetCommunicationFrame();
            frame_first.Receive(receiveByte);
            if (frame_first.CheckSumCorrect)//检查校验和
            {
                receive = frame_first.DataByte;//提取负载，此时负载为串口通信帧
            }
            else
            {
                //Program.mainForm.CheckFailedFrameCount++; //更新主窗体校验失败帧统计
                return;
            }

            if (receive == null)//收到无负载的帧
            {
                //暂不进行任何处理
                return;
            }
            #endregion

            #region 第二次负载提取

            #region 解析串口68通信帧
            if (receive[0] == (byte)SerialPortFrame.FrameHeadAndTail.Head) //串口通信帧头
            {
                SerialPortFrame frame = new SerialPortFrame();
                frame.Receive(receive);
                if (frame.CheckSumCorrect)//校验和正确
                {
                    if (frame.FrameType == SerialPortFrame.Type.OrderFrame)//接收到命令帧
                    {
                        #region 组网状态信息的应答
                        /* 读取集中器信息命令应答帧 */
                        if (Check(PacketProcess.readConcentratorMac, frame.CommandID))
                        {
                          
                        }
                        /* 读取网络状态命令应答帧 */
                        else if (Check(PacketProcess.readNetState, frame.CommandID))
                        {
                         
                        }
                        else if (Check(PacketProcess.readNodeMACIP, frame.CommandID))
                        {
                          
                        }
                        /* 读取节点信息命令应答帧 */
                        else if (Check(PacketProcess.readNodeInformation, frame.CommandID))
                        {
                          
                        }
                        /* 读取所有节点的状态信息命令应答帧 */
                        else if (Check(PacketProcess.readAllNodesStatus, frame.CommandID))
                        {
                            //建立CommandPayloadEventArgs 对象
                            CommandFrameEventArgs e = new CommandFrameEventArgs(frame.CommandPayload);
                            if (ReadAllNodesStatusEvent != null)
                            { // 如果有对象注册
                                ReadAllNodesStatusEvent(e); // 调用所有注册对象的方法
                            }
                        }
                        else if (Check(PacketProcess.writeEdBeginUpdate, frame.CommandID))
                        {
                            CommandFrameEventArgs e = new CommandFrameEventArgs(frame.CommandPayload);
                            if (WriteEdBeginUpdateEvent != null)
                            {
                                WriteEdBeginUpdateEvent(e);
                            }
                        }
                        #endregion

                        else if (Check(PacketProcess.sendfile_start, frame.CommandID))
                        {
                            
                        }
                        else if (Check(PacketProcess.sendfile_over, frame.CommandID))
                        {
                            
                        }

                        
                    }
                    else if (frame.FrameType == SerialPortFrame.Type.AlarmFrame)//接收到告警帧
                    {
                        AlarmFrameEventArgs e = new AlarmFrameEventArgs((frame as InternalFrame).DataByte);
                        if (ReceiveReplyFrameEvent != null)// 如果有对象注册
                        {
                            ReceiveAlarmFrameEventEvent(e); // 调用所有注册对象的方法
                        }
                        //MessageBox.Show("68帧告警"+Character.BytesToString(e.AlarmInfo, 0, e.AlarmInfo.Length - 1, true));
                        //Program.mainForm.AlarmFrameCount++; //更新主窗体告警帧统计
                        return;
                    }
                    else if (frame.FrameType == SerialPortFrame.Type.ReplyFrame)//接收到应答帧
                    {
                  
                    }
                    else if (frame.FrameType == SerialPortFrame.Type.DataFrame)//接收到数据帧
                    {
                        DataFrameEventArgs e = new DataFrameEventArgs(frame.NeedAnswer, frame.TransferState, frame.NodeMac, frame.FrameCount, frame.DataByte, frame.FrameType);
                        if (ReceiveDataFrameEvent != null)// 如果有对象注册
                        {
                            ReceiveDataFrameEvent(e); // 调用所有注册对象的方法
                        }
                        
                        return;
                    }
                    else//不是已知帧类型
                    {
                        //Program.mainForm.UnknownFrameCount++; //更新主窗体未识别帧统计
                        return;
                    }
                }
                else//校验和错误
                {
                    //Program.mainForm.CheckFailedFrameCount++; //更新主窗体校验失败帧统计
                    return;
                }
            }
            #endregion

        

            #region 未知帧头处理
            else//未知帧头
            {
                return;
            }
            #endregion

            #endregion

        }

       
        /// <summary>
        /// 检查两个字节数组是否相同.
        /// </summary>
        /// <param name="standard">字节数组1.</param>
        /// <param name="receive">字节数组2.</param>
        /// <returns>是否相同</returns>
        public static bool Check(byte[] standard, byte[] receive)
        {
            bool same = true;
            for (int i = 0; i < standard.Length; i++)
            {
                if (standard[i] != receive[i])
                {
                    same = false;
                    break;
                }
            }
            return same;
        }

        /// <summary>
        /// 检查一条完整帧的校验和是否正确.
        /// </summary>
        /// <param name="oneFrame">一条完整的帧.</param>
        /// <returns>校验和正确时返回真，否则返回假</returns>
        public static bool CheckSumCorrect(byte[] oneFrame)
        {
            byte checkSum = 0;
            for (int i = 0; i < oneFrame.Length - 2; i++)
            {
                checkSum += oneFrame[i];
            }
            if (checkSum == oneFrame[oneFrame.Length - 2])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}

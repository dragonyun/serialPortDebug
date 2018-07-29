using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 串口收发调试工具
{
    /// <summary>
    /// 帧的打包，组合，以及常用属性,命令标识等信息
    /// </summary>
    class PacketProcess
    {
        #region 公共属性
        public static NetCommunicationFrame.GatewayType GatewayType = NetCommunicationFrame.GatewayType.UnknowType;//网关类型
        public static string GatewayMAC = "00000000"; // 网关MAC地址,获得前应设为全0
        public static string PhoneMAC = "010203040506"; // 手机MAC地址,获得前应设为全0
        public static string SeverIp = "00000000"; // 服务器IP地址，获得前应设为全0
        public static string ConcentratorMac = "0000";//集中器地址，只有首个字节对于帧来说有效

        public static Dictionary<string, string> GatewayDictionary = new Dictionary<string, string>();//网关信息键值对，键为MAC地址
        public static Dictionary<string, string> GatewayIpDictionary = new Dictionary<string, string>();//网关IP地址键值对，键为MAC地址
        public static int result_login = 0;//88帧登录应答标志

        public static byte ConnectedEdCount = 0;     // 已连接节点数量
        public static byte OnLineEdCount = 0;        // 在网节点数量（能够显示在列表中的节点数量）

        public static bool EnableAnalysisFrame = true;//允许解析接收到的帧

        
        public static byte[] bytes = new byte[6];       //随机生成本机的MAC地址，在单次使用过程中，不改变MAC地址的信息
        public static byte[] receiveByte()
        {
            Random random = new Random();
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)random.Next(256);
            return bytes;
        }

        public static bool EnableCheck = false;
        public static bool lightenable = false;

        public static FrameType packetFrameType = FrameType.Unknown;//标识当前是什么模式下的通信（外网，内网，串口，未知）
        public enum FrameType
        {
            OutterNet,
            InnerNet,
            Serial,
            Unknown,
        }
        #endregion




        #region 命令帧命令标识定义
        //命令标识
        //[2017年5月15日]修改命令标识字节序。修改后的字节序为：DI3，DI2，DI1，DI0。先发送DI3.
        #region 68帧命令标识

        #region 集中器以及节点参数信息编码表
        public static byte[] writeFrequency = { 0x00, 0x01, 0x01, 0x01 };//设置基频
        public static byte[] writeChannelNumber = { 0x00, 0x01, 0x02, 0x01 };//设置信道号
        public static byte[] writeChannelGap = { 0x00, 0x01, 0x03, 0x01 };//设置信道间隔
        public static byte[] writeSpeed = { 0x00, 0x01, 0x04, 0x01 };//设置数据速率
        public static byte[] writeFilterBandwidth = { 0x00, 0x01, 0x05, 0x01 };
        public static byte[] writeManchester = { 0x00, 0x01, 0x06, 0x01 };//设置曼切斯特使能
        public static byte[] writeFrequencyOffset = { 0x00, 0x01, 0x07, 0x01 };//设置频率偏差
        public static byte[] writeModulationMode = { 0x00, 0x01, 0x08, 0x01 };//设置调制方式
        public static byte[] writeWhiteout = { 0x00, 0x01, 0x09, 0x01 };//设置白化使能
        public static byte[] writeTransmitPower = { 0x00, 0x01, 0x0A, 0x01 };//设置发射功率
        public static byte[] writeFecEnable = { 0x00, 0x01, 0x19, 0x01 };//设置FEC使能
        public static byte[] writeBaudRate = { 0x00, 0x01, 0x20, 0x01 };//设置波特率
        public static byte[] writeRelay = { 0x00, 0x01, 0x21, 0x01 };//设置节点的中继状态
        public static byte[] readFrequency = { 0x00, 0x01, 0x01, 0x00 };//读取基频
        public static byte[] readChannelNumber = { 0x00, 0x01, 0x02, 0x00 };//读取信道号
        public static byte[] readChannelGap = { 0x00, 0x01, 0x03, 0x00 };//读取信道间隔
        public static byte[] readSpeed = { 0x00, 0x01, 0x04, 0x00 };//读取数据速率
        public static byte[] readFilterBandwidth = { 0x00, 0x01, 0x05, 0x00 };
        public static byte[] readManchester = { 0x00, 0x01, 0x06, 0x00 };//读取曼切斯特使能
        public static byte[] readFrequencyOffset = { 0x00, 0x01, 0x07, 0x00 };//读取频率偏差
        public static byte[] readModulationMode = { 0x00, 0x01, 0x08, 0x00 };//读取调制方式
        public static byte[] readWhiteout = { 0x00, 0x01, 0x09, 0x00 };//读取白化使能
        public static byte[] readTransmitPower = { 0x00, 0x01, 0x0A, 0x00 };//读取发射功率
        public static byte[] readFecEnable = { 0x00, 0x01, 0x19, 0x00 };//读取FEC使能
        public static byte[] readChipPartNumber = { 0x00, 0x01, 0x0B, 0x00 };//读取芯片部件号
        public static byte[] readChipVersionNumber = { 0x00, 0x01, 0x0C, 0x00 };//读取芯片版本号
        public static byte[] readLQI = { 0x00, 0x01, 0x0E, 0x00 };//读取LQI
        public static byte[] readRSSI = { 0x00, 0x01, 0x2C, 0x00 };//读取RSSI
        public static byte[] readBaudRate = { 0x00, 0x01, 0x20, 0x00 };//读取波特率
        public static byte[] readRelay = { 0x00, 0x01, 0x21, 0x00 };//读取节点的中继状态 00为未开启，01为开启

        public static byte[] readDeviceType = { 0x00, 0x01, 0x30, 0x00 };//读设备类型，00为网关，01为节点

        public static byte[] sendfile_start = { 0x00, 0x01, 0x80, 0x01 };//网关更新，起始帧命令标识
        public static byte[] sendfile_over = { 0x00, 0x01, 0x88, 0x01 };//网关更新，结束帧命令标识
        #endregion

        #region 组网状态信息编码表
        public static byte[] readJoinLinkToken = { 0x00, 0x02, 0x03, 0x00 };//此功能未用
        public static byte[] writeTimeSyncEnable = { 0x00, 0x02, 0x31, 0x01 };//设置同步帧检测使能
        public static byte[] readTimeSyncEnable = { 0x00, 0x02, 0x31, 0x00 };//读取同步帧检测使能
        public static byte[] writeTimeSyncInterval = { 0x00, 0x02, 0x32, 0x01 };//设置同步帧间隔时间
        public static byte[] readTimeSyncInterval = { 0x00, 0x02, 0x32, 0x00 };//读取同步帧间隔时间
        public static byte[] writeEdBeginUpdate = { 0x00, 0x02, 0x70, 0x01 };//未知，未用
        public static byte[] writeResetAP = { 0x00, 0x02, 0x40, 0x01 };//恢复集中器到出厂状态(未试过)
        public static byte[] readNodeMACIP = { 0x00, 0x02, 0x90, 0x00 };//查询目标节点的MAC和IP
        public static byte[] writeModifyIP = { 0x00, 0x02, 0x91, 0x01 };
        public static byte[] writeDeleteED = { 0x00, 0x02, 0x92, 0x01 };
        #endregion

        #region 用户自定义信息编码表
        public static byte[] readNodeSwitchstate = { 0xfa, 0x80, 0x01, 0x00 };    //读取节点的开关状态
        public static byte[] writeNodestate = { 0xfa, 0x80, 0x01, 0x01 };       //设置节点的开关状态
        public static byte[] writeNodeRgb = { 0xfa, 0x80, 0x02, 0x01 };         //设置RGB3个通道亮度分量

        public static byte[] readDateGate = { 0xfa, 0x80, 0x10, 0x00 };//读取当前时间
        public static byte[] setDateGate = { 0xfa, 0x80, 0x10, 0x01 };//设定日期时间
        public static byte[] setTimer = { 0xfa, 0x80, 0x11, 0x01 };//设置闹钟项
        public static byte[] readTimer = { 0xfa, 0x80, 0x12, 0x00 };//读取闹钟列表

        public static byte[] readNodeInformation = { 0x00, 0x02, 0x11, 0x00 };//查询集中器记录的节点信息
        public static byte[] readAllNodesStatus = { 0x00, 0x02, 0x12, 0x00 };
        public static byte[] readNetState = { 0x00, 0x02, 0x10, 0x00 };
        public static byte[] readConcentratorMac = { 0x00, 0x02, 0x20, 0x00 };//读取集中器MAC地址
        public static byte[] readNode = { 0x00, 0x02, 0x80, 0x00 };
        public static byte[] addNode = { 0x00, 0x02, 0x13, 0x01 }; // 添加节点
        public static byte[] deleteNode = { 0x00, 0x02, 0x92, 0x01 };//删除目标节点
        #endregion

        #endregion

        #region 77帧命令标识
        public static byte[] readNodeState = { 0xFB, 0x00, 0x01, 0x01 };//获取集中器内添加的节点和集中器地址
        #endregion

        #region 88帧命令标识
        public static byte[] login = { 0xff, 0x00, 0x00, 0x01 };//登陆鉴权
        public static byte[] gateLinkCheck = { 0xff, 0x00, 0x02, 0x00 };//查询网关是否连接上服务器
        public static byte[] userRegister = { 0xff, 0x00, 0x04, 0x01 };//新用户注册
        public static byte[] userGateBindingPhone = { 0xff, 0x00, 0x10, 0x01 };//用户与网关绑定
        public static byte[] userGateUnbindingPhone = { 0xff, 0x00, 0x11, 0x01 };//用户与网关解绑
        public static byte[] writeWlanSSIDandPassword = { 0xff, 0x40, 0x00, 0x01 };
        public static byte[] readDetectionGateway = { 0xff, 0x20, 0x01, 0x00 };//网关地址和类型        
        #endregion

        #endregion






        #region 68帧，网关更新测试用方法（测试用，后面再改）
        //网关更新，用于发送起始和结束的命令帧
        public static byte[] SendCommand_68_updategateway(string destinationMac, byte length, byte[] commandID, byte[] commandPayload)
        {
            #region 构建串口帧
            byte[] load = new byte[4];
            load = commandPayload;
            SerialPortFrame frame = new SerialPortFrame(commandID, load, length);
            frame.ConcentratorMac = ConcentratorMac;

            if (destinationMac == ConcentratorMac)//目标是集中器
            {
                frame.NodeMac = "0000";
                frame.CommunicationWay = SerialPortFrame.Communication.InnerNet;
                if (ConcentratorMac.Length < 4)
                    ConcentratorMac = "00" + ConcentratorMac;
                if (ConcentratorMac.Length > 4)
                {
                    frame.ConcentratorMac = ConcentratorMac.Substring(4, 4);
                }
            }
            else//目标是节点
            {
                if (destinationMac.Length > 4)
                    frame.NodeMac = destinationMac.Substring(4, 4);
                else
                    frame.NodeMac = destinationMac;
                frame.CommunicationWay = SerialPortFrame.Communication.InnerNet;
            }
            byte[] serialPortFrame = frame.Send();//形成串口通信帧
            #endregion

            #region 构建网络通信帧

            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(serialPortFrame, Convert.ToByte(serialPortFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            netFrame.FrameCount = 0;

            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server2Gateway;
                netFrame.ServerIP = SeverIp;
            }
            if (packetFrameType == FrameType.InnerNet)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;
            if (packetFrameType == FrameType.Serial)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;

            return netFrame.Send();

            #endregion
        }
        public static int frameNum = 0;
        //网关更新，用于发送中间的数据帧
        public static byte[] SendCommand_68_SendGatewayData(bool needACK, int frame_position, byte[] dataPayload, byte length, ushort Framecount)
        {
            #region 构建串口帧
            SerialPortFrame frame = new SerialPortFrame(dataPayload, Convert.ToByte(dataPayload.Length));
            if (frame_position == 0)
            {
                frame.TransferState = SerialPortFrame.FrameState.FirstFrame;
            }
            else if (frame_position == 1)
            {
                frame.TransferState = SerialPortFrame.FrameState.MidFrame;
            }
            else if (frame_position == 2)
            {
                frame.TransferState = SerialPortFrame.FrameState.LastFrame;
            }
            frame.NeedAnswer = needACK;
            frameNum = Framecount;
            frame.FrameCount = Framecount;
            frame.CommunicationWay = SerialPortFrame.Communication.InnerNet;
            byte[] intranetFrame = frame.Send();//形成串口通信帧
            #endregion

            #region 构建网络通信帧

            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(intranetFrame, Convert.ToByte(intranetFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            netFrame.FrameCount = 0;

            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server2Gateway;
                netFrame.ServerIP = SeverIp;
            }
            if (packetFrameType == FrameType.InnerNet)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;
            if (packetFrameType == FrameType.Serial)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;

            return netFrame.Send();

            #endregion
        }

        public static byte[] SendCommand_88_SendGatewayData(bool needACK, int frame_position, byte[] dataPayload, byte length, ushort Framecount)
        {
            #region 构建串口帧
            IntranetFrame frame = new IntranetFrame(dataPayload, Convert.ToByte(dataPayload.Length));
            if (frame_position == 0)
            {
                frame.TransferState = IntranetFrame.FrameState.FirstFrame;
            }
            else if (frame_position == 1)
            {
                frame.TransferState = IntranetFrame.FrameState.MidFrame;
            }
            else if (frame_position == 2)
            {
                frame.TransferState = IntranetFrame.FrameState.LastFrame;
            }
            frame.NeedAnswer = needACK;
            frameNum = Framecount;
            frame.FrameCount = Framecount;
            frame.CommunicationWay = IntranetFrame.Communication.InnerNet;
            byte[] intranetFrame = frame.Send();//形成串口通信帧
            #endregion

            #region 构建网络通信帧

            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(intranetFrame, Convert.ToByte(intranetFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            netFrame.FrameCount = 0;

            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server2Gateway;
                netFrame.ServerIP = SeverIp;
            }
            if (packetFrameType == FrameType.InnerNet)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;
            if (packetFrameType == FrameType.Serial)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;

            return netFrame.Send();

            #endregion
        }

        #endregion

        #region 打包待发送的帧

        #region 77帧打包待发送的帧
        /// <summary>
        /// 发送最终目的地是集中器或节点的77命令帧.(全网通信)
        /// </summary>
        /// <param name="destinationMac">最终目的设备的MAC.</param>
        /// <param name="length">命令总长度.</param>
        /// <param name="commandID">命令标识.</param>
        /// <param name="commandPayload">命令负载.</param>
        /// <returns></returns>
        public static byte[] SendCommand_77(byte length, byte[] commandID, byte[] commandPayload)
        {
            #region 构建77帧
            SpecialFrame frame = new SpecialFrame(commandID, commandPayload, length);

            byte[] serialPortFrame = frame.Send();//形成77通信帧
            #endregion

            #region 构建69帧
            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(serialPortFrame, Convert.ToByte(serialPortFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            //
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            //netFrame.ServerIP = SeverIp;//
            netFrame.FrameCount = 0;

            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server2Gateway;
                netFrame.ServerIP = SeverIp;
            }
            if (packetFrameType == FrameType.InnerNet)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;
            if (packetFrameType == FrameType.Serial)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;

            return netFrame.Send();
            #endregion
        }

        #endregion

        #region 67帧打包待发送的帧

        #region 67帧分割
        /// <summary>
        /// 67组播一帧最多支持10个节点，此方法将节点数组进行分割
        /// </summary>
        /// <param name="ipNumber">节点数量</param>
        /// <param name="ipLoad">包含节点数组</param>
        /// <param name="destinationMac">最终目的设备的MAC</param>
        /// <param name="length">命令总长度</param>
        /// <param name="commandID">命令标识</param>
        /// <param name="commandPayload">命令负载</param>
        public static void SendCommand_67_Section(byte ipNumber, string[] ipLoad, string destinationMac, byte length, byte[] commandID, byte[] commandPayload)
        {
            int frameNumber = ipLoad.Length / 10 + 1;
            if (ipLoad.Length % 10 == 0)
            {
                frameNumber -= 1;
            }
            if (frameNumber == 0)
                return;
            for (int i = 0; i < frameNumber - 1; i++)
            {
                string[] ipgroup = new string[10];
                for (int j = 0; j < 10; j++)
                {
                    ipgroup[j] = ipLoad[i * 10 + j];
                }
                Queue.Push(SendCommand_67(ipNumber, ipgroup, destinationMac, length, commandID, commandPayload));
            }
            string[] ipgrouplast = new string[ipLoad.Length - (frameNumber - 1) * 10];
            for (int k = 0; k < ipgrouplast.Length; k++)
            {
                ipgrouplast[k] = ipLoad[(frameNumber - 1) * 10 + k];
            }
            Queue.Push(SendCommand_67(ipNumber, ipgrouplast, destinationMac, length, commandID, commandPayload));
        }
        #endregion

        #region 67帧组帧
        /// <summary>
        /// 发送最终目的地是集中器的67组播命令帧.(全网通信)
        /// </summary>
        /// <param name="ipNumber">节点数量</param>
        /// <param name="ipLoad">包含节点数组</param>
        /// <param name="destinationMac">最终目的设备的MAC</param>
        /// <param name="length">命令总长度</param>
        /// <param name="commandID">命令标识</param>
        /// <param name="commandPayload">命令负载</param>
        /// <returns></returns>
        public static byte[] SendCommand_67(byte ipNumber, string[] ipLoad, string destinationMac, byte length, byte[] commandID, byte[] commandPayload)
        {
            #region 构建67帧
            NodeMulticastFrame frame67 = new NodeMulticastFrame(ipLoad, ipNumber);
            byte[] NodeMulticastFrame = frame67.Send();//形成节点组播帧
            #endregion

            #region 构建68帧
            SerialPortFrame frame = new SerialPortFrame(commandID, commandPayload, length);
            if (ConcentratorMac.Length < 4)
                ConcentratorMac = "00" + ConcentratorMac;
            if (destinationMac.Length < 4)
                destinationMac = "00" + destinationMac;

            if (ConcentratorMac.Length > 4)
                ConcentratorMac = ConcentratorMac.Substring(ConcentratorMac.Length - 4, 4);
            if (destinationMac.Length > 4)
                destinationMac = destinationMac.Substring(destinationMac.Length - 4, 4);
            frame.ConcentratorMac = ConcentratorMac;
            frame.NodeMac = "eeee";
            frame.CommunicationWay = SerialPortFrame.Communication.Nontransparent;

            byte[] serialPortFrame = frame.Send();//形成串口通信帧
            #endregion

            #region 构建组合帧
            byte[] MulFrame = new byte[NodeMulticastFrame.Length + serialPortFrame.Length];
            NodeMulticastFrame.CopyTo(MulFrame, 0);
            serialPortFrame.CopyTo(MulFrame, NodeMulticastFrame.Length);
            #endregion

            #region 构建网络通信帧

            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(MulFrame, Convert.ToByte(MulFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            netFrame.FrameCount = 0;

            netFrame.TransferDirection = NetCommunicationFrame.Direction.Multicast;
            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server2Gateway;
                netFrame.ServerIP = SeverIp;
            }
            //if (packetFrameType == FrameType.InnerNet)
            //    netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;
            //if (packetFrameType == FrameType.Serial)
            //    netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;

            return netFrame.Send();

            #endregion
        }

        /// <summary>
        /// 发送最终目的地是集中器的67组播数据帧.(全网通信)
        /// </summary>
        /// <param name="ipNumber">节点数量</param>
        /// <param name="ipLoad">包含节点数组</param>
        /// <param name="destinationMac">最终目的设备的MAC</param>
        /// <param name="length">数据总长度</param>
        /// <param name="dataPayload">数据负载</param>
        /// <returns></returns>
        public static byte[] SendCommand_67_data(byte ipNumber, string[] ipLoad, string destinationMac, bool needACK, byte length, byte[] dataPayload)
        {
            #region 构建67帧
            NodeMulticastFrame frame67 = new NodeMulticastFrame(ipLoad, ipNumber);
            byte[] NodeMulticastFrame = frame67.Send();//形成节点组播帧
            #endregion

            #region 构建68帧
            SerialPortFrame frame = new SerialPortFrame(dataPayload, Convert.ToByte(dataPayload.Length));
            if (ConcentratorMac.Length < 4)
                ConcentratorMac = "00" + ConcentratorMac;
            if (destinationMac.Length < 4)
                destinationMac = "00" + destinationMac;

            if (ConcentratorMac.Length > 4)
                ConcentratorMac = ConcentratorMac.Substring(ConcentratorMac.Length - 4, 4);
            if (destinationMac.Length > 4)
                destinationMac = destinationMac.Substring(destinationMac.Length - 4, 4);
            frame.NeedAnswer = needACK;
            frame.ConcentratorMac = ConcentratorMac;
            frame.NodeMac = "eeee";
            frame.CommunicationWay = SerialPortFrame.Communication.phone;

            byte[] serialPortFrame = frame.Send();//形成串口通信帧
            #endregion

            #region 构建组合帧
            byte[] MulFrame = new byte[NodeMulticastFrame.Length + serialPortFrame.Length];
            NodeMulticastFrame.CopyTo(MulFrame, 0);
            serialPortFrame.CopyTo(MulFrame, NodeMulticastFrame.Length);
            #endregion

            #region 构建网络通信帧

            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(MulFrame, Convert.ToByte(MulFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            netFrame.FrameCount = 0;

            netFrame.TransferDirection = NetCommunicationFrame.Direction.Multicast;
            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server2Gateway;
                netFrame.ServerIP = SeverIp;
            }
            if (packetFrameType == FrameType.InnerNet)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;
            if (packetFrameType == FrameType.Serial)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;

            return netFrame.Send();

            #endregion
        }
        #endregion

        #endregion

        #region 68帧 打包待发送的帧
        /// <summary>
        /// 发送最终目的地是集中器或节点的68命令帧.(全网通信)
        /// </summary>
        /// <param name="destinationMac">最终目的设备的MAC.</param>
        /// <param name="length">命令总长度.</param>
        /// <param name="commandID">命令标识.</param>
        /// <param name="commandPayload">命令负载.</param>
        /// <returns></returns>
        public static byte[] SendCommand_68(string destinationMac, byte length, byte[] commandID, byte[] commandPayload, byte commandcount)
        {
            #region 构建68帧
            SerialPortFrame frame = new SerialPortFrame(commandID, commandPayload, length);
            if (ConcentratorMac.Length < 4)
                ConcentratorMac = "00" + ConcentratorMac;
            if (destinationMac.Length < 4)
                destinationMac = "00" + destinationMac;
            if (ConcentratorMac.Length > 4)
                ConcentratorMac = ConcentratorMac.Substring(ConcentratorMac.Length - 4, 4);
            if (destinationMac.Length > 4)
                destinationMac = destinationMac.Substring(destinationMac.Length - 4, 4);
            frame.ConcentratorMac = ConcentratorMac;
            if (destinationMac == ConcentratorMac)//目标是集中器
            {
                frame.NodeMac = "0000";
                frame.CommunicationWay = SerialPortFrame.Communication.InnerNet;
            }
            else//目标是节点
            {
                if (destinationMac.Length > 4)
                    frame.NodeMac = destinationMac.Substring(4, 4);
                else
                    frame.NodeMac = destinationMac;
                frame.CommunicationWay = SerialPortFrame.Communication.Nontransparent;
            }
            frame.FrameCount = commandcount;
            byte[] serialPortFrame = frame.Send();//形成串口通信帧
            #endregion

            #region 构建网络通信帧

            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(serialPortFrame, Convert.ToByte(serialPortFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            netFrame.FrameCount = 0;

            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server2Gateway;
                netFrame.ServerIP = SeverIp;
            }
            if (packetFrameType == FrameType.InnerNet)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;
            if (packetFrameType == FrameType.Serial)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;

            return netFrame.Send();

            #endregion
        }
        //重载
        public static byte[] SendCommand_68(string destinationMac, byte length, byte[] commandID, string commandPayload, byte commandcount)
        {
            #region 构建68帧
            byte[] commandLoad = Character.StringToBytes(commandPayload);
            byte[] command = new byte[4];
            command[0] = commandLoad[1];
            command[1] = commandLoad[0];
            command[2] = 0x0A;
            command[3] = 0xFD;

            SerialPortFrame frame = new SerialPortFrame(commandID, command, length);
            if (ConcentratorMac.Length < 4)
                ConcentratorMac = "00" + ConcentratorMac;
            if (destinationMac.Length < 4)
                destinationMac = "00" + destinationMac;
            if (ConcentratorMac.Length > 4)
                ConcentratorMac = ConcentratorMac.Substring(ConcentratorMac.Length - 4, 4);
            if (destinationMac.Length > 4)
                destinationMac = destinationMac.Substring(destinationMac.Length - 4, 4);
            frame.ConcentratorMac = ConcentratorMac;
            if (destinationMac == ConcentratorMac)//目标是集中器
            {
                frame.NodeMac = "0000";
                frame.CommunicationWay = SerialPortFrame.Communication.InnerNet;
            }
            else//目标是节点
            {
                if (destinationMac.Length > 4)
                    frame.NodeMac = destinationMac.Substring(4, 4);
                else
                    frame.NodeMac = destinationMac;
                frame.CommunicationWay = SerialPortFrame.Communication.Nontransparent;
            }
            frame.FrameCount = commandcount;
            byte[] serialPortFrame = frame.Send();//形成串口通信帧
            #endregion

            #region 构建网络通信帧

            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(serialPortFrame, Convert.ToByte(serialPortFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            netFrame.FrameCount = 0;

            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server2Gateway;
                netFrame.ServerIP = SeverIp;
            }
            if (packetFrameType == FrameType.InnerNet)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;
            if (packetFrameType == FrameType.Serial)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;

            return netFrame.Send();

            #endregion
        }

        /// <summary>
        /// 发送最终目的地是节点的命令帧.
        /// </summary>
        /// <param name="destinationMac">最终目的设备的MAC.</param>
        /// <param name="length">命令总长度.</param>
        /// <param name="commandID">命令标识.</param>
        /// <param name="commandPayload">命令负载.</param>
        /// <returns></returns>
        public static byte[] SendCommand_68_ToNode(string destinationMac, byte length, byte[] commandID, byte[] commandPayload)
        {
            #region 构建68帧
            SerialPortFrame frame = new SerialPortFrame(commandID, commandPayload, length);
            frame.ConcentratorMac = ConcentratorMac;

            if (destinationMac.Length > 4)
                frame.NodeMac = destinationMac.Substring(destinationMac.Length - 4, 4);
            else
                frame.NodeMac = destinationMac;
            frame.CommunicationWay = SerialPortFrame.Communication.Nontransparent;
            
            byte[] serialPortFrame = frame.Send();//形成串口通信帧
            #endregion

            #region 构建网络通信帧

            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(serialPortFrame, Convert.ToByte(serialPortFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            netFrame.FrameCount = 0;

            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server2Gateway;
                netFrame.ServerIP = SeverIp;
            }
            if (packetFrameType == FrameType.InnerNet)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;
            if (packetFrameType == FrameType.Serial)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;

            return netFrame.Send();

            #endregion
        }
        /// <summary>
        /// RSSI读取，68帧内加入了帧序号，用于区分重复帧
        /// </summary>
        /// <param name="frameNum">帧序号</param>
        /// <param name="destinationMac">最终目的设备的MAC</param>
        /// <param name="length">命令总长度</param>
        /// <param name="commandID">命令标识</param>
        /// <param name="commandPayload">命令负载</param>
        /// <returns></returns>
        public static byte[] SendCommand_68_ToNode(int frameNum, string destinationMac, byte length, byte[] commandID, byte[] commandPayload)
        {
            #region 构建68帧
            SerialPortFrame frame = new SerialPortFrame(commandID, commandPayload, length);
            frame.ConcentratorMac = ConcentratorMac;

            //AnalysisProcess.Node_frame = frameNum;
            //frame.FrameCount = (ushort)AnalysisProcess.Node_frame;

            frame.FrameCount = (ushort)frameNum;

            if (destinationMac == ConcentratorMac)//目标是集中器
            {
                frame.NodeMac = "0000";
                frame.CommunicationWay = SerialPortFrame.Communication.InnerNet;
            }
            else//目标是节点
            {
                frame.NodeMac = destinationMac;
                frame.CommunicationWay = SerialPortFrame.Communication.Nontransparent;
            }
            //frame.NeedAnswer = true;
            byte[] serialPortFrame = frame.Send();//形成串口通信帧
            #endregion

            #region 构建网络通信帧

            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(serialPortFrame, Convert.ToByte(serialPortFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            netFrame.FrameCount = 0;

            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server2Gateway;
                netFrame.ServerIP = SeverIp;
            }
            if (packetFrameType == FrameType.InnerNet)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;
            if (packetFrameType == FrameType.Serial)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;

            return netFrame.Send();

            #endregion
        }
        /// <summary>
        /// 速率测试中，用于发送数据帧
        /// </summary>
        /// <param name="destinationMac"></param>
        /// <param name="needACK"></param>
        /// <param name="mode"></param>
        /// <param name="state"></param>
        /// <param name="count"></param>
        /// <param name="dataPayload"></param>
        /// <returns></returns>
        public static byte[] SendCommand_68_SeriData(string destinationMac, bool needACK, SerialPortFrame.Communication mode, SerialPortFrame.FrameState state, ushort count, byte[] dataPayload)
        {
            #region 构建串口帧
            SerialPortFrame frame = new SerialPortFrame(dataPayload, Convert.ToByte(dataPayload.Length));
            frame.ConcentratorMac = ConcentratorMac;

            if (destinationMac == ConcentratorMac)//目标是集中器
            {
                frame.NodeMac = "0000";
                frame.CommunicationWay = SerialPortFrame.Communication.InnerNet;
            }
            else//目标是节点
            {
                frame.NodeMac = destinationMac;
                frame.CommunicationWay = mode;
            }
            frame.TransferState = state;
            frame.NeedAnswer = needACK;
            frame.FrameCount = count;
            byte[] serialPortFrame = frame.Send();//形成串口通信帧
            #endregion

            #region 构建网络通信帧

            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(serialPortFrame, Convert.ToByte(serialPortFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            netFrame.FrameCount = 0;

            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server2Gateway;
                netFrame.ServerIP = SeverIp;
            }
            if (packetFrameType == FrameType.InnerNet)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;
            if (packetFrameType == FrameType.Serial)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;

            return netFrame.Send();

            #endregion
        }


        public static byte[] SendCommand_68_serialtest(byte[] data,byte dataLength,string nodeMac,int frameNum,bool numflag)
        {
            SerialPortFrame frame = new SerialPortFrame(data, dataLength);
            frame.NodeMac = nodeMac;

            frame.FrameCount = (ushort)frameNum;

            //固定帧序号
            if (numflag)
            {
                frame.FrameCount = (ushort)6;
            }
            byte[] serialPortFrame = frame.Send();//形成串口通信帧

            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(serialPortFrame, Convert.ToByte(serialPortFrame.Length));
            netFrame.TransferDirection = NetCommunicationFrame.Direction.Computer2Gateway;
            netFrame.FrameCount = (ushort)frameNum;
            //固定帧序号
            if (numflag)
            {
                netFrame.FrameCount = (ushort)6;
            }
            return netFrame.Send();

        }

        #endregion

        #region 88帧打包待发送的帧
        /// <summary>
        /// 发送最终目的地是网关的命令帧.
        /// </summary>
        /// <param name="length">命令总长度.</param>
        /// <param name="commandID">命令标识.</param>
        /// <param name="commandPayload">命令负载.</param>
        /// <returns></returns>
        public static byte[] SendCommand_88(byte length, byte[] commandID, byte[] commandPayload)
        {
            #region 构建88帧
            /* 构建内网通信88帧 */
            IntranetFrame frame = new IntranetFrame(commandID, commandPayload, length);
            byte[] innerNetFrame = frame.Send();//形成内网通信帧
            #endregion

            #region 构建网络通信
            /* 构建网络通信帧 */
            NetCommunicationFrame netFrame = new NetCommunicationFrame(innerNetFrame, Convert.ToByte(innerNetFrame.Length));
            netFrame.Encryption = false;
            netFrame.NeedAnswer = false;
            netFrame.FrameFormat = NetCommunicationFrame.FrameType.DataFrame;
            //
            receiveByte();
            netFrame.MobilePhoneMAC = PhoneMAC;
            netFrame.GatewayClassification = GatewayType;
            netFrame.GatewayMAC = GatewayMAC;
            //
            netFrame.FrameCount = 0;

            if (packetFrameType == FrameType.OutterNet)
            {
                netFrame.TransferDirection = NetCommunicationFrame.Direction.MobilePhone2Server;
                netFrame.ServerIP = SeverIp;
            }
            if (packetFrameType == FrameType.InnerNet)
                netFrame.TransferDirection = NetCommunicationFrame.Direction.Mobile2Gateway;

            return netFrame.Send();
            #endregion
        }

        #endregion

        #endregion

    }


}

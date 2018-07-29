using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 串口收发调试工具
{
    //用来组帧，包括父类帧，67,77,68,88,69帧

    /// <summary>
    /// 内部使用的67帧的构造与解析
    /// </summary>
    class NodeMulticastFrame
    {
        #region 内部使用67帧的公共属性
        private byte _ipNumber;
        private byte[] _ipLoad;

        public enum FrameHeadAndTail
        {
            Head = 0x67,
        }
        protected byte IpNumber
        {
            get { return _ipNumber; }
            set { _ipNumber = value; }
        }
        protected byte[] IpLoad
        {
            get { return _ipLoad; }
            set { _ipLoad = value; }
        }
        #endregion

        #region 内部使用67帧的构建与解构
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalFrame"/> class.
        /// </summary>
        public NodeMulticastFrame()
        {
            //由此构造函数创造的对象仅可使用Receive()方法..
        }

        /// <summary>
        /// 构造函数，包括IP数组和IP数量
        /// </summary>
        /// <param name="ipload">IP数组</param>
        /// <param name="ipnumber">IP数量</param>
        public NodeMulticastFrame(string[] ipload, byte ipnumber)
        {
            _ipNumber = ipnumber;
            _ipLoad = new byte[ipnumber * 2];

            for (int i = 0; i < ipnumber; i++)
            {
                char[] iploadchar = ipload[i].ToArray();
                _ipLoad[i * 2] = Character.CharToByte(iploadchar[0], iploadchar[1]);
                _ipLoad[i * 2 + 1] = Character.CharToByte(iploadchar[2], iploadchar[3]);

            }
        }

        /// <summary>
        /// 构建将要发送的内部使用帧.
        /// </summary>
        /// <returns>要发送的字节流</returns>
        public byte[] Send()
        {
            byte[] output = new byte[2 + _ipNumber * 2];

            output[0] = (byte)FrameHeadAndTail.Head;
            output[1] = _ipNumber;

            if (_ipNumber > 0)
            {
                for (int i = 0; i < _ipNumber * 2; i++)
                {
                    output[2 + i] = _ipLoad[i];
                }
            }
            return output;
        }
        #endregion
    }

    /// <summary>
    /// 内部使用的77帧的构造与解析
    /// </summary>
    /// <summary>
    class SpecialFrame
    {
        #region 内部使用77帧的公共属性
        protected byte _controlCode;//控制码1位
        protected ushort _frameCount;//帧计数器1位
        protected ushort _dataLength;//数据域长度2位
        protected byte[] _dataByte;//数据域
        protected byte _checkSum;//校验和
        protected bool _CheckSumCorrect;//校验和是否正确
        protected byte[] _commandID;//命令标识
        protected byte[] _commandPayload;//命令负载
        private string _concentratorMac;//集中器MAC

        public enum FrameHeadAndTail
        {
            Head = 0x77,
            Tail = 0x16,
        }
        public enum FrameState
        {
            OnlyFrame,
            FirstFrame,
            MidFrame,
            LastFrame
        }
        public FrameState TransferState
        {
            get
            {
                switch (_controlCode & 0x30)
                {
                    case 0x00: return FrameState.OnlyFrame;
                    case 0x10: return FrameState.FirstFrame;
                    case 0x20: return FrameState.LastFrame;
                    case 0x30: return FrameState.MidFrame;
                    default: return FrameState.OnlyFrame;
                };
            }
            set
            {
                byte transferState;
                switch (value)
                {
                    case FrameState.OnlyFrame: transferState = 0x00;
                        break;
                    case FrameState.FirstFrame: transferState = 0x10;
                        break;
                    case FrameState.MidFrame: transferState = 0x30;
                        break;
                    case FrameState.LastFrame: transferState = 0x20;
                        break;
                    default: transferState = 0x00;
                        break;
                }
                _controlCode &= 0xff - 0x30;//
                _controlCode |= transferState;//传输状态只是控制码的一部分
            }
        }
        public enum Type
        {
            DataFrame,
            OrderFrame,
            ReplyFrame,
            AlarmFrame
        }
        public Type FrameType
        {
            get
            {
                switch (_controlCode & 0x03)
                {
                    case 0x00: return Type.DataFrame;
                    case 0x01: return Type.OrderFrame;
                    case 0x02: return Type.ReplyFrame;
                    case 0x03: return Type.AlarmFrame;
                    default: return Type.DataFrame;
                }
            }
            set
            {
                byte frameType;
                switch (value)
                {
                    case Type.DataFrame: frameType = 0x00;
                        break;
                    case Type.OrderFrame: frameType = 0x01;
                        break;
                    case Type.ReplyFrame: frameType = 0x02;
                        break;
                    case Type.AlarmFrame: frameType = 0x03;
                        break;
                    default: frameType = 0x00;
                        break;
                }
                _controlCode &= 0xff - 0x03;
                _controlCode |= frameType;
            }
        }
        public string ConcentratorMac
        {
            get { return _concentratorMac; }
            set { _concentratorMac = value; }
        }
        public ushort FrameCount
        {
            get { return _frameCount; }
            set { _frameCount = value; }
        }
        public ushort DataLength
        {
            get { return _dataLength; }
        }
        public byte[] DataByte
        {
            get { return _dataByte; }
        }
        public byte CheckSum
        {
            get { return _checkSum; }
        }
        public bool CheckSumCorrect
        {
            get { return _CheckSumCorrect; }
        }
        public byte[] CommandID
        {
            get { return _commandID; }
        }
        public byte[] CommandPayload
        {
            get { return _commandPayload; }
        }
        #endregion

        #region 内部使用77帧的构建与解构
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalFrame"/> class.
        /// </summary>
        public SpecialFrame()
        {
            //由此构造函数创造的对象仅可使用Receive()方法..
        }

        /// <summary>
        /// 构造函数，只包括数据域和数据域长度（数据帧）
        /// </summary>
        /// <param name="dataByte">数据域.</param>
        /// <param name="datalength">数据域长度.</param>
        public SpecialFrame(byte[] dataByte, byte datalength)
        {
            _controlCode = 0x00;

            _concentratorMac = "0000";

            _frameCount = 0;

            _dataLength = datalength;

            _dataByte = dataByte;

            _checkSum = 0;

            _CheckSumCorrect = false;
        }

        /// <summary>
        /// 构造函数，包括 命令标识，命令负载，命令总长度（命令帧）
        /// </summary>
        /// <param name="commandID">4字节命令标识.</param>
        /// <param name="commandPayload">命令负载.</param>
        /// <param name="payloadLength">命令总长度.</param>
        public SpecialFrame(byte[] commandID, byte[] commandPayload, byte commandLength)
        {
            _controlCode = 0x01;

            _concentratorMac = "0000";

            _frameCount = 0;

            _dataLength = commandLength;

            _dataByte = new byte[_dataLength];
            for (int i = 0; i < 4; i++)
            {
                _dataByte[i] = commandID[i];
            }
            if (commandLength > 4)
            {
                for (int i = 0; i < commandLength - 4; i++)
                {
                    _dataByte[4 + i] = commandPayload[i];
                }
            }

            _checkSum = 0;

            _CheckSumCorrect = false;
        }

        /// <summary>
        /// 构建将要发送的内部使用帧.
        /// </summary>
        /// <returns>要发送的字节流</returns>
        public byte[] Send()
        {
            byte[] output = new byte[11 + _dataLength];//除了数据域数据之外有11个字节

            output[0] = (byte)FrameHeadAndTail.Head;
            output[1] = _controlCode;

            output[6] = Convert.ToByte(_frameCount & 0x00FF);

            output[7] = Convert.ToByte((_dataLength >> 8) & 0x00FF);
            output[8] = Convert.ToByte(_dataLength & 0x00FF);

            if (_dataLength > 0)
            {
                for (int i = 0; i < _dataLength; i++)
                {
                    output[9 + i] = _dataByte[i];
                }
            }
            calculateCheckSum(output);

            output[10 + _dataLength] = (byte)FrameHeadAndTail.Tail;
            return output;
        }

        /// <summary>
        /// 计算校验和.
        /// </summary>
        /// <param name="output">其他部分已经填充好的帧.</param>
        protected void calculateCheckSum(byte[] output)
        {
            _checkSum = 0;
            for (int i = 0; i < 9 + _dataLength; i++)
            {
                _checkSum += output[i];
            }
            output[9 + _dataLength] = _checkSum;
        }

        /// <summary>
        /// 解析收到的内部使用帧.
        /// </summary>
        /// <param name="input">收到的字节流.</param>
        public void Receive(byte[] input)
        {
            try
            {
                if ((input[0] == (byte)FrameHeadAndTail.Head) && (input[input.Length - 1] == (byte)FrameHeadAndTail.Tail))
                {
                    int mm = Convert.ToInt32(input[7] << 8) + Convert.ToInt32(input[8]);

                    if ((mm + 11) == input.Length)
                    {
                        _controlCode = input[1];

                        _frameCount = input[6];

                        _dataLength = input[7];
                        _dataLength = Convert.ToUInt16((_dataLength << 8) + input[8]);

                        if (_dataLength > 0)
                        {
                            _dataByte = new byte[_dataLength];
                            for (int i = 0; i < _dataLength; i++)
                            {
                                _dataByte[i] = input[9 + i];
                            }
                        }

                        if ((_controlCode & 0x03) == 0x01) //是命令帧
                        {
                            _commandID = new byte[4];
                            for (int i = 0; i < 4; i++)
                            {
                                _commandID[i] = _dataByte[i];
                            }

                            _commandPayload = new byte[_dataLength - 4];
                            for (int i = 0; i < _dataLength - 4; i++)
                            {
                                _commandPayload[i] = _dataByte[4 + i];
                            }
                        }

                        _checkSum = 0;
                        for (int i = 0; i < 9 + _dataLength; i++)
                        {
                            _checkSum += input[i];
                        }
                        if (_checkSum == input[9 + _dataLength])
                        {
                            _CheckSumCorrect = true;
                        }
                        else
                        {
                            _CheckSumCorrect = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                _CheckSumCorrect = false;
            }
        }
        #endregion
    }

    /// <summary>
    /// 内部使用的68，88帧的构造与解析(父类)
    /// </summary>
    abstract class InternalFrame
    {
        #region 内部使用帧的公共属性
        protected byte _controlCode;//控制码1字节
        protected ushort _frameCount;//帧计数器1字节
        protected byte _dataLength;//数据域长度1字节
        protected byte[] _dataByte;//数据域n字节
        protected byte _checkSum;//校验和1字节
        protected bool _CheckSumCorrect;//校验是否正确
        protected byte[] _commandID;//命令标识
        protected byte[] _commandPayload;//命令标识加载数据

        public enum FrameState
        {
            OnlyFrame,
            FirstFrame,
            MidFrame,
            LastFrame
        }
        public FrameState TransferState
        {
            get
            {
                switch (_controlCode & 0x30)
                {
                    case 0x00: return FrameState.OnlyFrame;
                    case 0x10: return FrameState.FirstFrame;
                    case 0x20: return FrameState.LastFrame;
                    case 0x30: return FrameState.MidFrame;
                    default: return FrameState.OnlyFrame;
                };
            }
            set
            {
                byte transferState;
                switch (value)
                {
                    case FrameState.OnlyFrame: transferState = 0x00;
                        break;
                    case FrameState.FirstFrame: transferState = 0x10;
                        break;
                    case FrameState.MidFrame: transferState = 0x30;
                        break;
                    case FrameState.LastFrame: transferState = 0x20;
                        break;
                    default: transferState = 0x00;
                        break;
                }
                _controlCode &= 0xff - 0x30;//
                _controlCode |= transferState;//传输状态只是控制码的一部分
            }
        }
        public enum Type
        {
            DataFrame,
            OrderFrame,
            ReplyFrame,
            AlarmFrame
        }
        public Type FrameType
        {
            get
            {
                switch (_controlCode & 0x03)
                {
                    case 0x00: return Type.DataFrame;
                    case 0x01: return Type.OrderFrame;
                    case 0x02: return Type.ReplyFrame;
                    case 0x03: return Type.AlarmFrame;
                    default: return Type.DataFrame;
                }
            }
            set
            {
                byte frameType;
                switch (value)
                {
                    case Type.DataFrame: frameType = 0x00;
                        break;
                    case Type.OrderFrame: frameType = 0x01;
                        break;
                    case Type.ReplyFrame: frameType = 0x02;
                        break;
                    case Type.AlarmFrame: frameType = 0x03;
                        break;
                    default: frameType = 0x00;
                        break;
                }
                _controlCode &= 0xff - 0x03;
                _controlCode |= frameType;
            }
        }
        public ushort FrameCount
        {
            get { return _frameCount; }
            set { _frameCount = value; }
        }
        public byte DataLength
        {
            get { return _dataLength; }
        }
        public byte[] DataByte
        {
            get { return _dataByte; }
        }
        public byte CheckSum
        {
            get { return _checkSum; }
        }
        public bool CheckSumCorrect
        {
            get { return _CheckSumCorrect; }
        }
        public byte[] CommandID
        {
            get { return _commandID; }
        }
        public byte[] CommandPayload
        {
            get { return _commandPayload; }
        }
        #endregion

        #region 内部使用帧的构建与解构
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalFrame"/> class.
        /// </summary>
        public InternalFrame()
        {
            //由此构造函数创造的对象仅可使用Receive()方法..
        }

        /// <summary>
        /// 构造函数，只包含   数据域和数据域长度（数据帧）
        /// </summary>
        /// <param name="dataByte">数据域.</param>
        /// <param name="datalength">数据域长度.</param>
        public InternalFrame(byte[] dataByte, byte datalength)
        {
            _controlCode = 0x00;

            _frameCount = 0;

            _dataLength = datalength;

            _dataByte = dataByte;

            _checkSum = 0;

            _CheckSumCorrect = false;
        }

        /// <summary>
        /// 构造函数，包含 命令标识，命令负载，命令总长度（命令帧）
        /// </summary>
        /// <param name="commandID">4字节命令标识.</param>
        /// <param name="commandPayload">命令负载.</param>
        /// <param name="payloadLength">命令总长度.</param>
        public InternalFrame(byte[] commandID, byte[] commandPayload, byte commandLength)
        {
            _controlCode = 0x01;

            _frameCount = 0;

            _dataLength = commandLength;

            _dataByte = new byte[_dataLength];
            for (int i = 0; i < 4; i++)
            {
                _dataByte[i] = commandID[i];
            }
            if (commandLength > 4)
            {
                for (int i = 0; i < commandLength - 4; i++)
                {
                    _dataByte[4 + i] = commandPayload[i];
                }
            }

            _checkSum = 0;

            _CheckSumCorrect = false;
        }

        /// <summary>
        /// 构建将要发送的内部使用帧.
        /// </summary>
        /// <returns>要发送的字节流</returns>
        public byte[] Send()
        {
            byte[] output = new byte[10 + _dataLength];//除了数据域外 还有10个字节

            output[1] = _controlCode;

            output[6] = Convert.ToByte(_frameCount & 0x00FF);

            output[7] = _dataLength;

            if (_dataLength > 0)
            {
                for (int i = 0; i < _dataLength; i++)
                {
                    output[8 + i] = _dataByte[i];
                }
            }

            return output;
        }

        /// <summary>
        /// 计算校验和.
        /// </summary>
        /// <param name="output">其他部分已经填充好的帧.</param>
        protected void calculateCheckSum(byte[] output)
        {
            _checkSum = 0;
            for (int i = 0; i < 8 + _dataLength; i++)
            {
                _checkSum += output[i];
            }
            output[8 + _dataLength] = _checkSum;
        }

        /// <summary>
        /// 解析收到的内部使用帧.
        /// </summary>
        /// <param name="input">收到的字节流.</param>
        public void Receive(byte[] input)
        {
            try
            {
                if (input[7] + 10 == input.Length)
                {
                    _controlCode = input[1];

                    _frameCount = input[6];

                    _dataLength = input[7];

                    if (_dataLength > 0)
                    {
                        _dataByte = new byte[_dataLength];
                        for (int i = 0; i < _dataLength; i++)
                        {
                            _dataByte[i] = input[8 + i];
                        }
                    }

                    if ((_controlCode & 0x03) == 0x01) //是命令帧
                    {
                        _commandID = new byte[4];
                        for (int i = 0; i < 4; i++)
                        {
                            _commandID[i] = _dataByte[i];
                        }

                        _commandPayload = new byte[_dataLength - 4];
                        for (int i = 0; i < _dataLength - 4; i++)
                        {
                            _commandPayload[i] = _dataByte[4 + i];
                        }
                    }

                    _checkSum = 0;
                    for (int i = 0; i < 8 + _dataLength; i++)
                    {
                        _checkSum += input[i];
                    }
                    if (_checkSum == input[8 + _dataLength])
                    {
                        _CheckSumCorrect = true;
                    }
                    else
                    {
                        _CheckSumCorrect = false;
                    }
                }
                //用于处理88鉴权时用户下挂网关过多的情况，因为过多的网关会导致数据域长度超出范围
                //这个情况只能用于这么一种特殊情况下，最好永远都不要用到
                else
                {
                    _controlCode = input[1];

                    _frameCount = input[6];

                    int dataLeng = (input[13] * 7 + 6);
                    //_dataLength = (byte)(input[13]*7+6);

                    if (dataLeng > 0)
                    {
                        _dataByte = new byte[dataLeng];
                        for (int i = 0; i < dataLeng; i++)
                        {
                            _dataByte[i] = input[8 + i];
                        }
                    }

                    if ((_controlCode & 0x03) == 0x01) //是命令帧
                    {
                        _commandID = new byte[4];
                        for (int i = 0; i < 4; i++)
                        {
                            _commandID[i] = _dataByte[i];
                        }

                        _commandPayload = new byte[dataLeng - 4];
                        for (int i = 0; i < dataLeng - 4; i++)
                        {
                            _commandPayload[i] = _dataByte[4 + i];
                        }
                    }

                    _checkSum = 0;
                    for (int i = 0; i < 8 + dataLeng; i++)
                    {
                        _checkSum += input[i];
                    }
                    if (_checkSum == input[8 + dataLeng])
                    {
                        _CheckSumCorrect = true;
                    }
                    else
                    {
                        _CheckSumCorrect = false;
                    }
                }

            }
            catch (Exception)
            {
                _CheckSumCorrect = false;
            }
        }
        #endregion
    }

    /// <summary>
    /// 内部使用的68帧
    /// </summary>
    class SerialPortFrame : InternalFrame
    {
        #region 68帧的构成属性
        private string _nodeMac;
        private string _concentratorMac;

        public enum FrameHeadAndTail
        {
            Head = 0x68,
            Tail = 0x16,
        }
        public bool NeedAnswer
        {
            get
            {
                if ((_controlCode & 0x40) == 0x40)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    _controlCode |= 0x40;
                else
                    _controlCode &= 0xff - 0x40;
            }
        }
        public enum Communication
        {
            Transparent,//透传
            Nontransparent,//不透传
            InnerNet,//访问网络上的节点或集中器
            phone//保留位
        }
        public Communication CommunicationWay
        {
            get
            {
                switch (_controlCode & 0x0c)
                {
                    case 0x00: return Communication.Transparent;
                    case 0x04: return Communication.Nontransparent;
                    case 0x08: return Communication.InnerNet;
                    case 0x0c: return Communication.phone;
                    default: return Communication.Transparent;
                }
            }
            set
            {
                byte communicationWay;
                switch (value)
                {
                    case Communication.Transparent: communicationWay = 0x00;
                        break;
                    case Communication.Nontransparent: communicationWay = 0x04;
                        break;
                    case Communication.InnerNet: communicationWay = 0x08;
                        break;
                    case Communication.phone: communicationWay = 0x0c;
                        break;
                    default: communicationWay = 0x00;
                        break;
                }
                _controlCode &= 0xff - 0x0c;
                _controlCode |= communicationWay;
            }
        }
        public string NodeMac
        {
            get { return _nodeMac; }
            set { _nodeMac = value; }
        }
        public string ConcentratorMac
        {
            get { return _concentratorMac; }
            set { _concentratorMac = value; }
        }
        #endregion

        #region 68帧的构建与解构
        /// <summary>
        /// Initializes a new instance of the <see cref="SerialPortFrame"/> class.
        /// </summary>
        public SerialPortFrame()
            : base()
        {
            //由此构造函数创造的对象仅可使用Receive()方法..
        }

        /// <summary>
        /// 构造函数，只包括数据域和数据域长度（数据帧）
        /// </summary>
        /// <param name="dataByte">数据域.</param>
        /// <param name="datalength">数据域长度.</param>
        public SerialPortFrame(byte[] dataByte, byte datalength)
            : base(dataByte, datalength)
        {
            _nodeMac = "0000";
            _concentratorMac = "0000";
        }

        /// <summary>
        /// 构造函数，包括 命令标识，命令负载，命令总长度（命令帧）
        /// </summary>
        /// <param name="commandID">4字节命令标识.</param>
        /// <param name="commandPayload">命令负载.</param>
        /// <param name="payloadLength">命令总长度.</param>
        public SerialPortFrame(byte[] commandID, byte[] commandPayload, byte commandLength)
            : base(commandID, commandPayload, commandLength)
        {
            _nodeMac = "0000";
            _concentratorMac = "0000";
        }

        /// <summary>
        /// 构建将要发送的串口通信帧.
        /// </summary>
        /// <returns>
        /// 要发送的字节流
        /// </returns>
        new public byte[] Send()
        {
            byte[] output = base.Send();

            output[0] = (byte)FrameHeadAndTail.Head;

            char[] nodeMac = _nodeMac.ToCharArray();
            output[2] = Character.CharToByte(nodeMac[0], nodeMac[1]);//再传送节点地址的较高字节
            output[3] = Character.CharToByte(nodeMac[2], nodeMac[3]);//先传送节点地址的较低字节

            char[] concentratorMac = _concentratorMac.ToCharArray();
            output[4] = Character.CharToByte(concentratorMac[0], concentratorMac[1]);//再传送集中器地址的较高字节
            output[5] = Character.CharToByte(concentratorMac[2], concentratorMac[3]);//先传送集中器地址的较低字节

            calculateCheckSum(output);

            output[9 + _dataLength] = (byte)FrameHeadAndTail.Tail;

            return output;
        }

        /// <summary>
        /// 解析收到的串口通信帧.
        /// </summary>
        /// <param name="input">收到的字节流.</param>
        new public void Receive(byte[] input)
        {
            try
            {
                if ((input[0] == (byte)FrameHeadAndTail.Head) && (input[input.Length - 1] == (byte)FrameHeadAndTail.Tail))
                {
                    base.Receive(input);

                    StringBuilder nodeMac = new StringBuilder();
                    for (int i = 0; i < 2; i++)
                    {
                        nodeMac.Append(Character.ByteToChar(input[2 + i]));
                    }
                    _nodeMac = nodeMac.ToString();

                    StringBuilder concentratorMac = new StringBuilder();
                    for (int i = 0; i < 2; i++)
                    {
                        concentratorMac.Append(Character.ByteToChar(input[4 + i]));
                    }
                    _concentratorMac = concentratorMac.ToString();
                }
            }
            catch (Exception)
            {
                _CheckSumCorrect = false;
            }
        }
        #endregion
    }

    /// <summary>
    /// 内网通信帧88帧
    /// </summary>
    class IntranetFrame : InternalFrame
    {
        #region 内网通信88帧的构成属性
        public enum FrameHeadAndTail
        {
            Head = 0x88,
            Tail = 0x16,
        }
        public bool NeedAnswer
        {
            get
            {
                if ((_controlCode & 0x40) == 0x40)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    _controlCode |= 0x40;
                else
                    _controlCode &= 0xff - 0x40;
            }
        }
        public enum Communication
        {
            Transparent,//透传
            Nontransparent,//不透传
            InnerNet,//访问网络上的节点或集中器
            phone//保留位
        }
        public Communication CommunicationWay
        {
            get
            {
                switch (_controlCode & 0x0c)
                {
                    case 0x00: return Communication.Transparent;
                    case 0x04: return Communication.Nontransparent;
                    case 0x08: return Communication.InnerNet;
                    case 0x0c: return Communication.phone;
                    default: return Communication.Transparent;
                }
            }
            set
            {
                byte communicationWay;
                switch (value)
                {
                    case Communication.Transparent: communicationWay = 0x00;
                        break;
                    case Communication.Nontransparent: communicationWay = 0x04;
                        break;
                    case Communication.InnerNet: communicationWay = 0x08;
                        break;
                    case Communication.phone: communicationWay = 0x0c;
                        break;
                    default: communicationWay = 0x00;
                        break;
                }
                _controlCode &= 0xff - 0x0c;
                _controlCode |= communicationWay;
            }
        }
        #endregion

        #region 内网通信88帧的构建与解构
        public IntranetFrame()
            : base()
        {
            //由此构造函数创造的对象仅可使用Receive()方法..
        }

        /// <summary>
        /// 构造函数，只包含 数据域和数据域长度（数据帧）
        /// </summary>
        /// <param name="dataByte">数据域.</param>
        /// <param name="datalength">数据域长度.</param>
        public IntranetFrame(byte[] dataByte, byte datalength)
            : base(dataByte, datalength)
        {
            // 全部工作由父类的构造函数完成
        }

        /// <summary>
        /// 构造函数，包含 命令标识，命令负载，命令长度（命令帧）
        /// </summary>
        /// <param name="commandID">4字节命令标识.</param>
        /// <param name="commandPayload">命令负载.</param>
        /// <param name="payloadLength">命令总长度.</param>
        public IntranetFrame(byte[] commandID, byte[] commandPayload, byte commandLength)
            : base(commandID, commandPayload, commandLength)
        {
            // 全部工作由父类的构造函数完成
        }

        /// <summary>
        /// 构建将要发送的内网通信帧.
        /// </summary>
        /// <returns>
        /// 要发送的字节流
        /// </returns>
        new public byte[] Send()
        {
            byte[] output = base.Send();

            output[0] = (byte)FrameHeadAndTail.Head;

            calculateCheckSum(output);

            output[9 + _dataLength] = (byte)FrameHeadAndTail.Tail;

            return output;
        }

        /// <summary>
        /// 解析收到的内网通信帧.
        /// </summary>
        /// <param name="input">收到的字节流.</param>
        new public void Receive(byte[] input)
        {
            try
            {
                if ((input[0] == (byte)FrameHeadAndTail.Head) && (input[input.Length - 1] == (byte)FrameHeadAndTail.Tail))
                {
                    base.Receive(input);
                }
            }
            catch (Exception)
            {
                _CheckSumCorrect = false;
            }
        }
        #endregion
    }

    /// <summary>
    /// 网外通信69帧
    /// </summary>
    class NetCommunicationFrame
    {
        #region 网外通信69帧的构成属性

        private byte[] _controlCode;//控制码2字节
        private byte[] _gatewayClassification;//网关类别2字节
        private string _gatewayMAC;//网关MAC地址4字节
        private string _serverIP;//服务器IP地址4字节
        private string _mobilePhoneMAC;//手机MAC地址6字节
        private ushort _frameCount;//帧序号2字节
        private ushort _dataLength;//数据域长度2字节
        private byte[] _dataByte;//数据域N字节
        private byte _checkSum;//校验位1字节
        private bool _CheckSumCorrect;//校验是否正确

        public enum FrameHeadAndTail
        {
            Head = 0x69,
            Tail = 0x17,
        }
        /// <summary>
        /// 是否加密
        /// </summary>
        public bool Encryption
        {
            get
            {
                if ((_controlCode[1] & 0x80) == 0x80)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    _controlCode[1] |= 0x80;
                else
                    _controlCode[1] &= 0xff - 0x80;
            }
        }
        /// <summary>
        /// 是否需要应答
        /// </summary>
        public bool NeedAnswer
        {
            get
            {
                if ((_controlCode[1] & 0x40) == 0x40)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    _controlCode[1] |= 0x40;
                else
                    _controlCode[1] &= 0xff - 0x40;
            }
        }
        /// <summary>
        /// 帧类型（数据帧0，应答帧1）
        /// </summary>
        public enum FrameType
        {
            DataFrame,
            ReplyFrame,
        }
        public FrameType FrameFormat
        {
            get
            {
                if ((_controlCode[1] & 0x20) == 0x20)
                    return FrameType.ReplyFrame;
                else
                    return FrameType.DataFrame;
            }
            set
            {
                if (value == FrameType.ReplyFrame)
                    _controlCode[1] |= 0x20;
                else
                    _controlCode[1] &= 0xff - 0x20;
            }
        }
        /// <summary>
        /// 传输方向
        /// </summary>
        public enum Direction
        {
            MobilePhone2Server,
            Server2MobilePhone,
            Gateway2Server,
            Server2Gateway,
            Mobile2Gateway,//手机到网关（用于内网通信）
            Gateway2Mobile,
            MobilePhone2Server2Gateway,//手机到服务器到网关（用于外网通信）
            Gateway2Server2MobilePhone,
            Mobile2Computer,
            Computer2Mobile,
            Computer2Gateway,//电脑到网关（用于串口通信）
            Gateway2Computer,
            Multicast,//组播，用于67帧控制
            Broadcast,
            Mobile2Server2Computer,
            Computer2Server2Mobile,
        }
        public Direction TransferDirection
        {
            get
            {
                switch (_controlCode[1] & 0x0F)
                {
                    case 0x00: return Direction.MobilePhone2Server;
                    case 0x01: return Direction.Server2MobilePhone;
                    case 0x02: return Direction.Gateway2Server;
                    case 0x03: return Direction.Server2Gateway;
                    case 0x04: return Direction.Mobile2Gateway;//手机到网关（用于内网通信）
                    case 0x05: return Direction.Gateway2Mobile;
                    case 0x06: return Direction.MobilePhone2Server2Gateway;//手机到服务器到网关（用于外网通信）
                    case 0x07: return Direction.Gateway2Server2MobilePhone;
                    case 0x08: return Direction.Mobile2Computer;
                    case 0x09: return Direction.Computer2Mobile;
                    case 0x0A: return Direction.Computer2Gateway;//电脑到网关（用于串口通信）
                    case 0x0B: return Direction.Gateway2Computer;
                    case 0x0C: return Direction.Mobile2Server2Computer;
                    case 0x0D: return Direction.Computer2Server2Mobile;
                    case 0x0E: return Direction.Multicast;//组播，用于67帧控制
                    case 0x0F: return Direction.Broadcast;
                    default: return Direction.Broadcast;
                }
            }
            set
            {
                byte directionCode;
                switch (value)
                {
                    case Direction.MobilePhone2Server: directionCode = 0x00; break;
                    case Direction.Server2MobilePhone: directionCode = 0x01; break;
                    case Direction.Gateway2Server: directionCode = 0x02; break;
                    case Direction.Server2Gateway: directionCode = 0x03; break;
                    case Direction.Mobile2Gateway: directionCode = 0x04; break;//手机到网关（用于内网通信）
                    case Direction.Gateway2Mobile: directionCode = 0x05; break;
                    case Direction.MobilePhone2Server2Gateway: directionCode = 0x06; break;//手机到服务器到网关（用于外网通信）
                    case Direction.Gateway2Server2MobilePhone: directionCode = 0x07; break;
                    case Direction.Mobile2Computer: directionCode = 0x08; break;
                    case Direction.Computer2Mobile: directionCode = 0x09; break;
                    case Direction.Computer2Gateway: directionCode = 0x0A; break;//电脑到网关（用于串口通信）
                    case Direction.Gateway2Computer: directionCode = 0x0B; break;
                    case Direction.Mobile2Server2Computer: directionCode = 0x0C; break;
                    case Direction.Computer2Server2Mobile: directionCode = 0x0D; break;
                    case Direction.Multicast: directionCode = 0x0E; break;
                    case Direction.Broadcast: directionCode = 0x0F; break;
                    default: directionCode = 0x0F; break;
                }
                _controlCode[1] &= 0xff - 0x0f;
                _controlCode[1] |= directionCode;
            }
        }
        /// <summary>
        /// 网关类型
        /// </summary>
        public enum GatewayType
        {
            UnknowType,
            IntelligentFA,
            IntelligentSwitch,
            IntelligentAirPurifier,

        }
        public GatewayType GatewayClassification
        {
            get
            {
                if (_gatewayClassification[1] == 0x00)
                {
                    switch (_gatewayClassification[0])
                    {
                        case 0xfa: return GatewayType.IntelligentFA;
                        case 0xfd: return GatewayType.IntelligentSwitch;
                        case 0xfc: return GatewayType.IntelligentAirPurifier;
                        default: return GatewayType.UnknowType;
                    }
                }
                else
                    return GatewayType.UnknowType;
            }
            set
            {
                _gatewayClassification[1] = 0x00;
                switch (value)
                {
                    case GatewayType.UnknowType: _gatewayClassification[0] = 0x00; break;
                    case GatewayType.IntelligentFA: _gatewayClassification[0] = 0xfa; break;
                    case GatewayType.IntelligentSwitch: _gatewayClassification[0] = 0xfd; break;
                    case GatewayType.IntelligentAirPurifier: _gatewayClassification[0] = 0xfc; break;
                    default: _gatewayClassification[0] = 0x00; break;
                }
            }
        }
        public string GatewayMAC
        {
            get { return _gatewayMAC; }
            set { _gatewayMAC = value; }
        }
        public string ServerIP
        {
            get { return _serverIP; }
            set { _serverIP = value; }
        }
        public string MobilePhoneMAC
        {
            get { return _mobilePhoneMAC; }
            set { _mobilePhoneMAC = value; }
        }
        public ushort FrameCount
        {
            get { return _frameCount; }
            set { _frameCount = value; }
        }
        public ushort DataLength
        {
            get { return _dataLength; }
        }
        public byte[] DataByte
        {
            get { return _dataByte; }
        }
        public byte CheckSum
        {
            get { return _checkSum; }
        }
        public bool CheckSumCorrect
        {
            get { return _CheckSumCorrect; }
        }


        #endregion

        #region 网外通信69帧的构建与解构
        /// <summary>
        /// Initializes a new instance of the <see cref="NetCommunicationFrame"/> class.
        /// </summary>
        public NetCommunicationFrame()
        {
            //由此构造函数创造的对象仅可使用Receive()方法.
            _controlCode = new byte[2];
            _gatewayClassification = new byte[2];
        }

        /// <summary>
        /// 构造函数，69帧  包含 数据域和数据域长度
        /// </summary>
        /// <param name="dataByte">数据域.</param>
        /// <param name="datalength">数据域长度.</param>
        public NetCommunicationFrame(byte[] dataByte, byte datalength)
        {
            _controlCode = new byte[2];
            _controlCode[0] = 0x00;
            _controlCode[1] = 0x00;

            _gatewayClassification = new byte[2];
            _gatewayClassification[1] = 0x00;
            _gatewayClassification[0] = 0x00;

            _gatewayMAC = "00000000";

            _serverIP = "00000000";

            _mobilePhoneMAC = "000000000000";

            _frameCount = 0;

            _dataLength = datalength;

            _dataByte = dataByte;

            _checkSum = 0;

            _CheckSumCorrect = false;

        }

        /// <summary>
        /// 构建将要发送的网外通信帧.
        /// </summary>
        /// <returns>要发送的字节流.</returns>
        public byte[] Send()
        {
            byte[] output = new byte[26 + _dataLength];

            output[0] = (byte)FrameHeadAndTail.Head;

            output[1] = 0x00;

            output[2] = _controlCode[1];//控制位实际写在这里
            output[3] = _controlCode[0];

            output[4] = _gatewayClassification[1];
            output[5] = _gatewayClassification[0];//网关类型实际写在这里

            char[] gatewayMac = _gatewayMAC.ToCharArray();
            output[6] = Character.CharToByte(gatewayMac[0], gatewayMac[1]);//先传送网关地址的较高字节
            output[7] = Character.CharToByte(gatewayMac[2], gatewayMac[3]);
            output[8] = Character.CharToByte(gatewayMac[4], gatewayMac[5]);
            output[9] = Character.CharToByte(gatewayMac[6], gatewayMac[7]);//再传送网关地址的较低字节

            char[] serverIP = _serverIP.ToCharArray();
            output[10] = Character.CharToByte(serverIP[0], serverIP[1]);//先传送IP地址的较高字节
            output[11] = Character.CharToByte(serverIP[2], serverIP[3]);
            output[12] = Character.CharToByte(serverIP[4], serverIP[5]);
            output[13] = Character.CharToByte(serverIP[6], serverIP[7]);//再传送IP地址的较低字节

            char[] mobilePhoneMAC = _mobilePhoneMAC.ToCharArray();
            output[14] = Character.CharToByte(mobilePhoneMAC[0], mobilePhoneMAC[1]);//先传送手机MAC地址的较高字节
            output[15] = Character.CharToByte(mobilePhoneMAC[2], mobilePhoneMAC[3]);
            output[16] = Character.CharToByte(mobilePhoneMAC[4], mobilePhoneMAC[5]);
            output[17] = Character.CharToByte(mobilePhoneMAC[6], mobilePhoneMAC[7]);
            output[18] = Character.CharToByte(mobilePhoneMAC[8], mobilePhoneMAC[9]);
            output[19] = Character.CharToByte(mobilePhoneMAC[10], mobilePhoneMAC[11]);//再传送手机MAC地址的较低字节

            output[20] = Convert.ToByte((_frameCount >> 8) & 0x00FF);//先传送帧号的较高字节
            output[21] = Convert.ToByte(_frameCount & 0x00FF);//再传送帧号的较低字节

            output[22] = Convert.ToByte((_dataLength >> 8) & 0x00FF);//先传送数据长度的较高字节
            output[23] = Convert.ToByte(_dataLength & 0x00FF);//再传送数据长度的较低字节

            if (_dataLength > 0)
            {
                for (int i = 0; i < _dataLength; i++)
                {
                    output[24 + i] = _dataByte[i];
                }
            }

            _checkSum = 0;
            for (int i = 0; i < 24 + _dataLength; i++)
            {
                _checkSum += output[i];
            }
            output[24 + _dataLength] = _checkSum;

            output[25 + _dataLength] = (byte)FrameHeadAndTail.Tail;

            return output;
        }

        /// <summary>
        /// 解析收到的网外通信帧.检查校验和是否正确，用_CheckSumCorrect来标识。
        /// </summary>
        /// <param name="input">收到的字节流.</param>
        public void Receive(byte[] input)
        {
            try
            {
                if ((input[0] == (byte)FrameHeadAndTail.Head) && (input[input.Length - 1] == (byte)FrameHeadAndTail.Tail))
                {
                    _controlCode[1] = input[2];//
                    _controlCode[0] = input[3];

                    _gatewayClassification[1] = input[4];
                    _gatewayClassification[0] = input[5];//

                    StringBuilder gatewayMAC = new StringBuilder();
                    for (int i = 0; i < 4; i++)
                    {
                        gatewayMAC.Append(Character.ByteToChar(input[6 + i]));
                    }
                    _gatewayMAC = gatewayMAC.ToString();

                    StringBuilder serverIP = new StringBuilder();
                    for (int i = 0; i < 4; i++)
                    {
                        serverIP.Append(Character.ByteToChar(input[10 + i]));
                    }
                    _serverIP = serverIP.ToString();

                    StringBuilder mobilePhoneMAC = new StringBuilder();
                    for (int i = 0; i < 6; i++)
                    {
                        mobilePhoneMAC.Append(Character.ByteToChar(input[14 + i]));
                    }
                    _mobilePhoneMAC = mobilePhoneMAC.ToString();

                    _frameCount = input[20];
                    _frameCount = Convert.ToUInt16((_frameCount << 8) + input[21]);

                    _dataLength = input[22];
                    _dataLength = Convert.ToUInt16((_dataLength << 8) + input[23]);

                    if (_dataLength > 0)
                    {
                        _dataByte = new byte[_dataLength];
                        for (int i = 0; i < _dataLength; i++)
                        {
                            _dataByte[i] = input[24 + i];
                        }
                    }

                    _checkSum = 0;
                    for (int i = 0; i < 24 + _dataLength; i++)
                    {
                        _checkSum += input[i];
                    }
                    if (_checkSum == input[24 + _dataLength])
                    {
                        _CheckSumCorrect = true;
                    }
                    else
                    {
                        _CheckSumCorrect = false;
                    }
                }

            }
            catch (Exception)
            {
                _CheckSumCorrect = false;
            }
        }
        #endregion
    }

}

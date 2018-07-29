using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 串口收发调试工具
{
    /// <summary>
    /// 堆栈：用于发送帧的数据结构
    /// </summary>
    class Stack
    {
        private static int _length = 0;
        private static List<byte[]> storage = new List<byte[]>();
        public static int length
        {
            get { return _length; }
        }
        public static void Push(byte[] sendByte)
        {
            storage.Add(sendByte);
            _length++;
        }
        public static byte[] Pop()
        {
            if (_length < 0)
            {
                throw new Exception("堆栈已空！");
            }
            //byte[] send = storage[storage.Count - _length];//从头弹出。先进先出，实现队列的功能
            byte[] send = storage[_length];//实现栈的功能
            _length--;
            return send;
        }
        public static bool Clear()
        {
            bool clearSuccess = true;
            storage.Clear();
            if (storage.Count == 0)
            {
                clearSuccess = true;
            }
            else
            {
                clearSuccess = false;
            }
            _length = 0;
            return clearSuccess;
        }
        public static void Reset(short length)
        {
            _length = storage.Count - length;
        }
    }
     /// <summary>
    /// 队列：用于发送文件的数据结构
    /// </summary>
    class Queue
    {
        private static int _length = 0;
        private static List<byte[]> storage = new List<byte[]>();
        public static int length
        {
            get { return _length; }
        }
        public static void Push(byte[] sendByte)
        {
            storage.Add(sendByte);
            _length++;
        }
        public static byte[] Pop()
        {
            if (_length < 0)
            {
                throw new Exception("堆栈已空！");
            }
            byte[] send = storage[storage.Count - _length];
            //byte[] send = storage[_length];实现栈的功能
            _length--;
            if (_length == 0)
            {
                Clear();
            }
            return send;
        }
        public static bool Clear()
        {
            bool clearSuccess = true;
            storage.Clear();
            if (storage.Count == 0)
            {
                clearSuccess = true;
            }
            else
            {
                clearSuccess = false;
            }
            _length = 0;
            return clearSuccess;
        }
        public static void Reset(ushort length)
        {
            _length = storage.Count - length;
        }
    } 
}

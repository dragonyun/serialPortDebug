using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 串口收发调试工具
{
    /// <summary>
    /// 字节，字符串，字符数组 转换 工具类
    /// </summary>
    class Character
    {
        /// <summary>
        /// 把字符转换为字节
        /// 举例：把字符'a'和字符'b'转换成 1010 1011
        /// </summary>
        /// <param name="input1">字符1</param>
        /// <param name="input2">字符2</param>
        /// <returns></returns>
        public static byte CharToByte(char input1, char input2)
        {
            byte output = 0;
            if ((input1 >= 48) && (input1 <= 57))
            {
                input1 -= Convert.ToChar(48);
            }
            else if ((input1 >= 65) && (input1 <= 70))
            {
                input1 -= Convert.ToChar(55);
            }
            else if ((input1 >= 97) && (input1 <= 102))
            {
                input1 -= Convert.ToChar(87);
            }
            else
            {
                throw new Exception("输入的字符" + input1 + "非16进制符号");
            }
            if ((input2 >= 48) && (input2 <= 57))
            {
                input2 -= Convert.ToChar(48);
            }
            else if ((input2 >= 65) && (input2 <= 70))
            {
                input2 -= Convert.ToChar(55);
            }
            else if ((input2 >= 97) && (input2 <= 102))
            {
                input2 -= Convert.ToChar(87);
            }
            else
            {
                throw new Exception("输入的字符" + input2 + "非16进制符号");
            }
            output = Convert.ToByte((input1 << 4) + input2);
            return output;
        }

        /// <summary>
        /// 把字节转换为字符
        /// 举例：把1010 1011 转换成字符串 AB
        /// </summary>
        /// <param name="input">输入字节</param>
        /// <returns></returns>
        public static string ByteToChar(byte input)
        {
            StringBuilder output = new StringBuilder();
            if (input < 0x10)
            {
                output.Append("0").Append(Convert.ToString(input, 16));
            }
            else
            {
                output.Append(Convert.ToString(input, 16));
            }
            return output.ToString().ToUpper();
        }

        /// <summary>
        /// 把字符串转换为字节数组
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static byte[] StringToBytes(string input)
        {
            char[] inputChar = input.ToCharArray();
            byte[] outputByte = new byte[inputChar.Length / 2];
            for (int i = 0; i < outputByte.Length; i++)
            {
                outputByte[i] = CharToByte(inputChar[2 * i], inputChar[2 * i + 1]);
            }
            return outputByte;
        }

        /// <summary>
        /// 将字节数组转换为字符串.
        /// </summary>
        /// <param name="input">待转换的字节数组.</param>
        /// <param name="begin">要转换的第一个字节的位置.</param>
        /// <param name="end">要转换的最后一个个字节的位置.</param>
        /// <param name="needSpace">if set to <c>true</c> [need space].</param>
        /// <returns>转换好的字符串</returns>
        public static string BytesToString(byte[] input, int begin, int end, bool needSpace)
        {
            StringBuilder output = new StringBuilder();
            for (int i = begin; i < end + 1; i++)
            {
                if (needSpace)
                {
                    output.Append(ByteToChar(input[i])).Append(" ");
                }
                else
                {
                    output.Append(ByteToChar(input[i]));
                }
            }
            return output.ToString().ToUpper();
        }

    }
}

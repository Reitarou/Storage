using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Stg
{
    /// <summary>
    /// Класс, который позволяет преобразовывать массивы байт с строки и обратно
    /// </summary>
    [ComVisible(false)]
    public static class StgHexWriter
    {
        private static readonly string HexFormat = "X";
        private static readonly string Zero = "0";

        /// <summary>
        /// Метод переводит массив байт в строку, которую в последствии можно сохранить как текстовое поле
        /// </summary>
        /// <param name="buffer">Массив байт</param>
        /// <returns></returns>
        public static string BytesToString(byte[] buffer)
        {
            StringBuilder sb = new StringBuilder(buffer.Length * 2);
            for (int i = 0; i < buffer.Length; i++)
            {
                byte b = buffer[i];
                if (b < 16)
                {
                    sb.Append(Zero);
                    sb.Append(b.ToString(HexFormat));
                }
                else
                {
                    sb.Append(b.ToString(HexFormat));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Переводит строку в массив байт
        /// </summary>
        /// <param name="s">Строка</param>
        /// <returns></returns>
        public static byte[] StringToByte(string s)
        {
            int mod = s.Length % 2;
            if (mod != 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            byte[] result = new byte[s.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(byte.Parse(s[i * 2].ToString(), NumberStyles.AllowHexSpecifier) * 16 + byte.Parse(s[i * 2 + 1].ToString(), NumberStyles.AllowHexSpecifier));
            }
            return result;
        }
    }
}

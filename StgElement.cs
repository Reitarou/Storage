using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;
using System.Xml;

namespace Stg
{
    /// <summary>
    /// Интерфейс простого элемента сериализации в Storage
    /// </summary>
    public interface IStgElement
    {
        /// <summary>
        /// Флаг указывающий на тип данного элемента
        /// </summary>
        StgType ElementType { get; }

        /// <summary>
        /// Свойство, информирующее о том что значение данного элемента является значением по умолчанию. Сериализация элемента не требуется.
        /// </summary>
        Boolean Optional { get; }
    }
    
    /// <summary>
    /// Базовый класс простого элемента сериализации в Storage
    /// </summary>
    [ClassInterfaceAttribute(ClassInterfaceType.AutoDual)]
    public class StgElement<T> : IStgElement
    {
        internal const string sTrue = "true";
        internal const string sFasle = "false";
        
        internal T m_Target;

        internal static StgType GetElementType(Type type)
        {
            if (type == typeof(bool))
            {
                return StgType.Boolean;
            }
            else if (type == typeof(byte))
            {
                return StgType.Byte;
            }
            else if (type == typeof(char))
            {
                return StgType.Char;
            }
            else if (type == typeof(double))
            {
                return StgType.Double;
            }
            else if (type == typeof(float))
            {
                return StgType.Single;
            }
            else if (type == typeof(int))
            {
                return StgType.Int32;
            }
            else if (type == typeof(long))
            {
                return StgType.Int64;
            }
            else if (type == typeof(short))
            {
                return StgType.Int16;
            }
            else if (type == typeof(string))
            {
                return StgType.String;
            }
            else if (type == typeof(StgNode))
            {
                return StgType.Node;
            }
            else if (type == typeof(IStgArray))
            {
                return StgType.Array;
            }
            else
            {
                throw new InvalidCastException();
            }
        }
        
        /// <summary>
        /// Флаг указывающий на тип данного элемента
        /// </summary>
        public virtual StgType ElementType
        {
            get
            {
                return GetElementType(typeof(T));
            }
        }

        /// <summary>
        /// Объект хранимый в данном элементе
        /// </summary>
        public T Target
        {
            get
            {
                return this.m_Target;
            }
            internal set
            {
                this.m_Target = value;
            }
        }

        /// <summary>
        /// Свойство, информирующее о том что значение данного элемента является значением по умолчанию. Сериализация элемента не требуется.
        /// </summary>
        public virtual Boolean Optional
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Текстовое представление элемента в зависимости от конкретного типа
        /// </summary>
        /// <returns>Текстовое представление элемента</returns>
        public override string ToString()
        {
            switch (ElementType)
            {
                case StgType.Boolean:
                    {
                        if ((this as StgElement<bool>).Target)
                        {
                            return sTrue;
                        }
                        else
                        {
                            return sFasle;
                        }
                    }
                case StgType.Byte:
                    {
                        return (this as StgElement<byte>).Target.ToString("X2");
                    }
                case StgType.Char:
                    {
                        return ((ushort)(this as StgElement<char>).Target).ToString("X4");
                    }
                case StgType.Double:
                    {
                        return (this as StgElement<double>).Target.ToString(CultureInfo.InvariantCulture);
                    }
                case StgType.Single:
                    {
                        return (this as StgElement<float>).Target.ToString(CultureInfo.InvariantCulture);
                    }
                case StgType.Int32:
                    {
                        return Target.ToString();
                    }
                case StgType.Int64:
                    {
                        return Target.ToString();
                    }
                case StgType.Int16:
                    {
                        return Target.ToString();
                    }
                case StgType.String:
                    {
                        return (this as StgElement<string>).Target;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException("ImposibleConvertEntryToString");
                    }
            }
        }

        internal StgElement(T target)
        {
            this.m_Target = target;
        }

        private static string FloatToStr(double value, int digits)
        {
            return Math.Round(value, digits).ToString(CultureInfo.InvariantCulture);
        }

        internal static double StrToFloat(string value)
        {
            double result;
            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                throw new FormatException(string.Format("Invalid float point format {0}", value));
            }
            return result;
        }
    }
}

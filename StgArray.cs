using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;
using System.Text;
using Stg.Properties;

namespace Stg
{
    /// <summary>
    /// Интерфейс массива типизированных элементов
    /// </summary>
    public interface IStgArray : IStgElement
    {
        /// <summary>
        /// Тип данных, хранящихся в массиве
        /// </summary>
        StgType ArrayDataType { get; }

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Boolean"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Boolean"/></exception>
        void AddBoolean(Boolean value);

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Boolean"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Boolean"/></exception>
        void AddBoolean(params Boolean[] values);

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Byte"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Byte"/></exception>
        void AddByte(Byte value);

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Byte"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Byte"/></exception>
        void AddByte(params Byte[] values);

        void AddByte(Byte[] values, int index, int count);

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Char"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Char"/></exception>
        void AddChar(Char value);

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Char"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Char"/></exception>
        void AddChar(params Char[] values);

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Double"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Double"/></exception>
        void AddDouble(Double value);

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Double"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Double"/></exception>
        void AddDouble(params Double[] values);

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Single"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Single"/></exception>
        void AddSingle(Single value);

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Single"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Single"/></exception>
        void AddSingle(params Single[] values);

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Int16"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int16"/></exception>
        void AddInt16(Int16 value);

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Int16"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int16"/></exception>
        void AddInt16(params Int16[] values);

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Int32"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int32"/></exception>
        void AddInt32(Int32 value);

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Int32"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int32"/></exception>
        void AddInt32(params Int32[] values);

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Int64"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int64"/></exception>
        void AddInt64(Int64 value);

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Int64"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int64"/></exception>
        void AddInt64(params Int64[] values);

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="String"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.String"/></exception>
        void AddString(String value);

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="String"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.String"/></exception>
        void AddString(params String[] values);

        /// <summary>
        /// Функция добавляет новый массив, хранящий элементы типа <paramref name="dataType"/>
        /// </summary>
        /// <param name="dataType">Тип данных которые должен хранить массив</param>
        /// <returns>Созданный массив</returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Array"/></exception>
        IStgArray AddArray(StgType dataType);

        /// <summary>
        /// Функция добавляет новую ветку в массив
        /// </summary>
        /// <returns>Новую ветку</returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Node"/></exception>
        StgNode AddNode();

        /// <summary>
        /// Возвращает элемент типа <see cref="Boolean"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Boolean"/></exception>
        Boolean GetBoolean(int index);

        /// <summary>
        /// Возвращает элемент типа <see cref="Byte"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Byte"/></exception>
        Byte GetByte(int index);

        /// <summary>
        /// Возвращает элемент типа <see cref="Char"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Char"/></exception>
        Char GetChar(int index);

        /// <summary>
        /// Возвращает элемент типа <see cref="Double"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Double"/></exception>
        Double GetDouble(int index);

        /// <summary>
        /// Возвращает элемент типа <see cref="Single"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Single"/></exception>
        Single GetSingle(int index);

        /// <summary>
        /// Возвращает элемент типа <see cref="Int16"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int16"/></exception>
        Int16 GetInt16(int index);

        /// <summary>
        /// Возвращает элемент типа <see cref="Int32"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int32"/></exception>
        Int32 GetInt32(int index);

        /// <summary>
        /// Возвращает элемент типа <see cref="Int64"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int64"/></exception>
        Int64 GetInt64(int index);

        /// <summary>
        /// Возвращает элемент типа <see cref="String"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.String"/></exception>
        String GetString(int index);

        /// <summary>
        /// Возвращает элемент типа <see cref="StgNode"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Node"/></exception>
        StgNode GetNode(int index);

        /// <summary>
        /// Возвращает элемент типа <see cref="IStgArray"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <param name="dataType">Тип данных, которые хранит массив</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Array"/></exception>
        IStgArray GetArray(int index, StgType dataType);

        /// <summary>
        /// Имя элементов
        /// </summary>
        string ItemsName { get; set; }

        /// <summary>
        /// Количество элементов в массиве
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Метод устанавливает ёмкость массива достаточна для хранения указанного числа элементов
        /// </summary>
        /// <param name="capacity">Число элементов для хранения</param>
        void EnsureCapacity(int capacity);

        /// <summary>
        /// Возвращает элемент по указанному индексу
        /// </summary>
        /// <param name="index">Индекс запрашиваемого элемента</param>
        /// <returns>Приведенное к <see cref="Object"/> значение элемента</returns>
        object this[int index] { get; }

        void Copy(IStgArray array);

        void Pack(int index);

        void RemoveAt(int index);

        void RemoveRange(int index, int count);
    }

    sealed class StgNodeArray : StgElement<List<object>>, IStgArray
    {
        private static StgDocumentBinaryFormater m_DefaultPacker = new StgDocumentBinaryFormater();
        
        private StgDocument m_Doc;
        private string m_ItemsName = "Item";

        public StgNodeArray(StgDocument doc)
            : base(new List<object>())
        {
            this.m_Doc = doc;
        }

        private StgDocument Doc
        {
            get
            {
                return m_Doc;
            }
        }

        public override StgType ElementType
        {
            get
            {
                return StgType.Array;
            }
        }

        public override bool Optional
        {
            get
            {
                return Target.Count == 0;
            }
        }

        #region IStgArray Members

        public StgType ArrayDataType
        {
            get { return StgType.Node; }
        }

        #region NotSupported Add...(...)

        public void AddBoolean(bool value)
        {
            throw new NotSupportedException();
        }

        public void AddBoolean(params bool[] values)
        {
            throw new NotSupportedException();
        }

        public void AddByte(byte value)
        {
            throw new NotSupportedException();
        }

        public void AddByte(params byte[] values)
        {
            throw new NotSupportedException();
        }

        public void AddByte(Byte[] values, int index, int count)
        {
            throw new NotSupportedException();
        }

        public void AddChar(char value)
        {
            throw new NotSupportedException();
        }

        public void AddChar(params char[] values)
        {
            throw new NotSupportedException();
        }

        public void AddDouble(double value)
        {
            throw new NotSupportedException();
        }

        public void AddDouble(params double[] values)
        {
            throw new NotSupportedException();
        }

        public void AddSingle(float value)
        {
            throw new NotSupportedException();
        }

        public void AddSingle(params float[] values)
        {
            throw new NotSupportedException();
        }

        public void AddInt16(short value)
        {
            throw new NotSupportedException();
        }

        public void AddInt16(params short[] values)
        {
            throw new NotSupportedException();
        }

        public void AddInt32(int value)
        {
            throw new NotSupportedException();
        }

        public void AddInt32(params int[] values)
        {
            throw new NotSupportedException();
        }

        public void AddInt64(long value)
        {
            throw new NotSupportedException();
        }

        public void AddInt64(params long[] values)
        {
            throw new NotSupportedException();
        }

        public void AddString(string value)
        {
            throw new NotSupportedException();
        }

        public void AddString(params string[] values)
        {
            throw new NotSupportedException();
        }

        public IStgArray AddArray(StgType dataType)
        {
            throw new NotSupportedException();
        } 

        #endregion

        public StgNode AddNode()
        {
            StgNode result = new StgNode(Doc);
            Target.Add(result);
            return result;
        }

        #region NotSupported Get...(...)

        public bool GetBoolean(int index)
        {
            throw new NotSupportedException();
        }

        public byte GetByte(int index)
        {
            throw new NotSupportedException();
        }

        public char GetChar(int index)
        {
            throw new NotSupportedException();
        }

        public double GetDouble(int index)
        {
            throw new NotSupportedException();
        }

        public float GetSingle(int index)
        {
            throw new NotSupportedException();
        }

        public short GetInt16(int index)
        {
            throw new NotSupportedException();
        }

        public int GetInt32(int index)
        {
            throw new NotSupportedException();
        }

        public long GetInt64(int index)
        {
            throw new NotSupportedException();
        }

        public string GetString(int index)
        {
            throw new NotSupportedException();
        }

        public IStgArray GetArray(int index, StgType dataType)
        {
            throw new NotSupportedException();
        }

        #endregion

        public StgNode GetNode(int index)
        {
            var result = Target[index];
            if (result is byte[])
            {
                StgNode node = new StgNode(Doc);
                using (var ms = new MemoryStream((byte[])result))
                {
                    using (var reader = new StgBinaryReader(ms, Encoding.UTF8, StgDocumentBinaryFormater.VERSION))
                    {
                        StgDocumentBinaryFormater.LoadNode(reader, node);
                    }
                }
                return node;
            }
            else
            {
                return (StgNode)result;
            }
        }

        public byte[] GetPacked(int index)
        {
            return (byte[])Target[index];
        }

        public void SetPacked(int index, byte[] buffer)
        {
            Target[index] = buffer;
        }

        public void AddPacked(byte[] buffer)
        {
            Target.Add(buffer);
        }

        public bool IsPacked(int index)
        {
            return Target[index] is byte[];
        }

        public string ItemsName
        {
            get
            {
                return m_ItemsName;
            }
            set
            {
                m_ItemsName = value;
            }
        }

        public int Count
        {
            get { return Target.Count; }
        }

        public void EnsureCapacity(int capacity)
        {
            if (Target.Capacity < capacity)
            {
                Target.Capacity = capacity;
            }
        }

        public object this[int index]
        {
            get { return GetNode(index); }
        }

        public void Copy(IStgArray array)
        {
            if (array.ArrayDataType != StgType.Node)
            {
                throw new InvalidCastException();
            }
            Target.Clear();
            for (int i = 0; i < array.Count; i++)
            {
                AddNode().Copy(array.GetNode(i));
                Pack(i);
            }
        }

        public void Pack(int index)
        {
            if (!(Target[index] is byte[]))
            {
                var node = (StgNode)Target[index];
                using (var ms = new MemoryStream())
                {
                    using (var writer = new StgBinaryWriter(ms, Encoding.UTF8))
                    {
                        StgDocumentBinaryFormater.SaveNode(writer, node);
                        Target[index] = ms.ToArray();
                    }
                }
            }
        }

        public void RemoveAt(int index)
        {
            Target.RemoveAt(index);
        }

        public void RemoveRange(int index, int count)
        {
            Target.RemoveRange(index, count);
        }

        #endregion
    }
    
    /// <summary>
    /// Массив типизированных элементов
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public sealed class StgArray<T> : StgElement<List<T>>, IStgArray
    {
        private StgDocument m_Doc;
        private string m_ItemsName = "Item";

        internal List<T> Items
        {
            get
            {
                return Target;
            }
        }

        internal StgArray(StgDocument doc)
            : base(new List<T>())
        {
            this.m_Doc = doc;
        }

        private StgDocument Doc
        {
            get
            {
                return m_Doc;
            }
        }

        /// <summary>
        /// Тип данных, хранящихся в массиве
        /// </summary>
        public StgType ArrayDataType
        {
            get
            {
                return StgElement<object>.GetElementType(typeof(T));
            }
        }

        /// <summary>
        /// Флаг указывающий на тип данного элемента
        /// </summary>
        public override StgType ElementType
        {
            get
            {
                return StgType.Array;
            }
        }

        /// <summary>
        /// Количество элементов в массиве
        /// </summary>
        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        /// <summary>
        /// Метод устанавливает ёмкость массива достаточна для хранения указанного числа элементов
        /// </summary>
        /// <param name="capacity">Число элементов для хранения</param>
        public void EnsureCapacity(int capacity)
        {
            if (Items.Capacity < capacity)
            {
                Items.Capacity = capacity;
            }
        }

        /// <summary>
        /// Возвращает элемент по указанному индексу
        /// </summary>
        /// <param name="index">Индекс запрашиваемого элемента</param>
        /// <returns>Приведенное к <see cref="Object"/> значение элемента</returns>
        public T this[int index]
        {
            get
            {
                return Items[index];
            }
        }

        /// <summary>
        /// Возвращает элемент по указанному индексу
        /// </summary>
        /// <param name="index">Индекс запрашиваемого элемента</param>
        /// <returns>Приведенное к <see cref="Object"/> значение элемента</returns>
        object IStgArray.this[int index]
        {
            get { return Items[index]; }
        }

        /// <summary>
        /// Свойство, информирующее о том что значение данного элемента является значением по умолчанию. Сериализация элемента не требуется.
        /// </summary>
        public override bool Optional
        {
            get
            {
                return Items.Count == 0;
            }
        }

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Boolean"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Boolean"/></exception>
        public void AddBoolean(Boolean value)
        {
            (Items as List<bool>).Add(value);
        }

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Boolean"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Boolean"/></exception>
        public void AddBoolean(params Boolean[] values)
        {
            (Items as List<bool>).AddRange(values);
        }

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Byte"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Byte"/></exception>
        public void AddByte(Byte value)
        {
            (Items as List<byte>).Add(value);
        }

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Byte"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Byte"/></exception>
        public void AddByte(params Byte[] values)
        {
            (Items as List<byte>).AddRange(values);
        }

        public void AddByte(Byte[] values, int index, int count)
        {
            var list = (Items as List<byte>);
            if (list.Capacity < list.Count + count)
            {
                list.Capacity = list.Count + count;
            }
            for (int i = index; i < count; i++)
			{
                list.Add(values[i]);
			}
        }

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Char"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Char"/></exception>
        public void AddChar(Char value)
        {
            (Items as List<char>).Add(value);
        }

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Char"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Char"/></exception>
        public void AddChar(params Char[] values)
        {
            (Items as List<char>).AddRange(values);
        }

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Double"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Double"/></exception>
        public void AddDouble(Double value)
        {
            (Items as List<double>).Add(value);
        }

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Double"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Double"/></exception>
        public void AddDouble(params Double[] values)
        {
            (Items as List<double>).AddRange(values);
        }

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Single"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Single"/></exception>
        public void AddSingle(Single value)
        {
            (Items as List<float>).Add(value);
        }

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Single"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Single"/></exception>
        public void AddSingle(params Single[] values)
        {
            (Items as List<float>).AddRange(values);
        }

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Int16"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int16"/></exception>
        public void AddInt16(Int16 value)
        {
            (Items as List<short>).Add(value);
        }

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Int16"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int16"/></exception>
        public void AddInt16(params Int16[] values)
        {
            (Items as List<short>).AddRange(values);
        }

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Int32"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int32"/></exception>
        public void AddInt32(Int32 value)
        {
            (Items as List<int>).Add(value);
        }

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Int32"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int32"/></exception>
        public void AddInt32(params Int32[] values)
        {
            (Items as List<int>).AddRange(values);
        }

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="Int64"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int64"/></exception>
        public void AddInt64(Int64 value)
        {
            (Items as List<long>).Add(value);
        }

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="Int64"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int64"/></exception>
        public void AddInt64(params Int64[] values)
        {
            (Items as List<long>).AddRange(values);
        }

        /// <summary>
        /// Метод добавляет новый элемент типа <see cref="String"/> в коллекцию
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.String"/></exception>
        public void AddString(String value)
        {
            (Items as List<string>).Add(value);
        }

        /// <summary>
        /// Метод добавляет несколько новых элементов типа <see cref="String"/> в коллекцию
        /// </summary>
        /// <param name="values">Массив элементов</param>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.String"/></exception>
        public void AddString(params String[] values)
        {
            (Items as List<string>).AddRange(values);
        }

        /// <summary>
        /// Функция добавляет новый массив, хранящий элементы типа <paramref name="dataType"/>
        /// </summary>
        /// <param name="dataType">Тип данных которые должен хранить массив</param>
        /// <returns>Созданный массив</returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Array"/></exception>
        public IStgArray AddArray(StgType dataType)
        {
            List<IStgArray> list = Items as List<IStgArray>;
            switch (dataType)
            {
                case StgType.Array:
                    {
                        var result = new StgArray<IStgArray>(Doc);
                        list.Add(result);
                        return result;
                    }
                case StgType.Boolean:
                    {
                        var result = new StgArray<bool>(Doc);
                        list.Add(result);
                        return result;
                    }
                case StgType.Byte:
                    {
                        var result = new StgArray<byte>(Doc);
                        list.Add(result);
                        return result;
                    }
                case StgType.Char:
                    {
                        var result = new StgArray<char>(Doc);
                        list.Add(result);
                        return result;
                    }
                case StgType.Double:
                    {
                        var result = new StgArray<double>(Doc);
                        list.Add(result);
                        return result;
                    }
                case StgType.Int16:
                    {
                        var result = new StgArray<short>(Doc);
                        list.Add(result);
                        return result;
                    }
                case StgType.Int32:
                    {
                        var result = new StgArray<int>(Doc);
                        list.Add(result);
                        return result;
                    }
                case StgType.Int64:
                    {
                        var result = new StgArray<long>(Doc);
                        list.Add(result);
                        return result;
                    }
                case StgType.Node:
                    {
                        var result = new StgNodeArray(Doc);
                        list.Add(result);
                        return result;
                    }
                case StgType.Single:
                    {
                        var result = new StgArray<float>(Doc);
                        list.Add(result);
                        return result;
                    }
                case StgType.String:
                    {
                        var result = new StgArray<string>(Doc);
                        list.Add(result);
                        return result;
                    }
                default:
                    {
                        throw new InvalidCastException();
                    }
            }
        }

        /// <summary>
        /// Функция добавляет новую ветку в массив
        /// </summary>
        /// <returns>Новую ветку</returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Node"/></exception>
        public StgNode AddNode()
        {
            StgNode result = new StgNode(Doc);
            (Items as List<StgNode>).Add(result);
            return result;
        }

        /// <summary>
        /// Возвращает элемент типа <see cref="Boolean"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Boolean"/></exception>
        public Boolean GetBoolean(int index)
        {
            return (Items as List<bool>)[index];
        }

        /// <summary>
        /// Возвращает элемент типа <see cref="Byte"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Byte"/></exception>
        public Byte GetByte(int index)
        {
            return (Items as List<byte>)[index];
        }

        /// <summary>
        /// Возвращает элемент типа <see cref="Char"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Char"/></exception>
        public Char GetChar(int index)
        {
            return (Items as List<char>)[index];
        }

        /// <summary>
        /// Возвращает элемент типа <see cref="Double"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Double"/></exception>
        public Double GetDouble(int index)
        {
            return (Items as List<double>)[index];
        }

        /// <summary>
        /// Возвращает элемент типа <see cref="Single"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Single"/></exception>
        public Single GetSingle(int index)
        {
            return (Items as List<float>)[index];
        }

        /// <summary>
        /// Возвращает элемент типа <see cref="Int16"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int16"/></exception>
        public Int16 GetInt16(int index)
        {
            return (Items as List<short>)[index];
        }

        /// <summary>
        /// Возвращает элемент типа <see cref="Int32"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int32"/></exception>
        public Int32 GetInt32(int index)
        {
            return (Items as List<int>)[index];
        }

        /// <summary>
        /// Возвращает элемент типа <see cref="Int64"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Int64"/></exception>
        public Int64 GetInt64(int index)
        {
            return (Items as List<long>)[index];
        }

        /// <summary>
        /// Возвращает элемент типа <see cref="String"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.String"/></exception>
        public String GetString(int index)
        {
            return (Items as List<string>)[index];
        }

        /// <summary>
        /// Возвращает элемент типа <see cref="StgNode"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Node"/></exception>
        public StgNode GetNode(int index)
        {
            return (Items as List<StgNode>)[index];
        }

        /// <summary>
        /// Возвращает элемент типа <see cref="IStgArray"/> по указанному индексу <paramref name="index"/>
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <param name="dataType">Тип данных, которые хранит массив</param>
        /// <returns>Значение по индексу <paramref name="index"/></returns>
        /// <exception cref="System.InvalidCastException">Если тип данных, хранимых в <see cref="IStgArray"/> не совпадает с <see cref="StgType.Array"/></exception>
        public IStgArray GetArray(int index, StgType dataType)
        {
            var result = (Items as List<IStgArray>)[index];
            if (result.ArrayDataType != dataType)
            {
                throw new InvalidCastException(string.Format(Resources.eArrayNotFoundInArray, result.ArrayDataType, dataType));
            }
            return result;
        }

        /// <summary>
        /// Имя элементов
        /// </summary>
        public string ItemsName
        {
            get
            {
                return m_ItemsName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    m_ItemsName = value;
                }
            }
        }

        public void Copy(IStgArray array)
        {
            if (array.ArrayDataType != this.ArrayDataType)
            {
                throw new InvalidCastException();
            }
            Items.Clear();
            switch (ArrayDataType)
            {
                case StgType.Node:
                    for (int i = 0; i < array.Count; i++)
                    {
                        AddNode().Copy(array.GetNode(i));
                    }
                    break;
                case StgType.Array:
                    for (int i = 0; i < array.Count; i++)
                    {
                        var a = (IStgArray)array[i];
                        AddArray(a.ArrayDataType).Copy(a);
                    }
                    break;
                default:
                    Items.AddRange(((StgArray<T>)array).Items);
                    break;
            }
        }

        public void Pack(int index)
        {
            if (ElementType == StgType.Array)
            {
                for (int i = 0; i < Target.Count; i++)
                {
                    var array = (IStgArray)Target[i];
                    for (int j = 0; j < array.Count; j++)
                    {
                        array.Pack(index);
                    }
                }
            }
        }

        public void RemoveAt(int index)
        {
            Target.RemoveAt(index);
        }

        public void RemoveRange(int index, int count)
        {
            Target.RemoveRange(index, count);
        }
    }
}

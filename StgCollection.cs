using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Stg.Properties;

namespace Stg
{
    /// <summary>
    /// Коллекция простых именованных элементов
    /// </summary>
    /// <remarks>
    /// Возможно добавление следующих типов:
    /// <list type="bullet">
    ///     <item>
    ///         <term><see cref="Boolean"/></term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Byte"/></term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Char"/></term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Double"/></term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Single"/></term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Int16"/></term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Int32"/></term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Int64"/></term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="String"/></term>
    ///     </item>
    /// </list>
    /// </remarks>
    [ClassInterfaceAttribute(ClassInterfaceType.AutoDual)]
    public abstract class StgCollection: StgElement<Dictionary<int, IStgElement>>
    {
        internal Dictionary<int, IStgElement> Items
        {
            get
            {
                if (base.Target == null)
                {
                    base.Target = new Dictionary<int, IStgElement>();
                }
                return (Dictionary<int, IStgElement>)Target;
            }
        }

        private StgDocument m_Doc;

        internal StgCollection(StgDocument doc)
            : base(null)
        {
            this.m_Doc = doc;
        }

        internal StgDocument Doc
        {
            get
            {
                return m_Doc;
            }
        }

        internal int NotOptionalCount
        {
            get
            {
                if (base.Target == null)
                {
                    return 0;
                }
                int result = 0;
                foreach (var item in Items)
                {
                    if (!item.Value.Optional)
                    {
                        result++;
                    }
                }
                return result;
            }
        }

        internal int FindNameIndexOrCreate(string name)
        {
            int index;
            if (!Doc.NamespaceDictionary.TryGetValue(name, out index))
            {
                index = Doc.NamespaceList.Count;
                Doc.NamespaceList.Add(name);
                Doc.NamespaceDictionary[name] = index;
            }
            return index;
        }

        internal int FindNameIndex(string name)
        {
            int index;
            if (Doc.NamespaceDictionary.TryGetValue(name, out index))
            {
                return index;
            }
            return -1;
        }

        internal void ValidName(string name)
        {
#if DEBUG
            if (!StgSyntax.IsValidName(name))
            {
                throw new InvalidOperationException("Invalid stg element name");
            }
#endif
            if (IsExists(name))
            {
                throw new ArgumentOutOfRangeException("Name dublicated: " + name);
            }
        }

        public string GetName(int id)
        {
            return Doc.NamespaceList[id];
        }

        /// <summary>
        /// Существует ли элемент с указанным именем в коллекции
        /// </summary>
        /// <param name="name">Имя элемента</param>
        /// <returns><c>true</c> если элемент существует, в противном случае <c>false</c></returns>
        public bool IsExists(string name)
        {
            int index;
            if (Doc.NamespaceDictionary.TryGetValue(name, out index))
            {
                return Items.ContainsKey(index);
            }
            return false;
        }

        #region Boolean

        /// <summary>
        /// Метод добавляет обязательный именованный элемент типа <see cref="Boolean"/>
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddBoolean(string name, Boolean value)
        {
            ValidName(name);
            Items.Add(FindNameIndexOrCreate(name), new StgElement<bool>(value));
        }

        internal void AddBoolean(int index, Boolean value)
        {
            Items.Add(index, new StgElement<bool>(value));
        }

        /// <summary>
        /// Метод добавляет <c>необязательный</c> именованный элемент типа <see cref="Boolean"/>. В случае если значение <paramref name="value"/> совпадает со значение <paramref name="optional"/>, то данный элемент не будет сериализован.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <param name="optional">Значение по умолчанию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddBoolean(string name, Boolean value, Boolean optional)
        {
            if (optional != value)
            {
                ValidName(name);
                Items.Add(FindNameIndexOrCreate(name), new StgElement<bool>(value));
            }
        }

        #endregion

        #region Byte

        /// <summary>
        /// Метод добавляет обязательный именованный элемент типа <see cref="Byte"/>
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddByte(string name, Byte value)
        {
            ValidName(name);
            Items.Add(FindNameIndexOrCreate(name), new StgElement<byte>(value));
        }

        internal void AddByte(int index, Byte value)
        {
            Items.Add(index, new StgElement<byte>(value));
        }

        /// <summary>
        /// Метод добавляет <c>необязательный</c> именованный элемент типа <see cref="Byte"/>. В случае если значение <paramref name="value"/> совпадает со значение <paramref name="optional"/>, то данный элемент не будет сериализован.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <param name="optional">Значение по умолчанию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddByte(string name, Byte value, Byte optional)
        {
            if (optional != value)
            {
                ValidName(name);
                Items.Add(FindNameIndexOrCreate(name), new StgElement<byte>(value));
            }
        }

        #endregion

        #region Char

        /// <summary>
        /// Метод добавляет обязательный именованный элемент типа <see cref="Char"/>
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddChar(string name, Char value)
        {
            ValidName(name);
            Items.Add(FindNameIndexOrCreate(name), new StgElement<char>(value));
        }

        internal void AddChar(int index, Char value)
        {
            Items.Add(index, new StgElement<char>(value));
        }

        /// <summary>
        /// Метод добавляет <c>необязательный</c> именованный элемент типа <see cref="Char"/>. В случае если значение <paramref name="value"/> совпадает со значение <paramref name="optional"/>, то данный элемент не будет сериализован.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <param name="optional">Значение по умолчанию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddChar(string name, Char value, Char optional)
        {
            if (optional != value)
            {
                ValidName(name);
                Items.Add(FindNameIndexOrCreate(name), new StgElement<char>(value));
            }
        }

        #endregion

        #region Double

        /// <summary>
        /// Метод добавляет обязательный именованный элемент типа <see cref="Double"/>
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddDouble(string name, Double value)
        {
            ValidName(name);
            Items.Add(FindNameIndexOrCreate(name), new StgElement<double>(value));
        }

        internal void AddDouble(int index, Double value)
        {
            Items.Add(index, new StgElement<double>(value));
        }

        /// <summary>
        /// Метод добавляет <c>необязательный</c> именованный элемент типа <see cref="Double"/>. В случае если значение <paramref name="value"/> совпадает со значение <paramref name="optional"/>, то данный элемент не будет сериализован.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <param name="optional">Значение по умолчанию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddDouble(string name, Double value, Double optional)
        {
            if (optional != value)
            {
                ValidName(name);
                Items.Add(FindNameIndexOrCreate(name), new StgElement<double>(value));
            }
        }

        #endregion

        #region Single

        /// <summary>
        /// Метод добавляет обязательный именованный элемент типа <see cref="Single"/>
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddSingle(string name, Single value)
        {
            ValidName(name);
            Items.Add(FindNameIndexOrCreate(name), new StgElement<float>(value));
        }

        internal void AddSingle(int index, Single value)
        {
            Items.Add(index, new StgElement<float>(value));
        }

        /// <summary>
        /// Метод добавляет <c>необязательный</c> именованный элемент типа <see cref="Single"/>. В случае если значение <paramref name="value"/> совпадает со значение <paramref name="optional"/>, то данный элемент не будет сериализован.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <param name="optional">Значение по умолчанию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddSingle(string name, Single value, Single optional)
        {
            if (optional != value)
            {
                ValidName(name);
                Items.Add(FindNameIndexOrCreate(name), new StgElement<float>(value));
            }
        }

        #endregion

        #region Int16

        /// <summary>
        /// Метод добавляет обязательный именованный элемент типа <see cref="Int16"/>
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddInt16(string name, Int16 value)
        {
            ValidName(name);
            Items.Add(FindNameIndexOrCreate(name), new StgElement<short>(value));
        }

        internal void AddInt16(int index, Int16 value)
        {
            Items.Add(index, new StgElement<short>(value));
        }

        /// <summary>
        /// Метод добавляет <c>необязательный</c> именованный элемент типа <see cref="Int16"/>. В случае если значение <paramref name="value"/> совпадает со значение <paramref name="optional"/>, то данный элемент не будет сериализован.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <param name="optional">Значение по умолчанию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddInt16(string name, Int16 value, Int16 optional)
        {
            if (optional != value)
            {
                ValidName(name);
                Items.Add(FindNameIndexOrCreate(name), new StgElement<short>(value));
            }
        }

        #endregion
        
        #region Int32

        /// <summary>
        /// Метод добавляет обязательный именованный элемент типа <see cref="Int32"/>
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddInt32(string name, Int32 value)
        {
            ValidName(name);
            Items.Add(FindNameIndexOrCreate(name), new StgElement<int>(value));
        }

        internal void AddInt32(int index, Int32 value)
        {
            Items.Add(index, new StgElement<int>(value));
        }

        /// <summary>
        /// Метод добавляет <c>необязательный</c> именованный элемент типа <see cref="Int32"/>. В случае если значение <paramref name="value"/> совпадает со значение <paramref name="optional"/>, то данный элемент не будет сериализован.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <param name="optional">Значение по умолчанию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddInt32(string name, Int32 value, Int32 optional)
        {
            if (optional != value)
            {
                ValidName(name);
                Items.Add(FindNameIndexOrCreate(name), new StgElement<int>(value));
            }
        }

        public void AddUInt32(string name, UInt32 value)
        {
            unchecked
            {
                AddInt32(name, (int)value);
            }
        }

        public void AddUInt32(string name, UInt32 value, UInt32 optional)
        {
            unchecked
            {
                AddInt32(name, (int)value, (int)optional);
            }
        }

        #endregion

        #region Int64

        /// <summary>
        /// Метод добавляет обязательный именованный элемент типа <see cref="Int64"/>
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddInt64(string name, Int64 value)
        {
            ValidName(name);
            Items.Add(FindNameIndexOrCreate(name), new StgElement<long>(value));
        }

        internal void AddInt64(int index, Int64 value)
        {
            Items.Add(index, new StgElement<long>(value));
        }

        /// <summary>
        /// Метод добавляет <c>необязательный</c> именованный элемент типа <see cref="Int64"/>. В случае если значение <paramref name="value"/> совпадает со значение <paramref name="optional"/>, то данный элемент не будет сериализован.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <param name="optional">Значение по умолчанию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddInt64(string name, Int64 value, Int64 optional)
        {
            if (optional != value)
            {
                ValidName(name);
                Items.Add(FindNameIndexOrCreate(name), new StgElement<long>(value));
            }
        }

        #endregion

        #region String

        /// <summary>
        /// Метод добавляет обязательный именованный элемент типа <see cref="String"/>
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddString(string name, String value)
        {
            ValidName(name);
            if (value == null)
            {
                value = string.Empty;
            }
            Items.Add(FindNameIndexOrCreate(name), new StgElement<string>(value));
        }

        internal void AddString(int index, String value)
        {
            Items.Add(index, new StgElement<string>(value));
        }

        /// <summary>
        /// Метод добавляет <c>необязательный</c> именованный элемент типа <see cref="String"/>. В случае если значение <paramref name="value"/> совпадает со значение <paramref name="optional"/>, то данный элемент не будет сериализован.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="value">Значение, которое необходимо добавить в коллекцию</param>
        /// <param name="optional">Значение по умолчанию</param>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public void AddString(string name, String value, String optional)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            if (optional == null)
            {
                optional = string.Empty;
            }
            if (optional != value)
            {
                ValidName(name);
                Items.Add(FindNameIndexOrCreate(name), new StgElement<string>(value));
            }
        }

        #endregion

        /// <summary>
        /// Метод запрашивающий свойства, которые поддерживают значения по умолчанию.
        /// Возвращает именованное значение типа <see cref="Boolean"/>.
        /// Если элемент с таким именем не найден в коллекции, то возвращается значение по умолчанию <paramref name="defaultValue"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Возвращает найденное значение или значение по умолчанию</returns>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Boolean GetBoolean(string name, Boolean defaultValue)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Boolean)
                {
                    return (entry as StgElement<bool>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return ((entry as StgElement<string>).Target).Equals(sTrue, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Boolean), entry.ElementType.ToString(), name));
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые не поддерживают значений по умолчанию.
        /// Возвращает именованное значение типа <see cref="Boolean"/>.
        /// Если элемент с таким именем не найден в коллекции, то выбрасывается исключение <see cref="System.Collections.Generic.KeyNotFoundException"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <returns>Возвращает найденное значение</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Если элемент с заданным именем не найден в коллекции</exception>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Boolean GetBoolean(string name)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Boolean)
                {
                    return (entry as StgElement<bool>).Target;
                }
                if (entry.ElementType == StgType.String)
                {
                    return ((entry as StgElement<string>).Target).Equals(sTrue, StringComparison.OrdinalIgnoreCase);
                }
                throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Boolean), entry.ElementType.ToString(), name));
            }
            throw new KeyNotFoundException(string.Format(Resources.eElementNotFound, name, typeof(Boolean)));
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые поддерживают значения по умолчанию.
        /// Возвращает именованное значение типа <see cref="Byte"/>.
        /// Если элемент с таким именем не найден в коллекции, то возвращается значение по умолчанию <paramref name="defaultValue"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Возвращает найденное значение или значение по умолчанию</returns>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Byte GetByte(string name, Byte defaultValue)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Byte)
                {
                    return (entry as StgElement<byte>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return byte.Parse((entry as StgElement<string>).Target, System.Globalization.NumberStyles.AllowHexSpecifier);
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Byte), entry.ElementType.ToString(), name));
                    //return Convert.ToByte(entry.Target);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые не поддерживают значений по умолчанию.
        /// Возвращает именованное значение типа <see cref="Byte"/>.
        /// Если элемент с таким именем не найден в коллекции, то выбрасывается исключение <see cref="System.Collections.Generic.KeyNotFoundException"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <returns>Возвращает найденное значение</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Если элемент с заданным именем не найден в коллекции</exception>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Byte GetByte(string name)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Byte)
                {
                    return (entry as StgElement<byte>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return byte.Parse((entry as StgElement<string>).Target, System.Globalization.NumberStyles.AllowHexSpecifier);
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Byte), entry.ElementType.ToString(), name));
                    //return Convert.ToByte(entry.Target);
                }
            }
            throw new KeyNotFoundException(string.Format(Resources.eElementNotFound, name, typeof(Byte)));
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые поддерживают значения по умолчанию.
        /// Возвращает именованное значение типа <see cref="Char"/>.
        /// Если элемент с таким именем не найден в коллекции, то возвращается значение по умолчанию <paramref name="defaultValue"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Возвращает найденное значение или значение по умолчанию</returns>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Char GetChar(string name, Char defaultValue)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Char)
                {
                    return (entry as StgElement<char>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return (char)ushort.Parse((entry as StgElement<string>).Target, System.Globalization.NumberStyles.AllowHexSpecifier);
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Char), entry.ElementType.ToString(), name));
                    //return Convert.ToChar(entry.Target);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые не поддерживают значений по умолчанию.
        /// Возвращает именованное значение типа <see cref="Char"/>.
        /// Если элемент с таким именем не найден в коллекции, то выбрасывается исключение <see cref="System.Collections.Generic.KeyNotFoundException"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <returns>Возвращает найденное значение</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Если элемент с заданным именем не найден в коллекции</exception>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Char GetChar(string name)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Char)
                {
                    return (entry as StgElement<char>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return (char)ushort.Parse((entry as StgElement<string>).Target, System.Globalization.NumberStyles.AllowHexSpecifier);
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Char), entry.ElementType.ToString(), name));
                    //return Convert.ToChar(entry.Target);
                }
            }
            throw new KeyNotFoundException(string.Format(Resources.eElementNotFound, name, typeof(Char)));
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые поддерживают значения по умолчанию.
        /// Возвращает именованное значение типа <see cref="Double"/>.
        /// Если элемент с таким именем не найден в коллекции, то возвращается значение по умолчанию <paramref name="defaultValue"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Возвращает найденное значение или значение по умолчанию</returns>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Double GetDouble(string name, Double defaultValue)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Double)
                {
                    return (entry as StgElement<double>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return StgElement<string>.StrToFloat((entry as StgElement<string>).Target);
                }
                else if (entry.ElementType == StgType.Single)
                {
                    return (entry as StgElement<float>).Target;
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Double), entry.ElementType.ToString(), name));
                    //Convert.ToDouble(entry.Target);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые не поддерживают значений по умолчанию.
        /// Возвращает именованное значение типа <see cref="Double"/>.
        /// Если элемент с таким именем не найден в коллекции, то выбрасывается исключение <see cref="System.Collections.Generic.KeyNotFoundException"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <returns>Возвращает найденное значение</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Если элемент с заданным именем не найден в коллекции</exception>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Double GetDouble(string name)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Double)
                {
                    return (entry as StgElement<double>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return StgElement<string>.StrToFloat((entry as StgElement<string>).Target);
                }
                else if (entry.ElementType == StgType.Single)
                {
                    return (entry as StgElement<float>).Target;
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Double), entry.ElementType.ToString(), name));
                    //Convert.ToDouble(entry.Target);
                }
            }
            throw new KeyNotFoundException(string.Format(Resources.eElementNotFound, name, typeof(Double)));
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые поддерживают значения по умолчанию.
        /// Возвращает именованное значение типа <see cref="Single"/>.
        /// Если элемент с таким именем не найден в коллекции, то возвращается значение по умолчанию <paramref name="defaultValue"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Возвращает найденное значение или значение по умолчанию</returns>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Single GetSingle(string name, float defaultValue)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Single)
                {
                    return (entry as StgElement<float>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return (Single)StgElement<string>.StrToFloat((entry as StgElement<string>).Target);
                }
                else if (entry.ElementType == StgType.Double)
                {
                    Debug.Fail("Overflow");
                    return (float)(entry as StgElement<double>).Target;
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Single), entry.ElementType.ToString(), name));
                    //Convert.ToSingle(entry.Target);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые не поддерживают значений по умолчанию.
        /// Возвращает именованное значение типа <see cref="Single"/>.
        /// Если элемент с таким именем не найден в коллекции, то выбрасывается исключение <see cref="System.Collections.Generic.KeyNotFoundException"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <returns>Возвращает найденное значение</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Если элемент с заданным именем не найден в коллекции</exception>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Single GetSingle(string name)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Single)
                {
                    return (entry as StgElement<float>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return (Single)StgElement<string>.StrToFloat((entry as StgElement<string>).Target);
                }
                else if (entry.ElementType == StgType.Double)
                {
                    Debug.Fail("Overflow");
                    return (float)(entry as StgElement<double>).Target;
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Single), entry.ElementType.ToString(), name));
                    //Convert.ToSingle(entry.Target);
                }
            }
            throw new KeyNotFoundException(string.Format(Resources.eElementNotFound, name, typeof(Single)));
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые поддерживают значения по умолчанию.
        /// Возвращает именованное значение типа <see cref="Int16"/>.
        /// Если элемент с таким именем не найден в коллекции, то возвращается значение по умолчанию <paramref name="defaultValue"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Возвращает найденное значение или значение по умолчанию</returns>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Int16 GetInt16(string name, Int16 defaultValue)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Int16)
                {
                    return (entry as StgElement<short>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return Int16.Parse((entry as StgElement<string>).Target);
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Int16), entry.ElementType.ToString(), name));
                    //Convert.ToInt16(entry.Target);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые не поддерживают значений по умолчанию.
        /// Возвращает именованное значение типа <see cref="Int16"/>.
        /// Если элемент с таким именем не найден в коллекции, то выбрасывается исключение <see cref="System.Collections.Generic.KeyNotFoundException"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <returns>Возвращает найденное значение</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Если элемент с заданным именем не найден в коллекции</exception>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Int16 GetInt16(string name)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Int16)
                {
                    return (entry as StgElement<short>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return Int16.Parse((entry as StgElement<string>).Target);
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Int16), entry.ElementType.ToString(), name));
                    //Convert.ToInt16(entry.Target);
                }
            }
            throw new KeyNotFoundException(string.Format(Resources.eElementNotFound, name, typeof(Int16)));
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые поддерживают значения по умолчанию.
        /// Возвращает именованное значение типа <see cref="Int32"/>.
        /// Если элемент с таким именем не найден в коллекции, то возвращается значение по умолчанию <paramref name="defaultValue"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Возвращает найденное значение или значение по умолчанию</returns>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Int32 GetInt32(string name, Int32 defaultValue)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Int32)
                {
                    return (entry as StgElement<int>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return Int32.Parse((entry as StgElement<string>).Target);
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Int32), entry.ElementType.ToString(), name));
                    //Convert.ToInt32(entry.Target);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые не поддерживают значений по умолчанию.
        /// Возвращает именованное значение типа <see cref="Int32"/>.
        /// Если элемент с таким именем не найден в коллекции, то выбрасывается исключение <see cref="System.Collections.Generic.KeyNotFoundException"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <returns>Возвращает найденное значение</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Если элемент с заданным именем не найден в коллекции</exception>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Int32 GetInt32(string name)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Int32)
                {
                    return (entry as StgElement<int>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return Int32.Parse((entry as StgElement<string>).Target);
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Int32), entry.ElementType.ToString(), name));
                    //Convert.ToInt32(entry.Target);
                }
            }
            throw new KeyNotFoundException(string.Format(Resources.eElementNotFound, name, typeof(Int32)));
        }

        public UInt32 GetUInt32(string name)
        {
            unchecked
            {
                return (uint)GetInt32(name);
            }
        }

        public UInt32 GetUInt32(string name, UInt32 defaultValue)
        {
            unchecked
            {
                return (uint)GetInt32(name, (int)defaultValue);
            }
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые поддерживают значения по умолчанию.
        /// Возвращает именованное значение типа <see cref="Int64"/>.
        /// Если элемент с таким именем не найден в коллекции, то возвращается значение по умолчанию <paramref name="defaultValue"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Возвращает найденное значение или значение по умолчанию</returns>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Int64 GetInt64(string name, Int64 defaultValue)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Int64)
                {
                    return (entry as StgElement<long>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return Int64.Parse((entry as StgElement<string>).Target);
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Int64), entry.ElementType.ToString(), name));
                    //Convert.ToInt64(entry.Target);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые не поддерживают значений по умолчанию.
        /// Возвращает именованное значение типа <see cref="Int64"/>.
        /// Если элемент с таким именем не найден в коллекции, то выбрасывается исключение <see cref="System.Collections.Generic.KeyNotFoundException"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <returns>Возвращает найденное значение</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Если элемент с заданным именем не найден в коллекции</exception>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public Int64 GetInt64(string name)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Int64)
                {
                    return (entry as StgElement<long>).Target;
                }
                else if (entry.ElementType == StgType.String)
                {
                    return Int64.Parse((entry as StgElement<string>).Target);
                }
                else
                {
                    //Convert.ToInt64(entry.Target);
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(Int64), entry.ElementType.ToString(), name));
                }
            }
            throw new KeyNotFoundException(string.Format(Resources.eElementNotFound, name, typeof(Int64)));
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые поддерживают значения по умолчанию.
        /// Возвращает именованное значение типа <see cref="String"/>.
        /// Если элемент с таким именем не найден в коллекции, то возвращается значение по умолчанию <paramref name="defaultValue"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Возвращает найденное значение или значение по умолчанию</returns>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public String GetString(string name, String defaultValue)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.String)
                {
                    return (entry as StgElement<string>).Target;
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(String), entry.ElementType.ToString(), name));
                    //Convert.ToString(entry.Target);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Метод запрашивающий свойства, которые не поддерживают значений по умолчанию.
        /// Возвращает именованное значение типа <see cref="String"/>.
        /// Если элемент с таким именем не найден в коллекции, то выбрасывается исключение <see cref="System.Collections.Generic.KeyNotFoundException"/>.
        /// В случае если элемент с заданным именем найден и возможно приведение типов, то возвращается приведенное значение,
        /// в противном случае выбрасывается исключение <see cref="System.InvalidCastException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <returns>Возвращает найденное значение</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Если элемент с заданным именем не найден в коллекции</exception>
        /// <exception cref="System.InvalidCastException">Если элемент с именем <paramref name="name"/> найден и невозможно приведение к запрашиваемому типу</exception>
        public String GetString(string name)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.String)
                {
                    return (entry as StgElement<string>).Target;
                }
                else
                {
                    throw new InvalidCastException(string.Format(Resources.eInvalidTypeCast, typeof(String), entry.ElementType.ToString(), name));
                }
            }
            throw new KeyNotFoundException(string.Format(Resources.eElementNotFound, name, typeof(String)));
        }
    }
}

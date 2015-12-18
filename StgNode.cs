using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Stg.Properties;

namespace Stg
{
    /// <summary>
    /// Ветка дерева Stg документа
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public sealed class StgNode: StgCollection
    {
        private StgAttribute m_Attribute;

        internal StgNode(StgDocument doc)
            : base(doc)
        {
        }

        /// <summary>
        /// Атрибуты ветки <seealso cref="StgCollection"/>
        /// </summary>
        public StgCollection Attribute
        {
            get
            {
                if (m_Attribute == null)
                {
                    this.m_Attribute = new StgAttribute(base.Doc);
                }
                return m_Attribute;
            }
        }

        public bool IsAttributeExists
        {
            get
            {
                return m_Attribute != null;
            }
        }

        public void Copy(StgNode node)
        {
            #region Копирование атрибутов
            if (m_Attribute != null)
            {
                Attribute.Items.Clear();
            }
            if (node.m_Attribute != null)
            {
                foreach (var item in node.Attribute.Items)
                {
                    var s = node.Doc.NamespaceList[item.Key];
                    switch (item.Value.ElementType)
                    {
                        case StgType.Boolean:
                            Attribute.AddBoolean(s, (item.Value as StgElement<Boolean>).Target);
                            break;
                        case StgType.Byte:
                            Attribute.AddByte(s, (item.Value as StgElement<Byte>).Target);
                            break;
                        case StgType.Char:
                            Attribute.AddChar(s, (item.Value as StgElement<Char>).Target);
                            break;
                        case StgType.Int16:
                            Attribute.AddInt16(s, (item.Value as StgElement<Int16>).Target);
                            break;
                        case StgType.Int32:
                            Attribute.AddInt32(s, (item.Value as StgElement<Int32>).Target);
                            break;
                        case StgType.Int64:
                            Attribute.AddInt64(s, (item.Value as StgElement<Int64>).Target);
                            break;
                        case StgType.Single:
                            Attribute.AddSingle(s, (item.Value as StgElement<Single>).Target);
                            break;
                        case StgType.Double:
                            Attribute.AddDouble(s, (item.Value as StgElement<Double>).Target);
                            break;
                        case StgType.String:
                            Attribute.AddString(s, (item.Value as StgElement<String>).Target);
                            break;
                        default:
                            Debug.Assert(false);
                            break;
                    }
                }
            }
            #endregion
            #region Копирование свойств
            if (m_Target != null)
            {
                base.Items.Clear();
            }
            if (node.m_Target != null)
            {
                foreach (var item in node.Items)
                {
                    var s = node.Doc.NamespaceList[item.Key];
                    switch (item.Value.ElementType)
                    {
                        case StgType.Node:
                            AddNode(s).Copy((StgNode)item.Value);
                            break;
                        case StgType.Array:
                            AddArray(s, ((IStgArray)item.Value).ArrayDataType).Copy((IStgArray)item.Value);
                            break;
                        case StgType.Boolean:
                            AddBoolean(s, (item.Value as StgElement<Boolean>).Target);
                            break;
                        case StgType.Byte:
                            AddByte(s, (item.Value as StgElement<Byte>).Target);
                            break;
                        case StgType.Char:
                            AddChar(s, (item.Value as StgElement<Char>).Target);
                            break;
                        case StgType.Int16:
                            AddInt16(s, (item.Value as StgElement<Int16>).Target);
                            break;
                        case StgType.Int32:
                            AddInt32(s, (item.Value as StgElement<Int32>).Target);
                            break;
                        case StgType.Int64:
                            AddInt64(s, (item.Value as StgElement<Int64>).Target);
                            break;
                        case StgType.Single:
                            AddSingle(s, (item.Value as StgElement<Single>).Target);
                            break;
                        case StgType.Double:
                            AddDouble(s, (item.Value as StgElement<Double>).Target);
                            break;
                        case StgType.String:
                            AddString(s, (item.Value as StgElement<String>).Target);
                            break;
                        default:
                            Debug.Assert(false);
                            break;
                    }
                }
            } 
            #endregion
        }
                
        /// <summary>
        /// Метод добавляет новую именованную подветку.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <returns>Новая подветка</returns>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        public StgNode AddNode(string name)
        {
            ValidName(name);
            StgNode node = new StgNode(Doc);
            Items.Add(FindNameIndexOrCreate(name), node);
            return node;
        }

        internal StgNode AddNode(int index)
        {
            StgNode node = new StgNode(Doc);
            Items.Add(index, node);
            return node;
        }

        /// <summary>
        /// Метод добавляет новый именованный типизированный массив элементов заданного тип <paramref name="dataType"/>
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="dataType">Тип элементов в массив. Может быть любое значение <see cref="StgType"/></param>
        /// <returns>Новый массив</returns>
        /// <exception cref="System.InvalidOperationException">Некорректное имя элемента</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">В случае если элемент с заданным именем уже существует в коллекции</exception>
        /// <seealso cref="IStgArray"/>
        public IStgArray AddArray(string name, StgType dataType)
        {
            ValidName(name);
            return AddArray(FindNameIndexOrCreate(name), dataType);
        }

        internal IStgArray AddArray(int index, StgType dataType)
        {
            switch (dataType)
            {
                case StgType.Array:
                    {
                        var array = new StgArray<IStgArray>(Doc);
                        Items.Add(index, array);
                        return array;
                    }
                case StgType.Boolean:
                    {
                        var array = new StgArray<bool>(Doc);
                        Items.Add(index, array);
                        return array;
                    }
                case StgType.Byte:
                    {
                        var array = new StgArray<byte>(Doc);
                        Items.Add(index, array);
                        return array;
                    }
                case StgType.Char:
                    {
                        var array = new StgArray<char>(Doc);
                        Items.Add(index, array);
                        return array;
                    }
                case StgType.Double:
                    {
                        var array = new StgArray<double>(Doc);
                        Items.Add(index, array);
                        return array;
                    }
                case StgType.Int16:
                    {
                        var array = new StgArray<short>(Doc);
                        Items.Add(index, array);
                        return array;
                    }
                case StgType.Int32:
                    {
                        var array = new StgArray<int>(Doc);
                        Items.Add(index, array);
                        return array;
                    }
                case StgType.Int64:
                    {
                        var array = new StgArray<long>(Doc);
                        Items.Add(index, array);
                        return array;
                    }
                case StgType.Node:
                    {
                        var array = new StgNodeArray(Doc);
                        Items.Add(index, array);
                        return array;
                    }
                case StgType.Single:
                    {
                        var array = new StgArray<float>(Doc);
                        Items.Add(index, array);
                        return array;
                    }
                case StgType.String:
                    {
                        var array = new StgArray<string>(Doc);
                        Items.Add(index, array);
                        return array;
                    }
                default:
                    {
                        throw new InvalidCastException();
                    }
            }
        }

        /// <summary>
        /// Функция возвращает подветку с именем <paramref name="name"/>.
        /// Если элемент с заданным именем не найден в коллекции,
        /// то исключение не выбрасывается, создается пустая ветка с именем <paramref name="name"/>.
        /// Если элемент с заданным именем существут, но имеет тип отличный от <see cref="StgType.Node"/>,
        /// то выбрасывается исключение <see cref="System.Collections.Generic.KeyNotFoundException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <returns>Найденная подветка</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">В случае если элемент с именем <paramref name="name"/> найден, но имеет тип отличный от <see cref="StgType.Node"/></exception>
        public StgNode GetNode(string name)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Node)
                {
                    return (StgNode)entry;
                }
                throw new KeyNotFoundException(string.Format(Resources.eNodeNotFound, name, entry.ElementType));
            }
            return AddNode(name);
        }

        /// <summary>
        /// Функция возвращает массив с именем <paramref name="name"/>.
        /// Если элемент с заданным именем не найден в коллекции,
        /// то исключение не выбрасывается, создается пустой массив с именем <paramref name="name"/>.
        /// Если элемент с заданным именем существут, но имеет тип отличный от <see cref="StgType.Array"/>,
        /// или тип хранимых в массиве элементов отличен от <paramref name="dataType"/>,
        /// то выбрасывается исключение <see cref="System.Collections.Generic.KeyNotFoundException"/>.
        /// </summary>
        /// <param name="name">Имя уникальное в текущей коллекции</param>
        /// <param name="dataType">Тип данных, хранящийся в массиве</param>
        /// <returns>Найденный массив</returns>
        /// /// <exception cref="System.Collections.Generic.KeyNotFoundException">В случае если элемент с именем <paramref name="name"/> найден, но имеет тип отличный от <see cref="StgType.Array"/>,
        /// или тип данных, хранящихся в массиве отличен от <paramref name="dataType"/></exception>
        public IStgArray GetArray(string name, StgType dataType)
        {
            int index = FindNameIndex(name);
            IStgElement entry;
            if (Items.TryGetValue(index, out entry))
            {
                if (entry.ElementType == StgType.Array)
                {
                    if (((IStgArray)entry).ArrayDataType != dataType)
                    {
                        throw new KeyNotFoundException(string.Format(Resources.eArrayNotFoundOtherType, name, dataType, ((IStgElement)entry).ElementType));
                    }
                    else
                    {
                        return (IStgArray)entry;
                    }
                }
                throw new KeyNotFoundException(string.Format(Resources.eArrayNotFound, name, dataType));
            }
            return AddArray(name, dataType);           
        }

        /// <summary>
        /// Свойство, информирующее о том что значение данного элемента является значением по умолчанию. Сериализация элемента не требуется.
        /// </summary>
        public override bool Optional
        {
            get
            {
                return (base.NotOptionalCount == 0) && ((m_Attribute == null) || (Attribute.NotOptionalCount == 0));
            }
        }

        /// <summary>
        /// Флаг указывающий на тип данного элемента
        /// </summary>
        public override StgType ElementType
        {
            get
            {
                return StgType.Node;
            }
        }
    }
}

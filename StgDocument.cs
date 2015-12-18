using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Stg
{
    /// <summary>
    /// Документ Storage'а
    /// </summary>
    [ClassInterfaceAttribute(ClassInterfaceType.AutoDual)]
    [GuidAttribute("E504A7DE-0D61-4b1f-A542-4FCC7BFB95FF")]
    public sealed class StgDocument : MarshalByRefObject, IStgDocument
    {
        private Dictionary<string, int> m_NamespaceDictionary = new Dictionary<string, int>(127);
        private List<string> m_NamespaceList = new List<string>(127);
        private StgNode m_Body;
        private StgNode m_Header;

        internal Dictionary<string, int> NamespaceDictionary
        {
            get
            {
                return m_NamespaceDictionary;
            }
        }

        internal List<string> NamespaceList
        {
            get
            {
                return m_NamespaceList;
            }
        }

        /// <summary>
        /// Создаёт новый экземпляр <c>Stg</c> документа.
        /// </summary>
        public StgDocument()
        {
            this.m_Body = new StgNode(this);
            this.m_Header = new StgNode(this);
            Clear();
            HeaderName = "Header";
            BodyName = "Body";
        }

        /// <summary>
        /// Корневой элемент документа
        /// </summary>
        public StgNode Body
        {
            get
            {
                return m_Body;
            }
        }

        /// <summary>
        /// Заголовочный элемент документа
        /// </summary>
        public StgNode Header
        {
            get
            {
                return m_Header;
            }
        }

        private void Clear()
        {
            m_Body.Items.Clear();
            m_Header.Items.Clear();
            m_NamespaceDictionary.Clear();
            m_NamespaceList.Clear();
        }

        public string HeaderName { get; set; }

        public string BodyName { get; set; }

        #region Xml Serialization

        /// <summary>
        /// Сохранение файла в виде текстового <c>Xml</c>
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public void SaveToFileAsXml(string path)
        {
            StgDocumentXmlFormater writer = new StgDocumentXmlFormater();
            writer.SaveToFile(this, path);
        }

        /// <summary>
        /// Сохранение в поток в виде текстового <c>Xml</c>
        /// </summary>
        /// <param name="stream">Поток для записи</param>
        public void SaveToStreamAsXml(Stream stream)
        {
            StgDocumentXmlFormater writer = new StgDocumentXmlFormater();
            writer.SaveToStream(this, stream);
        }

        /// <summary>
        /// Чтение данных <c>StgDocument</c> из файла Xml
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <exception cref="System.IO.FileNotFoundException">В случае если файл не найден</exception>
        public void LoadFromFileAsXml(string path)
        {
            Clear();
            StgDocumentXmlFormater reader = new StgDocumentXmlFormater();
            reader.LoadFromFile(this, path, false);
        }

        /// <summary>
        /// Метод загружает данные из <c>Xml</c> потока <paramref name="stream"/>
        /// </summary>
        /// <param name="stream">Поток, из которого необходимо прочитать данные</param>
        public void LoadFromStreamAsXml(Stream stream)
        {
            Clear();
            StgDocumentXmlFormater reader = new StgDocumentXmlFormater();
            reader.LoadFromStream(this, stream, false);
        }

        /// <summary>
        /// Чтение данных <c>StgDocument</c> из файла Xml
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <exception cref="System.IO.FileNotFoundException">В случае если файл не найден</exception>
        /// <param name="headerOnly">В случае если headerOnly равет <c>True</c>, то загружается только заголовок документа,
        public void LoadFromFileAsXml(string path, bool headerOnly)
        {
            Clear();
            StgDocumentXmlFormater reader = new StgDocumentXmlFormater();
            reader.LoadFromFile(this, path, headerOnly);
        }

        /// <summary>
        /// Метод загружает данные из <c>Xml</c> потока <paramref name="stream"/>
        /// </summary>
        /// <param name="stream">Поток, из которого необходимо прочитать данные</param>
        /// <param name="headerOnly">В случае если headerOnly равет <c>True</c>, то загружается только заголовок документа,
        public void LoadFromStreamAsXml(Stream stream, bool headerOnly)
        {
            Clear();
            StgDocumentXmlFormater reader = new StgDocumentXmlFormater();
            reader.LoadFromStream(this, stream, headerOnly);
        }

        #endregion

        #region Binary Serialization

        /// <summary>
        /// Сохранение в компактном бинарном виде с возможностью сжатия и шифрования
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="compress"><c>True</c> если необходимо сжимать поток, в противном случае <c>False</c></param>
        /// <param name="password">Массив символов, который представляет пароль для шифрования. Возможно значение <c>Null</c> если не надо шифровать файл</param>
        public void SaveToFileAsBinary(string path, bool compress, char[] password)
        {
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                SaveToStreamAsBinary(stream, compress, password);
            }
        }
        
        /// <summary>
        /// Сохранение в компактном бинарном виде
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public void SaveToFileAsBinary(string path)
        {
            SaveToFileAsBinary(path, false, null);
        }

        /// <summary>
        /// Сохранение в поток в компактном бинарном виде с возможностью сжатия и шифрования
        /// </summary>
        /// <param name="stream">Поток для записи</param>
        /// <param name="compress"><c>True</c> если необходимо сжимать поток, в противном случае <c>False</c></param>
        /// <param name="password">Массив символов, который представляет пароль для шифрования. Возможно значение <c>Null</c> если не надо шифровать поток</param>
        public void SaveToStreamAsBinary(Stream stream, bool compress, char[] password)
        {
            StgDocumentBinaryFormater writer = new StgDocumentBinaryFormater();
            writer.SaveToStream(this, stream, compress, StgDocumentBinaryFormater.AlgName_Default, StgDocumentBinaryFormater.AlgHashName_Default, password);
        }

        /// <summary>
        /// Сохранение в поток в компактном бинарном виде
        /// </summary>
        /// <param name="stream">Поток для записи</param>
        public void SaveToStreamAsBinary(Stream stream)
        {
            SaveToStreamAsBinary(stream, false, null);
        }

        /// <summary>
        /// Чтение из бинарного формата
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="headerOnly">В случае если headerOnly равет <c>True</c>, то загружается только заголовок документа,
        /// в противном случае загружается весь документ</param>
        public void LoadFromFileAsBinary(string path, bool headerOnly)
        {
            LoadFromFileAsBinary(path, headerOnly, null);
        }

        /// <summary>
        /// Чтение из бинарного формата
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public void LoadFromFileAsBinary(string path)
        {
            LoadFromFileAsBinary(path, false);
        }

        /// <summary>
        /// Чтение из бинарного формата
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="headerOnly">В случае если headerOnly равет <c>True</c>, то загружается только заголовок документа,
        /// в противном случае загружается весь документ</param>
        /// <param name="password">Пароль</param>
        public void LoadFromFileAsBinary(string path, bool headerOnly, char[] password)
        {
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                LoadFromStreamAsBinary(stream, headerOnly, password);
            }
        }

        /// <summary>
        /// Загрузка из бинарного потока
        /// </summary>
        /// <param name="stream">Поток для чтения</param>
        /// <param name="headerOnly">В случае если headerOnly равет <c>True</c>, то загружается только заголовок документа,
        /// в противном случае загружается весь документ</param>
        /// <param name="password">Пароль</param>
        public void LoadFromStreamAsBinary(Stream stream, bool headerOnly, char[] password)
        {
            Clear();
            StgDocumentBinaryFormater reader = new StgDocumentBinaryFormater();
            reader.LoadFromStream(stream, this, headerOnly, password);
        }

        /// <summary>
        /// Загрузка из бинарного потока
        /// </summary>
        /// <param name="stream">Поток для чтения</param>
        /// <param name="headerOnly">В случае если headerOnly равет <c>True</c>, то загружается только заголовок документа,
        /// в противном случае загружается весь документ</param>
        public void LoadFromStreamAsBinary(Stream stream, bool headerOnly)
        {
            LoadFromStreamAsBinary(stream, headerOnly, null);
        }

        /// <summary>
        /// Загрузка из бинарного потока
        /// </summary>
        /// <param name="stream">Поток для чтения</param>
        public void LoadFromStreamAsBinary(Stream stream)
        {
            LoadFromStreamAsBinary(stream, false);
        }

        #endregion

        #region Clipboard Serialization

        /// <summary>
        /// Сохранение документа в буфере обмена
        /// </summary>
        /// <param name="alias">Уникальный псевдоним объекта, в основном GUID</param>
        public void SaveToClipboard(string alias)
        {
            if ((alias == null) || (alias == string.Empty))
            {
                throw new ArgumentOutOfRangeException("alias");
            }
            Clipboard.Clear();
            MemoryStream stream = new MemoryStream();
            ClipboardHeader header = new ClipboardHeader();
            header.Alias = alias;
            header.SaveToStream(stream);
            this.SaveToStreamAsBinary(stream);
            stream.Position = 0;
            Clipboard.SetAudio(stream);
            stream.Dispose();
        }

        /// <summary>
        /// Загрузка документа из буфера обмена
        /// </summary>
        /// <param name="alias">Уникальный псевдоним объекта, в основном GUID</param>
        /// <param name="headerOnly">В случае если headerOnly равет <c>True</c>, то загружается только заголовок документа,
        /// в противном случае загружается весь документ</param>
        /// <returns></returns>
        public bool LoadFromClipboard(string alias, bool headerOnly)
        {
            Body.Items.Clear();
            Body.Attribute.Items.Clear();
            try
            {
                if (Clipboard.ContainsAudio())
                {
                    Stream stream = Clipboard.GetAudioStream();
                    ClipboardHeader header = new ClipboardHeader();
                    if (header.LoadFromStream(stream))
                    {
                        if (alias.Equals(header.Alias, StringComparison.OrdinalIgnoreCase))
                        {
                            this.LoadFromStreamAsBinary(stream, headerOnly);
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message);
            }
            return false;
        }

        /// <summary>
        /// Загрузка документа из буфера обмена
        /// </summary>
        /// <param name="alias">Уникальный псевдоним объекта, в основном GUID</param>
        /// <returns></returns>
        public bool LoadFromClipboard(string alias)
        {
            return LoadFromClipboard(alias, false);
        }

        /// <summary>
        /// Метод проверяет существование в буфере обмена Stg документа с заданным псевдонимом
        /// </summary>
        /// <param name="alias">Уникальный псевдоним объекта, в основном GUID</param>
        public bool ContainsClipboardRecord(string alias)
        {
            return StgDocument.IsContainsClipboardRecord(alias);
        }

        /// <summary>
        /// Метод проверяет существование в буфере обмена Stg документа с заданным псевдонимом
        /// </summary>
        /// <param name="alias">Уникальный псевдоним объекта, в основном GUID</param>
        public static bool IsContainsClipboardRecord(string alias)
        {
            if (Clipboard.ContainsAudio())
            {
                ClipboardHeader header = new ClipboardHeader();
                if (header.LoadFromStream(Clipboard.GetAudioStream()))
                {
                    if (alias.Equals(header.Alias, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        private sealed class ClipboardHeader
        {
            private const string Signature = "StgDOMCBfmt";
            private const ushort Version = 1;

            private string m_Alias = string.Empty;
            
            public string Alias
            {
                get
                {
                    return m_Alias;
                }
                set
                {
                    m_Alias = value;
                }
            }

            public void SaveToStream(Stream stream)
            {
                BinaryWriter writer = new BinaryWriter(stream);
                for (int i = 0; i < Signature.Length; i++)
                {
                    writer.Write((byte)Signature[i]);
                }
                writer.Write(Version);
                byte[] buffer = Encoding.UTF8.GetBytes(Alias);
                writer.Write(buffer.Length);
                writer.BaseStream.Write(buffer, 0, buffer.Length);
                writer.Flush();
            }

            public bool LoadFromStream(Stream stream)
            {
                if (stream.Length - stream.Position < Signature.Length)
                {
                    return false;
                }
                else
                {
                    BinaryReader reader = new BinaryReader(stream);
                    byte[] signature = reader.ReadBytes(Signature.Length);
                    for (int i = 0; i < signature.Length; i++)
                    {
                        if (signature[i] != Signature[i])
                        {
                            return false;
                        }
                    }
                    ushort version = reader.ReadUInt16();
                    if (version > Version)
                    {
                        return false;
                    }
                    int count = reader.ReadInt32();
                    byte[] buffer = new byte[count];
                    reader.BaseStream.Read(buffer, 0, count);
                    Alias = Encoding.UTF8.GetString(buffer);
                    return true;
                }
            }
        }
    }
}

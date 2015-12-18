using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib;
using System.Diagnostics;

namespace Stg
{
    internal sealed class StgDocumentBinaryFormater
    {
        [Flags]
        enum SecureParams : byte
        {
            spNone = 0,
            spCompress = 1,
            spEncrypt = 2
        }

        internal const string AlgHashName_TripleDES = "TripleDES";
        internal const string AlgName_RC2 = "RC2";
        internal const string AlgName_DES = "DES";

        internal const string AlgName_Default = AlgHashName_TripleDES;

        //internal const string AlgHashName_ECDiffieHellmanP256 = "ECDH_P256";
        //internal const string AlgHashName_ECDiffieHellmanP384 = "ECDH_P384";
        //internal const string AlgHashName_ECDiffieHellmanP521 = "ECDH_P521";
        //internal const string AlgHashName_ECDsaP256 = "ECDSA_P256";
        //internal const string AlgHashName_ECDsaP384 = "ECDSA_P384";
        //internal const string AlgHashName_ECDsaP521 = "ECDsaP521";
        internal const string AlgHashName_MD5 = "MD5";
        internal const string AlgHashName_Sha1 = "SHA1";
        //internal const string AlgHashName_Sha256 = "SHA256";
        //internal const string AlgHashName_Sha384 = "SHA384";
        //internal const string AlgHashName_Sha512 = "SHA512";
        //internal const string AlgHashName_RIPEMD160 = "RIPEMD160";


        internal const string AlgHashName_Default = AlgHashName_MD5;
        
        public const string SIGNATURE = "BSTG";
        public const ushort VERSION = 2;
        private const ushort SALT_LENGHT = 16;
        private const int RANDOM_SALT_LENGTH = 256;

        private Encoding m_Encoding = Encoding.UTF8;
        private ushort m_Version = VERSION;

        public StgDocumentBinaryFormater()
        {
            //this.m_Buffer = new byte[0x10];
        }

        private static void WriteString(StgBinaryWriter writer, string name)
        {
            byte[] buffer = writer.Encoding.GetBytes(name);
            WriteInt(writer, buffer.Length);
            writer.BaseStream.Write(buffer, 0, buffer.Length);
        }

        private static string ReadString(StgBinaryReader reader)
        {
            int count = ReadInt(reader);
            byte[] buffer = new byte[count];
            reader.BaseStream.Read(buffer, 0, count);
            return reader.Encoding.GetString(buffer);
        }

        private static void WriteDouble(StgBinaryWriter writer, double value)
        {
            writer.Write(value);
        }

        private static double ReadDouble(StgBinaryReader reader)
        {
            return reader.ReadDouble();
        }

        private static void WriteInt(StgBinaryWriter writer, Int32 value)
        {
            uint num = (uint)value;
            while (num >= 0x80)
            {
                writer.Write((byte)(num | 0x80));
                num = num >> 7;
            }
            writer.Write((byte)num);
        }

        private static Int32 ReadInt(StgBinaryReader reader)
        {
            byte code;
            int result = 0;
            int offset = 0;
            do
            {
                if (offset == 35)
                {
                    throw new FormatException("Error Format_Bad7BitInt32");
                }
                code = reader.ReadByte();
                result |= (code & 127) << offset;
                offset += 7;
            }
            while ((code & 0x80) != 0);
            return result;
        }

        public static void SaveNode(StgBinaryWriter writer, StgNode stgElementNode)
        {
            if (!stgElementNode.IsAttributeExists)
            {
                WriteInt(writer, 0);
            }
            else
            {
                WriteInt(writer, stgElementNode.Attribute.NotOptionalCount);
                // Сохранение аттрибутов элемента
                foreach (KeyValuePair<int, IStgElement> pair in stgElementNode.Attribute.Items)
                {
                    if (!pair.Value.Optional)
                    {
                        writer.Write((byte)pair.Value.ElementType);
                        WriteInt(writer, pair.Key);
                        SaveSimpleValue(writer, pair.Value);
                    }
                }
            }
            // Сохранение данных
            var count = stgElementNode.NotOptionalCount;
            WriteInt(writer, count);
            if (count > 0)
            {
                foreach (KeyValuePair<int, IStgElement> pair in stgElementNode.Items)
                {
                    if (!pair.Value.Optional)
                    {
                        writer.Write((byte)pair.Value.ElementType);
                        WriteInt(writer, pair.Key);
                        switch (pair.Value.ElementType)
                        {
                            case StgType.Array:
                                {
                                    SaveArray(writer, (IStgArray)pair.Value);
                                    break;
                                }
                            case StgType.Node:
                                {
                                    SaveNode(writer, (StgNode)pair.Value);
                                    break;
                                }
                            default:
                                {
                                    SaveSimpleValue(writer, pair.Value);
                                    break;
                                }
                        }
                    }
                }
            }
        }

        private static void SaveArray(StgBinaryWriter writer, IStgArray value)
        {
            writer.Write((byte)value.ArrayDataType);
            WriteInt(writer, value.Count);
            switch (value.ArrayDataType)
            {
                case StgType.Array:
                    {
                        for (int i = 0; i < value.Count; i++)
                        {
                            SaveArray(writer, (IStgArray)value[i]);
                        }
                        break;
                    }
                case StgType.Node:
                    {
                        StgNodeArray nodearray = (StgNodeArray)value;
                        if (writer.Version > 1)
                        {
                            using (var ms = new MemoryStream())
                            {
                                using (var temporary = new StgBinaryWriter(ms, writer.Encoding, writer.Version))
                                {
                                    for (int i = 0; i < nodearray.Count; i++)
                                    {
                                        if (nodearray.IsPacked(i))
                                        {
                                            var buffer = nodearray.GetPacked(i);
                                            WriteInt(writer, buffer.Length);
                                            writer.Write(buffer);
                                        }
                                        else
                                        {
                                            ms.Position = 0;
                                            ms.SetLength(0);
                                            SaveNode(temporary, (StgNode)nodearray[i]);
                                            WriteInt(writer, (int)ms.Length);
                                            writer.BaseStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < nodearray.Count; i++)
                            {
                                if (nodearray.IsPacked(i))
                                {
                                    writer.Write(nodearray.GetPacked(i));
                                }
                                else
                                {
                                    SaveNode(writer, (StgNode)nodearray[i]);
                                }
                            }
                        }
                        break;
                    }
                case StgType.Boolean:
                    {
                        for (int i = 0; i < value.Count; i++)
                        {
                            writer.Write((Boolean)value[i]);
                        }
                        break;
                    }
                case StgType.Byte:
                    {
                        byte[] buffer = ((StgArray<byte>)value).Items.ToArray();
                        writer.Write(buffer);
                        break;
                    }
                case StgType.Char:
                    {
                        for (int i = 0; i < value.Count; i++)
                        {
                            writer.Write((Char)value[i]);
                        }
                        break;
                    }
                case StgType.Int16:
                    {
                        for (int i = 0; i < value.Count; i++)
                        {
                            WriteInt(writer, (Int16)value[i]);
                        }
                        break;
                    }
                case StgType.Int32:
                    {
                        for (int i = 0; i < value.Count; i++)
                        {
                            WriteInt(writer, (Int32)value[i]);
                        }
                        break;
                    }
                case StgType.Int64:
                    {
                        for (int i = 0; i < value.Count; i++)
                        {
                            writer.Write((Int64)value[i]);
                        }
                        break;
                    }
                case StgType.Single:
                    {
                        for (int i = 0; i < value.Count; i++)
                        {
                            writer.Write((Single)value[i]);
                        }
                        break;
                    }
                case StgType.Double:
                    {
                        for (int i = 0; i < value.Count; i++)
                        {
                            WriteDouble(writer, (Double)value[i]);
                        }
                        break;
                    }
                case StgType.String:
                    {
                        for (int i = 0; i < value.Count; i++)
                        {
                            WriteString(writer, (String)value[i]);
                        }
                        break;
                    }
            }
        }

        private static void SaveSimpleValue(StgBinaryWriter writer, IStgElement value)
        {
            switch (value.ElementType)
            {
                case StgType.Boolean:
                    {
                        writer.Write((value as StgElement<bool>).Target);
                        break;
                    }
                case StgType.Byte:
                    {
                        writer.Write((value as StgElement<byte>).Target);
                        break;
                    }
                case StgType.Char:
                    {
                        writer.Write((value as StgElement<char>).Target);
                        break;
                    }
                case StgType.Int16:
                    {
                        WriteInt(writer, (value as StgElement<short>).Target);
                        break;
                    }
                case StgType.Int32:
                    {
                        WriteInt(writer, (value as StgElement<int>).Target);
                        break;
                    }
                case StgType.Int64:
                    {
                        writer.Write((value as StgElement<long>).Target);
                        break;
                    }
                case StgType.Single:
                    {
                        writer.Write((value as StgElement<float>).Target);
                        break;
                    }
                case StgType.Double:
                    {
                        WriteDouble(writer, (value as StgElement<double>).Target);
                        break;
                    }
                case StgType.String:
                    {
                        WriteString(writer, (value as StgElement<string>).Target);
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("Unknown StgType");
                    }
            }
        }

        public static void LoadNode(StgBinaryReader reader, StgNode node)
        {
            int attribyteCount = ReadInt(reader);
            // Загрузка аттрибутов элемента
            for (int i = 0; i < attribyteCount; i++)
            {
                StgType type = (StgType)reader.ReadByte();
                var nameIndex = ReadInt(reader);
                #region Загрузка значения аттрибута
                switch (type)
                {
                    case StgType.Boolean:
                        {
                            node.Attribute.AddBoolean(nameIndex, reader.ReadBoolean());
                            break;
                        }
                    case StgType.Byte:
                        {
                            node.Attribute.AddByte(nameIndex, reader.ReadByte());
                            break;
                        }
                    case StgType.Char:
                        {
                            node.Attribute.AddChar(nameIndex, reader.ReadChar());
                            break;
                        }
                    case StgType.Int16:
                        {
                            node.Attribute.AddInt16(nameIndex, (Int16)ReadInt(reader));
                            break;
                        }
                    case StgType.Int32:
                        {
                            node.Attribute.AddInt32(nameIndex, ReadInt(reader));
                            break;
                        }
                    case StgType.Int64:
                        {
                            node.Attribute.AddInt64(nameIndex, reader.ReadInt64());
                            break;
                        }
                    case StgType.Single:
                        {
                            node.Attribute.AddSingle(nameIndex, reader.ReadSingle());
                            break;
                        }
                    case StgType.Double:
                        {
                            node.Attribute.AddDouble(nameIndex, ReadDouble(reader));
                            break;
                        }
                    case StgType.String:
                        {
                            node.Attribute.AddString(nameIndex, ReadString(reader));
                            break;
                        }
                    default:
                        {
                            throw new FormatException();
                        }
                }

                #endregion
            }
            // Загрузка данных
            int itemCount = ReadInt(reader);
            for (int i = 0; i < itemCount; i++)
            {
                StgType type = (StgType)reader.ReadByte();
                var nameIndex = ReadInt(reader);
                #region Загрузка значения

                switch (type)
                {
                    case StgType.Array:
                        {
                            StgType dataType = (StgType)reader.ReadByte();
                            LoadArray(reader, node.AddArray(nameIndex, dataType));
                            break;
                        }
                    case StgType.Node:
                        {
                            LoadNode(reader, node.AddNode(nameIndex));
                            break;
                        }
                    case StgType.Boolean:
                        {
                            node.AddBoolean(nameIndex, reader.ReadBoolean());
                            break;
                        }
                    case StgType.Byte:
                        {
                            node.AddByte(nameIndex, reader.ReadByte());
                            break;
                        }
                    case StgType.Char:
                        {
                            node.AddChar(nameIndex, reader.ReadChar());
                            break;
                        }
                    case StgType.Int16:
                        {
                            node.AddInt16(nameIndex, (Int16)ReadInt(reader));
                            break;
                        }
                    case StgType.Int32:
                        {
                            node.AddInt32(nameIndex, ReadInt(reader));
                            break;
                        }
                    case StgType.Int64:
                        {
                            node.AddInt64(nameIndex, reader.ReadInt64());
                            break;
                        }
                    case StgType.Single:
                        {
                            node.AddSingle(nameIndex, reader.ReadSingle());
                            break;
                        }
                    case StgType.Double:
                        {
                            node.AddDouble(nameIndex, ReadDouble(reader));
                            break;
                        }
                    case StgType.String:
                        {
                            node.AddString(nameIndex, ReadString(reader));
                            break;
                        }
                    default:
                        {
                            throw new FormatException();
                        }
                }

                #endregion
            }
        }

        private static void LoadArray(StgBinaryReader reader, IStgArray array)
        {
            int count = ReadInt(reader);
            array.EnsureCapacity(count);
            var dataType = array.ArrayDataType;
            for (int i = 0; i < count; i++)
            {
                switch (dataType)
                {
                    case StgType.Array:
                        {
                            StgType type = (StgType)reader.ReadByte();
                            LoadArray(reader, array.AddArray(type));
                            break;
                        }
                    case StgType.Node:
                        {
                            if ((reader.Version > 1) && (reader.Encoding.CodePage == 65001))
                            {
                                var size = ReadInt(reader);
                                byte[] buffer = new byte[size];
                                reader.Read(buffer, 0, buffer.Length);
                                ((StgNodeArray)array).AddPacked(buffer);
                            }
                            else
                            {
                                var node = array.AddNode();
                                LoadNode(reader, node);
                                array.Pack(i);
                            }
                            break;
                        }
                    case StgType.Byte:
                        {
                            var buffer = reader.ReadBytes(count);
                            array.AddByte(buffer);
                            return;
                        }
                    case StgType.Boolean:
                        {
                            array.AddBoolean(reader.ReadBoolean());
                            break;
                        }
                    case StgType.Char:
                        {
                            array.AddChar(reader.ReadChar());
                            break;
                        }
                    case StgType.Int16:
                        {
                            array.AddInt16((Int16)ReadInt(reader));
                            break;
                        }
                    case StgType.Int32:
                        {
                            array.AddInt32(ReadInt(reader));
                            break;
                        }
                    case StgType.Int64:
                        {
                            array.AddInt64(reader.ReadInt64());
                            break;
                        }
                    case StgType.Single:
                        {
                            array.AddSingle(reader.ReadSingle());
                            break;
                        }
                    case StgType.Double:
                        {
                            array.AddDouble(ReadDouble(reader));
                            break;
                        }
                    case StgType.String:
                        {
                            array.AddString(ReadString(reader));
                            break;
                        }
                    default:
                        {
                            throw new InvalidOperationException("Unknown stg type");
                        }
                }
            }
        }

        private void SaveHeader(StgDocument doc, Stream stream)
        {
            StgBinaryWriter writer = new StgBinaryWriter(stream, m_Encoding);
            WriteInt(writer, doc.NamespaceList.Count);
            for (int i = 0; i < doc.NamespaceList.Count; i++)
            {
                WriteString(writer, doc.NamespaceList[i]);
            }
            writer.Flush();
            using (MemoryStream ms = new MemoryStream())
            {
                writer = new StgBinaryWriter(ms, m_Encoding);
                SaveNode(writer, doc.Header);
                writer.Flush();
                writer = new StgBinaryWriter(stream, m_Encoding);
                WriteInt(writer, (int)ms.Length);
                writer.Flush();
                ms.WriteTo(stream);
            }
        }

        private void SaveData(StgDocument doc, Stream stream)
        {
            StgBinaryWriter writer = new StgBinaryWriter(stream, m_Encoding);
            SaveNode(writer, doc.Body);
            writer.Flush();
        }

        private void LoadHeader(StgDocument doc, Stream stream)
        {
            StgBinaryReader reader = new StgBinaryReader(stream, m_Encoding, m_Version);
            int nameSpaceCount = ReadInt(reader);
            doc.NamespaceList.Capacity = nameSpaceCount;
            for (int i = 0; i < nameSpaceCount; i++)
            {
                string s = ReadString(reader);
                doc.NamespaceDictionary[s] = doc.NamespaceList.Count;
                doc.NamespaceList.Add(s);
            }
            var size = ReadInt(reader);
            LoadNode(reader, doc.Header);
        }

        private void LoadData(StgDocument doc, Stream stream)
        {
            StgBinaryReader reader = new StgBinaryReader(stream, m_Encoding, m_Version);
            LoadNode(reader, doc.Body);
        }

        internal void SaveToStream(StgDocument doc, Stream stream, bool compress, string algName, string alghashname, char[] password)
        {
            #region Сохранение HEADER'а
            SecureParams sp = SecureParams.spNone;
            StgBinaryWriter writer = new StgBinaryWriter(stream, m_Encoding);
            try
            {
                writer.Write(SIGNATURE.ToCharArray()); // Сигнатура файла
                writer.Write(VERSION); // Версия файла
                writer.Write((int)m_Encoding.CodePage); // Кодовая страница
                if (compress)
                {
                    sp = sp | SecureParams.spCompress;
                }
                if ((password != null) && (password.Length > 0))
                {
                    sp = sp | SecureParams.spEncrypt;
                    if (algName == null)
                    {
                        throw new ArgumentNullException("algName");
                    }
                    if (alghashname == null)
                    {
                        throw new ArgumentNullException("alghashname");
                    }
                }
                writer.Write((byte)sp); // Параметры безопастности
                if ((sp & SecureParams.spCompress) == SecureParams.spCompress)
                {
                    WriteString(writer, "gzip");
                }
            }
            finally
            {
                writer.Flush();
            }
            SaveHeader(doc, stream);
            #endregion
            #region Реализация шифрования
            if ((sp & SecureParams.spEncrypt) == SecureParams.spEncrypt)
            {
                WriteInt(writer, 0); // ToDo Autorize info
                WriteString(writer, string.Empty); // ToDo Comments
                WriteString(writer, algName); // Имя алгоритма шифрования
                WriteString(writer, alghashname); // Имя алгоритма хеширования
                byte[] salt = CreateRandomSalt(SALT_LENGHT);
                try
                {
                    writer.Write((ushort)SALT_LENGHT); // Длина SALT
                    writer.Write(salt);
                    byte[] pwd = Encoding.Unicode.GetBytes(password);
                    try
                    {
                        PasswordDeriveBytes pdb = new PasswordDeriveBytes(pwd, salt);
                        using (SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(algName))
                        {
                            WriteInt(writer, symmetricAlgorithm.KeySize); // Длина ключа
                            var password_iteration_count = GenerateSecureInteger(1, ushort.MaxValue);
                            WriteInt(writer, password_iteration_count); // Количество итераций
                            WriteInt(writer, symmetricAlgorithm.IV.Length); // Initialization Vector
                            writer.Write(symmetricAlgorithm.IV);
                            writer.Flush();
                            pdb.IterationCount = password_iteration_count;
                            symmetricAlgorithm.Key = pdb.CryptDeriveKey(algName, alghashname, symmetricAlgorithm.KeySize, symmetricAlgorithm.IV);
                            if ((sp & SecureParams.spCompress) == SecureParams.spCompress)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    using (CryptoStream cryptoStream = new CryptoStream(ms, symmetricAlgorithm.CreateEncryptor(), CryptoStreamMode.Write))
                                    {
                                        using (GZipOutputStream zipStream = new GZipOutputStream(cryptoStream))
                                        {
                                            zipStream.IsStreamOwner = false;
                                            writer = new StgBinaryWriter(zipStream, m_Encoding);
                                            byte[] randomSalt = CreateRandomSalt(1, RANDOM_SALT_LENGTH);
                                            for (int i = 0; i < randomSalt.Length; i++)
                                            {
                                                if (randomSalt[i] == 0)
                                                {
                                                    randomSalt[i] = 1;
                                                }
                                            }
                                            writer.Write(randomSalt);
                                            writer.Write((byte)0);
                                            Crc32 crc = new Crc32();
                                            crc.Reset();
                                            crc.Update(randomSalt);
                                            crc.Update(pwd);
                                            crc.Update(salt);
                                            WriteInt(writer, (int)crc.Value); // CRC
                                            writer.Flush();
                                            ClearBytes(randomSalt);
                                            SaveData(doc, zipStream);
                                        }
                                        cryptoStream.Flush();
                                        cryptoStream.FlushFinalBlock();
                                        writer = new StgBinaryWriter(stream, m_Encoding);
                                        WriteInt(writer, (int)ms.Length);
                                        writer.Flush();
                                        ms.WriteTo(stream);
                                    }
                                }
                            }
                            else if (sp == SecureParams.spEncrypt)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    using (CryptoStream cryptoStream = new CryptoStream(ms, symmetricAlgorithm.CreateEncryptor(), CryptoStreamMode.Write))
                                    {
                                        writer = new StgBinaryWriter(cryptoStream, m_Encoding);
                                        byte[] randomSalt = CreateRandomSalt(1, RANDOM_SALT_LENGTH);
                                        for (int i = 0; i < randomSalt.Length; i++)
                                        {
                                            if (randomSalt[i] == 0)
                                            {
                                                randomSalt[i] = 1;
                                            }
                                        }
                                        writer.Write(randomSalt);
                                        writer.Write((byte)0);
                                        Crc32 crc = new Crc32();
                                        crc.Reset();
                                        crc.Update(randomSalt);
                                        crc.Update(pwd);
                                        crc.Update(salt);
                                        WriteInt(writer, (int)crc.Value); // CRC
                                        writer.Flush();
                                        ClearBytes(randomSalt);
                                        SaveData(doc, cryptoStream);
                                        cryptoStream.Flush();
                                        cryptoStream.FlushFinalBlock();
                                        writer = new StgBinaryWriter(stream, m_Encoding);
                                        WriteInt(writer, (int)ms.Length);
                                        writer.Flush();
                                        ms.WriteTo(stream);
                                    }
                                }
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }
                        }
                    }
                    finally
                    {
                        ClearBytes(pwd);
                    }
                }
                finally
                {
                    ClearBytes(salt);
                }
            }
            #endregion
            #region Реализация сжатия
            else if (sp == SecureParams.spCompress)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (GZipOutputStream zipStream = new GZipOutputStream(ms))
                    {
                        zipStream.IsStreamOwner = false;
                        SaveData(doc, zipStream);
                    }
                    writer = new StgBinaryWriter(stream, m_Encoding);
                    WriteInt(writer, (int)ms.Length);
                    writer.Flush();
                    ms.WriteTo(stream);
                }
            }
            #endregion
            #region Реализация обычного сохранения
            else if (sp == SecureParams.spNone)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    SaveData(doc, ms);
                    writer = new StgBinaryWriter(stream, m_Encoding);
                    WriteInt(writer, (int)ms.Length);
                    writer.Flush();
                    ms.WriteTo(stream);
                }
            }
            else
            {
                throw new NotSupportedException();
            }
            #endregion
            writer = new StgBinaryWriter(stream, m_Encoding);
            WriteInt(writer, 0); // digital sign
            writer.Write("EOF".ToCharArray());
        }

        internal void LoadFromStream(Stream stream, StgDocument doc, bool headerOnly, char[] password)
        {
            #region Чтение Header'а
            StgBinaryReader reader = new StgBinaryReader(stream, m_Encoding, m_Version);
            string signature = new string(reader.ReadChars(4));
            m_Version = reader.ReadUInt16();
            var codePage = reader.ReadInt32();
            if (signature != SIGNATURE)
            {
                throw new FormatException("StreamReadError Signature");
            }
            if (m_Version > VERSION)
            {
                throw new FormatException("StreamReadError Version");
            }
            m_Encoding = Encoding.GetEncoding(codePage);
            reader.Encoding = m_Encoding;
            reader.Version = m_Version;
            SecureParams sp = (SecureParams)reader.ReadByte();
            if ((sp & SecureParams.spCompress) == SecureParams.spCompress)
            {
                var zipStreamName = ReadString(reader);
                if (zipStreamName != "gzip")
                {
                    throw new FormatException("Unknown compression method");
                }
            }
            LoadHeader(doc, stream);
            #endregion
            if (!headerOnly)
            {
                if ((sp & SecureParams.spEncrypt) == SecureParams.spEncrypt)
                {
                    int count = ReadInt(reader); // ToDo Autorize info
                    reader.ReadBytes(count);
                    ReadString(reader); // ToDo Comments
                    #region Реализация шифрования
                    if ((password == null) || (password.Length == 0))
                    {
                        throw new CryptographicException("Поток зашифрован. Необходимо задать значение пароля.");
                    }
                    string algName = ReadString(reader); // Имя алгоритма шифрования
                    string alghashname = ReadString(reader); // Имя алгоритма хеширования
                    ushort saltlegth = reader.ReadUInt16(); // Длина SALT
                    byte[] salt = reader.ReadBytes(saltlegth); // SALT
                    int keySize = ReadInt(reader); // Длина ключа
                    if (keySize > 1024000)
                    {
                        throw new InvalidDataException("Key size is too logn");
                    }
                    int iterationCount = ReadInt(reader); // Количество итераций
                    if (iterationCount > 1024000)
                    {
                        throw new InvalidDataException("Iteration count is too logn");
                    }
                    byte[] pwd = Encoding.Unicode.GetBytes(password);
                    try
                    {
                        PasswordDeriveBytes pdb = new PasswordDeriveBytes(pwd, salt);
                        using (SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(algName))
                        {
                            pdb.IterationCount = iterationCount;
                            symmetricAlgorithm.KeySize = keySize;
                            int ivLength = ReadInt(reader);
                            if (ivLength > 1024000)
                            {
                                throw new InvalidDataException("Initialization Vector Length is too logn");
                            }
                            symmetricAlgorithm.IV = reader.ReadBytes(ivLength);
                            symmetricAlgorithm.Key = pdb.CryptDeriveKey(algName, alghashname, symmetricAlgorithm.KeySize, symmetricAlgorithm.IV);
                            if ((sp & SecureParams.spCompress) == SecureParams.spCompress)
                            {
                                #region Compress and Encrypt
                                reader = new StgBinaryReader(stream, m_Encoding, m_Version);
                                var size = ReadInt(reader);
                                using (MemoryStream ms = new MemoryStream(reader.ReadBytes(size), false))
                                {
                                    using (CryptoStream cryptoStream = new CryptoStream(ms, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Read))
                                    {
                                        using (GZipInputStream zipStream = new GZipInputStream(cryptoStream))
                                        {
                                            zipStream.IsStreamOwner = false;
                                            reader = new StgBinaryReader(zipStream, m_Encoding, m_Version);
                                            #region Чтение Random Salt и CRC
                                            List<byte> randomSaltList = new List<byte>(RANDOM_SALT_LENGTH);
                                            try
                                            {
                                                while (true)
                                                {
                                                    byte b = reader.ReadByte();
                                                    if (b == 0)
                                                    {
                                                        byte[] randomSalt = randomSaltList.ToArray();
                                                        int crcValue = ReadInt(reader);
                                                        Crc32 crc = new Crc32();
                                                        crc.Reset();
                                                        crc.Update(randomSalt);
                                                        crc.Update(pwd);
                                                        crc.Update(salt);
                                                        if (crcValue != (int)crc.Value)
                                                        {
                                                            throw new CryptographicException("Wrong password");
                                                        }
                                                        ClearBytes(randomSalt);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        randomSaltList.Add(b);
                                                        if (randomSaltList.Count > 1024000)
                                                        {
                                                            throw new CryptographicException("Wrong password");
                                                        }
                                                    }
                                                }
                                                randomSaltList.Clear();
                                                randomSaltList.TrimExcess();
                                            }
                                            catch (EndOfStreamException e)
                                            {
                                                throw new CryptographicException("Wrong password", e);
                                            }
                                            #endregion
                                            LoadData(doc, zipStream);
                                        }
                                    }
                                }
                                #endregion
                            }
                            else if (sp == SecureParams.spEncrypt)
                            {
                                #region Encrypt
                                reader = new StgBinaryReader(stream, m_Encoding, m_Version);
                                var size = ReadInt(reader);
                                using (MemoryStream ms = new MemoryStream(reader.ReadBytes(size), false))
                                {
                                    using (CryptoStream cryptoStream = new CryptoStream(ms, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Read))
                                    {
                                        reader = new StgBinaryReader(cryptoStream, m_Encoding, m_Version);
                                        #region Чтение Random Salt и CRC
                                        List<byte> randomSaltList = new List<byte>(RANDOM_SALT_LENGTH);
                                        try
                                        {
                                            while (true)
                                            {
                                                byte b = reader.ReadByte();
                                                if (b == 0)
                                                {
                                                    byte[] randomSalt = randomSaltList.ToArray();
                                                    int crcValue = ReadInt(reader);
                                                    Crc32 crc = new Crc32();
                                                    crc.Reset();
                                                    crc.Update(randomSalt);
                                                    crc.Update(pwd);
                                                    crc.Update(salt);
                                                    if (crcValue != (int)crc.Value)
                                                    {
                                                        throw new CryptographicException("Wrong password");
                                                    }
                                                    ClearBytes(randomSalt);
                                                    break;
                                                }
                                                else
                                                {
                                                    randomSaltList.Add(b);
                                                    if (randomSaltList.Count > 1024000)
                                                    {
                                                        throw new CryptographicException("Wrong password");
                                                    }
                                                }
                                            }
                                            randomSaltList.Clear();
                                            randomSaltList.TrimExcess();
                                        }
                                        catch (EndOfStreamException e)
                                        {
                                            throw new CryptographicException("Wrong password", e);
                                        }
                                        #endregion
                                        LoadData(doc, cryptoStream);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }
                        }
                    }
                    finally
                    {
                        ClearBytes(pwd);
                    }
                    #endregion
                }
                else if (sp == SecureParams.spCompress)
                {
                    #region Реализация сжатия
                    reader = new StgBinaryReader(stream, m_Encoding, m_Version);
                    var size = ReadInt(reader);
                    using (MemoryStream ms = new MemoryStream(reader.ReadBytes(size), false))
                    {
                        using (GZipInputStream zipStream = new GZipInputStream(ms))
                        {
                            zipStream.IsStreamOwner = false;
                            LoadData(doc, zipStream);
                        }
                    }
                    #endregion
                }
                else if (sp == SecureParams.spNone)
                {
                    reader = new StgBinaryReader(stream, m_Encoding, m_Version);
                    var size = ReadInt(reader);
                    using (MemoryStream ms = new MemoryStream(reader.ReadBytes(size), false))
                    {
                        LoadData(doc, ms);
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
                reader = new StgBinaryReader(stream, m_Encoding, m_Version);
                int ds = ReadInt(reader);
                reader.ReadBytes(ds); // digital sign
                string eof = new string(reader.ReadChars(3));
                if (eof != "EOF")
                {
                    throw new FormatException();
                }
            }
        }

        private static byte[] CreateRandomSalt(int min, int max)
        {
            byte[] m_buffer = CreateRandomSalt(4);
            uint result = (uint)(((m_buffer[0] | (m_buffer[1] << 8)) | (m_buffer[2] << 0x10)) | (m_buffer[3] << 0x18)); ;
            result = (uint)(min + (result % (max - min)));
            return CreateRandomSalt((int)result);
        }

        private static byte[] CreateRandomSalt(int length)
        {
            // Create a buffer
            byte[] randBytes;

            if (length >= 1)
            {
                randBytes = new byte[length];
            }
            else
            {
                randBytes = new byte[1];
            }

            // Create a new RNGCryptoServiceProvider.
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

            // Fill the buffer with random bytes.
            rand.GetNonZeroBytes(randBytes);

            // return the bytes.
            return randBytes;
        }

        private static int GenerateSecureInteger(int min, int max)
        {
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
            return 512;
        }

        private static void ClearBytes(byte[] Buffer)
        {
            // Check arguments.
            if (Buffer == null)
            {
                throw new ArgumentException("Buffer");
            }

            // Set each byte in the buffer to 0.
            for (int x = 0; x <= Buffer.Length - 1; x++)
            {
                Buffer[x] = 0;
            }
        }
    }
}

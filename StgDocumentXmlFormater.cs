using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Diagnostics;
using Stg.Properties;

namespace Stg
{
    internal class StgDocumentXmlFormater
    {
        private static readonly string EmptyStation = "  ";
        //private static readonly int MaxRowLength = 256;
        //private static readonly int MinRowLenght = 51;
        private const string sType = "aType";
        
        private StreamWriter m_Writer;
        private byte[] _XmlCharType = null;

        private void WriteEncodeString(string s)
        {
            char ch;
            if (_XmlCharType == null)
            {
                _XmlCharType = Resources.XmlCharType;
            }
            for (int i = 0; i < s.Length; i++)
            {
                if ((_XmlCharType[ch = s[i]] & 0x80) != 0)
                {
                    m_Writer.Write(ch);
                }
                else
                {
                    switch (ch)
                    {
                        case '\t':
                            {
                                m_Writer.Write(ch);
                                break;
                            }
                        case '\n':
                        case '\r':
                            {
                                m_Writer.Write("&#x");
                                m_Writer.Write(((int)ch).ToString("X", NumberFormatInfo.InvariantInfo));
                                m_Writer.Write(';');
                                break;
                            }
                        case '"':
                            {
                                m_Writer.Write("&quot;");
                                break;
                            }
                        case '&':
                            {
                                m_Writer.Write("&amp;");
                                break;
                            }
                        case '\'':
                            {
                                m_Writer.Write("&apos;");
                                break;
                            }
                        case '<':
                            {
                                m_Writer.Write("&lt;");
                                break;
                            }

                        case '>':
                            {
                                m_Writer.Write("&gt;");
                                break;
                            }
                        default:
                            if ((ch >= 0xd800) && (ch <= 0xdbff))
                            {
                                if (i >= s.Length - 1)
                                {
                                    throw new XmlException("Invalid High Surrogate Char " + ch);
                                }
                                i++;
                                char lowChar = s[i];
                                char highChar = ch;
                                if (((lowChar < 0xdc00) || (lowChar > 0xdfff)) || ((highChar < 0xd800) || (highChar > 0xdbff)))
                                {
                                    throw new XmlException("Invalid High Surrogate Char " + lowChar + " " + highChar);
                                }
                                m_Writer.Write(highChar);
                                m_Writer.Write(lowChar);
                            }
                            else
                            {
                                if ((ch >= 0xdc00) && (ch <= 0xdfff))
                                {
                                    throw new XmlException("Invalid High Surrogate Char " + ch);
                                }
                                m_Writer.Write("&#x");
                                m_Writer.Write(((int)ch).ToString("X", NumberFormatInfo.InvariantInfo));
                                m_Writer.Write(';');
                            }
                            break;
                    }
                }
            }
        }
        
        private void SaveStation(int station)
        {
            for (int i = 0; i < station; i++)
            {
                m_Writer.Write(EmptyStation);
            }
        }
        
        private void SaveArray(int station, string name, IStgArray stgElementArray)
        {
            switch (stgElementArray.ArrayDataType)
            {
                case StgType.Array:
                    {
                        SaveStation(station);
                        m_Writer.WriteLine("<{0} {1}=\"{2}\">", name, sType, (byte)stgElementArray.ArrayDataType);
                        for (int i = 0; i < stgElementArray.Count; i++)
                        {
                            SaveArray(station + 1, stgElementArray.ItemsName, (IStgArray)stgElementArray[i]);
                        }
                        SaveStation(station);
                        m_Writer.WriteLine("</{0}>", name);
                        break;
                    }
                case StgType.Byte:
                    {
                        byte[] data = new byte[stgElementArray.Count];
                        for (int i = 0; i < stgElementArray.Count; i++)
                        {
                            data[i] = stgElementArray.GetByte(i);
                        }
                        string s = Convert.ToBase64String(data, Base64FormattingOptions.InsertLineBreaks);
                        SaveStation(station);
                        if (s.Length < 76)
                        {
                            m_Writer.Write("<{0} {1}=\"{2}\">", name, sType, (byte)stgElementArray.ArrayDataType);
                            m_Writer.Write(s);
                            m_Writer.WriteLine("</{0}>", name);
                        }
                        else
                        {
                            m_Writer.WriteLine("<{0} {1}=\"{2}\">", name, sType, (byte)stgElementArray.ArrayDataType);
                            m_Writer.WriteLine(s);
                            SaveStation(station);
                            m_Writer.WriteLine("</{0}>", name);
                        }
                        break;                        
                    }
                case StgType.Char:
                    {
                        SaveStation(station);
                        m_Writer.Write("<{0} {1}=\"{2}\">", name, sType, (byte)stgElementArray.ArrayDataType);
                        for (int i = 0; i < stgElementArray.Count; i++)
                        {
                            int b = (int)(stgElementArray.GetChar(i));
                            string s = b.ToString("X4");
                            m_Writer.Write(s);
                        }
                        m_Writer.WriteLine("</{0}>", name);
                        break; 
                    }
                case StgType.String:
                    {
                        SaveStation(station);
                        m_Writer.WriteLine("<{0} {1}=\"{2}\">", name, sType, (byte)stgElementArray.ArrayDataType);
                        for (int i = 0; i < stgElementArray.Count; i++)
                        {
                            SaveStation(station + 1);
                            m_Writer.Write("<{0}>", stgElementArray.ItemsName);
                            WriteEncodeString(stgElementArray.GetString(i));
                            m_Writer.WriteLine("</{0}>", stgElementArray.ItemsName);
                        }
                        SaveStation(station);
                        m_Writer.WriteLine("</{0}>", name);
                        break;
                    }
                case StgType.Node:
                    {
                        SaveStation(station);
                        m_Writer.WriteLine("<{0} {1}=\"{2}\">", name, sType, (byte)stgElementArray.ArrayDataType);
                        for (int i = 0; i < stgElementArray.Count; i++)
                        {
                            SaveNode(station + 1, stgElementArray.ItemsName, (StgNode)stgElementArray[i]);
                        }
                        SaveStation(station);
                        m_Writer.WriteLine("</{0}>", name);
                        break;
                    }
                default:
                    {
                        SaveStation(station);
                        m_Writer.Write("<{0} {1}=\"{2}\">", name, sType, (byte)stgElementArray.ArrayDataType);
                        for (int i = 0; i < stgElementArray.Count; i++)
                        {
                            string sVal = stgElementArray[i].ToString();
                            if (i == stgElementArray.Count - 1)
                            {
                                m_Writer.Write(sVal);
                            }
                            else
                            {
                                m_Writer.Write(sVal);
                                m_Writer.Write(';');
                            }
                        }
                        m_Writer.WriteLine("</{0}>", name);
                        break;
                    }
            }
        }

        private void SaveSimpleValue(int station, string name, IStgElement stgElement)
        {
            SaveStation(station);
            if (stgElement.ElementType == StgType.String)
            {
                m_Writer.Write("<{0}>", name);
                WriteEncodeString((stgElement as StgElement<string>).Target);
                m_Writer.WriteLine("</{0}>", name);
            }
            else
            {
                m_Writer.WriteLine(string.Format("<{0}>{1}</{0}>", name, stgElement.ToString()));
            }
        }

        private void SaveNode(int station, string name, StgNode stgElementArray)
        {
            SaveStation(station);
            #region Сохранение аттрибутов элемента
            if ((stgElementArray.IsAttributeExists) && (stgElementArray.Attribute.NotOptionalCount > 0))
            {
                m_Writer.Write(string.Format("<{0}", name));
                foreach (KeyValuePair<int, IStgElement> pair in stgElementArray.Attribute.Items)
                {
                    if (!pair.Value.Optional)
                    {
                        if (pair.Value.ElementType == StgType.String)
                        {
                            m_Writer.Write(" {0}=\"", stgElementArray.Doc.NamespaceList[pair.Key]);
                            WriteEncodeString((pair.Value as StgElement<string>).Target);
                            m_Writer.Write("\"");
                        }
                        else
                        {
                            m_Writer.Write(" {0}=\"{1}\"", stgElementArray.Doc.NamespaceList[pair.Key], pair.Value);
                        }
                    }
                }
                if (stgElementArray.NotOptionalCount > 0)
                {
                    m_Writer.WriteLine(">");
                }
                else
                {
                    m_Writer.WriteLine(" />");
                    return;
                }
            }
            else
            {
                m_Writer.WriteLine(string.Format("<{0}>", name));
            }
            #endregion
            #region Сохранение данных
            station = station + 1;
            if (stgElementArray.NotOptionalCount > 0)
            {
                foreach (KeyValuePair<int, IStgElement> pair in stgElementArray.Items)
                {
                    if (!pair.Value.Optional)
                    {
                        switch (pair.Value.ElementType)
                        {
                            case StgType.Array:
                                {
                                    SaveArray(station, stgElementArray.Doc.NamespaceList[pair.Key], (IStgArray)pair.Value);
                                    break;
                                }
                            case StgType.Node:
                                {
                                    SaveNode(station, stgElementArray.Doc.NamespaceList[pair.Key], (StgNode)pair.Value);
                                    break;
                                }
                            default:
                                {
                                    SaveSimpleValue(station, stgElementArray.Doc.NamespaceList[pair.Key], pair.Value);
                                    break;
                                }
                        }
                    }
                }
            }
            #endregion
            SaveStation(station - 1);
            m_Writer.WriteLine(string.Format("</{0}>", name));
        }

        public void SaveToFile(StgDocument doc, string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                SaveToStream(doc, stream);
            }
        }

        public void SaveToStream(StgDocument doc, Stream stream)
        {
            m_Writer = new StreamWriter(stream, Encoding.UTF8);
            m_Writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            m_Writer.WriteLine("<Stg>");
            if (!doc.Header.Optional)
            {
                SaveNode(1, doc.HeaderName, doc.Header);
            }
            SaveNode(1, doc.BodyName, doc.Body);
            m_Writer.WriteLine("</Stg>");
            m_Writer.Flush();
        }

        private void LoadArray(XmlNode xmlNode, IStgArray array)
        {
            switch (array.ArrayDataType)
            {
                case StgType.Byte:
                    {
                        #region Load Array of bytes
                        if (xmlNode.ChildNodes.Count == 1)
                        {
                            string value = xmlNode.ChildNodes[0].Value;
                            var data = Convert.FromBase64String(value);
                            array.AddByte(data);
                        }
                        else
                        {
                            throw new InvalidDataException(string.Format("Imposible to read binary data name = \"{0}\"", xmlNode.Name));
                        }
                        break;
                        #endregion
                    }
                case StgType.Char:
                    {
                        #region Load Array of chars
                        if (xmlNode.ChildNodes.Count == 1)
                        {
                            string value = xmlNode.ChildNodes[0].Value;
                            for (int i = 0; i < value.Length; i++)
                            {
                                char c1 = value[i];
                                if (IsValidHexChar(c1))
                                {
                                    if (i >= value.Length - 3)
                                    {
                                        throw new InvalidDataException(string.Format("Imposible to read binary data name = \"{0}\"", xmlNode.Name));
                                    }
                                    char c2 = value[i + 1];
                                    char c3 = value[i + 2];
                                    char c4 = value[i + 3];
                                    i += 3;
                                    ushort v = (ushort)(ConvertHexToInt(c1) * 4096 + ConvertHexToInt(c2) * 256 + ConvertHexToInt(c3) * 16 + ConvertHexToInt(c4));
                                    array.AddChar((char)v);
                                }
                            }
                        }
                        else
                        {
                            throw new InvalidDataException(string.Format("Imposible to read binary data name = \"{0}\"", xmlNode.Name));
                        }
                        break;
                        #endregion
                    }
                case StgType.String:
                    {
                        foreach (XmlNode node in xmlNode.ChildNodes)
                        {
                            if (node.NodeType == XmlNodeType.Element)
                            {
                                array.AddString(node.InnerText);
                            }
                        }
                        break;
                    }
                case StgType.Double:
                case StgType.Int16:
                case StgType.Int32:
                case StgType.Int64:
                case StgType.Single:
                case StgType.Boolean:
                    {
                        #region Load Other simple types
                        if (xmlNode.ChildNodes.Count == 1)
                        {
                            string value = xmlNode.ChildNodes[0].Value;
                            string[] strings = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < strings.Length; i++)
                            {
                                switch (array.ArrayDataType)
                                {
                                    case StgType.Double:
                                        {
                                            double v = StgNode.StrToFloat(strings[i]);
                                            array.AddDouble(v);
                                            break;
                                        }
                                    case StgType.Int16:
                                        {
                                            Int16 v = Int16.Parse(strings[i]);
                                            array.AddInt16(v);
                                            break;
                                        }
                                    case StgType.Int32:
                                        {
                                            int v = int.Parse(strings[i]);
                                            array.AddInt32(v);
                                            break;
                                        }
                                    case StgType.Int64:
                                        {
                                            long v = long.Parse(strings[i]);
                                            array.AddInt64(v);
                                            break;
                                        }
                                    case StgType.Single:
                                        {
                                            float v = (float)StgNode.StrToFloat(strings[i]);
                                            array.AddSingle(v);
                                            break;
                                        }
                                    case StgType.Boolean:
                                        {
                                            bool v = strings[i].Trim(' ', '\r', '\n').Equals(StgElement<bool>.sTrue, StringComparison.OrdinalIgnoreCase);
                                            array.AddBoolean(v);
                                            break;
                                        }
                                    default:
                                        {
                                            throw new InvalidDataException(string.Format("Imposible to read simple array data name = \"{0}\"", xmlNode.Name));
                                        }
                                }
                            }
                        }
                        else
                        {
                            throw new InvalidDataException(string.Format("Imposible to read simple array data name = \"{0}\"", xmlNode.Name));
                        }
                        break;
                        #endregion
                    }
                case StgType.Node:
                    {
                        #region Load Array of nodes
                        foreach (XmlNode node in xmlNode.ChildNodes)
                        {
                            if (node.NodeType != XmlNodeType.Comment)
                            {
                                LoadNode(node, array.AddNode());
                            }
                        }
                        break;
                        #endregion
                    }
                case StgType.Array:
                    {
                        #region Load Array of Array
                        foreach (XmlNode node in xmlNode.ChildNodes)
                        {
                            if ((node.Attributes.Count == 1) && (node.Attributes[0].Name == sType))
                            {
                                StgType type = (StgType)byte.Parse(node.Attributes[0].Value);
                                IStgArray childArray = array.AddArray(type);
                                LoadArray(node, childArray);
                            }
                            else
                            {
                                throw new InvalidDataException(string.Format("Invalid array attribute name = {0} type = {1}", xmlNode.Name, array.ArrayDataType));                            
                            }
                        }
                        break;
                        #endregion
                    }
                default:
                    {
                        throw new InvalidDataException(string.Format("Invalid array attribute name = {0} type = {1}", xmlNode.Name, array.ArrayDataType));
                    }
            }
        }

        private void LoadNode(XmlNode xmlNode, StgNode stgNode)
        {
            foreach (XmlAttribute attr in xmlNode.Attributes)
            {
                if (attr.NodeType != XmlNodeType.Comment)
                {
                    stgNode.Attribute.AddString(attr.Name, attr.Value);
                }
            }
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }
                if (node.NodeType == XmlNodeType.Element)
                {
                    if ((node.Attributes.Count == 0) && (node.HasChildNodes) && (node.ChildNodes.Count == 1) && (node.ChildNodes[0].NodeType == XmlNodeType.Text))
                    {
                        stgNode.AddString(node.Name, node.ChildNodes[0].Value);
                    }
                    else if ((node.Attributes.Count == 1) && (node.Attributes[0].Name == sType))
                    {
                        StgType type = (StgType)byte.Parse(node.Attributes[0].Value);
                        IStgArray array = stgNode.AddArray(node.Name, type);
                        LoadArray(node, array);
                    }
                    else if ((node.Attributes.Count == 0) && (!node.HasChildNodes))
                    {
                        stgNode.AddString(node.Name, string.Empty);
                    }
                    else
                    {
                        LoadNode(node, stgNode.AddNode(node.Name));
                    }
                }
                else
                {
                    Debug.Assert(false, string.Format("Invalid data type name = {0} type = {1}", node.Name, node.NodeType));
                }
            }
        }

        private static bool IsValidHexChar(char c)
        {
            return (((c >= '0') && (c <= '9')) || (c >= 'A') && (c <= 'F') || (c >= 'a') && (c <= 'f')); 
        }

        private static int ConvertHexToInt(char c)
        {
            switch (c)
            {
                case '0': return 0;
                case '1': return 1;
                case '2': return 2;
                case '3': return 3;
                case '4': return 4;
                case '5': return 5;
                case '6': return 6;
                case '7': return 7;
                case '8': return 8;
                case '9': return 9;
                case 'A':
                case 'a': return 10;
                case 'B':
                case 'b': return 11;
                case 'C':
                case 'c': return 12;
                case 'D':
                case 'd': return 13;
                case 'E':
                case 'e': return 14;
                case 'F':
                case 'f': return 15;
                default: throw new InvalidDataException(string.Format("Imposible to convert string to hex, value = {0}", c));
            }
        }

        private static int ConvertHexToInt(char c1, char c2)
        {
            return ConvertHexToInt(c1) * 16 + ConvertHexToInt(c2);
        }

        public void LoadFromStream(StgDocument doc, Stream stream, bool headerOnly)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                if (node.Name == doc.HeaderName)
                {
                    LoadNode(node, doc.Header);
                    if (headerOnly)
                    {
                        return;
                    }
                }
                else if (node.Name == doc.BodyName)
                {
                    if (!headerOnly)
                    {
                        LoadNode(node, doc.Body);
                    }
                }
            }
        }

        public void LoadFromFile(StgDocument doc, string filename, bool headerOnly)
        {
            using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                LoadFromStream(doc, stream, headerOnly);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Stg
{
    public class StgDocument
    {
        private string m_Path;
        private XDocument m_Document;
        public StgDocument(string path)
        {
            m_Path = path;
        }

        public StgNode Body
        {
            get
            {
                m_Document = XDocument.Load(m_Path);
                return new StgNode(m_Document.Element("body"));
            }
        }
    }
}

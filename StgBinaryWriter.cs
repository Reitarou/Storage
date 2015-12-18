using System.IO;
using System.Text;

namespace Stg
{
    class StgBinaryWriter : BinaryWriter
    {
        private Encoding m_Encoding;
        private ushort m_Version;

        public StgBinaryWriter(Stream output, Encoding encoding, ushort version)
            : base(output, encoding)
        {
            this.m_Encoding = encoding;
            this.m_Version = version;
        }

        public StgBinaryWriter(Stream output, Encoding encoding)
            : this(output, encoding, StgDocumentBinaryFormater.VERSION)
        {

        }

        public Encoding Encoding
        {
            get
            {
                return m_Encoding;
            }
        }

        public ushort Version
        {
            get
            {
                return m_Version;
            }
            set
            {
                m_Version = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Stg
{
    class StgBinaryReader : BinaryReader
    {
        private Encoding m_Encoding;
        private ushort m_Version;

        public StgBinaryReader(Stream input, Encoding encoding, ushort version)
            : base(input, encoding)
        {
            this.m_Encoding = encoding;
            this.m_Version = version;
        }

        public Encoding Encoding
        {
            get
            {
                return m_Encoding;
            }
            set
            {
                m_Encoding = value;
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

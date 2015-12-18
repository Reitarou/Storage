using System;

namespace Stg
{
    public class StgDocumentOperationEventArgs : EventArgs
    {
        private StgDocument m_Doc;
        private string m_FileName;
        
        public StgDocumentOperationEventArgs(StgDocument doc, string filename)
        {
            if (doc == null) throw new ArgumentNullException("doc", "Null passed to method");
            this.m_Doc = doc;
            this.m_FileName = filename;
        }

        public StgDocument Document
        {
            get
            {
                return m_Doc;
            }
        }

        public string FileName
        {
            get
            {
                return m_FileName;
            }
        }
    }
}

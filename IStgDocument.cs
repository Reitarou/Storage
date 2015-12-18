using System;
using System.Runtime.InteropServices;

namespace Stg
{
    [GuidAttribute("CE4EF026-77A0-406b-BB80-A3B5CBE3E060")]
    public interface IStgDocument
    {
        StgNode Body { get; }
        bool ContainsClipboardRecord(string alias);
        StgNode Header { get; }
        bool LoadFromClipboard(string alias, bool headerOnly);
        bool LoadFromClipboard(string alias);
        void LoadFromFileAsBinary(string path, bool headerOnly);
        void LoadFromFileAsBinary(string path, bool headerOnly, char[] password);
        void LoadFromFileAsBinary(string path);
        void LoadFromFileAsXml(string path);
        void LoadFromFileAsXml(string path, bool headerOnly);
        void LoadFromStreamAsBinary(System.IO.Stream stream);
        void LoadFromStreamAsBinary(System.IO.Stream stream, bool headerOnly);
        void LoadFromStreamAsBinary(System.IO.Stream stream, bool headerOnly, char[] password);
        void LoadFromStreamAsXml(System.IO.Stream stream);
        void LoadFromStreamAsXml(System.IO.Stream stream, bool headerOnly);
        void SaveToClipboard(string alias);
        void SaveToFileAsBinary(string path);
        void SaveToFileAsBinary(string path, bool compress, char[] password);
        void SaveToFileAsXml(string path);
        void SaveToStreamAsBinary(System.IO.Stream stream, bool compress, char[] password);
        void SaveToStreamAsBinary(System.IO.Stream stream);
        void SaveToStreamAsXml(System.IO.Stream stream);
    }
}

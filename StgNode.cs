using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Stg
{
    public class StgNode
    {
        private XElement m_XElement;

        public StgNode(string name)
        {
            m_XElement = new XElement(name);
        }

        public StgNode(XElement xe)
        {
            m_XElement = xe;
        }

        #region Add

        public void AddNode(StgNode node)
        {
            m_XElement.Add(node.m_XElement);
        }

        public void AddString(string name, string value)
        {
            m_XElement.Add(new XAttribute(name, value));
        }

        public void AddInt(string name, int value)
        {
            m_XElement.Add(new XAttribute(name, value));
        }

        public void AddIntElement(string name, int value)
        {
            m_XElement.Add(new XElement(name, value));
        }

        public void AddBoolean(string name, bool value)
        {
            m_XElement.Add(new XAttribute(name, value.ToString()));
        }

        #endregion

        #region Get

        public StgNode GetNode(string name)
        {
            return new StgNode(m_XElement.Element(name));
        }

        public List<StgNode> GetNodes(string name)
        {
            var xes = m_XElement.Elements(name);
            var nodes = new List<StgNode>();
            foreach (var xe in xes)
            {
                var node = new StgNode(xe);
                nodes.Add(node);
            }
            return nodes;
        }

        public string GetString(string name, string defaultValue)
        {
            var attr = m_XElement.Attribute(name);
            if (attr == null)
            {
                return defaultValue;
            }
            return attr.Value;
        }

        //public List<string> GetStrings(string name)
        //{
        //    var res = new List<string>();
        //    var attrs = m_XElement.Attributes(name);
        //    foreach (var attr in attrs)
        //    {
        //        res.Add(attr.Value);
        //    }
        //    return res;
        //}

        public int GetInt(string name, int defaultValue)
        {
            var attr = m_XElement.Attribute(name);
            if (attr == null)
            {
                return defaultValue;
            }
            var i = int.Parse(attr.Value);
            return i;
        }

        public List<int> GetInts(string name)
        {
            var res = new List<int>();
            var elems = m_XElement.Elements(name);
            foreach (var elem in elems)
            {
                res.Add(int.Parse(elem.Value));
            }
            return res;
        }

        public bool GetBoolean(string name, bool defaultValue)
        {
            var attr = m_XElement.Attribute(name);
            if (attr == null)
            {
                return defaultValue;
            }
            var b = bool.Parse(attr.Value);
            return b;
        }

        #endregion
    }
}

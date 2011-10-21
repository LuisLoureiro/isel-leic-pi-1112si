using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PI.WebGarten.HttpContent.Html
{
    class HtmlSingleElem : HtmlElem
    {
        public HtmlSingleElem(string name) 
            : base(name, new IWritable[0])
        {}

        public override void WriteTo(TextWriter w)
        {
            w.Write(string.Format("<{0}", _name));
            foreach (var entry in _attrs)
            {
                // TODO attributes are not encoded
                w.Write(string.Format(" {0}='{1}'", entry.Key, entry.Value));
            }
            w.Write("/>");
        }
    }
}

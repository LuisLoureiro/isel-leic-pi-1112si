namespace PI.WebGarten.HttpContent.Html
{
    using System.IO;

    using PI.WebGarten.Html;

    public class HtmlDoc : HtmlBase, IHttpContent
    {
        private readonly IWritable[] _c;
        private readonly string _t;

        public HtmlDoc(string t, params IWritable[] content)
        {
            _t = t;
            _c = content;
        }

        public void WriteTo(TextWriter tw)
        {
            new HtmlElem("html",
                    new HtmlElem("head", 
                        new HtmlElem("title", new HtmlText(_t)), 
                        new HtmlElem("link", new HtmlText(""))
                            .WithAttr("rel", "stylesheet")
                            .WithAttr("href", "http://twitter.github.com/bootstrap/1.3.0/bootstrap.min.css")),
                    new HtmlElem("body", _c)
                ).WriteTo(tw);
        }

        public string ContentType
        {
            get { return "text/html"; }
        }
    }
}

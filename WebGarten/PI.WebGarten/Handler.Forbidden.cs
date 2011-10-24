using PI.WebGarten.HttpContent.Html;

namespace PI.WebGarten
{
    public partial class Handler
    {
        public class Forbidden : HtmlDoc
        {
            public Forbidden() :
                base("Forbidden", new TextContent("Access Forbidden"))
            {}
        }
    }
}

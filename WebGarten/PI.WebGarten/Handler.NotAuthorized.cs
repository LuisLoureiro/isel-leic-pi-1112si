using PI.WebGarten.HttpContent.Html;

namespace PI.WebGarten
{
    partial class Handler
    {
        public class NotAuthorized : HtmlDoc
        {
            public NotAuthorized() : 
                base("Not Authorized",
                    new TextContent("Not Authorized"))
            {}
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;

namespace PI.WebGarten
{
    using PI.WebGarten.HttpContent.Html;

    public partial class Handler
    {
        public class NotFound : HtmlDoc
        {
            public NotFound()
                :base("NotFound",
                      Img("http://lava360.com/wp-content/uploads/2010/01/csstricks.jpg", "404"))
            {}
        }

    }
}
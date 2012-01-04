using System;
using System.Text;
using System.Web.Mvc;
using mvc.Models;

namespace mvc.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            var ulInnerHtml = new StringBuilder();
            var li = new TagBuilder("li");
            li.AddCssClass("prev");

            var a = new TagBuilder("a") { InnerHtml = "Anterior" };

            if (pagingInfo.CurrentPage == 1)
                li.AddCssClass("disabled");
            else
                a.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));

            li.InnerHtml = a.ToString();

            ulInnerHtml.Append(li.ToString());

            for (var i = 1; i <= pagingInfo.TotalPages; i++)
            {
                a = new TagBuilder("a"){ InnerHtml = i.ToString() };

                a.MergeAttribute("href", pageUrl(i));
                

                li = new TagBuilder("li"){ InnerHtml = a.ToString() };

                if (i == pagingInfo.CurrentPage)
                    li.AddCssClass("active");

                ulInnerHtml.Append(li.ToString());
            }

            li = new TagBuilder("li");
            li.AddCssClass("next");

            a = new TagBuilder("a"){ InnerHtml = "Próximo" };

            if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
                li.AddCssClass("disabled");
            else
                a.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));

            li.InnerHtml = a.ToString();

            ulInnerHtml.Append(li.ToString());

            var ul = new TagBuilder("ul"){ InnerHtml = ulInnerHtml.ToString() };

            var div = new TagBuilder("div"){ InnerHtml = ul.ToString() };

            div.AddCssClass("pagination");

            return MvcHtmlString.Create(div.ToString());
        }
    }
}
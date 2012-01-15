using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using mvc.Models;

namespace mvc.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, int, string> pageUrl)
        {
            var ulInnerHtml = new StringBuilder();
            var li = new TagBuilder("li");
            li.AddCssClass("prev");

            var a = new TagBuilder("a") { InnerHtml = "Anterior" };

            if (pagingInfo.CurrentPage == 1)
                li.AddCssClass("disabled");
            else
                a.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1, pagingInfo.ItemsPerPage));

            li.InnerHtml = a.ToString();

            ulInnerHtml.Append(li.ToString());

            int totalPages = pagingInfo.TotalPages;
            for (int i = 1; i <= totalPages; i++)
            {
                a = new TagBuilder("a"){ InnerHtml = i.ToString(CultureInfo.InvariantCulture) };

                a.MergeAttribute("href", pageUrl(i, pagingInfo.ItemsPerPage));
                

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
                a.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1, pagingInfo.ItemsPerPage));

            li.InnerHtml = a.ToString();

            ulInnerHtml.Append(li.ToString());

            var ul = new TagBuilder("ul"){ InnerHtml = ulInnerHtml.ToString() };

            var div = new TagBuilder("div"){ InnerHtml = ul.ToString() };

            div.AddCssClass("pagination");

            return MvcHtmlString.Create(div.ToString());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EComm.Web.CustomHtmlHelpers
{
   public static class MyHelpers
    {

        public static IHtmlString img(this HtmlHelper helper,string src,string alt)
        {
            TagBuilder tb = new TagBuilder("img");
            tb.Attributes.Add("src", src);
            tb.Attributes.Add("alt", alt);
            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));
        }
    }
}
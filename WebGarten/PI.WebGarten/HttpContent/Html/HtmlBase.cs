namespace PI.WebGarten.HttpContent.Html
{
    using System;

    using PI.WebGarten.Html;

    public class HtmlBase
    {
        public static IWritable Text(String s) { return new HtmlText(s); }
        public static IWritable H1(params IWritable[] c) { return new HtmlElem("h1", c); }
        public static IWritable H2(params IWritable[] c) { return new HtmlElem("h2", c); }
        public static IWritable H3(params IWritable[] c) { return new HtmlElem("h3", c); }
        public static IWritable Form(String method, String url, params IWritable[] c)
        {
            return new HtmlElem("form", c)
                .WithAttr("method", method)
                .WithAttr("action", url);
        }
        
        public static IWritable Label(String to, String text)
        {
            return new HtmlElem("label", new HtmlText(text))
                .WithAttr("for", to);
        }
        
        public static IWritable InputText(String name)
        {
            return new HtmlElem("input")
                .WithAttr("type", "text")
                .WithAttr("name", name);
        }

        public static IWritable InputSubmit(String value)
        {
            return new HtmlElem("input")
                .WithAttr("type", "submit")
                .WithAttr("value", value);
        }
        public static IWritable Ul(params IWritable[] c)
        {
            return new HtmlElem("ul", c);
        }
        public static IWritable Li(params IWritable[] c)
        {
            return new HtmlElem("li", c);
        }
        public static IWritable P(params IWritable[] c)
        {
            return new HtmlElem("p", c);
        }
        public static IWritable A(String href, String t)
        {
            return new HtmlElem("a", Text(t))
                .WithAttr("href", href);
        }
        public static IWritable Img(String src, String alt)
        {
            return new HtmlElem("img")
                .WithAttr("src", src)
                .WithAttr("alt", alt);
        }

        #region Métodos Adicionados

        //Adicionado
        public static IWritable InputText(String name, String value)
        {
            return new HtmlElem("input")
                .WithAttr("type", "text")
                .WithAttr("name", name)
                .WithAttr("value", value);
        }

        //Adcionado
        public static IWritable InputCheckBox(String name, String value, Boolean isChecked)
        {
            var input = new HtmlElem("input")
                .WithAttr("type", "checkbox")
                .WithAttr("name", name)
                .WithAttr("value", value);

            if(isChecked)
                input.WithAttr("checked", "checked");

            return input;
        }

        //Adcionado
        public static IWritable InputRadioButton(String name, String value, Boolean isChecked)
        {
            var input = new HtmlElem("input")
                .WithAttr("type", "radio")
                .WithAttr("name", name)
                .WithAttr("value", value);

            if (isChecked)
                input.WithAttr("checked", "checked");

            return input;
        }

        //Adicionado
        public static IWritable Br()
        {
            return new HtmlSingleElem("br");
        }

        //Adicionado
        public static IWritable Label(String text)
        {
            return new HtmlElem("label", Text(text));
        }

        //Adicionado
        public static IWritable Div(String cls, params IWritable[] c)
        {
            return new HtmlElem("div", c)
                .WithAttr("class", cls);
        }

        //Adicionado
        public static IWritable Legend(String text)
        {
            return new HtmlElem("legend", Text(text));
        }

        //Adicionado
        public static IWritable Fieldset(params IWritable[] c)
        {
            return new HtmlElem("fieldset", c);
        }

        public static IWritable Ul(String cls, params IWritable[] c)
        {
            return new HtmlElem("ul", c)
                .WithAttr("class", cls);
        }

        #endregion
    }
}
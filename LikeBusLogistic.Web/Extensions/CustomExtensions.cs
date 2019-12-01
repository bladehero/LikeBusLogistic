using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LikeBusLogistic.Web.Extensions
{
    public static class CustomExtensions
    {
        public static string GetDescription<T>(this T source)
        {
            var fi = source.GetType().GetField(source.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
        public static HtmlString SelectList<T>(this IEnumerable<T> source, Func<T, object> value, Func<T, object> display, object selected = null, object attributes = null)
        {
            var sb = new StringBuilder("<select");
            foreach (var property in attributes?.GetType().GetProperties())
            {
                sb.Append(' ');
                sb.Append(property.Name.ToLower());
                sb.Append("=\"");
                sb.Append(property.GetValue(attributes));
                sb.Append('\"');
            }
            sb.Append('>');

            foreach (var option in source)
            {
                var optionValue = value(option);
                sb.Append("<option value=\"");
                sb.Append(optionValue);
                sb.Append('\"');
                if (optionValue.Equals(selected))
                {
                    sb.Append(" selected");
                }
                sb.Append('>');
                sb.Append(display(option));
                sb.Append("</option>");
            }

            sb.Append("</select>");
            return new HtmlString(sb.ToString());
        }
        public static HtmlString SelectListWithEmptyOption<T>(this IEnumerable<T> source, Func<T, object> value, Func<T, object> display, string emptyOption, object selected = null, object attributes = null)
        {
            var sb = new StringBuilder("<select");
            foreach (var property in attributes?.GetType().GetProperties())
            {
                sb.Append(' ');
                sb.Append(property.Name.ToLower());
                sb.Append("=\"");
                sb.Append(property.GetValue(attributes));
                sb.Append('\"');
            }
            sb.Append('>');

            sb.Append("<option>");
            sb.Append(emptyOption);
            sb.Append("</option>");

            foreach (var option in source)
            {
                var optionValue = value(option);
                sb.Append("<option value=\"");
                sb.Append(optionValue);
                sb.Append('\"');
                if (optionValue.Equals(selected))
                {
                    sb.Append(" selected");
                }
                sb.Append('>');
                sb.Append(display(option));
                sb.Append("</option>");
            }

            sb.Append("</select>");
            return new HtmlString(sb.ToString());
        }
        public static HtmlString HtmlEditIcon(string @class = "") => new HtmlString($"<span class=\"uk-text-warning {@class}\" uk-icon=\"cog\"></span>");
        public static HtmlString HtmlDeleteIcon(string @class = "") => new HtmlString($"<span class=\"uk-text-danger {@class}\" uk-icon=\"trash\"></span>");
        public static HtmlString HtmlRestoreIcon(string @class = "") => new HtmlString($"<span class=\"uk-text-success {@class}\" uk-icon=\"refresh\"></span>");
        public static HtmlString HtmlDeleteOrRestoreIcon(bool isDeleted, string @class = "") => isDeleted ? HtmlRestoreIcon(@class) : HtmlDeleteIcon(@class);
        public static HtmlString HtmlEnabledIcon => new HtmlString("<span class=\"uk-text-success\" uk-icon=\"check\"></span>");
        public static HtmlString HtmlDisabledIcon => new HtmlString("<span class=\"uk-text-danger\" uk-icon=\"close\"></span>");
    }
}

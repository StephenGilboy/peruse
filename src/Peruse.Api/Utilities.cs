using System.Text;
using HtmlAgilityPack;

namespace Peruse.Api;

public static class Utilities
{
    /// <summary>
    /// To cut down on tokens we only want to return tags that contain useful information.
    /// </summary>
    /// <param name="document">HTML Document</param>
    /// <returns>A string of all the tags we care about</returns>
    public static string SanitizeHtmlDocument(HtmlDocument document)
    {
        // List of node names to remove
        var sb = new StringBuilder();
        string[] includeTags = new[] { "h1", "h2", "h3", "h4", "h5", "h6", "p", "img", "a", "button", "input" };


        foreach (var tag in includeTags)
        {
            var nodes = document.DocumentNode.SelectNodes($"//{tag}");
            if (nodes == null) continue;
            foreach (var node in nodes)
            {
                if (tag == "img")
                {
                    sb.Append($"<img src='{node.GetAttributeValue("src", string.Empty)}' alt='{node.GetAttributeValue("alt", string.Empty)}'>");
                }
                else
                {
                    sb.Append(node.OuterHtml);    
                }
            }
        }
        return sb.ToString();
    }
}
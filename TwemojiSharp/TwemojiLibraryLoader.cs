namespace TwemojiSharp
{
    /// <summary>
    /// Gets original twemoji library
    /// </summary>
    internal static class TwemojiLibraryLoader
    {
        internal static string GetJs()
        {
            return Properties.Resources.twemoji_min_js;
        }
    }
}

namespace TwemojiSharp
{
    /// <summary>
    /// An &lt;img&gt; returned by the original twemoji.parse()
    /// represented as c# class
    /// </summary>
    public class TwemojiImg
    {
        /// <summary>
        /// An emoji representing the image
        /// </summary>
        public string Emoji { get; set; }

        /// <summary>
        /// Emojis image link
        /// </summary>
        public string Src { get; set; }
    }
}

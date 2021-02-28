using Jint;
using SoftCircuits.HtmlMonkey;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TwemojiSharp
{
    /// <summary>
    /// Twemoji.js wrapper
    /// </summary>
    public class TwemojiLib
    {
        /// <summary>
        /// A copy of default options used by <see cref="TwemojiLib"/> 
        /// </summary>
        public static TwemojiOptions DefaultOptions
            => new TwemojiOptions();

        private static readonly Lazy<Engine> _jsRuntime = new Lazy<Engine>(() => 
        {
            var jsRuntime = new Engine();
            jsRuntime.Execute(TwemojiLibraryLoader.GetJs());
            
            // Jint.Engine.Invoke() invokes only functions,
            // so we make a parse() function out of a twemoji.parse() method
            jsRuntime.Execute("const parse = twemoji.parse");
            
            return jsRuntime;
        });

        /// <summary>
        /// Replaces all emojis within a string with &lt;img&gt; tags
        /// <br/> Uses <see cref="TwemojiLib.DefaultOptions"/>
        /// </summary>
        public string Parse(string str)
        {
            // invoke original parse() function from the twemoji.js
            var result = _jsRuntime
                .Value
                .Invoke("parse", str, new {
                    callback = DefaultOptions.ImageSourceGenerator,
                    DefaultOptions.Base,
                    ext = DefaultOptions.Ext,
                    size = DefaultOptions.Size,
                    className = DefaultOptions.ClassName,
                })
                .AsString();

            return result;
        }

        /// <summary>
        /// Replaces all emojis within a string with &lt;img&gt; tags
        /// <br/> Uses custom <paramref name="options"/>
        /// </summary>
        public string Parse(string str, Action<TwemojiOptions> options)
        {
            // configure the default options
            var parsingOptions = DefaultOptions;
            options.Invoke(parsingOptions);

            // invoke original parse() function from the twemoji.js
            // with custom options
            var result = _jsRuntime
                .Value
                .Invoke("parse", str, new {
                    callback = parsingOptions.ImageSourceGenerator,
                    //attributes = (rawText, iconId) => null,
                    parsingOptions.Base,
                    ext = parsingOptions.Ext,
                    size = parsingOptions.Size,
                    className = parsingOptions.ClassName,
                    //onerror = ?
                })
                .AsString();

            return result;
        }

        /// <summary>
        /// Gets a list of emojis' images within a string
        /// <br/> Uses <see cref="TwemojiLib.DefaultOptions"/>
        /// </summary>
        public List<TwemojiImg> ParseToList(string str)
        {
            var parsed = Parse(str);
            return GetImgObjectsFromString(parsed);
        }

        /// <summary>
        /// Gets a list of emojis' images within a string
        /// <br/> Uses custom <paramref name="options"/>
        /// </summary>
        public List<TwemojiImg> ParseToList(string str, Action<TwemojiOptions> options)
        {
            var parsed = Parse(str, options);
            return GetImgObjectsFromString(parsed);
        }

        /// <summary>
        /// Gets a list of <see cref="TwemojiImg"/> from the <paramref name="parsedString"/>
        /// <br/> Parsed string is a string with &lt;img&gt; tags
        /// </summary>
        /// <param name="parsedString">A string with &lt;img&gt; tags</param>
        private List<TwemojiImg> GetImgObjectsFromString(string parsedString)
        {
            var html = HtmlDocument.FromHtml(parsedString);
            var imgs = html.Find("img").Select(n => new TwemojiImg()
            {
                Emoji = n.Attributes["alt"].Value,
                Src = n.Attributes["src"].Value,
            }).ToList();

            return imgs;
        }
    }
}

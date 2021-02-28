using System;
using System.Collections.Generic;
using System.Dynamic;

namespace TwemojiSharp
{
    public class TwemojiOptions
    {
        /// <summary>
        /// A base callback for genarating src's
        /// </summary>
        public Func<string, ExpandoObject, string> ImageSourceGenerator { get; set; }
            = (string icon, ExpandoObject options) => {
                var opt = (TwemojiOptions)options;
                return string.Join("", opt.Base, opt.Size, "/", icon, opt.Ext);
            };

        /// <summary>
        /// Size of an image
        /// <br/> Default is 72x72
        /// </summary>
        public string Size { get; set; }
            = "72x72";

        /// <summary>
        /// Base url
        /// <br/> Default is https://twemoji.maxcdn.com/v/13.0.1/
        /// </summary>
        public string Base { get; set; }
            = "https://twemoji.maxcdn.com/v/13.0.1/";


        /// <summary>
        /// Extension of an image
        /// <br/> Default is .png
        /// </summary>
        public string Ext { get; set; }
            = ".png";

        /// <summary>
        /// Class name of the produced html &lt;img&gt; tag
        /// <br/> Default is emoji
        /// </summary>
        public string ClassName { get; set; }
            = "emoji";

        public string Folder { get; set; }
            = string.Empty;

        /// <summary>
        /// Helps to convert <see cref="ExpandoObject"/>
        /// returned from js
        /// to <see cref="TwemojiOptions"/>
        /// </summary>
        public static explicit operator TwemojiOptions(ExpandoObject obj)
        {
            var objValues = new Dictionary<string, object>(obj);
            if (objValues.Count == 0)
                return null;

            var result = new TwemojiOptions();
            if (objValues.ContainsKey("size") 
                && objValues["size"] is string)
            {
                result.Size = objValues["size"] as string;
            }

            if (objValues.ContainsKey("base") 
                && objValues["base"] is string)
            {
                result.Base = objValues["base"] as string;
            }

            if(objValues.ContainsKey("ext") 
                && objValues["ext"] is string)
            {
                result.Ext = objValues["ext"] as string;
            }

            if (objValues.ContainsKey("className")
                && objValues["className"] is string)
            {
                result.ClassName = objValues["className"] as string;
            }

            if (objValues.ContainsKey("folder")
                && objValues["folder"] is string)
            {
                result.Folder = objValues["folder"] as string;
            }

            if (objValues.ContainsKey("callback")
                && objValues["callback"] is Func<string, ExpandoObject, string>)
            {
                result.ImageSourceGenerator = objValues["callback"] as Func<string, ExpandoObject, string>;
            }

            return result;
        }
    }
}
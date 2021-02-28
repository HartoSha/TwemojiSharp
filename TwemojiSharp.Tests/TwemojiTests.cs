using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using Xunit;

namespace TwemojiSharp.Tests
{
    internal class TwemojiImgComparer : EqualityComparer<TwemojiImg>
    {
        public override bool Equals([AllowNull] TwemojiImg x, [AllowNull] TwemojiImg y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public override int GetHashCode([DisallowNull] TwemojiImg obj)
        {
            return (obj.Emoji, obj.Src).GetHashCode();
        }
    }

    public class TwemojiTests
    {
        [Theory]
        [InlineData("123213")]
        [InlineData("")]
        [InlineData("/U123")]
        public void Parse_ShouldReturnOriginalString_OnNoEmojisInAString(string input)
        {
            var twemoji = new TwemojiLib();
            var expected = input;

            string actual = twemoji.Parse(input);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("string with a basket emoji 🧺", "string with a basket emoji <img class=\"emoji\" draggable=\"false\" alt=\"\U0001f9fa\" src=\"https://twemoji.maxcdn.com/v/13.0.1/72x72/1f9fa.png\"/>")]
        [InlineData("🧺🧺🧺", "<img class=\"emoji\" draggable=\"false\" alt=\"\U0001f9fa\" src=\"https://twemoji.maxcdn.com/v/13.0.1/72x72/1f9fa.png\"/><img class=\"emoji\" draggable=\"false\" alt=\"\U0001f9fa\" src=\"https://twemoji.maxcdn.com/v/13.0.1/72x72/1f9fa.png\"/><img class=\"emoji\" draggable=\"false\" alt=\"\U0001f9fa\" src=\"https://twemoji.maxcdn.com/v/13.0.1/72x72/1f9fa.png\"/>")]
        public void Parse_ShouldReturnStringWithInlineImgTags_OnEmojisInAString(string input, string expected)
        {
            var twemoji = new TwemojiLib();
            
            string actual = twemoji.Parse(input);
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Parse_ShouldUseCustomOptions_WhenProvided()
        {
            var twemoji = new TwemojiLib();
            var input = "🧺";
            var expected = "<img class=\"testClassName\" draggable=\"false\" alt=\"\U0001f9fa\" src=\"test/_testingCustomSrcGenerator_1000x1000_yes_1f9fa.png\"/>";

            string actual = twemoji.Parse(input, (TwemojiOptions options) => {
                options.ClassName = "testClassName";
                options.Base = "test/";
                options.Size = "1000x1000";
                options.ImageSourceGenerator = (string icon, ExpandoObject callbackOptions) =>
                {
                    var opt = (TwemojiOptions)callbackOptions;
                    return $"{opt.Base}_testingCustomSrcGenerator_{opt.Size}_yes_{icon}{opt.Ext}";
                };
            });

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("123213")]
        [InlineData("")]
        [InlineData("/U123")]
        public void ParseToList_ShouldReturnEmptyList_OnNoEmojisInAString(string input)
        {
            var twemoji = new TwemojiLib();

            List<TwemojiImg> actual = twemoji.ParseToList(input);

            Assert.Empty(actual);
        }

        [Fact]
        public void ParseToList_ShouldReturnImgsList_OnEmojiesInAString()
        {
            var twemoji = new TwemojiLib();
            var input = "123🧺32a🧺fdsdf🧺fsafd";
            var expected = new List<TwemojiImg> {
                new TwemojiImg() {
                    Emoji = "🧺",
                    Src = "https://twemoji.maxcdn.com/v/13.0.1/72x72/1f9fa.png",
                },
                new TwemojiImg() {
                    Emoji = "🧺",
                    Src = "https://twemoji.maxcdn.com/v/13.0.1/72x72/1f9fa.png",
                },
                new TwemojiImg() {
                    Emoji = "🧺",
                    Src = "https://twemoji.maxcdn.com/v/13.0.1/72x72/1f9fa.png",
                },
            };

            List<TwemojiImg> actual = twemoji.ParseToList(input);

            Assert.Equal(expected, actual, new TwemojiImgComparer());
        }

        [Fact]
        public void ParseToList_ShouldUseCustomOptions_WhenProvided()
        {
            var twemoji = new TwemojiLib();
            var input = "🧺";
            var expected = new List<TwemojiImg> {
                new TwemojiImg() {
                    Emoji = "🧺",
                    Src = "test/_testingCustomSrcGenerator_1000x1000_yes_1f9fa.png",
                },
            };

            var actual = twemoji.ParseToList(input, (TwemojiOptions options) => {
                options.ClassName = "testClassName";
                options.Base = "test/";
                options.Size = "1000x1000";
                options.ImageSourceGenerator = (string icon, ExpandoObject callbackOptions) =>
                {
                    var opt = (TwemojiOptions)callbackOptions;
                    return $"{opt.Base}_testingCustomSrcGenerator_{opt.Size}_yes_{icon}{opt.Ext}";
                };
            });

            Assert.Equal(expected, actual, new TwemojiImgComparer());
        }
    }
}

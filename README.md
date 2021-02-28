# TwemojiSharp
Unofficial C# wrapper of [twemoji](https://github.com/twitter/twemoji).
It uses [Jint](https://github.com/sebastienros/jint) to execute the original [twemoji](https://github.com/twitter/twemoji) and provides you access in c#.
# Usage
### Get a list of emojis within a string
```c#
var str = "I 💗 emoji!";
var twemoji = new TwemojiLib();
var emojisList = twemoji.ParseToList(str);

// emojisList[0].Emoji -> 💗
// emojisList[0].Src -> https://twemoji.maxcdn.com/v/13.0.1/72x72/1f497.png
```

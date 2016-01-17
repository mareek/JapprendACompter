using System.Linq;

namespace JapprendACompter
{
    enum ResponseLevel
    {
        Fast,
        Normal,
        Slow,
        TooSlow,
        Wrong
    }

    static class ResponseLevelExtension
    {
        public static bool IsRight(this ResponseLevel responseLevel) 
            => new[] { ResponseLevel.Fast, ResponseLevel.Normal, ResponseLevel.Slow }.Contains(responseLevel);
    }
}

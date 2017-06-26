using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowserPoker.GameObjects.PokerEval
{
    public static class Extensions
    {
        public static string ToFriendlyString(this ulong cardMask)
        {
            return Utils.CardMaskToStringSorted(cardMask);
        }

        public static ulong ToCardMask(this string cards)
        {
            return Utils.StringToHandMask(cards);
        }
    }
}

using BrowserPoker.GameObjects.PokerEval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowserPoker.GameObjects
{
    public class Player
    {
        public string Name { get; set; }

        /// <summary>
        /// Money the player has.
        /// </summary>
        public double BankRoll;

        /// <summary>
        /// Cards of the player represented as ulong.
        /// </summary>
        public ulong[] Cards = new ulong[2];

        public bool IsSessionUser = false;
    }

    public class PlayerViewModel
    {
        public string Name;
        public string Cards;
        public double BankRoll;
        public int Position;

        public PlayerViewModel(Player player, int position)
        {
            Name = player.Name;
            BankRoll = player.BankRoll;
            Position = position;
            Cards = player.IsSessionUser ? Utils.CardMaskToString(player.Cards[0] | player.Cards[1]) : Cards = "? ?";
        }
    }
}

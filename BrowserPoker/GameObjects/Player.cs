using BrowserPoker.GameObjects.PokerEval;
using System.Collections.Generic;

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

        public SortedList<PlayerAction, double> HandActions = new SortedList<PlayerAction, double>();
    }

    public class PlayerViewModel
    {
        public string Name;
        public string Cards;
        public double BankRoll;
        public int Position;
        public SortedList<PlayerAction, double> HandActions;

        public PlayerViewModel(Player player, int position)
        {
            Name = player.Name;
            BankRoll = player.BankRoll;
            Position = position;
            HandActions = player.HandActions;
            Cards = player.IsSessionUser ? Utils.CardMaskToString(player.Cards[0] | player.Cards[1]) : Cards = "? ?";
        }
    }
}

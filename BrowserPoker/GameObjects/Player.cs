using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowserPoker.GameObjects
{
    public class Player
    {
        public string Name { get; private set; }

        /// <summary>
        /// Money the player has.
        /// </summary>
        double BankRoll;

        /// <summary>
        /// Cards of the player represented as ulong.
        /// </summary>
        ulong[] Cards;



    }
}

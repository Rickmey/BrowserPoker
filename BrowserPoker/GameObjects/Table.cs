using BrowserPoker.GameObjects.PokerEval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowserPoker.GameObjects
{
    /// <summary>
    /// Instance to handle everything game related.
    /// </summary>
    public class Table
    {
        /// <summary>
        /// ID to map client to this instance.
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// TODO put this somewhere else.
        /// This way diffent tables will deal the same cards when they are initialized at the same time. 
        /// </summary>
        Random rnd = new Random();

        Queue<ulong> deck;

        public Table(Guid id)
        {
            ID = id;

            // create deck
            deck = new Queue<ulong>(Utils.CardMasksTable.OrderBy(x => rnd.Next()).ToArray());

        }

        // get request method

        // notify client (signalR?)
    }
}

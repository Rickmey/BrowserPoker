using BrowserPoker.GameObjects.PokerEval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserPoker.GameObjects
{
    /// <summary>
    /// Instance to handle everything game related.
    /// </summary>
    public class Table
    {
        const int playerCount = 6;

        /// <summary>
        /// ID to map client to this instance.
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// TODO put this somewhere else.
        /// This way diffent tables will deal the same cards when they are initialized at the same time. 
        /// </summary>
        Random rnd = new Random();

        Player[] players;

        Queue<ulong> deck;

        public Table(Guid id)
        {
            ID = id;

            // create deck
            deck = new Queue<ulong>(Utils.CardMasksTable.OrderBy(x => rnd.Next()).ToArray());

        }

        public GameStateModel HandleRequest(RequestObject requestObject)
        {
            // convert players to playermodels
            Func<Player[], PlayerViewModel[]> playersToModels = (players) =>
            {
                var playerModels = new PlayerViewModel[players.Length];
                for (int i = 0; i < players.Length; i++)
                {
                    var model = new PlayerViewModel(players[i], i);
                    playerModels[i] = model;
                }
                return playerModels;
            };

            GameStateModel result = new GameStateModel();
            switch (requestObject.RequestType)
            {
                case RequestTypes.InitializeGame:
                    // create players
                    players = new Player[playerCount];
                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i] = new Player() { Name = randomString(), BankRoll = 1000d };
                    }
                    // player 0 is the current user
                    players[0].IsSessionUser = true;
                    break;

                case RequestTypes.StartGame:
                    // new deck
                    deck = new Queue<ulong>(Utils.CardMasksTable.OrderBy(x => rnd.Next()).ToArray());

                    // deal cards
                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].Cards[0] = deck.Dequeue();
                        players[i].Cards[1] = deck.Dequeue();
                    }
                    break;

                default: throw new ArgumentException("unknown request type");
            }

            // build answer object
            result.ID = requestObject.ID;
            result.PlayerViewModels = playersToModels(players);
            result.RequestType = requestObject.RequestType;
            return result;
        }

        /// <summary>
        /// Just for early development. Will be removed later.
        /// Taken from https://stackoverflow.com/questions/9995839/how-to-make-random-string-of-numbers-and-letters-with-a-length-of-5
        /// </summary>
        /// <param name="Size"></param>
        /// <returns></returns>
        private string randomString()
        {
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            int size = rnd.Next(5, 21);
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = input[rnd.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}

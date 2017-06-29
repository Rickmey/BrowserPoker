using BrowserPoker.GameObjects.PokerEval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrowserPoker.GameObjects
{
    /// <summary>
    /// Instance to handle everything game related.
    /// </summary>
    public class Table
    {
        const int playerCount = 6;
        const double betSize = 2d;
        const double smallBlindSize = betSize / 2;
        const double bigBet = betSize * 2;

        /// <summary>
        /// ID to map client to this instance.
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// TODO put this somewhere else.
        /// This way diffent tables will deal the same cards when they are initialized at the same time. 
        /// </summary>
        Random rnd = new Random();

        /// <summary>
        /// There are only limited possible next actions.
        /// If the next action doesn't match one of the expected actions there's something wrong.
        /// </summary>
        RequestTypes expectedNextActions = RequestTypes.StartGame;


        /// <summary>
        /// Amount of money currently in the game.
        /// </summary>
        double pot;

        int buttonPosition;

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

            // check if the requested action matches the expected action
            if ((requestObject.RequestType & expectedNextActions) == 0)
                throw new ArgumentException("requested action is not expected");
            //if (!requestObject.RequestType.HasFlag(expectedNextActions))

            GameStateModel result = new GameStateModel();
            switch (requestObject.RequestType)
            {
                case RequestTypes.StartGame:

                    // initialize when this is the first hand
                    if (players == null)
                    {
                        // randomize button position
                        buttonPosition = rnd.Next(playerCount);

                        // create players
                        players = new Player[playerCount];
                        players[0] = new Player() { Name = "Player", BankRoll = 1000d, IsSessionUser = true }; // player 0 is the session user
                        for (int i = 1; i < players.Length; i++)
                        {
                            players[i] = new Player() { Name = "AI: " + randomString(), BankRoll = 1000d };
                        }
                    }

                    // move button
                    buttonPosition++;
                    if (buttonPosition >= playerCount)
                        buttonPosition = 0;
                    // new deck
                    deck = new Queue<ulong>(Utils.CardMasksTable.OrderBy(x => rnd.Next()).ToArray());

                    // clear pot
                    // TODO: if the hand is restarted, the money in the pot will disappear
                    pot = 0;

                    // deal cards
                    for (int i = 0; i < players.Length; i++)
                    {
                        var player = players[i];
                        player.Cards[0] = deck.Dequeue();
                        player.Cards[1] = deck.Dequeue();
                        player.HandActions.Clear();
                    }

                    // post blinds
                    int smallBlindPosition = buttonPosition + 1;
                    if (smallBlindPosition >= playerCount)
                        smallBlindPosition = 0;
                    int bigBlindPosition = smallBlindPosition + 1;
                    if (bigBlindPosition >= playerCount)
                        bigBlindPosition = 0;

                    var smallBlindPlayer = players[smallBlindPosition];
                    smallBlindPlayer.BankRoll -= smallBlindSize;
                    pot += smallBlindSize;
                    smallBlindPlayer.HandActions.Add(PlayerAction.PostSmallBlind, smallBlindSize);

                    var bigBlindPlayer = players[bigBlindPosition];
                    bigBlindPlayer.BankRoll -= betSize;
                    pot += betSize;
                    bigBlindPlayer.HandActions.Add(PlayerAction.PostBigBlind, betSize);

                    expectedNextActions = (RequestTypes.PlayerAction | RequestTypes.StartGame);
                    break;

                default: throw new ArgumentException("unknown request type");
            }

            // build response object
            result.ButtonPosition = buttonPosition;
            result.PlayerViewModels = playersToModels(players);
            result.Pot = pot;
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
            int size = rnd.Next(5, 10);
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

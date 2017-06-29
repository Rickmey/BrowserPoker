﻿using BrowserPoker.GameObjects.PokerEval;
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
        RequestTypes expectedNextActions = RequestTypes.InitializeGame;


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
                case RequestTypes.InitializeGame:
                    // randomize button position
                    buttonPosition = rnd.Next(playerCount);

                    // create players
                    players = new Player[playerCount];
                    players[0] = new Player() { Name = "Player", BankRoll = 1000d, IsSessionUser = true }; // player 0 is the session user
                    for (int i = 1; i < players.Length; i++)
                    {
                        players[i] = new Player() { Name = "AI: " + randomString(), BankRoll = 1000d };
                    }

                    expectedNextActions = RequestTypes.StartGame;
                    break;

                case RequestTypes.StartGame:
                    buttonPosition++;
                    if (buttonPosition >= playerCount)
                        buttonPosition = 0;
                    // new deck
                    deck = new Queue<ulong>(Utils.CardMasksTable.OrderBy(x => rnd.Next()).ToArray());

                    // deal cards
                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].Cards[0] = deck.Dequeue();
                        players[i].Cards[1] = deck.Dequeue();
                    }

                    expectedNextActions = (RequestTypes.SetBlinds | RequestTypes.StartGame);
                    break;

                case RequestTypes.SetBlinds:



                    expectedNextActions = (RequestTypes.PlayerAction | RequestTypes.StartGame);
                    break;

                default: throw new ArgumentException("unknown request type");
            }

            // build response object
            result.ID = requestObject.ID;
            result.RequestType = requestObject.RequestType;
            result.ButtonPosition = buttonPosition;
            result.PlayerViewModels = playersToModels(players);
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

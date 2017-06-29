using BrowserPoker.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowserPoker
{
    public enum RequestTypes
    {
        InitializeGame,
        StartGame,
    }

    public class GameStateModel
    {
        public Guid ID;
        public RequestTypes RequestType;
        public PlayerViewModel[] PlayerViewModels;
        public int ButtonPosition;
    }
}

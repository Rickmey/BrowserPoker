using BrowserPoker.GameObjects;
using System;

namespace BrowserPoker
{
    public class GameStateModel
    {
        public Guid ID;
        public RequestTypes RequestType;
        public PlayerViewModel[] PlayerViewModels;
        public int ButtonPosition;
        public double Pot;
    }
}

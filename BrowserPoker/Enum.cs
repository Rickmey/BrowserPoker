using System;

namespace BrowserPoker
{
    [Flags]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum RequestTypes
    {
        InitializeGame = 1,
        StartGame = 2,
        PostBlinds = 4,
        PlayerAction = 8,
        DealBoard = 16
    }

    public enum PlayerAction
    {
        Fold,
        PostSmallBlind,
        PostBigBlind,
        Bet,
        Call, // includes check
        AllIn
    }
}

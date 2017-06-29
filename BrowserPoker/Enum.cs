using System;

namespace BrowserPoker
{
    [Flags]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum RequestTypes
    {
        StartGame = 1,
        PlayerAction = 2,
        DealBoard = 4
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

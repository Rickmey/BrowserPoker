using System;

namespace BrowserPoker
{
    [Flags]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum RequestTypes
    {
        InitializeGame = 1,
        StartGame = 2,
        SetBlinds = 4,
        PlayerAction = 8,
        DealBoard = 16
    }
}

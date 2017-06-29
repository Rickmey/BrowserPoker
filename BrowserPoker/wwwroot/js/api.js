/*
This files contains objects and names relevant for the backend.
*/

'use strict';
var Api = new Api();
// freeze to keep playercount constant
Object.freeze(Api);

function Api() {
    this.playerCount = 6;

    this.RequestTypeEnum = Object.freeze({
        "StartGame": 1,
        "PlayerAction": 2,
        "DealBoard": 4
    });

    this.PlayerActionsEnum = Object.freeze({
        "Fold": 1,
        "PostSmallBlind": 2,
        "PostBigBlind": 4,
        "Bet": 8,
        "Call": 16,
        "AllIn": 32
    });

    this.RequestPathEnum = Object.freeze({
        RequestID: './onRequestID',
        Default: './onDefault'
    });
}
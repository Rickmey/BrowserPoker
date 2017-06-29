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
        "InitializeGame": 1,
        "StartGame": 2,
        "SetBlinds": 4,
        "PlayerAction": 8,
        "DealBoard": 16
    });

    this.RequestPathEnum = Object.freeze({
        RequestID: './onRequestID',
        Default: './onDefault'
    });
}
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
        "InitializeGame": 0,
        "StartGame": 1
    });

    this.RequestPathEnum = Object.freeze({
        RequestID: './onRequestID',
        Default: './onDefault'
    });
}
/*
This files contains objects and names relevant for the backend.
*/

'use strict';

const playerCount = 6;

var RequestTypeEnum = Object.freeze({
    "InitializeGame": 0,
    "StartGame": 1
});

var RequestPathEnum = Object.freeze({
    RequestID: './onRequestID',
    Default: './onDefault'
});
'use script';

var requestHandler = new RequestHandler();
var playerModels = [];
var currentButtonPosition;

window.addEventListener('load', function () {

    // initialize player models
    for (let i = 0; i < Api.playerCount; ++i) {
        playerModels[i] = new PlayerViewModel(i);
    }

    requestID(requestInitializeGame);

    document.getElementById('new-game-button').addEventListener('click', function () {
        requestStartGame();
    });

});

// REQUESTS
function jsonToPlayerViewModel(json) {
    currentButtonPosition = json.ButtonPosition;

    for (var i = 0; i < playerModels.length; i++) {
        var backendModel = json.PlayerViewModels[i];
        var viewModel = playerModels[i];
        viewModel.setName(backendModel.Name);
        viewModel.setCards(backendModel.Cards);
        viewModel.setPosition(backendModel.Position);
        viewModel.setBankRoll(backendModel.BankRoll);
        viewModel.setHasButton(viewModel.getPosition() === currentButtonPosition);
    }
}

function requestInitializeGame(next) {
    requestHandler.sendRequest(Api.RequestPathEnum.Default, { 'RequestType': Api.RequestTypeEnum.InitializeGame }, function (request) {
        var json = JSON.parse(request.response);
        jsonToPlayerViewModel(json);
        if (next !== undefined)
            next();
    });
}

function requestStartGame(next) {
    requestHandler.sendRequest(Api.RequestPathEnum.Default, { 'RequestType': Api.RequestTypeEnum.StartGame }, function (request) {
        var json = JSON.parse(request.response);
        jsonToPlayerViewModel(json);
        if (next !== undefined)
            next();
    });
}

function requestID(next) {
    requestHandler.sendRequest(Api.RequestPathEnum.RequestID, undefined, function (request) {
        sessionStorage['ID'] = request.responseText;
        if (next !== undefined)
            next();
    });
}

// OBJECTS
function RequestHandler() {

    this.sendRequest = function (path, obj, callback) {
        var request = new XMLHttpRequest();
        request.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                console.log(this.responseText);
                if (callback !== undefined) {
                    callback(this);
                }
            }
        };
        console.log(obj);
        request.open('POST', path, true);
        request.setRequestHeader("Content-Type", "application/json");
        if (obj === undefined)
            obj = {};
        // always add session id to request object
        obj.id = sessionStorage['ID'];
        request.send(JSON.stringify(obj));
    };
}

function PlayerViewModel(index) {
    var name;
    this.getName = function () { return name; };
    this.setName = function (newName) {
        if (name === newName)
            return;
        name = newName;
        nameElement.innerText = name;
    };

    var position;
    this.getPosition = function () { return position; };
    this.setPosition = function (newPosition) {
        if (position === newPosition)
            return;
        position = newPosition;
        positionElement.innerText = 'position: ' + position;
    };

    var bankRoll;
    this.getBankRoll = function () { return bankRoll; };
    this.setBankRoll = function (newBankRoll) {
        if (bankRoll === newBankRoll)
            return;
        bankRoll = newBankRoll;
        bankRollElement.innerText = 'bankroll: ' + bankRoll.toFixed(2) + 'u';
    };

    var cards;
    this.setCards = function (newCards) {
        if (cards === newCards)
            return;
        cards = newCards;
        cardsElement.innerText = 'cards: ' + cards;
    };

    var hasButton = false;
    this.setHasButton = function (newHasButton) {
        if (hasButton === newHasButton)
            return;
        hasButton = newHasButton;
        if (hasButton)
            playerElement.classList.add('button-position');
        else
            playerElement.classList.remove('button-position');
    };

    //DOM ELEMENTS
    var parent = document.getElementById('player-parent');

    var playerElement = document.createElement('ul');
    playerElement.classList.add('player');

    var nameElement = document.createElement('li');
    playerElement.appendChild(nameElement);
    this.setName('default');

    var positionElement = document.createElement('li');
    playerElement.appendChild(positionElement);
    this.setPosition(index);

    var bankRollElement = document.createElement('li');
    playerElement.appendChild(bankRollElement);
    this.setBankRoll(0);

    var cardsElement = document.createElement('li');
    playerElement.appendChild(cardsElement);
    this.setCards('? ?');

    document.getElementById('player-parent').appendChild(playerElement);

}
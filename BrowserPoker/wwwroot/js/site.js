'use script';

const currency = 'u';

window.addEventListener('load', function () {

    // request ID. this id is stored in the session storage and used to indentify the game instance in the backend.
    var requestHandler = new RequestHandler();
    requestHandler.sendRequest(Api.RequestPathEnum.RequestID, undefined, function (request) {
        sessionStorage['ID'] = request.responseText;
    });

    var tableViewModel = new TableViewModel();


    document.getElementById('new-game-button').addEventListener('click', function () {
        // clear all previous hand actions
        tableViewModel.clearPlayerActions();
        requestHandler.sendRequest(Api.RequestPathEnum.Default, { 'RequestType': Api.RequestTypeEnum.StartGame }, function (request) {
            tableViewModel.updateViewModels(request.response);
        });
    });
});


// OBJECTS
// table is the game instance where the game takes place
function TableViewModel() {
    var that = this;
    var playerModels = [];
    this.getPlayerModels = function () {
        return playerModels;
    };

    var dealerPosition;
    this.setDealerPosition = function (newDealerPosition) {
        if (dealerPosition === newDealerPosition)
            return;
        dealerPosition = newDealerPosition;

        var dealerImages = document.getElementsByClassName('dealer-button-img');
        for (var i = 0; i < dealerImages.length; i++) {
            var dealerImageElement = dealerImages[i];
            dealerImageElement.classList.add('hidden');
            if (dealerPosition === i)
                dealerImageElement.classList.remove('hidden');
        }

    };

    var pot = 0.00;
    this.setPot = function (newPot) {
        if (pot === newPot)
            return;
        potElement.innerText = 'Pot: ' + pot.toFixed(2) + currency;
    };

    this.clearPlayerActions = function () {
        // clear all previous hand actions
        for (var i = 0; i < playerModels.length; i++) {
            playerModels[i].setPlayerActions({});
        }
    };

    // parse JSON from request response and update view models
    this.updateViewModels = function updateViewModels(response) {
        var json = JSON.parse(response);
        that.setDealerPosition(json.ButtonPosition);
        that.setPot(json.Pot);
        for (var i = 0; i < playerModels.length; i++) {
            var backendModel = json.PlayerViewModels[i];
            var viewModel = playerModels[i];
            viewModel.setName(backendModel.Name);
            viewModel.setCards(backendModel.Cards);
            viewModel.setBankRoll(backendModel.BankRoll);
            viewModel.setPlayerActions(backendModel.HandActions);
        }
    };

    //DOM
    // initialize player models and add players to dom
    for (let i = 0; i < Api.playerCount; ++i) {
        playerModels[i] = new PlayerViewModel(i);
    }

    var potElement = document.getElementById('pot-element');
}

function RequestHandler() {

    this.sendRequest = function (path, obj, callback) {
        var request = new XMLHttpRequest();
        request.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                if (callback !== undefined) {
                    callback(this);
                }
            }
        };
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
    this.setName = function (newName) {
        if (name === newName)
            return;
        name = newName;
        nameElement.innerText = name;
    };

    var bankRoll;
    this.setBankRoll = function (newBankRoll) {
        if (bankRoll === newBankRoll)
            return;
        bankRoll = newBankRoll;
        bankRollElement.innerText = 'bankroll: ' + bankRoll.toFixed(2) + currency;
    };

    var cards;
    this.setCards = function (newCards) {
        if (cards === newCards)
            return;
        cards = newCards;
        cardsElement.innerText = 'cards: ' + cards;
    };

    var playerActions;
    this.setPlayerActions = function (actions) {
        // remove all children
        while (playerActionsElement.firstChild) {
            playerActionsElement.removeChild(playerActionsElement.firstChild);
        }

        if (Object.keys(actions).length === 0)
            return;

        playerActions = actions;
        for (var i = 0; i < Object.keys(playerActions).length; i++) {
            var child = document.createElement('li');
            child.innerText = Object.keys(playerActions)[i] + ': ' + playerActions[Object.keys(playerActions)[i]];
            playerActionsElement.appendChild(child);
        }
    };

    // DOM
    var parent = document.getElementById('player-parent');

    var playerElement = document.createElement('ul');
    playerElement.classList.add('player');

    var nameElement = document.createElement('li');
    playerElement.appendChild(nameElement);
    this.setName('default');

    var bankRollElement = document.createElement('li');
    playerElement.appendChild(bankRollElement);
    this.setBankRoll(0);

    var cardsElement = document.createElement('li');
    playerElement.appendChild(cardsElement);
    this.setCards('? ?');

    var playerActionsElement = document.createElement('ul');
    playerElement.appendChild(playerActionsElement);
    parent.appendChild(playerElement);

}

'use script'

var requestHandler = new RequestHandler();

window.addEventListener('load', function () {

    var output = document.getElementById('output-text');

    requestHandler.sendRequest('./onRequestID', undefined, function (request) {
        sessionStorage['ID'] = request.responseText;
    });

    document.getElementById('request-id-button').addEventListener('click', function () {
        requestHandler.sendRequest('./onRequestID', undefined, function (request) {
            sessionStorage['ID'] = request.responseText;
            output.innerHTML = request.response;
        });
    });

});


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
        // use POST not GET
        request.open('POST', path, true);
        obj === undefined ? request.send() : request.send(obj);
    }
}

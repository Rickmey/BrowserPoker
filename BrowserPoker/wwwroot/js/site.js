'use script'

window.addEventListener('load', function () {
    console.log("i'm alive");

    RequestObject('http://localhost:51617/', undefined); // returns unmapped default because map3 does not exist
    RequestObject('http://localhost:51617/map1', undefined); // returns result of map1 function
    RequestObject('http://localhost:51617/map2', undefined); // returns result of map2 function
    RequestObject('http://localhost:51617/map3', undefined); // returns unmapped default because map3 does not exist
    RequestObject('./', undefined);
    RequestObject('./map1', undefined); // this is the way to go
    RequestObject('/', undefined);
    RequestObject('', undefined);

    RequestObject('/bodytest', '12345');
});

function RequestObject(path, obj) {
    var request = new XMLHttpRequest();


    request.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            console.log('');
            console.log('==== Request Handled (' + path + ') ====');
            console.log('responseURL= ' + this.responseURL);
            console.log('responseType= ' + this.responseType);
            console.log('responseText= ' + this.responseText);

            var p = document.createElement('p');
            p.innerText = '=== Request Handled || Path: ' + path + ' || Answer: ' + this.responseText;
            document.body.appendChild(p);
        }
    };
    // use POST not GET
    request.open('POST', path, true);
    if (obj != undefined)
        request.send(obj);
    else
        request.send();
}

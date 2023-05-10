$(function(){
    var ws = new WebSocket('ws://' + window.location.host + '/ws/');
            ws.onopen = function(e){
                console.log("Socket open success!", e)
            };
            ws.onmessage = function(e){
                console.log("message",e)
                const data = JSON.parse(e.data);
                document.querySelector('#log').innerHTML += (data.message + '<br>');
            };
            ws.onerror = function(e) {
                console.log('error', e);
            };
            ws.onclose = function(e){
                console.log("Chat socket closed unexpectedly!",e)
            };
            document.querySelector('#button').onclick = function(e){
                const username = document.querySelector('#username');
                const message = username.value + 'Click the button!';
                ws.send(JSON.stringify({
                    'message': message
                }));
            };
    });
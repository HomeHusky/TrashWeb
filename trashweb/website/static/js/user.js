$(function(){
    var ws = new WebSocket('ws://' + window.location.host + '/ws/');
            ws.onopen = function(e){
                console.log("Socket open success!", e)
            };
            ws.onmessage = function(e){
                console.log("message",e)
                var userData = JSON.parse(e.data)
                $('#new_user').html(userData.html_users)
            };
            ws.onerror = function(e) {
                console.log('error', e);
            };
            ws.onclose = function(e){
                console.log("Socket close!",e)
            };
    });
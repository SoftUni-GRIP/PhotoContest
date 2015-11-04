(function () {
    var myHub = $.connection.baseHub;
    $.connection.hub.logging = true;
    $.connection.hub.start();

    //example

    myHub.client.userConnected = function (data) {
        // Set the received serverTime in the span to show in browser
        toastr["success"](data)
        
    };
}());
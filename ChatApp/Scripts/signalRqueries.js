var uHub = $.connection.usersOnline;

$(document).ready(function () {
   

    $("#gifid").hide();

    $("#submitbtn").click(function () {
        updateNotification();
    });

    uHub.client.messages = function () {
        console.log("messages hub function");
        getMessages();
    }

    uHub.client.checkOnline = function () {
        console.log("checkOnline hub function");
        getOnlineUsers();
    }

     $.connection.hub.start().done(function () {
        console.log("Hub Connection Successfull");
        uHub.server.announce("Announce working");

    }).fail(function () {
        alert("Failed");
    });

    getOnlineUsers();

    getMessages();

});


function gettoken() {
    var token = '@Html.AntiForgeryToken()';
    token = $(token).val();
    return token;
}


uHub.client.announce = function (message) {
    console.log(message);
}

function callimg(x, y) {

    document.getElementById(x).appendChild(y);

}


function getOnlineUsers() {
    var myUrl = $("#MyURL").val();
    var uid = $("#Uid").val();
    $d = $("#Userslist")
    $.ajax({
        type: 'Post',
        url: myUrl,
        dataType: 'json',
        data: {__RequestVerificationToken: gettoken()},
        success: function (data) {
            $d.empty();
            $.each(data.Onlinelist, function (i, model) {
                var ids = 'div' + i;
                var s;
                var url = model.ImagePath;
                url = url.substring(1);
                url = window.location.origin + url;
                console.log(url);
                if (model.status == 1) { s = "Online" } else { s = "offline" }
                if (model.Id != uid) {
                    $d.append(
                        '<div id="' + ids + '">' +
                        '<img src="' + url + '" class="img img-responsive img-circle" style="width:80px;height:80px;display: inline-block" />' +
                        '<span>' + model.Email + '</span>' +
                        '<h4>' + model.Name + '</h4>' + '<h2>' + s + '</h2>' + '</div>' 

                    );
                }
                $("#div" + i).click(function () {

                    $("#recevier_Id").val(model.Id);
                    $("#heading").empty();
                    var mess = "You are chating with " + model.Email + " Now...";
                    $("#heading").append(mess);
                    $.notify(mess, 'Info')

                    getMessages();

                });
            });
        }
    });

}






    

function getMessages() {
    var MyMessage = $("#MyMessage").val();
    var uid = $("#Uid").val();
    var rid = $("#recevier_Id").val();
    $dd = $("#Messagelist")
    $.ajax({
        type: 'Post',
        url: MyMessage,
        dataType: 'json',
        data: {__RequestVerificationToken: gettoken()},
        success: function (data) {
            $dd.empty();
            $.each(data.Messagelist, function (i, model) {

               // if (uid === model.sender_Id && rid === model.recevier_Id) {

                    var temp = new Array();
                    var a = model.ImagePath
                    temp = a.split(",");
                    //console.log(temp);
                    if (temp == "Null") {
                        temp.shift("Null")
                    }


                    var im;
                    var img = "imgdiv" + i;
                    var x = document.createElement("DIV");
                    x.id = img;

                    for (var j = 0; j < temp.length; j++) {

                        var url = temp[j];

                        url = url.substring(1);
                        url = window.location.origin + url;
                        // console.log(url);
                        if (url == "https://localhost:44370ull") {
                            if (temp[j] == "Null") {
                            }
                            else if (temp[j] == "") {

                            }
                            else {
                                im = document.createElement("IMG");
                                im.src = url
                                im.style.width = '60px';
                                im.style.height = '60px';
                                im.padding = '10px';
                                im.style.display = none;
                            }
                        }
                        else {
                            if (temp[j] == "Null") {
                            }
                            else if (temp[j] == "") {

                            }
                            else {
                                im = document.createElement("IMG");
                                im.src = url
                                im.style.width = '60px';
                                im.style.height = '60px';
                                im.padding = '10px';
                                im.className = "img img-responsive img-circle";

                                x.appendChild(im);
                            }
                        }
                    }
                //    console.log(x);

                var mes;
                if (model.Messages === "Null") {
                    mes = "";

                } else {
                    mes = model.Messages;

                }
                    if (uid == model.sender_Id) {

                        $dd.append(

                            '<div  style="height:100px;"> <div style="border-radius:8px;padding:8px;margin:10px;width:80%;float:right;background-color:#00734F">' +
                            x.outerHTML +
                            '<br><span style="color:white;font-size:16px">' + mes + '</span> <br>' +
                            '<span style="color:white;font-size:16px"> Sent From :' + model.sender_Name  + '</div></div>'

                        );

                    } else {

                       
                        $dd.append(

                            '<div  style="width:500px;height:100px;"> <div style="border-radius:8px;padding:8px;margin:10px;width:80%;float:left;background-color:#232323">' +
                            x.outerHTML +
                            '<br><span style="color:white;font-size:16px">' + model.Messages + '</span> <br>' +
                            '<span style="color:white;font-size:16px"> Sent From :' + model.sender_Name + '</div></div>'


                        );
                    }

               // console.log(Date.parse(model.DateTime.toISOString()));
                //}

                //console.log(model.datetime.toString());
                //console.log(new Date(model.datetime));
            });
        }
    });

}
//updateNotification();
    // Click on notification icon for show notification
    $('span.noti').click(function (e) {
        e.stopPropagation();
        $('.noti-content').show();
        var count = 0;
        count = parseInt($('span.count').html()) || 0;
        //only load notification if not already loaded
        if (count > 0) {
            updateNotification();
        }
        $('span.count', this).html('&nbsp;');
    })
    // hide notifications
    $('html').click(function () {
        $('.noti-content').hide();
    })

var acb = 0;
    // update notification
function updateNotification() {
    
        var MyNoti = $("#MyNoti").val();
        $('#notiContent').empty();
        $('#notiContent').append($('<li>Loading...</li>'));
        $.ajax({
            type: 'GET',
            url: MyNoti,
            success: function (response) {
                console.log("from c# " + response.count);
                $('#notiContent').empty();
                if (response.length == 0) {
                    $('#notiContent').append($('<li>No data available</li>'));
                }
                acb = response.list.length;
                $.each(response.list, function (index, value) {
                    console.log("IN noti each " + value.Messages);
                    $('#notiContent').append($('<li>New contact : ' + value.Messages + ' (' + value.sender_Id + ') added</li>'));
                  
                });
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    // update notification count
    function updateNotificationCount() {
        //var count = 0;
        //count = parseInt($('span.count').html()) || 0;
        //count++;
       
        $('#count').empty();
        $('#count').append("Notifications " + acb);
    }
    // signalr js code for start hub and send receive notification
  //  var notificationHub = $.connection.usersOnline;
  
    //signalr method for push server message to client
    $.connection.usersOnline.client.notify = function (message) {
        console.log("insde notify");
        if (message && message.toLowerCase() == "added") {
            updateNotificationCount();
      
             console.log("insde notify if");
        }
}



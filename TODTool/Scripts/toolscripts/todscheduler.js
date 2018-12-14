var delay = 1000 * 5 * 60;
var timeout;

function loadschStatus() {
    try {
        //alert("loaded");

        //invoke server calls

        $.ajax({
            type: "GET",
            async: true,
            dataType: 'json',
            cache: false,
            url: "/TOD/GetSchedulerResultInfo",
            success: onSuccess,
            error: onError
        });
    } catch (err) {
        alert("Function invokation error." + err.message);
    }
    setTimeout(loadschStatus, delay);
};

function onSuccess(response) {
    //alert(response.oldval);
    var bool = response.success;
    //alert(bool);
    //alert($("#schstat"));
    if ($("#schstat") === 'undefined') {
        //dont do anything
        //alert("here");
    } else {
        if (bool === true) {
            //alert("here 1");
            $("#schstat").attr("class", "text-info");
        } else {
            //alert("here 2");
            $("#schstat").attr("class", "text-warning");
        }
        //alert("here 3");
        $("#schstat").html(response.oldval)
    }
    //alert(response.oldval + ' changed to ' + newval);
};

function onError(jqXHR, textStatus, errorThrown) {
    //alert('Error - ' + errorThrown);
    $("#schstat").html(errorThrown)
};
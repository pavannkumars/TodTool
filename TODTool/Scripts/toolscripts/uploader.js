function MultiplefileSelected(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    //$('#drop_zone').removeClass('hover');
    selectedFiles = evt.target.files || evt.dataTransfer.files;
    if (selectedFiles) {
        $('#Files').empty();
        for (var i = 0; i < selectedFiles.length; i++) {
            DataURLFileReader.read(selectedFiles[i], function (err, fileInfo) {
                if (err !== null) {
                    //alert("not Null");
                    var RowInfo = '<div id="File_' + i + '"class="info" >' +
                        '< div class="InfoContainer" > ' +
                        '<div class="Error">' + err + '</div>' +
                        '<div data-name="FileName" class="info">' + fileInfo.name + '</div>' +
                        '<div data-type="FileType" class="info">' + fileInfo.type + '</div>' +
                        '<div data-size="FileSize" class="info">' + fileInfo.size() + '</div>' +
                        '</div > <hr/>' +
                        '</div > ';
                    $('#Files').append(RowInfo);
                } else {
                    //alert("Null");
                    var RowInfo1 = '<div class="row"><div class="col-sm-4"><p class="text-left"><b> FileName </b> : ' + fileInfo.name + "</p></div>\n" +
                                   '<div class="col-sm-4"><p class="text-left"><b> File Type </b> : ' + fileInfo.type + "</p></div>\n" +
                                   '<div class="col-sm-4"><p class="text-left"><b> File Size </b> : ' + fileInfo.size() + "</p></div></div>";
                    //alert("RowInfo1 : " + RowInfo1);
                    $('#Files').append(RowInfo1);
                }
                /*if (err != null) {
                    var RowInfo = '<div id="File_' + i + '" 
                    class="info" > <div class="InfoContainer">' +
                                   '<div class="Error">' + err + '</div>' +
                                  '<div data-name="FileName"
                            class="info">' + fileInfo.name + '</div>' +
                                  '<div data-type="FileType"
                            class="info">' + fileInfo.type + '</div>' +
                                  '<div data-size="FileSize"
                            class="info">' + fileInfo.size() +
                                  '</div></div> <hr /></div > ';
                    $('#Files').append(RowInfo);
                }
                else {
                    var image = '<img src="' + fileInfo.fileContent +
                        '" class="thumb" title="' +
                        fileInfo.name + '" />';
                    var RowInfo = '<div id="File_' + i + '" 
                    class="info" > <div class="InfoContainer">' +
                                  '<div data_img="Imagecontainer">' +
                                  image + '</div>' +
                                  '<div data-name="FileName"
                            class="info">' + fileInfo.name + '</div>' +
                                  '<div data-type="FileType"
                            class="info">' + fileInfo.type + '</div>' +
                                  '<div data-size="FileSize"
                            class="info">' + fileInfo.size() +
                                  '</div></div> <hr /></div > ';
                    $('#Files').append(RowInfo);
                }*/
            });
        }
    } 
};

function progressHandlingFunction(e) {
    if (e.lengthComputable) {
        var percentComplete = Math.round(e.loaded * 100 / e.total);
        $("#FileProgress").css("width",
            percentComplete + '%').attr('aria-valuenow', percentComplete);
        $('#FileProgress span').text(percentComplete + "%");
    }
    else {
        $('#FileProgress span').text('unable to compute');
    }
};

UploadFile = function () {
    //we can create form by passing the form to Constructor of formData object
    //or creating it manually using append function 
    //but please note file name should be same like the action Parameter
    //var dataString = new FormData();
    //dataString.append("UploadedFile", selectedFile);

    var rr = [];
    $('#cefwklist :selected').each(function (i, selected) {
        rr[i] = $(selected).text();
    });
    //alert(rr);

    //var selFW = $('');
    var form = $('#FormUpload')[0];
    var dataString = new FormData(form);
    $.ajax({
        url: '/Uploader/Upload',  //Server script to process data
        type: 'POST',
        xhr: function () {  // Custom XMLHttpRequest
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) { // Check if upload property exists
                //myXhr.upload.onprogress = progressHandlingFunction
                myXhr.upload.addEventListener('progress', progressHandlingFunction,
                    false); // For handling the progress of the upload
            }
            return myXhr;
        },
        //Ajax events
        success: successHandler,
        error: errorHandler,
        complete: completeHandler,
        // Form data
        data: dataString,
        //Options to tell jQuery not to process data or worry about content-type.
        cache: false,
        contentType: false,
        processData: false
    });
};

var DataURLFileReader = {
    read: function (file, callback) {
        var reader = new FileReader();
        var fileInfo = {
            name: file.name,
            type: file.type,
            fileContent: null,
            size: function () {
                var FileSize = 0;
                if (file.size > 1048576) {
                    FileSize = Math.round(file.size * 100 / 1048576) / 100 + " MB";
                }
                else if (file.size > 1024) {
                    FileSize = Math.round(file.size * 100 / 1024) / 100 + " KB";
                }
                else {
                    FileSize = file.size + " bytes";
                }
                return FileSize;
            }
        };
        /*if (!file.type.match('image.*')) {
            callback("file type not allowed", fileInfo);
            return;
        }*/
        reader.onload = function () {
            fileInfo.fileContent = reader.result;
            callback(null, fileInfo);
        };
        reader.onerror = function () {
            callback(reader.error, fileInfo);
        };
        reader.readAsDataURL(file);
    }
};

function UploadMultipleFiles() {
    
    var rr = [];
    $('.celist :selected').each(function (i, selected) {
        rr[i] = $(selected).text();
    });
    //alert("rr :" + rr[0]);
    //alert("fweek :" + fiscalWeek);
    

    if (typeof fiscalWeek === 'undefined' || rr === 'undefined' || rr.length === 0) {
        alert("please select the fiscal week before uploading required omniture report files.");
        return;
    }

    //alert(selectedFiles);
    if (typeof selectedFiles === 'undefined' || selectedFiles === null) {
        alert("please select the required omniture report files to upload.");
        return;
    }
    // here we will create FormData manually to prevent sending mon image files
    var dataString = new FormData();
    //var files = document.getElementById("UploadedFiles").files;
    for (var i = 0; i < selectedFiles.length; i++) {
        /*if (!selectedFiles[i].type.match('image.*')) {
            continue;
        }*/
        dataString.append("uploadedFiles", selectedFiles[i]);
    }
    dataString.append("fweek", rr[0]);

    $.ajax({
        url: '/TOD/UploadMultiple?fweek=' + rr[0],  //Server script to process data
        type: 'POST',
        xhr: function () {  // Custom XMLHttpRequest
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) { // Check if upload property exists
                //myXhr.upload.onprogress = progressHandlingFunction
                myXhr.upload.addEventListener('progress', progressHandlingFunction,
                    false); // For handling the progress of the upload
            }
            return myXhr;
        },
        //Ajax events
        success: successHandler,
        error: errorHandler,
        complete: completeHandler,
        // Form data
        data: dataString,
        //Options to tell jQuery not to process data or worry about content-type.
        cache: false,
        contentType: false,
        processData: false
    });
};

function successHandler(response) {
    //alert(response);
    var statusinfoblock = $("#InfoContainer");
    statusinfoblock.attr("class", "text-info");
    var myJSON = JSON.stringify(response);
    statusinfoblock.html(response);
}

function errorHandler(err) {
    //alert(err);
    var statusinfoblock = $("#InfoContainer");
    statusinfoblock.attr("class", "text-danger");
    statusinfoblock.html(err);
    
}

function completeHandler() {
    //alert("Ajax Processing completed");
}

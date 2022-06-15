let CLOUDINARY_URL = 'https://api.cloudinary.com/v1_1/fanfancloud/upload';
let CLOUDINARY_UPLOAD_PRESET = 'wurq2r6w';
let imgPreview = document.getElementById('img-preview');
let fileUpload = document.getElementById('customFile');
fileUpload.addEventListener('change', function () {
    let file = event.target.files[0];
    let formData = new FormData();
    formData.append('file', file);
    formData.append('upload_preset', CLOUDINARY_UPLOAD_PRESET);
    axios({
        url: CLOUDINARY_URL,
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        data: formData
    }).then(function (res) {
        console.log(res);
        console.log(res.data);
        imgPreview.src = res.data.secure_url;
        document.getElementById('file-upload-url').value = res.data.secure_url;
    }).catch(function (err) {
    });
});

function RemoveFanFiction(index){
    $.ajax({
        type: "get",
        url: `/FanFiction/RemoveFanFiction?id=${index}`,
        success: function (message) {
            alert(message);
            window.location.reload();
        },
        error: function (response) {
        }
    });
}

function RemoveComment(index){
    $.ajax({
        type: "get",
        url: `/FanFiction/RemoveComment?id=${index}`,
        success: function (message) {
            alert(message);
            window.location.reload();
        },
        error: function (response) {
        }
    });
}

function PublicationFanFiction(index){
    $.ajax({
        type: "get",
        url: `/FanFiction/PublicationFanFiction?id=${index}`,
        success: function (message) {
            alert(message);
            window.location.reload();
        },
        error: function (response) {
        }
    });
}

function RemoveLike(index){
    $.ajax({
        type: "get",
        url: `/FanFiction/RemoveLike?id=${index}`,
        success: function () {
            window.location.reload();
        },
        error: function (response) {
        }
    });
}

function SetLike(index){
    $.ajax({
        type: "get",
        url: `/FanFiction/SetLike?id=${index}`,
        success: function () {
            window.location.reload();
        },
        error: function (response) {
        }
    });
}

function RemoveChapter(index){
    $.ajax({
        type: "get",
        url: `/FanFiction/RemoveChapter?id=${index}`,
        success: function (message) {
            alert(message);
            window.location.reload();
        },
        error: function (response) {
        }
    });
}

function SetAdminCheck(index){
    $.ajax({
        type: "get",
        url: `/Moderation/SetAdminChecked?id=${index}`,
        success: function (message) {
            alert(message);
            window.location.reload();
        },
        error: function (response) {
        }
    });
}

function BlockFanFiction(index){
    $.ajax({
        type: "get",
        url: `/Admin/Block?id=${index}`,
        success: function (message) {
            alert(message);
            window.location.reload();
        },
        error: function (response) {
        }
    });
}
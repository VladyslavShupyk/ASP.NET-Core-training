$(".trainer-info-a").on('click', function (e) {
    var searchId = $(this).attr("id");
    $.ajax({
        type: 'POST',
        url: "/Trainers/SearchTrainer?Id=" + searchId,
        dataType: "json",
        success: function (response) {
            $("#trainerInformationModalLabel").text(response.user.fullName);
            $(".trainer-modal-image").css("background", "url(data:image/jpeg;base64," + response.user.photo);
            if (response.description != null)
                $('.trainer-modal-description').text(response.description);
            else
                $('.trainer-modal-description').text("Тренер пока что не имеет описания.");
        }
    });
});
$("#create-course-button").on('click', function (e) {
    console.log("create course clicked");
    var name = $("#name").val();
    if (name.length == 0 || name.length > 64)
    {
        alert("Name must be of 1...64 characters");
        var name = $("#name").val("");
        return;
    }
    var is_new = false;
    if ($('#is-new').is(':checked'))
    {
        is_new = true;
    }
    var group = $("#course-group").val();
    var type = $("#course-type").val();
    var order = $("#order").val();
    if (order < 0) {
        alert("Order must be 0 and more");
        var name = $("#order").val(0);
        return;
    }
    var description = $("#description").val();
    $.ajax({
        type: 'POST',
        url: "/Courses/Create?name=" + name + "&isNew=" + is_new + "&groupId=" + group + "&typeId=" + type + "&order=" + order + "&description=" + description,
        dataType: "json",
        success: function (data) {
            location.href = data.redirectUrl;
        }
    })
});
$(".courses-list-item-delete").on('click', function (e) {
    var searchId = $(this).attr("id");
    $.ajax({
        type: 'POST',
        url: "/Courses/Delete?Id=" + searchId,
        dataType: "json",
        success: function (data) {
            location.href = data.redirectUrl;
        }
    });
});
$("#update-course-button").on('click', function (e) {
    var courseId = $(this).attr("id-course");
    var name = $("#name").val();
    if (name.length == 0 || name.length > 64) {
        alert("Name must be of 1...64 characters");
        var name = $("#name").val("");
        return;
    }
    var is_new = false;
    if ($('#is-new').is(':checked')) {
        is_new = true;
    }
    var group = $("#course-group").val();
    var type = $("#course-type").val();
    var order = $("#order").val();
    if (order < 0) {
        alert("Order must be 0 and more");
        var name = $("#order").val(0);
        return;
    }
    var description = $("#description").val();
    $.ajax({
        type: 'POST',
        url: "/Courses/Update?id=" + courseId + "&name=" + name + "&isNew=" + is_new + "&groupId=" + group + "&typeId=" + type + "&order=" + order + "&description=" + description,
        dataType: "json",
        success: function (data) {
            location.href = data.redirectUrl;
        }
    })
});
$(".trainer-item-delete").on('click', function (e) {
    var trainerId = $(this).attr("id");
    var courseId = $(".page-name").attr("id");
    $.ajax({
        type: 'POST',
        url: "/Courses/DeleteCourseTrainer?courseId=" + courseId + "&trainerId=" + trainerId,
        dataType: "json",
        success: function (data) {
            location.href = data.redirectUrl;
        }
    });
});
$("#add-course-trainer").on('click', function (e) {
    console.log("Click add");
    var trainerId = $("#trainers-select").val();
    var courseId = $(".page-name").attr("id");
    $.ajax({
        type: 'POST',
        url: "/Courses/AddCourseTrainer?courseId=" + courseId + "&trainerId=" + trainerId,
        dataType: "json",
        success: function (data) {
            location.href = data.redirectUrl;
        }
    });
});




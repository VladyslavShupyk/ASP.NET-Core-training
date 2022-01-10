$("#create-trainer-button").on('click', function (e) {
    var userId = $("#username").val();
    var groupId = $("#trainer-group").val();
    var order = $("#order").val();
    if (order < 0) {
        alert("Order must be 0 and more");
        var name = $("#order").val(0);
        return;
    }
    var description = $("#description").val();
    if (description.length > 4000) {
        alert("Description can be less or equal 4000 symbols.")
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Trainers/Create?userId=" + userId + "&groupId=" + groupId + "&order=" + order + "&description=" + description,
        dataType: "json",
        success: function (data) {
            location.href = data.redirectUrl;
        }
    })
});
$("#update-trainer-button").on('click', function (e) {
    var trainerId = $(this).attr("trainer-id");
    var groupId = $("#trainer-group").val();
    var order = $("#order").val();
    if (order < 0) {
        alert("Order must be 0 and more");
        var name = $("#order").val(0);
        return;
    }
    var description = $("#description").val();
    if (description.length > 4000) {
        alert("Description can be less or equal 4000 symbols.")
    }
    $.ajax({
        type: 'POST',
        url: "/Trainers/Update?trainerId=" + trainerId + "&groupId=" + groupId + "&order=" + order + "&description=" + description,
        dataType: "json",
        success: function (data) {
            location.href = data.redirectUrl;
        }
    })
});
$(".trainer-list-item-delete").on('click', function (e) {
    var trainerId = $(this).attr("id");
    $.ajax({
        type: 'POST',
        url: "/Trainers/Delete?id=" + trainerId,
        dataType: "json",
        success: function (data) {
            location.href = data.redirectUrl;
        }
    });
});
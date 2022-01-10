$("#save-trainer-group-button").on('click', function (e) {
    console.log("click save");
    var groupName = $("#group-name").val();
    if (groupName.length == 0 || groupName.length > 64) {
        alert("Group name must have 1 ... 64 characters!");
        $("#group-name").val("");
        return;
    }
    var groupOrder = $("#group-order").val();
    if (groupOrder < 0) {
        alert("Order must be 0 and more!");
        $("#group-order").val(0);
        return;
    }
    var groupDescription = $("#group-description").val();
    if (groupDescription.length > 256) {
        alert("Description can be less or equal 256 symbols!")
        $("#group-description").val("");
        return;
    }
    $("#group-name").val("");
    $("#group-order").val(0);
    $("#group-description").val("");
    $.ajax({
        type: 'POST',
        url: "/Trainers/CreateTrainerGroup?name=" + groupName + "&order=" + groupOrder + "&description=" + groupDescription,
        dataType: "json",
        success: function (response) {
            if (response.status == "Group with this name already created.") {
                $('#createTrainerGroupModal').modal('hide');
                $('#error-modal-body-text').text(response.status);
                $('#errorTrainerGroupModal').modal('show');
            }
            else {
                $('#createTrainerGroupModal').modal('hide');
                $("#course-group-list").empty();
                var htmlString = $("#course-group-list").html();
                for (var i = 0; i < response.trainerGroups.length; i++) {
                    var descString = response.trainerGroups[i].description;
                    if (descString == null)
                        descString = "";
                    htmlString += "<tr><td><p style='margin:5px;'>" + response.trainerGroups[i].name + "</p></td><td><p style='margin:5px;'>" + descString + "</p></td><td><p style='margin:5px;'>" + response.trainerGroups[i].visualOrder + "</p></td><td><div style='display:inline-block;'><button type='button' id='" + response.trainerGroups[i].id + "' class='btn course-group-item-edit courses-event-item' onclick='EditTrainerGroup(this)'></button></div><div style='display:inline-block;margin-left:5px;'><button type='button' id='" + response.trainerGroups[i].id + "' class='btn course-group-item-delete courses-event-item' onclick='DeleteTrainerGroup(this)'></button></div></td></tr>";
                }
                $("#course-group-list").html(htmlString);
            }
        }
    })
});
function DeleteTrainerGroup(el) {
    el.value = el.getAttribute('id');
    var groupId = el.value;
    $.ajax({
        type: 'POST',
        url: "/Trainers/DeleteTrainerGroup?id=" + groupId,
        dataType: "json",
        success: function (response) {
            if (response.status == "Group have dependent items.") {
                $('#error-modal-body-text').text(response.status);
                $('#errorTrainerGroupModal').modal('show');
            }
            else {
                $("#course-group-list").empty();
                var htmlString = $("#course-group-list").html();
                for (var i = 0; i < response.trainerGroups.length; i++) {
                    var descString = response.trainerGroups[i].description;
                    if (descString == null)
                        descString = "";
                    htmlString += "<tr><td><p style='margin:5px;'>" + response.trainerGroups[i].name + "</p></td><td><p style='margin:5px;'>" + descString + "</p></td><td><p style='margin:5px;'>" + response.trainerGroups[i].visualOrder + "</p></td><td><div style='display:inline-block;'><button type='button' id='" + response.trainerGroups[i].id + "' class='btn course-group-item-edit courses-event-item' onclick='EditTrainerGroup(this)'></button></div><div style='display:inline-block;margin-left:5px;'><button type='button' id='" + response.trainerGroups[i].id + "' class='btn course-group-item-delete courses-event-item' onclick='DeleteTrainerGroup(this)'></button></div></td></tr>";
                }
                $("#course-group-list").html(htmlString);
            }
        }
    })
}
function EditTrainerGroup(el) {
    el.value = el.getAttribute('id');
    var groupId = el.value;
    $.ajax({
        type: 'POST',
        url: "/Trainers/EditTrainerGroup?id=" + groupId,
        dataType: "json",
        success: function (response) {
            $("#edit-group-name").val(response.name);
            $("#edit-group-order").val(response.visualOrder);
            $("#edit-group-description").text(response.description);
            $("#save-trainer-group-changes").attr("group-id", response.id);
            $('#editTrainerGroupModal').modal('show');
        }
    })
}
function SaveTrainerChanges() {
    var groupId = $("#save-trainer-group-changes").attr("group-id");
    var name = $("#edit-group-name").val();
    if (name.length > 64 || name.length == 0) {
        alert("Group name must have 1 ... 64 characters!");
        $("#edit-group-name").val("");
        return;
    }
    var order = $("#edit-group-order").val();
    if (order < 0) {
        alert("Order must be 0 and more!");
        $("#edit-group-order").val(0);
        return;
    }
    var desc = $("#edit-group-description").val();
    if (desc.length > 256) {
        alert("Description can be less or equal 256 symbols!");
        $("#edit-group-description").val("");
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Trainers/UpdateTrainerGroup?id=" + groupId + "&name=" + name + "&order=" + order + "&description=" + desc,
        dataType: "json",
        success: function (response) {
            $('#editTrainerGroupModal').modal('hide');
            if (response.status == "Some group already have this name.") {
                $('#error-modal-body-text').text(response.status);
                $('#errorTrainerGroupModal').modal('show');
            }
            else {
                $("#course-group-list").empty();
                var htmlString = $("#course-group-list").html();
                for (var i = 0; i < response.trainerGroups.length; i++) {
                    var descString = response.trainerGroups[i].description;
                    if (descString == null)
                        descString = "";
                    htmlString += "<tr><td><p style='margin:5px;'>" + response.trainerGroups[i].name + "</p></td><td><p style='margin:5px;'>" + descString + "</p></td><td><p style='margin:5px;'>" + response.trainerGroups[i].visualOrder + "</p></td><td><div style='display:inline-block;'><button type='button' id='" + response.trainerGroups[i].id + "' class='btn course-group-item-edit courses-event-item' onclick='EditTrainerGroup(this)'></button></div><div style='display:inline-block;margin-left:5px;'><button type='button' id='" + response.trainerGroups[i].id + "' class='btn course-group-item-delete courses-event-item' onclick='DeleteTrainerGroup(this)'></button></div></td></tr>";
                }
                $("#course-group-list").html(htmlString);
            }
        }
    })
}
var originText = "";
$('.user-name').hover(function () {
    originText = $(this).text();
    $(this).text('Log off');
},
    function () {
        $(this).text(originText);
    }
);
function AddRole(el){
    el.value = el.getAttribute('id');
    var userId = el.value;
    var roleId = $("#roles-select").val();
    $.ajax({
        type: 'POST',
        url: "/User/AddRole?id=" + userId + "&roleId=" + roleId,
        dataType: "json",
        success: function (response) {
            console.log(response);
            $("#user-roles-table-body").empty();
            var htmlString = $("#course-group-list").html();
            for (var i = 0; i < response.userRoles.length; i++){
                htmlString += "<tr><td>" + response.userRoles[i].name + "</td ><td><button type='button' id='" + response.userRoles[i].id +"' class='btn user-role-item-delete user-event-item' onclick='DeleteRole(this)'></button></td></tr>";
            }
            $("#user-roles-table-body").html(htmlString);
            $("#roles-select").empty();
            htmlString = $("#roles-select").html();
            for (var i = 0; i < response.roles.length; i++) {
                    htmlString += "<option value='" + response.roles[i].id + "'>" + response.roles[i].name + "</option>";
            }
            $("#roles-select").html(htmlString);
        }
    })
}
function DeleteRole(el) {
    el.value = el.getAttribute('id');
    var roleId = el.value;
    var userId = $(".add-role").attr("id");
    $.ajax({
        type: 'POST',
        url: "/User/DeleteRole?id=" + userId + "&roleId=" + roleId,
        dataType: "json",
        success: function (response) {
            $("#user-roles-table-body").empty();
            var htmlString = $("#course-group-list").html();
            for (var i = 0; i < response.userRoles.length; i++) {
                htmlString += "<tr><td>" + response.userRoles[i].name + "</td ><td><button type='button' id='" + response.userRoles[i].id + "' class='btn user-role-item-delete user-event-item' onclick='DeleteRole(this)'></button></td></tr>";
            }
            $("#user-roles-table-body").html(htmlString);
            $("#roles-select").empty();
            htmlString = $("#roles-select").html();
            for (var i = 0; i < response.roles.length; i++) {
                htmlString += "<option value='" + response.roles[i].id + "'>" + response.roles[i].name + "</option>";
            }
            $("#roles-select").html(htmlString);
        }
    })
}
$(".password-change-button").click(function () {
    var password = $("#new-password").val();
    if (password.length == 0) {
        alert("Password can't be less then 1 character!");
        $("#new-password").val("");
        $("#repeat-password").val("");
        return;
    }
    var repeatPassword = $("#repeat-password").val();
    if (password != repeatPassword) {
        alert("Password not equals with repeat password!");
        return;
    }
    var userId = $(".add-role").attr("id");
    $.ajax({
        type: 'POST',
        url: "/User/ChangePassword?id=" + userId + "&newPassword=" + password,
        dataType: "json",
        success: function (response) {
            if (response.response == "Success") {
                $("#new-password").val("");
                $("#repeat-password").val("");
                alert("Password change successfuly.");
            }
        }
    })
})
function SearchUser() {
    var searchString = $("#search-user-name").val();
    location.href = "/admin/users/page/" + 1 + "/" + searchString;
}
var originText = "";
$('.user-name').hover( function () {
        originText = $(this).text();
        $(this).text('Log off');
    },
    function () {
        $(this).text(originText);
    }
);
function SelectChanged() {
    var typeId = $(".select-types").val();
    var groupId = $(".select-groups").val();
    if (typeId != "all" && groupId != "all") {
        location.href = "/courses/page/" + 1 + "/" + groupId + "/" + typeId;
    }
    else if (typeId != "all" && groupId == "all") {
        location.href = "/courses/page/" + 1 + "/" + null + "/" + typeId;
    }
    else if (typeId == "all" && groupId != "all") {
        location.href = "/courses/page/" + 1 + "/" + groupId;
    }
    else {
        location.href = "/courses/page/" + 1;
    }
}
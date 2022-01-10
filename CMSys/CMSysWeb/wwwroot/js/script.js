$(".trainer-info").on('click', function (e) {
    var searchId = $(this).attr("id");
    $.ajax({
        type: 'POST',
        url: "/Trainers/SearchTrainer?Id=" + searchId,
        dataType: "json",
        success: function (response) {
            $("#trainerInformationModalLabel").text(response.user.fullName);
            $(".trainer-modal-image").css("background", "url(data:image/jpeg;base64," + response.user.photo);
            $('.trainer-modal-description').text(response.description);
        }
    });
});
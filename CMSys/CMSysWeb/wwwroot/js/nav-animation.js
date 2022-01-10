$(".nav-arrow").click(function () {
    $(".left-plate").css("visibility", "hidden");
    $("#right-arrow").css("visibility", "visible");
    $(".left-nav").animate({ width: "45px" }, 100);
});
$("#right-arrow").click(function () {
    $("#right-arrow").css("visibility", "hidden");
    $(".left-nav").animate({ width: "250px" }, 100, function () { $(".left-plate").css("visibility", "visible");});
});

var originText = "";
$('.user-name').hover(function () {
    originText = $(this).text();
    $(this).text('Log off');
},
    function () {
        $(this).text(originText);
    }
);
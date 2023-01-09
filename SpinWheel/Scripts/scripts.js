AOS.init({
    once: true,
});


$("#contact-form").on("submit", function (e) {
    e.preventDefault();
    $.post("/Home/Contact", $(this).serialize(), function (data) {
        if (data.status) {
            $.toast({
                heading: 'Liên hệ thành công',
                text: data.msg,
                icon: 'success'
            })
            $("#contact-form").trigger("reset");
        } else {
            $.toast({
                heading: 'Liên hệ không thành công',
                text: data.msg,
                icon: 'error'
            })
        }
    });
});

$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('.header').css('box-shadow', '0 5px 10px rgba(0, 0, 0, 0.2)');
        $('.header-mobile').css('box-shadow', '0 5px 10px rgba(0, 0, 0, 0.2)');
    }
    else {
        $('.header').css('box-shadow', 'none');
        $('.header-mobile').css('box-shadow', 'none');
    }
    if ($(this).scrollTop() > 200) {
        $('.btn-scroll').fadeIn(200);
    } else {
        $('.btn-scroll').fadeOut(200);
    }
});

$('.btn-scroll').click(function (event) {
    event.preventDefault();
    $('html, body').animate({ scrollTop: 0 }, 300);
});

$('.hamburger').click(function() {
    $(this).toggleClass('active');
    $('.header').toggleClass('active');
});

var gifts = [];
var url = $("#Event_Url").val();
var awardSort;
var awardName;
var awardQuantity;
var awardImage;
var awardTW;
var awardId;
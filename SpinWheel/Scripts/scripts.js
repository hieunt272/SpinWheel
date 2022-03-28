var gifts = [];

$.getJSON("/Home/GetAwardData", function (data) {
    $.each(data, function (key, val) {
        console.log(data)
        var percent = winwheelPercentToDegrees(val.Percent);
        if (percent === 0) {
            theWheel.addSegment({
                'text': val.AwardName,
                'fillStyle': val.BgColor,
                'textFillStyle': val.TextColor,
            }, 1)
        }
        else {
            theWheel.addSegment({
                'text': val.AwardName,
                'fillStyle': val.BgColor,
                'textFillStyle': val.TextColor,
                'size': percent
            }, 1)
        }
    });
});

function ChangeBackground(x) {
        $.getJSON("/Home/GetEventData", function (data) {
            $.each(data, function (key, val) {
                if (x.matches) {
                    $(".wrapper").css("background", "url(images/events/" + val.BgMobile + ") no-repeat top center / cover");
                }
                else {
                    $(".wrapper").css("background", "url(images/events/" + val.BgPC + ") no-repeat top center / cover");
                }
            });
        });
    }

var x = window.matchMedia("(max-width: 840px)");
ChangeBackground(x);
x.addListener(ChangeBackground);

const theWheel = new Winwheel({
    numSegments: gifts.length,
    segments: gifts,
    outerRadius: 250,
    textFontSize: 18,
    responsive: true,
    animation: {
        type: "spinToStop",
        duration: 5,
        spins: 15,
        //callbackSound: playSound,
        callbackFinished: alertPrize,
    }
});

//let ding = new Audio("./content/sound/applause.mp3");
//let applause = new Audio("./content/sound/ding.mp3");

//function playSound() {
//    ding.pause();
//    ding.currentTime = 0;
//    ding.play();
//}

let wheelSpinning = false;

function StatusButton(status) {
    if (status == 1) {
        document
            .querySelector(".inner-spin")
            .removeAttribute("disabled");
    } else if (status == 2) {
        document
            .querySelector(".inner-spin")
            .setAttribute("disabled", false);
    } else if (status == 3) {
        document
            .querySelector(".inner-spin")
            .removeAttribute("disabled");

        theWheel.stopAnimation(false);
        theWheel.rotationAngle = 0;
        theWheel.draw();

        wheelSpinning = false;
    }
}
StatusButton(1);

function infoConfirm() {
    var fullName = $("[name=fullname]").val();
    var phone = $("[name=phone]").val();
    if (fullName == "" || phone == "") {
        Swal.fire({
            icon: "warning",
            text: "Vui lòng nhập đủ thông tin",
        });
    } else {
        $("#Modal").removeClass("show");
        $("#Modal").css("display", "none");
        $(".modal-backdrop").remove();
        $(".card").addClass("active");
        $("#fullname").text(fullName);
        $("#phone").text(phone);
    }
}

function startSpin() {
    var fullName = $("[name=fullname]").val();
    var phone = $("[name=phone]").val();
    if (fullName != "" && phone != "") {
        //$.getJSON("/Home/GetClientData/", function (data) {
        //    $.each(data, function (key, value) {
        //        console.log(value.CreateDate);
        //        if (phone == value.Mobile) {
        //            Swal.fire({
        //                icon: 'error',
        //                text: 'Số lượt quay trong ngày đã hết!!'
        //            });
        //        } else if (wheelSpinning == false) {
        //            StatusButton(1);
        //            theWheel.startAnimation();
        //            wheelSpinning = true;
        //            StatusButton(2);
        //        }
        //    });
        //});
        if (wheelSpinning == false) {
            StatusButton(1);
            theWheel.startAnimation();
            wheelSpinning = true;
            StatusButton(2);
        }
    }
}

function alertPrize() {
    //applause.play();
    let winningSegment = theWheel.getIndicatedSegment();
    Swal.fire({
        title: "Chúc mừng",
        text: "Bạn đã trúng: " + winningSegment.text,
        icon: "success",
    });
    var fullName = $("[name=fullname]").val();
    var phone = $("[name=phone]").val();
    $.post("/Home/InfoForm", { phone: phone, fullName: fullName, prize: winningSegment.text }, function (data) {
    })
    $("#info-form").trigger("reset");
    StatusButton(3);
}
var gifts = [];

var url = $("#Event_Url").val();
$.getJSON("/Home/GetAwardData", { url: url }, function (data) {
    $.each(data, function (key, val) {
        var percent = winwheelPercentToDegrees(val.percent);
        if (percent === 0) {
            theWheel.addSegment({
                'text': val.name,
                'fillStyle': val.bgColor,
                'textFillStyle': val.txtColor,
                'quantity': val.quantity,
                'totalwin': val.totalWin,
            }, 1);
        }
        else {
            theWheel.addSegment({
                'text': val.name,
                'fillStyle': val.bgColor,
                'textFillStyle': val.txtColor,
                'size': percent,
                'quantity': val.quantity,
                'totalwin': val.totalWin,
            }, 1);
        }
    });
});

function ChangeBackground(x) {
    $.getJSON("/Home/GetAwardData", { url: url }, function (data) {
        $.each(data, function (key, val) {
            if (x.matches) {
                $(".wrapper").css("background", "url(images/events/" + val.bgMobile + ") no-repeat top center / cover");
            }
            else {
                $(".wrapper").css("background", "url(images/events/" + val.bgPc + ") no-repeat top center / cover");
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
    var phone = $("[name=phone]").val();
    var isPost = true;
    if (phone == "") {
        isPost = false;
    }
    if (isPost) {
        $.getJSON("/Home/GetClientData", { phone: phone }, function (data) {
            if (data == 1) {
                if (wheelSpinning == false) {
                    StatusButton(1);
                    theWheel.startAnimation();
                    wheelSpinning = true;
                    StatusButton(2);
                }
            }
            else {
                Swal.fire({
                    icon: 'error',
                    text: 'Số lượt quay trong ngày đã hết!!'
                });
            }
        });
    }
}

function alertPrize() {
    //applause.play();
    let winningSegment = theWheel.getIndicatedSegment();
    const quantity = parseInt(winningSegment.quantity);
    const totalWin = parseInt(winningSegment.totalwin);
    if (quantity <= totalWin) {
        Swal.fire({
            title: "Số lượng giải <span>" + winningSegment.text + "</span> đã hết",
            text: "Bạn có 1 lượt quay mới",
            icon: "warning",
            confirmButtonText: "Xác nhận",
            confirmButtonColor: "#218838",
        }).then((result) => {
            StatusButton(3);
        });
    }
    else {
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
}
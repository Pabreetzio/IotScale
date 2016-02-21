var scale = {}
scale.offset = 0;
scale.lastRead = 0;
scale.read = function () {
    scale.lastRead = Math.floor((Math.random() * 10) + 1500);
    $("#result").text(scale.lastRead - scale.offset + "kg");
}
scale.tare = function () {
    scale.offset = scale.lastRead;
    $("#result").text(scale.lastRead - scale.offset + "kg");
}
scale.calibrate = function (currentWeight) {
    
}
$(document).ready(function () {
    $('#button-read').on('click', function () {
        scale.read();
    });
    $('#button-tare').on('click', function () {
        scale.tare();
    });
    $('#button-calibrate').on('click', function () {
        scale.calibrate($('#input-calibrate').val());
    });
});
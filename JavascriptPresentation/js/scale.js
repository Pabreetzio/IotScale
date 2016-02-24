var scale = {}
scale.offset = 0;
scale.lastRead = 0;
scale.component = new Components.Scale();
scale.read = function () {
    scale.lastRead = scale.component.getReading();
    $("#result").text(scale.lastRead + " kg");
}
scale.tare = function () {
    scale.component.tare();
    scale.lastRead = 0;
    $("#result").text(scale.lastRead + " kg");
}
scale.calibrate = function (currentWeight) {
    scale.component.calibrate(currentWeight);
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
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
scale.calibrate = function () {
    var currentWeight = $('#input-calibrate').val()
    scale.component.calibrate(currentWeight);
}
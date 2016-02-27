var Scale = (function(){
    var self = {};
    var lastRead = 0;
    var component = new Components.Scale();
    self.read = WinJS.UI.eventHandler(function () {
        lastRead = component.getReading();
        $("#result").text(lastRead.toPrecision(5) + " g");
    });
    self.tare = WinJS.UI.eventHandler(function () {
        component.tare();
        lastRead = 0;
        $("#result").text(lastRead.toPrecision(5) + " g");
    });
    self.calibrate = WinJS.UI.eventHandler(function () {
        var currentWeight = $('#input-calibrate input').val();
        currentWeight = currentWeight || 1;
        component.calibrate(currentWeight);
    });
    return self;
})();

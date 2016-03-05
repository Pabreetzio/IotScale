# Iot Scale
An Windows 10 Universal library to interface the Avia Semiconductor HX711 24-Bit Analog-to-Digital Converter (ADC) for Weight Scales.

Pretty much any cheap old digital scale can be connected to the internet of things using this digital scale application.

The key to reading weight from the scale with the Raspberry Pi is the load cell or collection of load cells. When you step on a scale your weight causes a change in resistance in the load cell that can be measured with the right analog to digital converter.

It may be possible to hack some scales in such a way as to use their built in electronics, but I find that it's better to bi-pass their circuitry altogether and use a chip that is well documented. The chip I've chosen to use is the [Avia Semiconductor HX711](https://cdn.sparkfun.com/datasheets/Sensors/ForceFlex/hx711_english.pdf). You can find these with the breakout board for just over a buck each if you shop around.

The load cell should have 4 wires coming off of it. Remove the load cell wires from the scale's built in electronics and extend them so you can work with them. I extended mine by connecting some jumper wires that easily fit into a breadboard. You may have to play around with plugging in different combinations of these wires once you have the rest of your setup finished if the colors are not standard. The typical color scheme is as follows:

* Red: Excitation (E)+
* Black: Excitation-
* White: Signal +
* Green: Signal -
* Yellow: (Shield)

For my tests I've been using a small gram scale that can easily fit in my pocket. My yellow wire was E- and blue was signal-. Your results may vary as well. 

Once you have the wires extended re-assemble the scale but with the wires hanging out where you can access them. 

Here is what my setup looks like.
![Patrick Graham's Iot Scale](http://graham.tech/content/images/2016/03/20160229_101241-1.jpg)
Connect the Raspberry Pi to the HX711 using 3.3v to vcc, Raspberry Pi pin 23 to the clock pin that is labeled CLK on the HX711 board, pin 24 to data(DT) and ground to ground. 

Next you'll want to get the code from the [IoT Scale](https://github.com/Pabreetzio/IotScale) github repository. Load the application onto the Raspberry Pi and viola, you're reading weight. 

I'm still laying the groundwork for my [bee hive scale](http://graham.tech/hive-scale) but in the mean time I've got a great simple WinJS interface for putting a digital scale on the Raspberry Pi.

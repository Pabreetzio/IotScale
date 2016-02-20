using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AviaSemiconductor;
using Windows.Devices.Gpio;

namespace Scale
{
    public sealed class Scale
    {
        private int offset;
        private double calibrationConstant;
        private HX711 device; 
        public Scale()
        {
            GpioController controller = GpioController.GetDefault();
            device = new HX711();
            device.PowerDownAndSerialClockInput = controller.OpenPin(14);
            device.SerialDataOutput = controller.OpenPin(15);
            offset = 0;
            calibrationConstant = 1;
        }
        public double GetReading()
        {
            return (device.Read() - offset) * calibrationConstant;
        }
        public void Tare()
        {
            offset = device.Read();
        }
        public void Calibrate(int grams)
        {
            calibrationConstant = (device.Read() - offset) / grams;
        }
        
    }
}

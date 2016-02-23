using AviaSemiconductor;
using Windows.Devices.Gpio;

namespace Components
{
    public sealed class Scale
    {
        private int offset;
        private double calibrationConstant;
        private HX711 device; 
        GpioPin dataPin;
        GpioPin clockPin;
        public Scale()
        {
            offset = 0;
            calibrationConstant = 1;
        }
        private void initializeDevice()
        {
            if(device== null)
            {
                GpioController controller = GpioController.GetDefault();
                GpioOpenStatus status;
                if (controller != null
                    && controller.TryOpenPin(23, GpioSharingMode.Exclusive, out clockPin, out status)
                    && controller.TryOpenPin(24, GpioSharingMode.Exclusive, out dataPin, out status))
                {
                    clockPin.SetDriveMode(GpioPinDriveMode.Output);
                    dataPin.SetDriveMode(GpioPinDriveMode.Input);
                    device = new HX711(clockPin, dataPin);
                    device.PowerOn();
                }
                else device = null;
            }
        }
        private void releaseDevice()
        {
            GpioController controller = GpioController.GetDefault();
            device.PowerDown();
            dataPin.Dispose();
            clockPin.Dispose();
        }
        int BoolArrayToInt(bool[] bits)
        {
            if (bits.Length > 32) throw new System.Exception("Can only fit 32 bits in a int");

            int r = 0;
            for (int i = 0; i < bits.Length; i++) if (bits[i]) r |= 1 << (bits.Length - i);
            return r;
        }
        public double GetReading()
        {
            initializeDevice();
            double result = 1337;
            if (device != null)
            {
                result = (device.Read() - offset) * calibrationConstant;
            }
            releaseDevice();
            return result;
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

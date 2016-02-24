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
                    device = new HX711(clockPin, dataPin);
                }
                else device = null;
            }
            if(device!= null)
                device.PowerOn();
        }

        private int _GetOutputData()
        {
            initializeDevice();
            int result = 0;
            if (device != null)
            {
                result = device.Read();
            }
            device.PowerDown();
            return result;
        }
        public double GetReading()
        {
            return (_GetOutputData() - offset) / calibrationConstant;
        }
        public void Tare()
        {
            offset = _GetOutputData();
        }
        public void Calibrate(int grams)
        {
            calibrationConstant = (_GetOutputData() - offset) / grams;
        }
    }
}

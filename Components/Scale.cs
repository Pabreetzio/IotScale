using AviaSemiconductor;
using Windows.Devices.Gpio;

namespace Components
{
    public sealed class Scale
    {
        private int offset;
        private double calibrationConstant;
        private HX711 device; 
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
                GpioPin dt;
                GpioPin sck;
                if (controller != null
                    && controller.TryOpenPin(23, GpioSharingMode.Exclusive, out sck, out status)
                    && controller.TryOpenPin(24, GpioSharingMode.Exclusive, out dt, out status))
                {
                    device = new HX711(sck, dt);
                    device.PowerOn();
                }
                else device = null;
            }
        }
        public double GetReading()
        {
            initializeDevice();
            if (device != null)
                return (device.Read() - offset) * calibrationConstant;
            else return 1337;
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

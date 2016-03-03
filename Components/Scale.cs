using AviaSemiconductor;
using Windows.Devices.Gpio;
using Windows.Storage;
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
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            offset = System.Convert.ToInt32(localSettings.Values["offset"]);
            calibrationConstant = System.Convert.ToDouble(localSettings.Values["calibrationConstant"]);
            if (calibrationConstant == 0) calibrationConstant = 1;
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
        public string GetLeadingUnit()
        {
            return (string)ApplicationData.Current.LocalSettings.Values["leadingUnit"];
        }
        public string GetTrailingUnit()
        {
            return (string)ApplicationData.Current.LocalSettings.Values["trailingUnit"];
        }
        public void Calibrate(int grams, string trailingUnit, string leadingUnit)
        {
            if (grams == 0)
                grams = 1;
            calibrationConstant = (_GetOutputData() - offset) / grams;
            ApplicationData.Current.LocalSettings.Values["calibrationConstant"] = calibrationConstant;
            ApplicationData.Current.LocalSettings.Values["offset"] = offset;
            ApplicationData.Current.LocalSettings.Values["trailingUnit"] = trailingUnit;
            ApplicationData.Current.LocalSettings.Values["leadingUnit"] = leadingUnit;
        }
    }
}

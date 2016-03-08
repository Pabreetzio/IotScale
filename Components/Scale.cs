using AviaSemiconductor;
using System.Collections.Generic;
using Windows.Devices.Gpio;
using Windows.Storage;
namespace Components
{
    public sealed class Scale
    {
        private double calibrationConstant;
        private HX711 device; 
        GpioPin dataPin;
        GpioPin clockPin;
        ApplicationDataContainer localSettings;
        public Scale()
        {
            localSettings = ApplicationData.Current.LocalSettings;
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        }
        //should figure out how to refactor these public properties.
        //these prevent multiple scales from the same application...
        //should store the values where they're easier to read from source control
        #region public properties
        private int getIntSetting(string key, int defaultValue)
        {
            if (localSettings.Values[key] == null)
            {
                localSettings.Values[key] = defaultValue;
            }
            return System.Convert.ToInt32(localSettings.Values[key]);
        }

        public int ClockPinNumber
        {
            get
            {
                return getIntSetting("clockPinNumber", 23);
            }
            set
            {
                localSettings.Values["clockPinNumber"] = value;
            }
        }

        public int DataPinNumber
        {
            get
            {
                return getIntSetting("DataPinNumber", 24);
            }
            set
            {
                localSettings.Values["DataPinNumber"] = value;
            }
        }

        public double CalibrationConstant {
            get
            {
                string key = "CalibrationConstant";
                double defaultValue = 1;
                if (localSettings.Values[key] == null)
                {
                    localSettings.Values[key] = defaultValue;
                }
                return System.Convert.ToDouble(localSettings.Values[key]);
            }
            set
            {
                localSettings.Values["CalibrationConstant"] = value;
            }
        }

        public int Offset
        {
            get
            {
                return getIntSetting("Offset", 0);
            }
            set
            {
                localSettings.Values["Offset"] = value;
            }
        }

        public string LeadingUnit
        {
            get
            {
               return (string)localSettings.Values["LeadingUnit"] ?? "";
            }
            set
            {
                localSettings.Values["LeadingUnit"] = value;
            }
        }

        public string TrailingUnit
        {
            get
            {
                return (string)localSettings.Values["TrailingUnit"] ?? "";
            }
            set
            {
                localSettings.Values["TrailingUnit"] = value;
            }
        }

        public int Precision
        {
            get
            {
                return getIntSetting("Precision", 5);
            }
            set
            {
                localSettings.Values["Precision"] = value;
            }
        }

        #endregion

        private void initializeDevice()
        {
            if(device== null)
            {
                GpioController controller = GpioController.GetDefault();
                GpioOpenStatus status;
                if (controller != null
                    && controller.TryOpenPin(ClockPinNumber, GpioSharingMode.Exclusive, out clockPin, out status)
                    && controller.TryOpenPin(DataPinNumber, GpioSharingMode.Exclusive, out dataPin, out status))
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
        
        public string GetReading()
        {
            string numberFormat = "G" + Precision;
            return LeadingUnit + ((_GetOutputData() - Offset) / CalibrationConstant).ToString(numberFormat) + TrailingUnit;
        }

        public void Tare()
        {
            Offset = _GetOutputData();
        }

        public void Calibrate(int weight)
        {
            if (weight == 0)
                weight = 1;
            double calibrationConstant = (_GetOutputData() - Offset) / weight;
            CalibrationConstant = calibrationConstant;
        }
    }
}

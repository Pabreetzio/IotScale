using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Devices.Gpio;

namespace AviaSemiconductor
{
    //24-Bit Analog-to-Digital Converter (ADC) for Weigh Scales
    public class HX711
    {
        //PD_SCK
        public GpioPin PowerDownAndSerialClockInput { get; set; }

        //DOUT
        public GpioPin SerialDataOutput { get; set; }

        #region data retrieval
        //When output data is not ready for retrieval,
        //digital output pin DOUT is high.
        public bool IsReady()
        {
            return SerialDataOutput.Read() == GpioPinValue.Low;
        }
        //By applying 25~27 positive clock pulses at the
        //PD_SCK pin, data is shifted out from the DOUT
        //output pin.Each PD_SCK pulse shifts out one bit,
        //starting with the MSB bit first, until all 24 bits are
        //shifted out.
        public int Read()
        {
            byte[] rawData = new byte[] {ReadByte(), ReadByte(), ReadByte() };
            for(int pulses = 24; pulses < 24 + (int)InputAndGainSelection; pulses++)
            {
                PowerDownAndSerialClockInput.Write(GpioPinValue.High);
                PowerDownAndSerialClockInput.Write(GpioPinValue.Low);
            }
            return GetInt32FromBit24(rawData);
        }
        private static int GetInt32FromBit24(byte[] byteArray)
        {
            int result = (
                 ((0xFF & byteArray[0]) << 16) |
                 ((0xFF & byteArray[1]) << 8) |
                 (0xFF & byteArray[2])
               );
            if ((result & 0x00800000) > 0)
            {
                result = (int)((uint)result | (uint)0xFF000000);
            }
            else
            {
                result = (int)((uint)result & (uint)0x00FFFFFF);
            }
            return result;
        }
        private byte ReadByte()
        {
            byte result = new byte();
            bool[] Byte = new bool[8];
            int bitIndex = 0;
            for (int i = 0; i < Byte.Length; i++)
            {
                while (PowerDownAndSerialClockInput.Read() == GpioPinValue.Low) { }
                while (PowerDownAndSerialClockInput.Read() == GpioPinValue.High)
                {
                    Byte[i] = SerialDataOutput.Read() == GpioPinValue.High;
                }
                if (Byte[i])
                {
                    result |= (byte)(((byte)1) << bitIndex);
                }
                bitIndex++;
            }
            return result;
        }
        #endregion

        #region input selection/ gain selection
        public enum InputAndGainOption : int
        {
            A128 = 1, B32 = 2, A64 = 3
        }
        private InputAndGainOption _InputAndGainSelection = InputAndGainOption.A128;
        public InputAndGainOption InputAndGainSelection
        {
            get
            {
                return _InputAndGainSelection;
            }
            set
            {
                _InputAndGainSelection = value;
                Read();
            }
        }
        #endregion

        #region power
        //When PD_SCK pin changes from low to high
        //and stays at high for longer than 60µs, HX711
        //enters power down mode
        public void PowerDown()
        {
            PowerDownAndSerialClockInput.Write(GpioPinValue.Low);
            PowerDownAndSerialClockInput.Write(GpioPinValue.High);
            //wait 60 microseconds
        }

        //When PD_SCK returns to low,
        //chip will reset and enter normal operation mode
        public void PowerOn()
        {
            PowerDownAndSerialClockInput.Write(GpioPinValue.Low);
            _InputAndGainSelection = InputAndGainOption.A128;
        }
        //After a reset or power-down event, input
        //selection is default to Channel A with a gain of 128. 
        #endregion

    }
}

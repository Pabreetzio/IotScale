using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Components;

namespace BackgroundApplication
{
    public sealed class MqttServer : IDisposable
    {
        Dictionary<string, byte> topics;
        Scale scale;
        MqttClient client;
        public void StartServer()
        {
            client = new MqttClient("192.168.1.145");
            scale = new Scale();
            topics = new Dictionary<string, byte>();
            topics.Add("act/weigh", MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE);
            topics.Add("act/tare", MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE);

            string thisMachineId = new EasClientDeviceInformation().Id.ToString();
            client.Connect(thisMachineId);
            client.Subscribe(topics.Keys.ToArray(), topics.Values.ToArray());
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        }
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            switch(e.Topic)
            {
                case "act/weigh":
                    client.Publish("world", Encoding.UTF8.GetBytes("Hello"));
                    client.Publish("perceive/weight", Encoding.UTF8.GetBytes(scale.GetReading().ToString()));
                    break;
                case "act/tare":
                    scale.Tare();
                    break;
                case "act/calibrate":
                    scale.Calibrate(Convert.ToInt32(Encoding.UTF8.GetString(e.Message)));
                    break;
                default:
                    break;
            }
        }

        public void Dispose()
        {
            client.Disconnect();
        }
    }
}

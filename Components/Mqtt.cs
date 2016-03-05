using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace Components
{
    public sealed class Mqtt
    {
        public static void sendData(string data)
        {
            MqttClient client = new MqttClient("test.mosquitto.org");
            client.Connect(Guid.NewGuid().ToString());
            string json = "{ weight : " + data + " }";
            client.Publish("/pi2mqtt/weight", Encoding.UTF8.GetBytes(json));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace WebApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        MqttClient client;
        public ActionResult Index()
        {
            client = new MqttClient("192.168.1.145");
            var topics = new Dictionary<string, byte>();
            string thisMachineId = System.Environment.MachineName;
            client.Connect(thisMachineId);
            topics.Add("perceive/weight", MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE);
            client.Subscribe(topics.Keys.ToArray(), topics.Values.ToArray());
            return View();
        }
        public JsonResult ReadWeight()
        {
            client.Publish("act/weigh",null);
            return Json(19);
        }
    }
}

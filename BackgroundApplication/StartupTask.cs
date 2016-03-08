using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation;

namespace BackgroundApplication
{
    public sealed class StartupTask : IBackgroundTask
    {
        MqttServer _server;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            _server = new MqttServer();
            IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
                (workItem) =>
                {
//                    AppServiceConnection appServiceConnection = new AppServiceConnection();
                    _server.StartServer();
                });
            //var result = await asyncAction();
        }
    }
}

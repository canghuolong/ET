namespace ET.Client
{
    [Event(SceneType.SunMain)]
    public class AppStartInitFinish_CreateMainView : AEvent<Scene,AppStartInitFinish>
    {
        protected override async ETTask Run(Scene scene, AppStartInitFinish a)
        {
            Log.Info("AppStartInitFinish");

            var mainView = await UIHelper.OpenUI<UIMainViewComponent>(scene, "Default_UIMainView",
                UILayer.Low);
            mainView.Text.text = "Hello World!";

            // await scene.Root().GetComponent<TimerComponent>().WaitAsync(3*1000);
            // Log.Info("1");
            await ETTask.CompletedTask;
        }
    }
}


namespace ET.Client
{
    [Event(SceneType.SunMain)]
    public class AppStartInitFinish_CreateMainView : AEvent<Scene,AppStartInitFinish>
    {
        protected override async ETTask Run(Scene scene, AppStartInitFinish a)
        {
            Log.Info("AppStartInitFinish");
            await ETTask.CompletedTask;
        }
    }
}


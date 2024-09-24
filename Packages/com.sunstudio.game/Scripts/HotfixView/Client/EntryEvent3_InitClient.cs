namespace ET.Client
{
    [Event(SceneType.SunMain)]
    public class EntryEvent3_InitClient : AEvent<Scene,EntryEvent3>
    {
        protected override async ETTask Run(Scene scene, EntryEvent3 args)
        {
            scene.AddComponent<GlobalComponent>();
            scene.AddComponent<ResourcesLoaderComponent>();
            await ETTask.CompletedTask;
            await EventSystem.Instance.PublishAsync(scene, new AppStartInitFinish());
        }
    }
}


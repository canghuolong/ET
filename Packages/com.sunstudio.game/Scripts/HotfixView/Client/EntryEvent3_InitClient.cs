namespace ET.Client
{
    [Event(SceneType.SunMain)]
    [FriendOf(typeof(GlobalComponent))]
    public class EntryEvent3_InitClient : AEvent<Scene,EntryEvent3>
    {
        protected override async ETTask Run(Scene scene, EntryEvent3 args)
        {
            scene.AddComponent<TimerComponent>();
            scene.AddComponent<CoroutineLockComponent>();
            
            var globalComponent = scene.AddComponent<GlobalComponent>();
            scene.AddComponent<ResourcesLoaderComponent>();
            
            var rc = globalComponent.UI.GetComponent<ReferenceCollector>() ?? throw new System.ArgumentNullException("globalComponent.UI.GetComponent<ReferenceCollector>()");
            scene.AddComponent<UIComponent,ReferenceCollector>(rc);
            await ETTask.CompletedTask;
            await EventSystem.Instance.PublishAsync(scene, new AppStartInitFinish());
        }
    }
}


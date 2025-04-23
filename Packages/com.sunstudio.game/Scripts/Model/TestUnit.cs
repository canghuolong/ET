namespace ET
{
    
    public class TestUnit : Entity,IAwake<string>
    {
        public string Name;
        
    }
    
    [UniqueId]
    public static partial class E
    {
        public const int A = 1;
        public const int B = 2;
        public const int C = 3;
    }
}

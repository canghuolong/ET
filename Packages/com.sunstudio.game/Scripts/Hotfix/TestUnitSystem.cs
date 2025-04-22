

namespace ET
{
    [EntitySystemOf(typeof(TestUnit))]
    public static partial class TestUnitSystem
    {
        [EntitySystem]
        private static void Awake(this ET.TestUnit self, string args2)
        {
            self.Name = args2;
        }
    }
}


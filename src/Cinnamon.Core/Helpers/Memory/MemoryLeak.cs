using System.Diagnostics;

namespace Cinnamon.Core
{
    public static class MemoryLeak
    {
        public static WeakReference? Reference { get; private set; }

        public static void CheckObject(object ObjToCheck)
        {
            MemoryLeak.Reference = new WeakReference(ObjToCheck);
        }

        public static void IsItDead()
        {
            if (Reference is null)
                return;
            RunGC();
            if (MemoryLeak.Reference.IsAlive)
                Debug.WriteLine("Still here");
            else
                Debug.WriteLine("I 'm dead");
        }

        public static void RunGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}

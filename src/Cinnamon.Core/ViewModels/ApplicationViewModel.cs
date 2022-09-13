namespace Cinnamon.Core
{
    public class ApplicationViewModel
    {
        void test() {
            // Test Call for Database
            CoreDI.DataStore.Activities.GetAllAsync();
        }
    }
}

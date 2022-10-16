namespace Cinnamon.Core
{
    public class ScheduleModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DateTime { get; set; }
        public decimal Price { get; set; } = 0;
        public string PriceUnit1 { get; set; } = "Head";
        public string PriceUnit2 { get; set; } = "Session";
    }
}

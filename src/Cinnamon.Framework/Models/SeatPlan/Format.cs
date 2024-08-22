namespace Cinnamon.Framework.Models.SeatPlan;

public class Format 
{
    public string Name { get; set; }
    public IList<Category> Categories { get; set; }
}

public class Category 
{
    public string Name { get; set; }
    public string Color { get; set; }
    public IList<Row> Rows { get; set; }
}

public class Row 
{
    public string RowNumber { get; set; }
    public string Uuid { get; set; }
    public IList<Seat> Seats { get; set; }
}

public class Seat 
{
    public string SeatNumber { get; set; }
    public string Uuid { get; set; }
}
namespace Cinnamon.Framework.Models.SeatPlan;

public class Format 
{
    public string Name { get; set; }
    public IDictionary<string, Category> Categories { get; set; }
}

public class Category 
{
    public string Uuid { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public IDictionary<string, Row> Rows { get; set; }
}

public class Row 
{
    public string Uuid { get; set; }
    public string Color { get; set; }
    public string Category { get; set; }
    public string RowNumber { get; set; }
    public IDictionary<string, Seat> Seats { get; set; }
}

public class Seat 
{
    public string SeatNumber { get; set; }
    public string Uuid { get; set; }

    // for extra fields need for creation and purchase
    public bool Occupied { get; set; }
    public bool Excluded { get; set; }
}
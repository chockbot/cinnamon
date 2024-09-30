namespace Cinnamon.Framework.Models.SeatPlan.Pretix;

public class PretixFormat
{
  public string name { get; set; }
  public List<Category> categories { get; set; }
  public List<Zone> zones { get; set; }
}

public class Category
{
  public string name { get; set; }
  public string color { get; set; }
}

public class Row
{
  public string row_number { get; set; }
  public string row_number_position { get; set; }
  public List<Seat> seats { get; set; }
  public string uuid { get; set; }
}

public class Seat
{
  public string seat_number { get; set; }
  public string seat_guid { get; set; }
  public string uuid { get; set; }
  public string category { get; set; }
  public int radius { get; set; }
}

public class Zone
{
  public string name { get; set; }
  public string zone_id { get; set; }
  public string uuid { get; set; }
  public List<Row> rows { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ValidationAttributes;

public class DateAgeRangeAttribute : ValidationAttribute 
{
    public int MinAge {get; set;}
    public int MaxAge {get; set;}

    public DateAgeRangeAttribute()
    {
        // default values
        this.MinAge = 18;
        this.MaxAge = 120;
    }

    public override bool IsValid(object? value)
    {
        var date = value == null ? DateTime.Today : Convert.ToDateTime(value);
        var age = DateTime.Today.Year - date.Year;
        return !(age < 18 || age > 120);
    }
}
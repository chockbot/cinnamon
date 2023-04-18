using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ValidationAttributes;

public class CardExpirationAttribute : ValidationAttribute 
{
    public override bool IsValid(object? value)
    {
        var val = value == null ? string.Empty : value.ToString().Trim();
        if(val.Length != 5) return false;

        var splitted = val.Split("/");
        if(splitted.Length != 2) return false;

        if(!int.TryParse(splitted[0], out int month)) return false;
        if(!int.TryParse(splitted[1], out int year)) return false;

        if(month > 12) return false;

        var curYearFirstTwoDigit = DateTime.Now.Year.ToString().Substring(0,2);

        var formattedYear = year.ToString().Length == 1 ? $"0{year}" : year.ToString();
        formattedYear = $"{curYearFirstTwoDigit}{formattedYear}";

        var actualYear = int.Parse(formattedYear);

        if(actualYear < DateTime.Now.Year) return false;

        return true;
    }
}
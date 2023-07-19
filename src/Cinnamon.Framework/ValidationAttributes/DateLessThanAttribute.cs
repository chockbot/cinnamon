using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ValidationAttributes;

public class DateLessThanAttribute : ValidationAttribute
{
    private readonly string comparisonProperty;

    public bool Strict {get; set;} = true;

    public DateLessThanAttribute(string comparisonProperty)
    {
        this.comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null) 
            throw new ArgumentException("Required property to compare");

        ErrorMessage = ErrorMessageString;
        var currentValue = (DateTime)value;

        var property = validationContext.ObjectType.GetProperty(comparisonProperty);

        if (property == null)
            throw new ArgumentException("Property with this name not found");

        var comparisonValue = property.GetValue(validationContext.ObjectInstance);
        if(comparisonValue == null)
            throw new ArgumentException("Property with this name not found");
        
        var comparisonDate = (DateTime)comparisonValue;

        // remove time to compare
        if(!Strict)
        {
            currentValue = currentValue.Date;
            comparisonDate = comparisonDate.Date;
        }

        if(currentValue < comparisonDate)
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}
using System;

namespace Cinnamon.Api.Data.Repository.Entities;

public class ResendEmail : BaseEntity 
{
    public string Email {get; set;}
    public DateTime DateResend {get; set;}
}
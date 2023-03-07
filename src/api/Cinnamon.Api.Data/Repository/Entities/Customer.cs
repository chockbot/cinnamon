using System;
using System.Collections.Generic;

namespace Cinnamon.Api.Data.Repository.Entities;

public class Customer : BaseEntity
{
    public string UserId {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public DateTime Birthdate {get; set;}
    public string? About {get; set;}
    public string? ProfilePath {get; set;}
    public DateTime DateJoined {get; set;}
    public bool IsMaker {get; set;}
    public bool ExternalLogin {get; set;}
    public int IsVerifiedBadge { get; set; }
    public string? FrontIdImagePath {get; set;}
    public string? BackIdImagePath {get; set;}
    public string Handler {get; set;}

    public virtual IList<FamilyMember> FamilyMembers {get; set;}
    public virtual IList<OngoingActivity> OngoingActivities { get; set;}
}
using System;
using System.Collections.Generic;

namespace Cinnamon.Api.Data.Repository.Entities;

public class Customer : BaseEntity
{
    public string? UserId {get; set;}
    public string? FirstName {get; set;}
    public string? LastName {get; set;}
    public string Email {get; set;}
    public DateTime Birthdate {get; set;}
    public string? PhoneNumber { get; set; }
    public string? About {get; set;}
    public string? ProfilePath {get; set;}
    public DateTime DateJoined {get; set;}
    public bool IsMaker {get; set;}
    public bool ExternalLogin {get; set;}
    public int IsVerifiedBadge { get; set; }
    public DateTime IsVerifiedDate { get; set; }
    public bool IsOG { get; set; }
    public DateTime IsOGDate { get; set; }
    public bool IsOfficialPartner { get; set; }
    public DateTime IsOfficialDate { get; set; }
    public string? FrontIdImagePath {get; set;}
    public string? BackIdImagePath {get; set;}
    public string Handler {get; set;}
    public decimal TotalCredits {get; set;}
    public bool HasAcceptedTerms { get; set; }
    public string? ConnectionId { get; set; }
    public bool IsAccountBan { get; set; }
    public bool IsGuest { get; set; } = false;
    public virtual IList<FamilyMember> FamilyMembers {get; set;}
    public virtual IList<OngoingActivity> OngoingActivities { get; set;}
    public virtual CustomerPricing CustomerPricing {get; set;}
    public virtual IList<ChatMember> ChatMembers {get; set; }
    public virtual IList<ChatHistory> FromChatHistories {get; set; }
    public virtual IList<ChatHistory> ToChatHistories { get; set; }
    public virtual IList<ChatConnection> ChatConnections { get; set; }

}
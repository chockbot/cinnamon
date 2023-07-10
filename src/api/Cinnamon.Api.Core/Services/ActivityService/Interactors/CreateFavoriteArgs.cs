using Cinnamon.Framework.Interactor;
using System;
using Cinnamon.Framework.Enums;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class CreateFavoriteArgs : IInteractor
{
    public int CustomerId { get; set; }
    public int ActivityId { get; set; }
}
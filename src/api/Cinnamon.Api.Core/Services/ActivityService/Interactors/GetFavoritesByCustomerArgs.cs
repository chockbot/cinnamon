using Cinnamon.Framework.Interactor;
using System;
using Cinnamon.Framework.Enums;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class GetFavoritesByCustomerArgs : IInteractor
{
    public int CustomerId { get; set; }
}
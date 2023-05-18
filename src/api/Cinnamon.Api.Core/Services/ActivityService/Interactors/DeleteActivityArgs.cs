using Cinnamon.Framework.Interactor;
using Cinnamon.Framework.Enums;
using System;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class DeleteActivityArgs : IInteractor
{
    public int ActivityId {get; set;}
}
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IDirectStudentPayment : IGenericEntity<DirectStudentPayment>
{
    Task<AppResult<IEnumerable<DirectStudentPaymentDTO>>> GetStudentPaymentByProvider(int? ProviderId, DateTime? dateFrom);
}
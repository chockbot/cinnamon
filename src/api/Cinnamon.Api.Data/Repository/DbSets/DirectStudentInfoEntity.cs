using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DirectStudentInfoEntity : GenericEntity<DirectStudentInfo>, IDirectStudentInfo
{
    private readonly ApplicationContext applicationContext;

    public DirectStudentInfoEntity(ApplicationContext applicationContext)
        :base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<DirectStudentDTO>>> CreateDirectStudents(IEnumerable<DirectStudentDTO> directStudents)
    {
        try
        {
            foreach (var student in directStudents)
            {
                var studentInfo = new DirectStudentInfo {
                    BirthMonth = student.DirectStudentInfo.BirthMonth,
                    BirthYear = student.DirectStudentInfo.BirthYear,
                    Gender = student.DirectStudentInfo.Gender,
                    Name = student.DirectStudentInfo.Name,
                    ProviderId = student.DirectStudentInfo.ProviderId
                };

                var studentSession = new DirectStudentSession {
                    ActivityId = student.DirectStudentSession.ActivityId,
                    DirectStudentInfo = studentInfo,
                    Name = student.DirectStudentSession.Name,
                    NumberOfSessions = student.DirectStudentSession.NumberOfSessions,
                    ScheduleId = student.DirectStudentSession.ScheduleId,
                    Remarks = student.DirectStudentSession.Remarks,
                    SessionsAttended = student.DirectStudentSession.SessionsAttended,
                    Status = student.DirectStudentSession.Status,
                    StudentNo = student.DirectStudentSession.StudentNo,
                };

                var studentPayment = new DirectStudentPayment {
                    Amount = student.DirectStudentPayment.Amount,
                    DirectStudentSession = studentSession,
                };

                applicationContext.DirectStudentInfos.Add(studentInfo);
                applicationContext.DirectStudentSessions.Add(studentSession);
                applicationContext.DirectStudentPayments.Add(studentPayment);
            }

            await applicationContext.SaveChangesAsync();

            return AppResult<IEnumerable<DirectStudentDTO>>.CreateSucceeded(directStudents, "Successfully create direct students.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DirectStudentDTO>>.CreateFailed(ex, "An error occured when creating direct students.");
        }
    }
}
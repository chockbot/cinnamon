using Cinnamon.Api.Data.Extensions;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

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
                if(student.DirectStudentInfo.Id == 0)
                {
                    var studentInfo = new DirectStudentInfo {
                        BirthMonth = student.DirectStudentInfo.BirthMonth,
                        BirthYear = student.DirectStudentInfo.BirthYear,
                        Gender = student.DirectStudentInfo.Gender,
                        Name = student.DirectStudentInfo.Name,
                        ProviderId = student.DirectStudentInfo.ProviderId,
                        Id = student.DirectStudentInfo.Id,
                        Email = student.DirectStudentInfo.Email
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
                        ExpirationDateEnd = student.DirectStudentSession.ExpirationDateEnd.SetKindUtc(),
                        ExpirationDateStart = student.DirectStudentSession.ExpirationDateStart.SetKindUtc()
                    };

                    var studentPayment = new DirectStudentPayment {
                        Amount = student.DirectStudentPayment.Amount,
                        DirectStudentSession = studentSession,
                        PaymentDate = student.DirectStudentPayment.PaymentDate.SetKindUtc()
                    };

                    applicationContext.DirectStudentInfos.Add(studentInfo);
                    applicationContext.DirectStudentSessions.Add(studentSession);
                    applicationContext.DirectStudentPayments.Add(studentPayment);
                }
                else 
                {
                    var studentSession = new DirectStudentSession {
                        ActivityId = student.DirectStudentSession.ActivityId,
                        DirectStudentInfoId = student.DirectStudentInfo.Id,
                        Name = student.DirectStudentSession.Name,
                        NumberOfSessions = student.DirectStudentSession.NumberOfSessions,
                        ScheduleId = student.DirectStudentSession.ScheduleId,
                        Remarks = student.DirectStudentSession.Remarks,
                        SessionsAttended = student.DirectStudentSession.SessionsAttended,
                        Status = student.DirectStudentSession.Status,
                        StudentNo = student.DirectStudentSession.StudentNo,
                        ExpirationDateEnd = student.DirectStudentSession.ExpirationDateEnd.SetKindUtc(),
                        ExpirationDateStart = student.DirectStudentSession.ExpirationDateStart.SetKindUtc()
                    };

                    var studentPayment = new DirectStudentPayment {
                        Amount = student.DirectStudentPayment.Amount,
                        DirectStudentSession = studentSession,
                        PaymentDate = student.DirectStudentPayment.PaymentDate.SetKindUtc()
                    };

                    applicationContext.DirectStudentSessions.Add(studentSession);
                    applicationContext.DirectStudentPayments.Add(studentPayment);
                }
            }

            await applicationContext.SaveChangesAsync();

            return AppResult<IEnumerable<DirectStudentDTO>>.CreateSucceeded(directStudents, "Successfully create direct students.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DirectStudentDTO>>.CreateFailed(ex, "An error occured when creating direct students.");
        }
    }

    public async Task<AppResult<DirectStudentInfo>> UpdateDirectStudent(DirectStudentInfo? directStudent, IEnumerable<DirectStudentSession>? sessions)
    {
        try
        {
            if(directStudent is not null)
            {
                var student = await applicationContext.DirectStudentInfos
                                    .Where(s => s.Id == directStudent.Id)
                                    .FirstOrDefaultAsync();
                
                if(student is null)
                {
                    return AppResult<DirectStudentInfo>.CreateFailed(new ApplicationException("Unable to find direct student."), "Unable to find direct student.");
                }

                student.BirthMonth = directStudent.BirthMonth ?? student.BirthMonth;
                student.BirthYear = directStudent.BirthYear == 0 ? student.BirthYear : directStudent.BirthYear;
                student.Gender = directStudent.Gender ?? student.Gender;
                student.Name = directStudent.Name ?? student.Name;
                student.Email = directStudent.Email ?? student.Email;
            }

            if(sessions is not null)
            {
                var sessionsIds = sessions.Select(s => s.Id);
                var studentSessions = await applicationContext.DirectStudentSessions
                                        .Where(s => sessionsIds.Contains(s.Id))
                                        .ToListAsync();
                
                if(studentSessions is null)
                {
                    return AppResult<DirectStudentInfo>.CreateFailed(new ApplicationException("Unable to find direct student."), "Unable to find direct student.");
                }

                foreach(var sessionToUpdate in studentSessions)
                {
                    var session = sessions.FirstOrDefault(s => s.Id == sessionToUpdate.Id);

                    if(session is not null)
                    {
                        if(session.DirectStudentPayment is not null)
                        {
                            var studentPayment = await applicationContext.DirectStudentPayments
                                            .Where(p => p.DirectStudentSessionId == sessionToUpdate.Id)
                                            .FirstOrDefaultAsync();

                            if(studentPayment is null)
                            {
                                return AppResult<DirectStudentInfo>.CreateFailed(
                                    new ApplicationException("Unable to find direct student payment."), "Unable to find direct student payment.");
                            }

                            studentPayment.Amount = session.DirectStudentPayment.Amount == 0 ? studentPayment.Amount : session.DirectStudentPayment.Amount;
                            studentPayment.PaymentDate = session.DirectStudentPayment.PaymentDate == DateTime.MinValue ? studentPayment.PaymentDate : session.DirectStudentPayment.PaymentDate.SetKindUtc();
                        }

                        sessionToUpdate.ActivityId = session.ActivityId == 0 ? sessionToUpdate.ActivityId : session.ActivityId;
                        sessionToUpdate.ScheduleId = session.ScheduleId == 0 ? sessionToUpdate.ScheduleId : session.ScheduleId;
                        sessionToUpdate.Name = session.Name ?? sessionToUpdate.Name;
                        sessionToUpdate.NumberOfSessions = session.NumberOfSessions == 0 ? sessionToUpdate.NumberOfSessions : session.NumberOfSessions;
                        sessionToUpdate.SessionsAttended = session.SessionsAttended == 0 ? sessionToUpdate.SessionsAttended : session.SessionsAttended;
                        sessionToUpdate.Remarks = session.Remarks ?? sessionToUpdate.Remarks;
                        sessionToUpdate.StudentNo = session.StudentNo ?? sessionToUpdate.StudentNo;
                        
                        sessionToUpdate.ExpirationDateEnd = session.ExpirationDateEnd.SetKindUtc();
                        sessionToUpdate.ExpirationDateStart = session.ExpirationDateStart.SetKindUtc();
                    }
                }
            }

            await applicationContext.SaveChangesAsync();

            return AppResult<DirectStudentInfo>.CreateSucceeded(new DirectStudentInfo {}, "Direct student successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<DirectStudentInfo>.CreateFailed(ex, "An error occured when updating direct student.");
        }
    }

    public async Task<AppResult<DirectStudentDTO>> DirecStudentInfo(int studentId)
    {
        try
        {
            var student = await applicationContext.DirectStudentInfos.FirstAsync(s => s.Id == studentId);
            if(student is null)
            {
                return AppResult<DirectStudentDTO>.CreateFailed(new ApplicationException("Unable to locate student."), "Unable to locate student.");
            }

            var studentSession = await applicationContext.DirectStudentSessions.FirstAsync(s => s.DirectStudentInfoId == student.Id);
            if(studentSession is null)
            {
                return AppResult<DirectStudentDTO>.CreateFailed(new ApplicationException("Unable to locate student session."), "Unable to locate student session.");
            }

            var studentPayment = await applicationContext.DirectStudentPayments.FirstAsync(s => s.DirectStudentSessionId == studentSession.Id);
            if(studentPayment is null)
            {
                return AppResult<DirectStudentDTO>.CreateFailed(new ApplicationException("Unable to locate student payment."), "Unable to locate student payment.");                
            }

            var directStudentInfo = new DirectStudentDTO {
                DirectStudentInfo = new DirectStudentInfoDTO {
                    BirthMonth = student.BirthMonth,
                    BirthYear = student.BirthYear,
                    Gender = student.Gender,
                    Id = student.Id,
                    Name = student.Name,
                    ProviderId = student.ProviderId,
                    Email = student.Email
                },
                DirectStudentPayment = new DirectStudentPaymentDTO {
                    Amount = studentPayment.Amount,
                    DirectStudentSessionId = studentPayment.DirectStudentSessionId,
                    Id = studentPayment.Id,
                    PaymentDate = studentPayment.PaymentDate
                },
                DirectStudentSession = new DirectStudentSessionDTO {
                    ActivityId = studentSession.ActivityId,
                    DirectStudentInfoId = studentSession.DirectStudentInfoId,
                    Id = studentSession.Id,
                    Name = studentSession.Name,
                    NumberOfSessions = studentSession.NumberOfSessions,
                    Remarks = studentSession.Remarks,
                    ScheduleId = studentSession.ScheduleId,
                    SessionsAttended = studentSession.SessionsAttended,
                    Status = studentSession.Status,
                    StudentNo = studentSession.StudentNo
                }
            };

            return AppResult<DirectStudentDTO>.CreateSucceeded(directStudentInfo, "Successfully get direct student info.");
        }
        catch (Exception ex)
        {
            return AppResult<DirectStudentDTO>.CreateFailed(ex, "An error occured when getting direct student info.");
        }
    }
}
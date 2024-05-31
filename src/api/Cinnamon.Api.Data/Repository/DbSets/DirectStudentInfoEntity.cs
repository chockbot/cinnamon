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
                    ExpirationDateEnd = student.DirectStudentSession.ExpirationDateEnd.SetKindUtc(),
                    ExpirationDateStart = student.DirectStudentSession.ExpirationDateStart.SetKindUtc()
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

    public async Task<AppResult<DirectStudentInfo>> UpdateDirectStudent(DirectStudentInfo directStudent, 
        DirectStudentSession directStudentSession, DirectStudentPayment directStudentPayment)
    {
        try
        {
            var student = await applicationContext.DirectStudentInfos
                                    .Where(s => s.Id == directStudent.Id)
                                    .FirstOrDefaultAsync();
            
            if(student is null)
            {
                return AppResult<DirectStudentInfo>.CreateFailed(new ApplicationException("Unable to find direct student."), "Unable to find direct student.");
            }

            var studentSession = await applicationContext.DirectStudentSessions
                                    .Where(s => s.DirectStudentInfoId == student.Id)
                                    .FirstOrDefaultAsync();
            
            if(studentSession is null)
            {
                return AppResult<DirectStudentInfo>.CreateFailed(new ApplicationException("Unable to find direct student."), "Unable to find direct student.");
            }

            var studentPayment = await applicationContext.DirectStudentPayments
                                        .Where(p => p.DirectStudentSessionId == studentSession.Id)
                                        .FirstOrDefaultAsync();
            
            if(studentPayment is null)
            {
                return AppResult<DirectStudentInfo>.CreateFailed(
                    new ApplicationException("Unable to find direct student payment."), "Unable to find direct student payment.");
            }

            student.BirthMonth = directStudent.BirthMonth ?? student.BirthMonth;
            student.BirthYear = directStudent.BirthYear == 0 ? student.BirthYear : directStudent.BirthYear;
            student.Gender = directStudent.Gender ?? student.Gender;
            student.Name = directStudent.Name ?? student.Name;
            
            studentSession.ActivityId = directStudentSession.ActivityId == 0 ? studentSession.ActivityId : directStudentSession.ActivityId;
            studentSession.ScheduleId = directStudentSession.ScheduleId == 0 ? studentSession.ScheduleId : directStudentSession.ScheduleId;
            studentSession.Name = directStudent.Name ?? studentSession.Name;
            studentSession.NumberOfSessions = directStudentSession.NumberOfSessions == 0 ? studentSession.NumberOfSessions : directStudentSession.NumberOfSessions;
            studentSession.Remarks = directStudentSession.Remarks ?? studentSession.Remarks;
            studentSession.StudentNo = directStudentSession.StudentNo ?? studentSession.StudentNo;

            studentPayment.Amount = directStudentPayment.Amount == 0 ? studentPayment.Amount : directStudentPayment.Amount;

            await applicationContext.SaveChangesAsync();

            return AppResult<DirectStudentInfo>.CreateSucceeded(student, "Direct student successfully updated.");
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
                    ProviderId = student.ProviderId
                },
                DirectStudentPayment = new DirectStudentPaymentDTO {
                    Amount = studentPayment.Amount,
                    DirectStudentSessionId = studentPayment.DirectStudentSessionId,
                    Id = studentPayment.Id
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
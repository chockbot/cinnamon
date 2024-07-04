using Cinnamon.Framework.ApiCommand.ApiCore.DTO.DirectStudents;

namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.DirectStudents;

public class DirectStudentsDTO
{
    public DirectStudentInfoDTO DirectStudentInfo { get; set; }
    public DirectStudentSessionDTO DirectStudentSession { get; set; }
    public DirectStudentPaymentDTO DirectStudentPayment { get; set; }
}

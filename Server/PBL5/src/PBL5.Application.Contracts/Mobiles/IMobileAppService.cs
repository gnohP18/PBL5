using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PBL5.Mobiles
{
    public interface IMobileAppService
    {
        /// <summary>
        /// Account
        /// </summary>
        Task<IActionResult> SignInMobileAsync(string email, string password);
        Task<IActionResult> GetEmployeeMobileByEmployeeIdAsync(Guid employeeId);
        Task<IActionResult> ChangePasswordEmployeeMobileAsync(Guid employeeId, string oldPassword, string newPassword);
        Task<IActionResult> UpdateEmployeeMobileInfoAsync(EmployeeMobileDto employeeMobileDto);
        Task<IActionResult> SendEmailRestPasswordAsync(string employeeCode);

        /// <summary>
        /// TimeSheet
        /// </summary>
        Task<IActionResult> GetTimeSheetByTimeMobileAsync(Guid employeeId, DateTime? timeSheetByMonth);
        Task<IActionResult> GetTotalTimeWorkInADay(Guid id);
        Task<IActionResult> GetTotalTimeWorkInAMonth(Guid employeeId, DateTime? timeSheetByMonth);

        /// <summary>
        /// Report
        /// </summary>
        Task<IActionResult> GetHistoryReportMobileAsync(Guid employeeId);
        Task<IActionResult> ReportTimeSheetMobileAsync(CreateReportMobileDto input);
        Task<IActionResult> UpdateReportMobileAsync(UpdateReportMobileDto input);

        /// <summary>
        /// Statistic
        /// </summary>
        Task<IActionResult> StatisticByYear(Guid employeeId, int year);

        /// <summary>
        /// Notification
        /// </summary>
        Task<IActionResult> CreateDeviceIdFromAccount(Guid employeeId, string deviceId);
        Task<IActionResult> SendNotification(Guid employeeId);
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PBL5.Enum
{
    public static class Constants
    {
        public static Guid ReportTimeSheetDefault = Guid.Parse("12345678-abcd-abcd-abcd-12345678abcd");
        public static int TimeZoneVietNam = 7;
    }

    public enum Gender
    {
        [Display(Name = "Common:Gender:Male")]
        Male,
        [Display(Name = "Common:Gender:Female")]
        Female
    }

    public static class WorkStatus
    {
        public static string LATE = "LATE";
        public static string ON_TIME = "ON TIME";
        public static string NONE_WORK = "NON WORK";
    }

    public static class StandardTimeWork
    {
        public static TimeSpan BEGIN_WORK = new TimeSpan(9,0,0);
        public static TimeSpan END_WORK = new TimeSpan(18,0,0);
        public static int RELAX_TIME = 1;
    }

    public static class ReportStatus
    {
        /// <summary>
        /// Hàng đợi
        /// </summary>
        public static string PENDING = "Pending";
        

        /// <summary>
        /// Chấp nhận
        /// </summary>
        public static string ACCEPT = "Accept";

        /// <summary>
        /// Từ chối
        /// </summary>
        public static string DENY = "Deny";
    }

    public static class ReportingReason
    {
        /// <summary>
        /// Nghỉ phép
        /// </summary>
        public static string DAY_OFF= "Day Off";

        /// <summary>
        /// Check-in lỗi
        /// </summary>
        public static string FAILED_CHECK_IN = "Failed Check-in";

        /// <summary>
        /// Lỗi khác
        /// </summary>
        public static string OTHER_ERROR = "Other Error"; 
    }

    public static class Email
    {
        public static string TITLE = "Reset Password";
        public static string HEADER = "Hi employee ";
        public static string BODY = "<p>We sent you a new password<p>";
        public static string CLOSE = "Here: ";
    }

    public static class FCMKey
    {
        public static string SenderId = "270133827071";
        public static string ServerKey = "AAAAUDtw9Tk:APA91bGRWs4df1aDHBLRO922WwEcFM_SEuFdchBpDt2TNlns10ZI-8wkgfV2WggH5R1w5DcTWJz5LWdrbt6t32J9EM1HW7yRzfeV89eKV0DcdQ0DZYVp4kgdh4qWLVX9TxVZzaSndOgJ";
    }

    public static class ContentNotification
    {
        public static string TITLE = "TimeSheet";
        public static string TITLE_REPORT = "Status changes";
        public static string CONTENT_CHECK_IN = "Check-in successfully";
        public static string CONTENT_CHECK_OUT = "Check-out successfully";
        public static string CONTENT_REPORT_ACCEPT = "accepted";
        public static string CONTENT_REPORT_DENIED = "denied";
        public static string CONTENT_REPORT_PENDING = "pended";
    }
}
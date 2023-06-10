using System;
using PBL5.Employees;
using PBL5.TimeSheets;
using Volo.Abp.Domain.Entities.Auditing;

namespace PBL5.IdentificationImages
{
    public class IdentificationImage : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Ảnh
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Nhân viên
        /// </summary>
        public Guid TimeSheetId { get; set; }

        /// <summary>
        /// Là check-in
        /// </summary>
        public bool IsCheckIn { get; set; }
    }
}
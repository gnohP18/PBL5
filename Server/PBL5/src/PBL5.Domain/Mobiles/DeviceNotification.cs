using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace PBL5.Mobiles
{
    public class DeviceNotification : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Mã thiết bị
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public Guid EmployeeId { get; set; }
    }
}
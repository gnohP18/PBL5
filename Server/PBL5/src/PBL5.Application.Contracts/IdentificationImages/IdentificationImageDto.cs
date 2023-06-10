using System;
using PBL5.Employees;
using PBL5.TimeSheets;
using Volo.Abp.Application.Dtos;

namespace PBL5.IdentificationImages
{
    public class IdentificationImageDto : EntityDto<Guid>
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
using AutoMapper;
using PBL5.TimeSheets;
using PBL5.Employees;
using PBL5.Extensions;
using PBL5.Mobiles;
using System;
using PBL5.Reports;

namespace PBL5;

public class PBL5ApplicationAutoMapperProfile : Profile
{
    public PBL5ApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Employee, EmployeeDto>()
            .ForMember(des => des.GenderName,
                act => act.MapFrom(src => EnumExtensions.GetDisplayName(src.Gender)));
        CreateMap<CreateUpdateEmployeeDto, Employee>();

        CreateMap<Employee, EmployeeMobileDto>();
        CreateMap<EmployeeMobileDto, Employee>();
        CreateMap<Report, CreateReportMobileDto>();

        CreateMap<TimeSheet, TimeSheetDto>()
            .ForMember(des => des.EmployeeName, 
                act => act.MapFrom(src => src.Employee.Name))
            .ForMember(des => des.EmployeeCode, 
                act => act.MapFrom(src => src.Employee.EmployeeCode));
        
        CreateMap<TimeSheet, StatisticDto>();
        CreateMap<CreateUpdateTimeSheetDto, TimeSheet>();
        CreateMap<Report, ReportDto>()
            .ForMember(des => des.Name, 
                act => act.MapFrom(src => src.Employee.Name))
            .ForMember(des => des.EmployeeCode, 
                act => act.MapFrom(src => src.Employee.EmployeeCode));
    }
}

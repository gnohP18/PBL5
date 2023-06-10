using System;
using AutoMapper;
using PBL5.Employees;
using PBL5.Reports;
using PBL5.TimeSheets;
using PBL5.Web.Pages.Employees;
using PBL5.Web.Pages.Employees.EmployeeViewModel;
using PBL5.Web.Pages.Reports;
using PBL5.Web.Pages.Statistics;
using PBL5.Web.Pages.Statistics.StatisticViewModel;
using PBL5.Web.Pages.TimeSheets;

namespace PBL5.Web;

public class PBL5WebAutoMapperProfile : Profile
{
    public PBL5WebAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Web project.
        CreateMap<EmployeeDto, EmployeeViewModel>();
        //Create
        CreateMap<CreateEmployeeViewModel, CreateUpdateEmployeeDto>();
        CreateMap<EmployeeDto, CreateEmployeeViewModel>();

        CreateMap<UpdateEmployeeViewModel, CreateUpdateEmployeeDto>();
        CreateMap<EmployeeDto, UpdateEmployeeViewModel>();

        CreateMap<TimeSheetDto, TimeSheetViewModel>();
        CreateMap<TimeSheetDto, UpdateTimeSheetViewModel>();
        CreateMap<UpdateTimeSheetViewModel, CreateUpdateTimeSheetDto>();

        CreateMap<ReportDto, SolveReportViewModel>(); 
        CreateMap<SolveReportViewModel, UpdateReportDto>();
        CreateMap<DetailEmployeeTimeSheet, DetailEmployeeViewModel>();
    }
}

using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PBL5.EntityFrameworkCore;
using PBL5.Localization;
using PBL5.MultiTenancy;
using PBL5.Web.Menus;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using System;
using PBL5.Permissions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using Volo.Abp.Caching;
using Volo.Abp.AspNetCore.ExceptionHandling;

namespace PBL5.Web;

[DependsOn(
    typeof(PBL5HttpApiModule),
    typeof(PBL5ApplicationModule),
    typeof(PBL5EntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpTenantManagementWebModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
    )]
public class PBL5WebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(PBL5Resource),
                typeof(PBL5DomainModule).Assembly,
                typeof(PBL5DomainSharedModule).Assembly,
                typeof(PBL5ApplicationModule).Assembly,
                typeof(PBL5ApplicationContractsModule).Assembly,
                typeof(PBL5WebModule).Assembly
            );
        });

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("PBL5");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();
        
        ConfigureSameSiteCookiePolicy(context);
        ConfigureAuthentication(context);
        ConfigureUrls(configuration);
        ConfigureBundles();
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureNavigationServices();
        ConfigureAutoApiControllers();
        ConfigureSwaggerServices(context.Services);
        ConfigureLocalizationServices();
        ConfigureAuthorizePages();
        ConfigDistributedCacheOptions();
    }

    private void ConfigureAuthorizePages()
    {
        Configure<RazorPagesOptions>(p =>
        {
            p.Conventions.AuthorizePage("/Employee/Index", PBL5Permissions.Employee.Default);
            p.Conventions.AuthorizePage("/Employee/CreateModal", PBL5Permissions.Employee.Create);
            //p.Conventions.AuthorizePage("/Employee/Index", PBL5Permissions.Employee.Update);
            p.Conventions.AuthorizePage("/Employee/Index", PBL5Permissions.Employee.Delete);

            p.Conventions.AuthorizePage("/TimeSheet/Index", PBL5Permissions.TimeSheet.Default);
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
    }

    private static void ConfigureSameSiteCookiePolicy(ServiceConfigurationContext context)
    {
        context.Services.AddSameSiteCookiePolicy();
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<PBL5WebModule>();
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<PBL5DomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PBL5.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<PBL5DomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PBL5.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<PBL5ApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PBL5.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<PBL5ApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PBL5.Application"));
                options.FileSets.ReplaceEmbeddedByPhysical<PBL5WebModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new PBL5MenuContributor());
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(PBL5ApplicationModule).Assembly);
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PBL5 API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        // var supportedCultures = new[]
        //     {
        //         new CultureInfo("vi")
        //     };

        // app.UseAbpRequestLocalization(options =>
        //     {
        //         options.DefaultRequestCulture = new RequestCulture("vi");
        //         options.SupportedCultures = supportedCultures;
        //         options.SupportedUICultures = supportedCultures;
        //         options.RequestCultureProviders = new List<IRequestCultureProvider>
        //         {
        //             new QueryStringRequestCultureProvider(),
        //             new CookieRequestCultureProvider()
        //         };
        //     });

        app.UseCookiePolicy();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "PBL5 API");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }

    private void ConfigureLocalizationServices()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            //options.Languages.Add(new LanguageInfo("vi", "vi", "Tiếng Việt"));
            //options.Languages.Add(new LanguageInfo("en", "en", "English"));
        });
    }

    private void ConfigDistributedCacheOptions()
    {
        Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "PBL5";
            });

        Configure<AbpExceptionHandlingOptions>(options =>
            {
                options.SendExceptionsDetailsToClients = true;
                options.SendStackTraceToClients = false;
            });
    }
}

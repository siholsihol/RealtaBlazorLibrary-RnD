using BlazorMenu.Extensions;
using BlazorMenu.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls.Extensions;
using R_BlazorFrontEnd.FileConverter;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd.Interfaces;
using R_BlazorFrontEnd.Report;
using R_BlazorFrontEnd.Tenant.Extensions;
using R_BlazorStartup;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<BlazorMenu.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.R_AddBlazorFrontEndControls();

builder.R_RegisterBlazorServices();

builder.Services.R_AddBlazorFrontEnd();

builder.Services.AddTransient<R_IFileConverter, R_FileConverter>();
builder.Services.AddTransient<R_IReport, R_TenantReportService>();
builder.Services.AddSingleton<R_IFileDownloader, R_FileDownloader>();
builder.Services.AddTransient<HttpInterceptorService>();
builder.Services.AddSingleton<R_IEnvironment, BlazorMenuEnvironmentService>();

Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", builder.HostEnvironment.Environment);

builder.Services.AddMultiTenantancy();

var host = builder.Build();

host.Services.AddServiceProviderToTenantRoutes();

host.R_SetupBlazorService();

await host.R_UseBlazorFrontEnd();

await host.RunAsync();

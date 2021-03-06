using Application.Extensions;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using WebUI.Extensions;
using WebUI.Abstractions;
using WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using WebUI.Permission;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;

namespace WebUI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		[System.Obsolete]
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
			services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

			services.AddNotyf(o =>
			{
				o.DurationInSeconds = 10;
				o.IsDismissable = true;
				o.HasRippleEffect = true;
			});

			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 0;
				options.Password.RequiredUniqueChars = 0;
			});

			services.AddApplicationLayer();
			services.AddInfrastructure(Configuration);
			services.AddPersistenceContexts(Configuration);
			services.AddRepositories();
			services.AddSharedInfrastructure(Configuration);

			services.AddControllersWithViews().AddFluentValidation(fv =>
			{
				fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
				fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
			});
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddDistributedMemoryCache();
			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
			services.AddScoped<IViewRenderService, ViewRenderService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseNotyf();
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{area=Dashboard}/{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}

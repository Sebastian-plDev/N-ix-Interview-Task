using MvcProject.Services;
using Microsoft.AspNetCore.HttpLogging;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<EFContext>();

    builder.Services.AddHttpLogging(options =>
    {
        options.LoggingFields = HttpLoggingFields.Request |
                                HttpLoggingFields.RequestBody;
                                //HttpLoggingFields.Response;
                                //HttpLoggingFields.ResponseBody;
        options.ResponseHeaders.Add("Non-Sensitive");
        options.MediaTypeOptions.AddText("application/json");
    });

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // configure DI for application services
    builder.Services.AddTransient<IShortUrlService, ShortUrlService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.UseHttpLogging();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Dashboard}/{id?}");

    app.Run();
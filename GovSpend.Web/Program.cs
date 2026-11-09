using GovSpend.Web.Services;
using GovSpend.Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

bool demoMode = builder.Configuration.GetValue<bool>("DemoMode");
builder.Services.AddScoped<IAgencyRecordService, AgencyRecordService>();
// Add services to the container.
builder.Services.AddRazorPages();
// Register the ISpendingService with its implementation
builder.Services.AddHttpClient<ISpendingService, UsaSpendingService>(client =>
{
    client.BaseAddress = new Uri("https://api.usaspending.gov/");
});

if (!demoMode)
{
    builder.Services.AddDbContext<GovSpendDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GovSpendDb")));

    builder.Services.AddScoped<IAgencyRecordService, AgencyRecordService>();
}
else
{
    builder.Services.AddScoped<IAgencyRecordService, DemoAgencyRecordService>();
}




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

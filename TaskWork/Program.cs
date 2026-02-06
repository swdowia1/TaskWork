using Microsoft.EntityFrameworkCore;
using TaskWork.Models;
using TaskWork.Serwices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// 🔹 Rejestracja serwisu ML — singleton (tworzy się tylko raz)
builder.Services.AddSingleton<TagPredictionService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

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


app.MapRazorPages();


app.MapControllers(); // <<< to dodaj

//// Seed przykładowych danych (opcjonalnie)
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    if (!db.Companies.Any())
//    {
//        var f1 = new Company { Name = "EasySender" };
//        var f2 = new Company { Name = "Firma Beta" };
//        db.Companies.AddRange(f1, f2);
//        db.SaveChanges();

//        db.Tasks.AddRange(
//            new TaskItem { Title = "Raport sprzedaży", CompanyId = f1.Id },
//            new TaskItem { Title = "Testy regresyjne", CompanyId = f2.Id }
//        );
//        await db.SaveChangesAsync();
//    }
//}

app.Run();

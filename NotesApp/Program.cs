using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Repositories.Implementations;
using NotesApp.Repositories.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Set connection to db
builder.Services.AddDbContext<DbManager>(
	options => options.UseNpgsql(
		builder.Configuration.GetConnectionString("DefaultConnection")));
// Set logging
//Log.Logger = new LoggerConfiguration()
//	.Enrich.FromLogContext()
//	.WriteTo.File(@"C:\")
//	.CreateLogger();
builder.Services.AddSerilog(
	new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File(@"C:\log\log.txt")
    .CreateLogger())
	.AddTransient<NoteRepository>();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
// Sweetalert lib for better UI
builder.Services.AddSweetAlert2(options =>
{
	options.Theme = SweetAlertTheme.Dark;
});
// AutoMap NoteUI & Note models
builder.Services.AddAutoMapper(typeof(Program));

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
// For tests
public partial class Program { }
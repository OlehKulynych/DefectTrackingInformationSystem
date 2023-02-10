using DefectTrackingInformationSystem.Commands;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using DefectTrackingInformationSystem.Service.Interfaces;
using DefectTrackingInformationSystem.State;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<TelegramBotService>();

builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
},ServiceLifetime.Singleton);

builder.Services.AddSingleton<IStateExecutorService, StateExecutorService>();
builder.Services.AddSingleton<IUserService, UserService>();




builder.Services.AddSingleton<State, StartState>();
builder.Services.AddSingleton<State, InputDefectState>();
builder.Services.AddSingleton<State, InputNumberRoomState>();
builder.Services.AddSingleton<State, InputDescriptionState>();
builder.Services.AddSingleton<State, FinishInputDefectState>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Services.GetRequiredService<TelegramBotService>().GetTelegramBot().Wait();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    app.MapControllers();
}

);

app.Run();

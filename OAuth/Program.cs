using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var clientId = configuration["Authentication:Google:ClientId"];
var clientSecret = configuration["Authentication:Google:ClientSecret"];


if (string.IsNullOrWhiteSpace(clientId))
{
    throw new InvalidOperationException("Google ClientId is not configured. Please set it in user-secrets or configuration.");
}

if (string.IsNullOrWhiteSpace(clientSecret))
{
    throw new InvalidOperationException("Google ClientSecret is not configured. Please set it in user-secrets or configuration.");
}


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(options =>
    {
        options.ClientId = clientId;
        options.ClientSecret = clientSecret;
        options.CallbackPath = "/signin-google"; 
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader() 
            .AllowAnyMethod() 
            .AllowCredentials(); 
    });
});


var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

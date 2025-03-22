using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BankingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<BankingContext>()
    .AddDefaultTokenProviders();

// Add scoped services for Account and Transaction
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAuthService, AuthService>();


// JWT Authentication setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

// Add Authorization (optional, if you need role-based policies)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    // Add more policies if needed
});

// Add controllers (MVC or API)
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline (middleware).
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Use authentication middleware
app.UseAuthentication();

// Use authorization middleware
app.UseAuthorization();

// Enable routing and map controllers
app.MapControllers();

// Run the application
app.Run();

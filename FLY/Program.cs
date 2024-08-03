using FLY.Business.Services.Implements;
using FLY.Business.Services;
using FLY.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using FLY.Business.Mapper;
using FLY.DataAccess.Repositories;
using FLY.DataAccess.Repositories.Implements;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using FLY.Business.BackgroundServices;
using FLY.Business.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program), typeof(MapperProfile));

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
  {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };
  });

//Define policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("1"));
    options.AddPolicy("SellerPolicy", policy => policy.RequireRole("2"));
    options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("3"));
    options.AddPolicy("Customer&SellerPolicy", policy => policy.RequireRole("2", "3"));
});

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FLY API", Version = "v1" });

    // Configure Swagger to use JWT Authentication
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});

//Define local DbContext
//builder.Services.AddDbContext<FlyContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Define GC DbContext
builder.Services.AddDbContext<FlyContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("GOOGLE_CLOUD_CONNECTION")));

//// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//DI Service
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IGoogleDriveService, GoogleDriveService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IShopManagerService, ShopManagerService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IBlogService, BlogService>();

// Register in-memory caching
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

//Register backgroundService
//builder.Services.AddHostedService<TokenCleanupService>();

DotNetEnv.Env.Load();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FLY API v1");
    });
}

//Un-comment when build docker
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

app.UseSession();

app.UseCors("AllowAll");

app.UseMiddleware<AuthMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<RegisterMiddleware>();

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

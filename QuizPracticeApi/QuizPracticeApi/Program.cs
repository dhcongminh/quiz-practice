using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using QuizPracticeApi;
using QuizPracticeApi.Models;
using QuizPracticeApi.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()).Select().Filter().OrderBy().Expand().SetMaxTop(100));
builder.Services.AddDbContext<QuizPracticeContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});
builder.Services.AddAutoMapper(typeof(MappingProfile));
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<UserServices>();
var app = builder.Build();

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();

static Microsoft.OData.Edm.IEdmModel GetEdmModel() {
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<User>("Users"); // Map the "Users" entity.
    return builder.GetEdmModel();
}
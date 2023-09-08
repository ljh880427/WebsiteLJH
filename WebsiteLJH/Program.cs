using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebsiteLJH;
using WebsiteLJH.Common;
using WebsiteLJH.Components;
using WebsiteLJH.Models;
using WebsiteLJH.Settings;

//////////////////////////////////////////////////////////////////////////////////////////////////// Property
////////////////////////////////////////////////////////////////////////////////////////// Public


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // DB ���� ��Ʈ�� get
builder.Services.AddSingleton<ICommentRepository>(new CommentRepository(connectionString)); // �����߰�

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<INoticeRepository, NoticeRepository>(); // �������̽��� �������͸� ������ ����


#region ȸ�� ���� ����
//[User][5] ȸ�� ����            
builder.Services.AddSingleton<IUserRepository>(new UserRepository(connectionString)); // �α��� ������Ʈ ��������� IUserRepository, UserRepository �̱��� �����߰�

// LoginFailedManager
builder.Services.AddScoped<ILoginFailedRepository, LoginFailedRepository>(); // �������̽��� �������͸� ������ ����
builder.Services.AddScoped<ILoginFailedManager, LoginFailedManager>();
builder.Services.AddScoped<IUserModelRepository, UserModelRepository>(); // ����� ���� ���� ���� ������Ʈ

#endregion

#region [1] Session ��ü ���
//[0] ���� ��ü ���: Microsoft.AspNetCore.Session.dll NuGet ��Ű�� ���� 
//services.AddSession(); 
// Session ��ü ���� �ɼ� �ο� 
builder.Services.AddSession(options =>
{
    // ���� ���� �ð�
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
#endregion

#region ASP.NET Core ��Ű ����: ConfigureServices()
////[1] ASP.NET Core ��Ű ����: �ܼ��� => �н��� ��Ű �������� �����ϵ�, ���� ����� ASP.NET Core Identity�� ����, ���ķ��� ��� ���Ǵ� Identity ���
//services.AddAuthentication("Cookies").AddCookie();
// ��Ű ���� ���� �ּ����� �ڵ� 
builder.Services.AddAuthentication("Cookies") // CookieAuthenticationDefaults.AuthenticationScheme
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login/";
        options.AccessDeniedPath = "/User/Forbidden/";
    })

// JWT ��ū ���� ���� �ڵ� 
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        // ����Ű ���ڿ� ��� ������ ��
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SymmetricSecurityKey").ToString())), // ��Ʈ�� ��ȯ Ȯ��
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5)
    };
});

// JWT ��ū ���� ���� �ڵ� 
// ������ �ذ�: ISignRepository => SignRepositoryInMemory
builder.Services.AddScoped<ISignRepository, SignRepositoryInMemory>();
#endregion

//[User][9] Policy ����
builder.Services.AddAuthorization(options =>
{

    // Users Role�� ������, Users Policy �ο�
    options.AddPolicy("Users", policy => policy.RequireRole("Users"));

    // Users Role�� �ְ� UserId�� DotNetNoteSettings:SiteAdmin�� 
    // ������ ��(���� ��� "Admin")�̸� "Administrators" �ο�
    // "UserId" - ��ҹ��� ����
    options.AddPolicy("Administrators", policy =>
    policy.RequireRole("Users").RequireClaim("UserId", builder.Configuration.GetSection("WebsiteLJHSettings").GetSection("SiteAdmin").Value));
});


var app = builder.Build();

//// Configure the HTTP request pipeline. �⺻ ���� �޽��� �ּ�ó��(�󼼳���Ȯ���� �ȵǰ� Error.cshtml ���븸 ��µǼ� �ּ�ó����)
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
app.UseDeveloperExceptionPage(); // �������� Ȯ���� ���� �߰�
//app.UseDatabaseErrorPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

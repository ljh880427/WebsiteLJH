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


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // DB 연결 스트링 get
builder.Services.AddSingleton<ICommentRepository>(new CommentRepository(connectionString)); // 연결추가

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<INoticeRepository, NoticeRepository>(); // 인터페이스에 레포지터리 의존성 주입


#region 회원 인증 관련
//[User][5] 회원 관리            
builder.Services.AddSingleton<IUserRepository>(new UserRepository(connectionString)); // 로그인 컴포넌트 사용을위한 IUserRepository, UserRepository 싱글톤 연결추가

// LoginFailedManager
builder.Services.AddScoped<ILoginFailedRepository, LoginFailedRepository>(); // 인터페이스에 레포지터리 의존성 주입
builder.Services.AddScoped<ILoginFailedManager, LoginFailedManager>();
builder.Services.AddScoped<IUserModelRepository, UserModelRepository>(); // 사용자 정보 보기 전용 컴포넌트

#endregion

#region [1] Session 개체 사용
//[0] 세션 개체 사용: Microsoft.AspNetCore.Session.dll NuGet 패키지 참조 
//services.AddSession(); 
// Session 개체 사용시 옵션 부여 
builder.Services.AddSession(options =>
{
    // 세션 유지 시간
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
#endregion

#region ASP.NET Core 쿠키 인증: ConfigureServices()
////[1] ASP.NET Core 쿠키 인증: 단순형 => 학습은 쿠키 인증으로 시작하되, 실제 사용은 ASP.NET Core Identity를 권장, 이후로의 모든 강의는 Identity 사용
//services.AddAuthentication("Cookies").AddCookie();
// 쿠키 인증 적용 최소한의 코드 
builder.Services.AddAuthentication("Cookies") // CookieAuthenticationDefaults.AuthenticationScheme
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login/";
        options.AccessDeniedPath = "/User/Forbidden/";
    })

// JWT 토큰 인증 관련 코드 
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        // 보안키 문자열 길게 설정할 것
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SymmetricSecurityKey").ToString())), // 스트링 변환 확인
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5)
    };
});

// JWT 토큰 인증 관련 코드 
// 의존성 해결: ISignRepository => SignRepositoryInMemory
builder.Services.AddScoped<ISignRepository, SignRepositoryInMemory>();
#endregion

//[User][9] Policy 설정
builder.Services.AddAuthorization(options =>
{

    // Users Role이 있으면, Users Policy 부여
    options.AddPolicy("Users", policy => policy.RequireRole("Users"));

    // Users Role이 있고 UserId가 DotNetNoteSettings:SiteAdmin에 
    // 지정된 값(예를 들어 "Admin")이면 "Administrators" 부여
    // "UserId" - 대소문자 구분
    options.AddPolicy("Administrators", policy =>
    policy.RequireRole("Users").RequireClaim("UserId", builder.Configuration.GetSection("WebsiteLJHSettings").GetSection("SiteAdmin").Value));
});


var app = builder.Build();

//// Configure the HTTP request pipeline. 기본 에러 메시지 주석처리(상세내용확인이 안되고 Error.cshtml 내용만 출력되서 주석처리함)
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
app.UseDeveloperExceptionPage(); // 에러내용 확인을 위해 추가
//app.UseDatabaseErrorPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

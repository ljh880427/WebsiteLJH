using Microsoft.Extensions.Configuration;
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

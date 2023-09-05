using Microsoft.Extensions.Configuration;
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

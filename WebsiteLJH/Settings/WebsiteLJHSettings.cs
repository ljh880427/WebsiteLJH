namespace WebsiteLJH.Settings;

// POCO(Plain Old CLR Object) 클래스
public class WebsiteLJHSettings
{
    public string SiteName { get; set; } = "WebsiteLJH";
    public string SiteUrl { get; set; } = "http://localhost:80";

    //[1] Administrators 이름으로 관리자 권한(Policy) 설정 관련
    public string SiteAdmin { get; set; } = "Admin"; // 관리자 아이디 지정
}

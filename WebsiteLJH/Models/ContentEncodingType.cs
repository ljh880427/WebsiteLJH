namespace WebsiteLJH.Models
{
    /// <summary>
    /// 내용 인코딩 타입
    /// </summary>
    public enum ContentEncodingType
    {
        /// <summary>
        /// 텍스트
        /// </summary>
        Text,

        /// <summary>
        /// HTML로 실행
        /// </summary>
        HTML,

        /// <summary>
        /// HTML + 개행(\r\n) 적용
        /// </summary>
        Mixed
    }
}
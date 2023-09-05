using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteLJH.Models
{
    /// <summary>
    /// 게시글 모델
    /// </summary>
    public class NoticeModel
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region ID - ID

        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        public int ID { get; set; }

        #endregion
        #region 작성자명 - Name

        /// <summary>
        /// 작성자명
        /// </summary>
        [Display(Name = "작성자명")]
        [Required(ErrorMessage = "작성자명을 입력해 주시기 바랍니다.")]
        public string Name { get; set; }

        #endregion
        #region 메일 주소 - MailAddress

        /// <summary>
        /// 메일 주소
        /// </summary>
        [EmailAddress(ErrorMessage = "메일 주소를 입력해 주시기 바랍니다.")]
        public string MailAddress { get; set; }

        #endregion
        #region 제목 - Title

        /// <summary>
        /// 제목
        /// </summary>
        [Display(Name = "제목")]
        [Required(ErrorMessage = "제목을 입력해 주시기 바랍니다.")]
        public string Title { get; set; }

        #endregion
        #region 작성일 - WriteDate

        /// <summary>
        /// 작성일
        /// </summary>
        [Display(Name = "작성일")]
        public DateTime WriteDate { get; set; }

        #endregion
        #region 작성 IP - WriteIP

        /// <summary>
        /// 작성 IP
        /// </summary>
        public string WriteIP { get; set; }

        #endregion
        #region 내용 - Content

        /// <summary>
        /// 내용
        /// </summary>
        [Display(Name = "내용")]
        [Required(ErrorMessage = "내용을 입력해 주시기 바랍니다.")]
        public string Content { get; set; }

        #endregion
        #region 패스워드 - Password

        /// <summary>
        /// 패스워드
        /// </summary>
        [Display(Name = "패스워드")]
        [Required(ErrorMessage = "패스워드를 입력해 주시기 바랍니다.")]
        public string Password { get; set; }

        #endregion
        #region 조회 수 - ReadCount

        /// <summary>
        /// 조회 수
        /// </summary>
        [Display(Name = "조회 수")]
        public int ReadCount { get; set; }

        #endregion
        #region 인코딩 - Encoding

        /// <summary>
        /// 인코딩
        /// </summary>
        [Display(Name = "인코딩")]
        public string Encoding { get; set; } = "Text";

        #endregion
        #region 홈페이지 - Homepage

        /// <summary>
        /// 홈페이지
        /// </summary>
        public string Homepage { get; set; }

        #endregion
        #region 수정일 - UpdateDate

        /// <summary>
        /// 수정일
        /// </summary>
        public DateTime UpdateDate { get; set; }

        #endregion
        #region 수정 IP - UpdateIP

        /// <summary>
        /// 수정 IP
        /// </summary>
        public string UpdateIP { get; set; }

        #endregion
        #region 파일명 - FileName

        /// <summary>
        /// 파일명
        /// </summary>
        [Display(Name = "파일명")]
        public string FileName { get; set; }

        #endregion
        #region 파일 크기 - FileSize

        /// <summary>
        /// 파일 크기
        /// </summary>
        public int FileSize { get; set; }

        #endregion
        #region 다운로드 수 - DownloadCount

        /// <summary>
        /// 다운로드 수
        /// </summary>
        public int DownloadCount { get; set; }

        #endregion
        #region 참조 ID - ReferenceID

        /// <summary>
        /// 참조 ID
        /// </summary>
        public int ReferenceID { get; set; }

        #endregion
        #region 답변 레벨 - ReplyLevel

        /// <summary>
        /// 답변 레벨
        /// </summary>
        public int ReplyLevel { get; set; }

        #endregion
        #region 답변 순서 - ReplyOrder

        /// <summary>
        /// 답변 순서
        /// </summary>
        public int ReplyOrder { get; set; }

        #endregion
        #region 답변 수 - ReplyCount

        /// <summary>
        /// 답변 수
        /// </summary>
        public int ReplyCount { get; set; }

        #endregion
        #region 부모 ID - ParentID

        /// <summary>
        /// 부모 ID
        /// </summary>
        public int ParentID { get; set; }

        #endregion
        #region 댓글 수 - CommentCount

        /// <summary>
        /// 댓글 수
        /// </summary>
        public int CommentCount { get; set; }

        #endregion
        #region 카테고리 - Category

        /// <summary>
        /// 카테고리
        /// </summary>
        public string Category { get; set; } = "FREE";

        #endregion
    }
}
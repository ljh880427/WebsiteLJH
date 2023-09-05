using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteLJH.Models
{
    /// <summary>
    /// 댓글 모델
    /// </summary>
    public class CommentModel
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region ID - ID

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        #endregion
        #region 게시판 ID - NoticeID

        /// <summary>
        /// 게시판 ID
        /// </summary>
        public int NoticeID { get; set; }

        #endregion
        #region 작성자명 - Name

        /// <summary>
        /// 작성자명
        /// </summary>
        [Required(ErrorMessage = "작성자명을 입력해 주시기 바랍니다.")]
        public string Name { get; set; }

        #endregion
        #region 댓글 - Comment

        /// <summary>
        /// 댓글
        /// </summary>
        [Required(ErrorMessage = "댓글을 입력해 주시기 바랍니다.")]
        public string Comment { get; set; }

        #endregion
        #region 작성일 - WriteDate

        /// <summary>
        /// 작성일
        /// </summary>
        public DateTime WriteDate { get; set; }

        #endregion
        #region 패스워드 - Password

        /// <summary>
        /// 패스워드
        /// </summary>
        [Required(ErrorMessage = "패스워드를 입력해 주시기 바랍니다.")]
        public string Password { get; set; }

        #endregion
    }
}
using System.Collections.Generic;

namespace WebsiteLJH.Models
{
    /// <summary>
    /// 게시판 댓글 모델
    /// </summary>
    public class NoticeCommentModel
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 게시판 ID - NoticeID

        /// <summary>
        /// 게시판 ID
        /// </summary>
        public int NoticeID { get; set; }

        #endregion
        #region 댓글 리스트 - CommentList

        /// <summary>
        /// 댓글 리스트
        /// </summary>
        public List<CommentModel> CommentList { get; set; }

        #endregion
    }
}
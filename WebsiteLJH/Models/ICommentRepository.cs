using System.Collections.Generic;

namespace WebsiteLJH.Models
{
    /// <summary>
    /// 댓글 저장소
    /// </summary>
    public interface ICommentRepository
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Method

        #region 댓글 추가하기 - WriteComment(comment)

        /// <summary>
        /// 댓글 추가하기
        /// </summary>
        /// <param name="comment">댓글</param>
        void WriteComment(CommentModel comment);

        #endregion
        #region 댓글 리스트 구하기 - GetCommentList(noticeID)

        /// <summary>
        /// 댓글 리스트 구하기
        /// </summary>
        /// <param name="noticeID">게시판 ID</param>
        /// <returns>댓글 리스트</returns>
        List<CommentModel> GetCommentList(int noticeID);

        #endregion
        #region 카운트 구하기 - GetCount(noticeID, id, password)

        /// <summary>
        /// 카운트 구하기
        /// </summary>
        /// <param name="noticeID">게시판 ID</param>
        /// <param name="id">ID</param>
        /// <param name="password">패스워드</param>
        /// <returns>카운트</returns>
        int GetCount(int noticeID, int id, string password);

        #endregion
        #region 댓글 삭제하기 - DeleteComment(noticeID, id, password)

        /// <summary>
        /// 댓글 삭제하기
        /// </summary>
        /// <param name="noticeID">게시판 ID</param>
        /// <param name="id">ID</param>
        /// <param name="password">패스워드</param>
        int DeleteComment(int noticeID, int id, string password);

        #endregion
        #region 최근 댓글 리스트 구하기 - GetRecentCommentList()

        /// <summary>
        /// 최근 댓글 리스트 구하기
        /// </summary>
        /// <returns>최근 댓글 리스트</returns>
        List<CommentModel> GetRecentCommentList();

        #endregion
    }
}
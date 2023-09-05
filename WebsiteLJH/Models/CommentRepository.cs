using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Dapper;

namespace WebsiteLJH.Models
{
    /// <summary>
    /// 댓글 저장소
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 연결
        /// </summary>
        private SqlConnection connection;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - CommentRepository(connectionString)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="connectionString">연결 문자열</param>
        public CommentRepository(string connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 댓글 추가하기 - WriteComment(comment)

        /// <summary>
        /// 댓글 추가하기
        /// </summary>
        /// <param name="comment">댓글</param>
        public void WriteComment(CommentModel comment)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();

            dynamicParameters.Add("@NoticeID", value : comment.NoticeID, dbType : DbType.Int32 );
            dynamicParameters.Add("@Name"    , value : comment.Name    , dbType : DbType.String);
            dynamicParameters.Add("@Comment" , value : comment.Comment , dbType : DbType.String);
            dynamicParameters.Add("@Password", value : comment.Password, dbType : DbType.String);

            string sql = @"
INSERT INTO dbo.NoticeComment
(
    NoticeID,
    [Name],
    Comment,
    Password
)
Values
(
    @NoticeID,
    @Name,
    @Comment,
    @Password
);

UPDATE dbo.Notice
SET    CommentCount = CommentCount + 1 
Where  ID = @NoticeID
";

            this.connection.Execute(sql, dynamicParameters, commandType : CommandType.Text);
        }

        #endregion
        #region 댓글 리스트 구하기 - GetCommentList(noticeID)

        /// <summary>
        /// 댓글 리스트 구하기
        /// </summary>
        /// <param name="noticeID">게시판 ID</param>
        /// <returns>댓글 리스트</returns>
        public List<CommentModel> GetCommentList(int noticeID)
        {
            return this.connection.Query<CommentModel>
            (
                "SELECT * FROM dbo.NoticeComment WHERE NoticeID = @NoticeID",
                new { NoticeID = noticeID },
                commandType : CommandType.Text
            )
            .ToList();
        }

        #endregion
        #region 카운트 구하기 - GetCount(noticeID, id, password)

        /// <summary>
        /// 카운트 구하기
        /// </summary>
        /// <param name="noticeID">게시판 ID</param>
        /// <param name="id">ID</param>
        /// <param name="password">패스워드</param>
        /// <returns>카운트</returns>
        public int GetCount(int noticeID, int id, string password)
        {
            return this.connection.Query<int>
            (
                "SELECT COUNT(*) FROM dbo.NoticeComment WHERE NoticeID = @NoticeID AND ID = @ID AND Password = @Password",
                new { NoticeID = noticeID, ID = id, Password = password },
                commandType : CommandType.Text
            )
            .SingleOrDefault();
        }

        #endregion
        #region 댓글 삭제하기 - DeleteComment(noticeID, id, password)

        /// <summary>
        /// 댓글 삭제하기
        /// </summary>
        /// <param name="noticeID">게시판 ID</param>
        /// <param name="id">ID</param>
        /// <param name="password">패스워드</param>
        public int DeleteComment(int noticeID, int id, string password)
        {
            return this.connection.Execute
            (
                @"
DELETE dbo.NoticeComment
WHERE  NoticeID = @NoticeID
And    ID       = @ID
And    Password = @Password;

UPDATE dbo.Notice
SET    CommentCount = CommentCount - 1 
WHERE  ID = @NoticeID",
                new { NoticeID = noticeID, Id = id, Password = password },
                commandType : CommandType.Text
            );
        }

        #endregion
        #region 최근 댓글 리스트 구하기 - GetRecentCommentList()

        /// <summary>
        /// 최근 댓글 리스트 구하기
        /// </summary>
        /// <returns>최근 댓글 리스트</returns>
        public List<CommentModel> GetRecentCommentList()
        {
            string sql = "SELECT TOP 3 ID, NoticeID, Comment, WriteDate FROM dbo.NoticeComment ORDER BY ID DESC";

            return this.connection.Query<CommentModel>(sql).ToList();
        }

        #endregion
    }
}
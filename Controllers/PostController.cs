using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpPost("Post/{postid}/{userid}/{searchparam}")]
        public IEnumerable<Post> GetPosts(int postid = 0, int userid = 0, string searchparam = "")
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get";
            string parameters = "";

            if ( postid != 0)
            {
                parameters +=  ", @PostId=" + postid.ToString();
            }
            if ( userid != 0)
            {
                parameters +=  ", @UserId=" + userid.ToString();
            }
            if ( searchparam != "None")
            {
                parameters +=  ", @SearchValue=" + searchparam.ToString();
            }

            sql += parameters.Substring(1);

            return _dapper.LoadData<Post>(sql);
        }

        [HttpGet("MyPosts/{postid}/{userid}/{searchparam}")]
        public IEnumerable<Post> GetMyPosts(int postid = 0, int userid = 0, string searchparam = "")
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get @UserId" + this.User.FindFirst("userId")?.Value ;
            string parameters = "";
            
            if ( postid != 0)
            {
                parameters +=  ", @PostId=" + postid.ToString();
            }
            if ( userid != 0)
            {
                parameters +=  ", @UserId=" + userid.ToString();
            }
            if ( searchparam != "None")
            {
                parameters +=  ", @SearchValue=" + searchparam.ToString();
            }

            return _dapper.LoadData<Post>(sql);
        }

      
        [HttpPut("UpsertPost")]
        public IActionResult UpsertAddPost(PostToAddDto postToUpsert)
        {
            string sql = @" EXEC TutorialAppSchema.spPosts_Upsert
                    @UserId =" + this.User.FindFirst("userId")?.Value +
                    ", @PostTitle ='" + postToUpsert.PostTitle +
                    "', @PostContent = '" + postToUpsert.PostContent + "'";

            if (postToUpsert.PostId > 0 )
            {
                    sql += ", @PostId = " + postToUpsert.PostId;
            }

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Falied to create new post");
        }

        [HttpPut("Post")]
        public IActionResult EditPost(PostToEditDto postToEdit)
        {
            string sql = @"
            UPDATE TutorialAppSchema.Posts 
                SET PostContent = '" + postToEdit.PostContent +
                 "' PostTitle = '" + postToEdit.PostTitle +
                 @"' PostUpdated = GETDATE()
                    WHERE PostId =  " + postToEdit.PostId.ToString() +
                    "AND UserId = "  + this.User.FindFirst("userId")?.Value;

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Falied to create new post");
        }

        [HttpDelete("Post/{postid}")]
        public IActionResult DeletePost( int postId)
        {
            string sql = @" EXEC TutorialAppSchema.spPost_Delete PostId =" +
                 postId.ToString() +
            "AND UserId = "  + this.User.FindFirst("userId")?.Value;;

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Falied to delete new post");
        }
    }
}
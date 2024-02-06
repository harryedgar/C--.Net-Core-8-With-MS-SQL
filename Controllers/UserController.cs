using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
//What this does, it will rpoute to the weatherforecast on program cs and access the controller of ot which you see below -H
[Route("[Controller]")]

public class UserController : ControllerBase
{
    DataContextDapper _dapper;

    //constructor
    public UserController(IConfiguration config)
    {
        //Console.WriteLine(config.GetConnectionString("DefaultConnection"));
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("select getdate()");
    }

    //We are going to use this to get this controller and use it to display the info -H
    //("route(which is nothing)" ,  name ="name of controller") - H

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"select [UserId],
                        [FirstName],
                        [LastName],
                        [Email],
                        [Gender],
                        [Active] 
                    from TutorialAppSchema.Users";
        IEnumerable<User> users = _dapper.LoadData<User>(sql);

        return users;
    }

     [HttpGet("GetSingleUser/{testValue}")]
    public User GetSingleUser(int userId)
    {
        string sql = @"select [UserId],
                        [FirstName],
                        [LastName],
                        [Email],
                        [Gender],
                        [Active] 
                    from TutorialAppSchema.Users 
                    where UserId = " + userId.ToString();
        User user = _dapper.LoadDataSingle<User>(sql);
        return user                ;
    }


    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql =@"
        UPDATE TutorialAppSchema.Users
            SET  [FirstName]= '" + user.Firstname +
            "'[LastName] = '" + user.LastName + 
            "'[Email]= '" + user.Email +
            "'[Gender] = '" + user.Gender +
            "'[Active] = '" + user.Active +
            "'WHERE USER_ID = " + user.UserId;

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to update user");
    }

    [HttpPost("AddUser")]
    public IActionResult Aduser(UserDto user)
    {
         string sql =@"
        INSERT INTO TutorialAppSchema.Users()
            [FirstName]e,
            [LastName] ,
            [Email],
            [Gender], 
            [Active],
            ) VAKUES (" +
                "'" + user.Firstname +
                "'" + user.LastName + 
                "'" + user.Email +
                "'" + user.Gender +
                "'" + user.Active + "')";
       
          if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to update user");

    }
    [HttpDelete("Deleteuser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
         string sql =@"
            DELETE FROM TutorialAppSchema.Users
            WHERE USER_ID = " + userId.ToString();

             if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to Delete user");
    }

     [HttpGet("UserSalary/{userId}")]
    public IEnumerable<UserSalary> GetUserSalary(int userId)
    {
        return _dapper.LoadData<UserSalary>(@"
            SELECT UserSalary.UserId
                    , UserSalary.Salary
            FROM  TutorialAppSchema.UserSalary
                WHERE UserId = " + userId.ToString());
    }

    [HttpPost("UserSalary")]
    public IActionResult PostUserSalary(UserSalary userSalaryForInsert)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.UserSalary (
                UserId,
                Salary
            ) VALUES (" + userSalaryForInsert.UserId.ToString()
                + ", " + userSalaryForInsert.Salary
                + ")";

        if (_dapper.ExecuteSqlWithRowCount(sql) > 0)
        {
            return Ok(userSalaryForInsert);
        }
        throw new Exception("Adding User Salary failed on save");
    }

    [HttpPut("UserSalary")]
    public IActionResult PutUserSalary(UserSalary userSalaryForUpdate)
    {
        string sql = "UPDATE TutorialAppSchema.UserSalary SET Salary=" 
            + userSalaryForUpdate.Salary
            + " WHERE UserId=" + userSalaryForUpdate.UserId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userSalaryForUpdate);
        }
        throw new Exception("Updating User Salary failed on save");
    }

    [HttpDelete("UserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = "DELETE FROM TutorialAppSchema.UserSalary WHERE UserId=" + userId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Deleting User Salary failed on save");
    }

    [HttpGet("UserJobInfo/{userId}")]
    public IEnumerable<UserJobInfo> GetUserJobInfo(int userId)
    {
        return _dapper.LoadData<UserJobInfo>(@"
            SELECT  UserJobInfo.UserId
                    , UserJobInfo.JobTitle
                    , UserJobInfo.Department
            FROM  TutorialAppSchema.UserJobInfo
                WHERE UserId = " + userId.ToString());
    }

    [HttpPost("UserJobInfo")]
    public IActionResult PostUserJobInfo(UserJobInfo userJobInfoForInsert)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.UserJobInfo (
                UserId,
                Department,
                JobTitle
            ) VALUES (" + userJobInfoForInsert.UserId
                + ", '" + userJobInfoForInsert.Department
                + "', '" + userJobInfoForInsert.JobTitle
                + "')";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userJobInfoForInsert);
        }
        throw new Exception("Adding User Job Info failed on save");
    }

    [HttpPut("UserJobInfo")]
    public IActionResult PutUserJobInfo(UserJobInfo userJobInfoForUpdate)
    {
        string sql = "UPDATE TutorialAppSchema.UserJobInfo SET Department='" 
            + userJobInfoForUpdate.Department
            + "', JobTitle='"
            + userJobInfoForUpdate.JobTitle
            + "' WHERE UserId=" + userJobInfoForUpdate.UserId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userJobInfoForUpdate);
        }
        throw new Exception("Updating User Job Info failed on save");
    }

    // [HttpDelete("UserJobInfo/{userId}")]
    // public IActionResult DeleteUserJobInfo(int userId)
    // {
    //     string sql = "DELETE FROM TutorialAppSchema.UserJobInfo  WHERE UserId=" + userId;

    //     if (_dapper.ExecuteSql(sql))
    //     {
    //         return Ok();
    //     }
    //     throw new Exception("Deleting User Job Info failed on save");
    // }
    
    [HttpDelete("UserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.UserJobInfo 
                WHERE UserId = " + userId.ToString();
        
        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        } 

        throw new Exception("Failed to Delete User");
    }
}
using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;

namespace DotnetAPI.Controllers;

[ApiController]
//What this does, it will rpoute to the weatherforecast on program cs and access the controller of ot which you see below -H
[Route("[Controller]")]

public class UserCompleteController : ControllerBase
{
    DataContextDapper _dapper;

    //constructor
    public UserCompleteController(IConfiguration config)
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
     [HttpGet("GetUsers/{userId}/{isActive}")]
    public IEnumerable<UserComplete> GetUsers(int userId, bool isActive)
    {
        string sql = @"EXEC TutorialAppSchema.spUsers_Get";
        string stringparameters = "";
        DynamicParameters sqlParameters = new DynamicParameters()
        ;
        
        if (userId != 0)
        {
            stringparameters += ", @UserId= @UserIdParameter";
            sqlParameters.Add("@UserIdParameter", userId, System.Data.DbType.Int32);
        }
        if (isActive)
        {
            stringparameters += ", @Active=@ActiveParameter" ;
             sqlParameters.Add("@ActiveParamter", isActive, System.Data.DbType.Boolean);
        }

        if (stringparameters.Length > 0)
        {
            sql += stringparameters.Substring(1);//, parameters.Length);
        }

        IEnumerable<UserComplete> users = _dapper.LoadDataWithParameters<UserComplete>(sql, sqlParameters);
        return users;
    }
    
    [HttpPut("UpsertUser")]
    public IActionResult UpsertUser(UserComplete user)
    {
         string sql = @"EXEC TutorialAppSchema.spUser_Upsert
            @FirstName = @FirstNameParameter, 
            @LastName = @LastNameParameter, 
            @Email = @EmailParameter, 
            @Gender = @GenderParameter, 
            @Active = @ActiveParameter, 
            @JobTitle = @JobTitleParameter, 
            @Department = @DepartmentParameter, 
            @Salary = @SalaryParameter, 
            @UserId = @UserIdParameter";

        DynamicParameters sqlParameters = new DynamicParameters();

        sqlParameters.Add("@FirstNameParameter", user.Firstname, DbType.String);
        sqlParameters.Add("@LastNameParameter", user.LastName, DbType.String);
        sqlParameters.Add("@EmailParameter", user.Email, DbType.String);
        sqlParameters.Add("@GenderParameter", user.Gender, DbType.String);
        sqlParameters.Add("@ActiveParameter", user.Active, DbType.Boolean);
        sqlParameters.Add("@JobTitleParameter", user.JobTitle, DbType.String);
        sqlParameters.Add("@DepartmentParameter", user.Department, DbType.String);
        sqlParameters.Add("@SalaryParameter", user.Salary, DbType.Decimal);
        sqlParameters.Add("@UserIdParameter", user.UserId, DbType.Int32);

        if (_dapper.ExecuteSqlWithParameterst(sql, sqlParameters))
        {
            return Ok();
        } 

        throw new Exception("Failed to Update User");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"TutorialAppSchema.spUser_Delete
            @UserId = " + userId.ToString();

        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);

        if (_dapper.ExecuteSqlWithParameterst(sql, sqlParameters))
        {
            return Ok();
        } 

        throw new Exception("Failed to Delete User");
    }
    
    
}
using DotnetAPI.Models;
using DotnetAPI.Data;

namespace DotnetAPI.Data
{
    public class UserRespository
    {
        DataContextEF _entityFramework;
    
        public UserRespository(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if ( entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToAdd)
        {
            if ( entityToAdd != null)
            {
                _entityFramework.Remove(entityToAdd);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return users;
        }

        public User GetSingleUser(int userId)
        {
            User? user = _entityFramework.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefault<User>();
            
            if (user != null)
            {
                return user;
            }
            throw new Exception("Failed to get user");
        }

        public UserSalary GetSingleUserSalary(int userId)
        {
            UserSalary? userSalary = _entityFramework.UserSalaries
                .Where(u => u.UserId == userId)
                .FirstOrDefault<UserSalary>();
            
            if (userSalary != null)
            {
                return userSalary;
            }
            throw new Exception("Failed to get user");
        }

        public UserJobInfo GetSingleUserJobInfo(int userId)
        {
            UserJobInfo? userJobInfo = _entityFramework.UserJobInfos
                .Where(u => u.UserId == userId)
                .FirstOrDefault<UserJobInfo>();
            
            if (userJobInfo != null)
            {
                return userJobInfo;
            }
            throw new Exception("Failed to get user");
        }
    }
}
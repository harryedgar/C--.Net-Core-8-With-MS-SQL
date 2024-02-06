using DotnetAPI.Models;
using DotnetAPI.Data;

namespace DotnetAPI.Data
{
    public interface IUserRepository 
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToAdd);
        public IEnumerable<User> GetUsers();
        public User GetSingleUser(int userId); 
        public UserSalary GetSingleUserSalary(int userId);
        public User GetSingleUserJobInfo(int userId);
    }
}
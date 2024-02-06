namespace DotnetAPI.Dtos
{
     public partial class UserDto
     {
        public int UserId {get; set;}
        public string Firstname {get; set;} = "";
        public string LastName {get; set;} = "";
        public string Email {get; set;} = "";
        public string Gender {get; set;} = "";
        public bool Active {get; set;}

     }
}
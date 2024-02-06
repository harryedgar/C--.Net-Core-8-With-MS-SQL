namespace DotnetAPI.Dtos
{
    public partial class PostToAddDto
    {
        public int PostId {get; set;} = 0;
        public string PostTitle {get; set;} = "";
        public string PostContent {get; set;} = "";
    }
}
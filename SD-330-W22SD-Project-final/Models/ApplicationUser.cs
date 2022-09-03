using Microsoft.AspNetCore.Identity;

namespace SD_330_W22SD_Project_final.Models
{
    public class ApplicationUSer : IdentityUser
    {
        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}

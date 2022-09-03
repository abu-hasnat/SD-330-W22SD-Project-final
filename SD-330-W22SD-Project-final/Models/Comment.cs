using Microsoft.AspNetCore.Identity;

namespace SD_330_W22SD_Project_final.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string? UserId { get; set; }
        public string UserName { get; set; }
        public virtual IdentityUser User { get; set; }
        public int? AnswerId { get; set; }
        public virtual Answer Answer { get; set; }
    }
}

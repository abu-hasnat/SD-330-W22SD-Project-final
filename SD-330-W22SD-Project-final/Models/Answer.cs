using Microsoft.AspNetCore.Identity;

namespace SD_330_W22SD_Project_final.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }

        public virtual Question Question { get; set; }
        public int? QuestionId { get; set; }
        public string? UserId { get; set; }
        public string UserName { get; set; }
        public string Body { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsCorrect { get; set; }
        public int Reputation { get; set; } = 0;
    }
}

using Microsoft.AspNetCore.Identity;

namespace SD_330_W22SD_Project_final.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual IdentityUser User { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public ICollection<Vote> Votes { get; set; } = new HashSet<Vote>();
        public ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();
    }
}

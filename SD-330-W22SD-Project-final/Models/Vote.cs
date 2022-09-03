using Microsoft.AspNetCore.Identity;

namespace SD_330_W22SD_Project_final.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public IdentityUser User { get; set; }
        public string UserId { get; set; }
        public Answer Answer { get; set; }
        public int? AnswerId { get; set; }
        public virtual ICollection<IdentityUser> Users { get; set; }
        public int VoteValue { get; set; } = 0;
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD_330_W22SD_Project_final.Data;
using SD_330_W22SD_Project_final.Models;

namespace SD_330_W22SD_Project_final.Controllers
{
    public class AnswerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AnswerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        public async Task<IActionResult> AnswerList()
        {
            ViewBag.UserInfo = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.Answers.Include(a => a.Question).Where(x => x.UserName != null);
            return View(await applicationDbContext.Distinct().ToListAsync());
        }
        public async Task<IActionResult> ViewAnswer(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.AnswerId == Id);
            if (answer == null)
            {
                return NotFound();
            }
            var comment = _context.Comments.Where(a => a.AnswerId == answer.AnswerId).Distinct().ToList();
            answer.Comments = comment;
            ViewBag.UserInfo = await _userManager.GetUserAsync(User);
            return View(answer);
        }

        public async Task<IActionResult> AddAnswer(int? Id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (Id == null)
            {
                return RedirectToAction("QuestionList", "Question");
            }
            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.QuestionId == Id);
            if (user.UserName == question.UserName)
            {
                return RedirectToAction("QuestionList", "Question");
            }
            Answer answer = new Answer
            {
                Question = question,
                QuestionId = Id,
                UserId = user.Id,
            };
            return View(answer);
        }
        public async Task<IActionResult> AddVote(Answer answer)
        {
            if (answer != null)
            {
                var user = await _userManager.GetUserAsync(User);
                var answers = await _context.Answers.FindAsync(answer.AnswerId);
                var ifRecord = _context.Votes.Where(x => x.AnswerId == answers.AnswerId && x.UserId == user.Id).FirstOrDefault();
                if (ifRecord == null)
                {

                    Vote vote = new Vote()
                    {
                        AnswerId = answer.AnswerId,
                        UserId = user.Id,
                        User = user,
                        VoteValue = 5
                    };
                    _context.Votes.Add(vote);
                }
            }
            return RedirectToAction(nameof(AnswerList));
        }
        public async Task<IActionResult> RemoveVote(Answer answer)
        {
            if (answer != null)
            {
                var user = await _userManager.GetUserAsync(User);
                var answers = await _context.Answers.FindAsync(answer.AnswerId);
                var ifRecord = _context.Votes.Where(x => x.AnswerId == answers.AnswerId && x.UserId == user.Id).FirstOrDefault();
                if (ifRecord == null)
                {

                    Vote vote = new Vote()
                    {
                        AnswerId = answer.AnswerId,
                        UserId = user.Id,
                        User = user,
                        VoteValue = -5
                    };
                    _context.Votes.Add(vote);
                }
            }
            return RedirectToAction(nameof(AnswerList));
        }
            public IActionResult Index()
        {
            return View();
        }
    }
}

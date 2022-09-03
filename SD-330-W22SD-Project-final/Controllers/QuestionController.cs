using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD_330_W22SD_Project_final.Data;
using SD_330_W22SD_Project_final.Models;

namespace SD_330_W22SD_Project_final.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public QuestionController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> QuestionList()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserInfo = user;
            var question = await _context.Questions.ToListAsync();
            if (user != null)
            {
                var answer = await _context.Answers.ToListAsync();
                question.ForEach(c => c.Answers = (ICollection<Answer>)answer.Where(x => x.QuestionId == c.QuestionId).ToList());
            }
            return View(question.Distinct().Where(x => x.UserName != null).ToList());
        }
        public async Task<IActionResult> ViewQuestion(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(QuestionList));
            }
            Question question = await _context.Questions.Where(x => x.QuestionId == id)
                                        .Include(inc => inc.Answers).FirstOrDefaultAsync();
            if (question == null)
            {
                return RedirectToAction(nameof(QuestionList));
            }
            var answer = await _context.Answers.Where(x => x.QuestionId == question.QuestionId).ToListAsync();
            if (answer != null)
            {
                answer.ForEach(x => x.Comments = _context.Comments.Where(a => a.AnswerId == x.AnswerId).ToList());
            }
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserInfo = user;
            question.Answers = answer;
            return View(question);
        }
        public IActionResult AddQuestion()
        {
            return View();
        }
        public async Task<IActionResult> UpdateQuestion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            if (question == null && user.UserName != question.UserName)
            {
                return RedirectToAction(nameof(QuestionList));
            }
            return View(question);
        }
        public async Task<IActionResult> DeleteQuestion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }
            if (question.UserName != user.UserName)
            {
                return RedirectToAction(nameof(QuestionList));
            }

            return View(question);
        }
        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionId == id);
        }

    }
}

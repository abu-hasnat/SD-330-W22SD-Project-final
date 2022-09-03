using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD_330_W22SD_Project_final.Data;
using SD_330_W22SD_Project_final.Models;

namespace SD_330_W22SD_Project_final.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CommentController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserInfo = user;
            var comments = await _context.Comments.ToListAsync();
            if (user != null)
            {
                var answer = await _context.Answers.ToListAsync();
                comments.ForEach(c => c.Answer = answer.Where(x => x.AnswerId == c.AnswerId).FirstOrDefault());
            }
            return View(comments);
        }
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var comments = await _context.Comments.Where(x => x.Id == id).FirstOrDefaultAsync();
            ViewBag.UserInfo = await _userManager.GetUserAsync(User);
            return View(comments);
        }
        public async Task<ActionResult> Create(int? id)
        {
            if (id != null)
            {
                var answer = await _context.Answers.Where(x => x.AnswerId == id).FirstOrDefaultAsync();
                answer.Question = new Question();
                answer.Question.Title = _context.Questions.Where(x => x.QuestionId == answer.QuestionId).FirstOrDefault().Title;
                Comment comment = new Comment()
                {
                    Answer = answer,
                    AnswerId = id
                };
                return View(comment);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Comment comment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    comment.Id = 0;
                    comment.User = await _userManager.GetUserAsync(User);
                    comment.UserName = comment.User.UserName;
                    comment.UserId = comment.User.Id;
                    _context.Add(comment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ViewAnswer", "Answer", comment.AnswerId);
                }
                return View(comment);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var comments = await _context.Comments.Where(x => x.Id == id).FirstOrDefaultAsync();
            return View(comments);
        }
        public async Task<ActionResult> Delete(int? id)
        {
            if (id != null)
            {

                var comment = await _context.Comments.FindAsync(id);
                return View(comment);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
     
    }
}

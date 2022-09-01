using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlcoAxe.Data;
using Test_OP_Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace AlcoAxe.Controllers
{

    public class GamesController : Controller
    {
        private readonly OptionContext _context;
        IHttpContextAccessor _httpContextAccessor;
        public GamesController(OptionContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;


            //GetCookies();
        }

        private string GetCookies()
        {
            string key = "user";
            string cookie;
            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(key))
                cookie = _httpContextAccessor.HttpContext.Request.Cookies[key];
            else
            {
                cookie = Guid.NewGuid().ToString();
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddYears(1);
                _httpContextAccessor.HttpContext.Response.Cookies.Append(key, cookie, options);
            }

            return cookie;
        }


        static List<SampleQuestion> Shuffle(List<SampleQuestion> a)
        {
            Random rand = new Random();
            for (int i = a.Count - 1; i > 0; i--)
            {
                int j = rand.Next(0, i + 1);
                var tmp = a[i];
                a[i] = a[j];
                a[j] = tmp;
            }

            return a;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            Response.Cookies.Append("somekey", "somevalue");

            var games = await _context.Games.
                Include(x => x.GameType).
                Include(x => x.Players).
                Where(x => x.User.UserHash == GetCookies()).
                ToListAsync();


            return View(games.OrderByDescending(x => x.Id));
        }



        // GET: Games/Create
        public IActionResult Create()
        {
            var gametypes = _context.GameTypes.ToList();

            ViewBag.GameTypes = new SelectList(gametypes, "Id", "NameGameType");

            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {

            // Проверка GameType
            game.GameType = _context.GameTypes.FirstOrDefault(c => c.Id == game.GameType.Id);

            // Установка времени

            game.DateTime = DateTime.Now;

            //Проверка куки и бд
            CookieUser cookieUser = _context.CookieUsers.FirstOrDefault(x => x.UserHash == GetCookies());

            if (cookieUser == null)
                cookieUser = new CookieUser() { UserHash = GetCookies() };
            game.User = cookieUser;




            // Получение GameMode и SampleQuestions
            List<SampleQuestion> sampleQuestions = null;

            if (game.GameMode == GameMode.Random)
                sampleQuestions = await _context.SampleQuestions.Where
                    (x => x.GameType == game.GameType).ToListAsync();
            else
                sampleQuestions = await _context.SampleQuestions.Where
                        (x => x.GameType == game.GameType && x.GameMode == game.GameMode).ToListAsync();

            if (sampleQuestions == null)
                ModelState.TryAddModelError("error", "Error in searching sampleQuestion");

            // Создание Questions




            sampleQuestions = Shuffle(sampleQuestions);

            if (ModelState.IsValid)
            {

                List<Question> questions = new List<Question>();

                for (int i = 0; i < sampleQuestions.Count; i++)
                {
                    questions.Add(new Question()
                    {
                        Game = game,
                        SampleQuestion = sampleQuestions[i],
                        Player = game.Players[i % game.Players.Count],
                        QuestionDate = QuestionDate.NoDate,
                        Position = i
                    });

                    Console.WriteLine(game.Players[i % game.Players.Count].Name + " " + sampleQuestions[i].Id);
                    Console.WriteLine();

                }
                game.Questions = questions;



                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Play), new { id = game.Id });
            }
            return View(game);
        }


        // GET: Games/Play/5
        public async Task<IActionResult> Play(int? id, int count = 10, bool ajax = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.Include(x => x.Questions).ThenInclude(x => x.SampleQuestion)
                .Include(x => x.Players)
                .Include(x => x.GameType)
                .Where(x => x.User.UserHash == GetCookies())
                .FirstOrDefaultAsync(m => m.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            // game.Questions = game.Questions.Where(x => x.QuestionDate == QuestionDate.NoDate).Take(count).ToList();


   

            ViewBag.Ajax = ajax;

            if (game.Questions.Count == 0)
                return View("Empty");

            return View(game);
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> Send(int GameId, QuestionDate questionDate, int questionId)
        {

            var game = _context.Games.Include(x => x.Questions).
                FirstOrDefault(x => x.Id == GameId && x.User.UserHash == GetCookies());

            if (game == null)
            {
                return "Error! game is null";
            }

            var question = game.Questions.FirstOrDefault(x => x.Id == questionId);
            if (question == null)
            {
                return "Error! question is null";
            }

            question.QuestionDate = questionDate;
            await _context.SaveChangesAsync();

            return "Ok";

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> Refresh(int GameId, int questionId)
        {

            var game = _context.Games.Include(x => x.Questions).
                Include(x => x.GameType).
                FirstOrDefault(x => x.Id == GameId && x.User.UserHash == GetCookies());

            if (game == null)
            {
                return "Error! game is null";
            }

            var question = game.Questions.FirstOrDefault(x => x.Id == questionId);

            var sampleQuestions = _context.SampleQuestions.Where(x => x.GameType == game.GameType).ToList();

            var rand = new Random();
            int number = rand.Next(sampleQuestions.Count - 1);
            var sampleQuestion = sampleQuestions[number];



            if (sampleQuestion == null)
            {
                return "Error! question is null";
            }

            question.SampleQuestion = sampleQuestion;


            await _context.SaveChangesAsync();

            return sampleQuestion.QuestionString;

        }

    }
}

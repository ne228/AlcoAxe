using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_OP_Web.Data;
using Test_OP_Web.Models;

namespace Test_OP_Web.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {

    //    private readonly ILogger<AdminController> _logger;
    //    private readonly OptionContext _context;
    //    private readonly UserManager<UserAxe> _userManager;
    //    //UserAxe UserAxe { get; set; }



    //    public AdminController(ILogger<AdminController> logger, OptionContext context, UserManager<UserAxe> userManager)
    //    {
    //        _logger = logger;
    //        _context = context;
    //        _userManager = userManager;
    //    }

    //    public IActionResult Index(FilterModel filterModel)
    //    {
    //        if (!ModelState.IsValid)
    //            return View(filterModel);


    //        if (filterModel.DateTimeFinish == new DateTime())
    //            filterModel.DateTimeFinish = DateTime.Now;

    //        if (filterModel.NumVFinish == 0)
    //            filterModel.NumVFinish = 25;

    //        var ses = _context.GetSessionsByFilter(filterModel);

    //        return View(ses);
    //    }


    //    public IActionResult Session(Session session)
    //    {


    //        var ses = _context.GetSessionById(session.Id);


    //        if (ses == null)
    //            return View("NoSession");

    //        if (ses.Сompleted)
    //            return View("Details", ses);


    //        return View(ses);
    //    }


    //    public IActionResult Details(Session session)
    //    {


    //        var ses = _context.GetSessionById(session.Id);

    //        if (ses == null)
    //            return View("NoSession");



    //        return View(ses);
    //    }


    //    public IActionResult Users()
    //    {
    //        var users = _userManager.Users.Include(x => x.Sessions).OrderByDescending(x => x.Sessions.Count).ToList();

    //        return View(users);
    //    }
    }
}


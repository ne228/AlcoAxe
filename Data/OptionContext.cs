using AlcoAxe.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_OP_Web.Data;
using Test_OP_Web.Models;

namespace Test_OP_Web
{
    public class OptionContext : IdentityDbContext<UserAxe>
    {


        public OptionContext(DbContextOptions<OptionContext> options) : base(options)
        {


        }


        public DbSet<Question> Questions { get; set; }

        public DbSet<CookieUser> CookieUsers { get; set; }
        public DbSet<Game> Games { get; set; }

        public DbSet<GameType> GameTypes { get; set; }
        public DbSet<SampleQuestion> SampleQuestions { get; set; }




        //public DbSet<Question> Questions { get; set; }

        //public DbSet<Session> Sessions { get; set; }

        //public DbSet<SessionQuestion> SessionQuestion { get; set; }



    }
}



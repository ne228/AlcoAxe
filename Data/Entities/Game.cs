using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlcoAxe.Data
{
    public class Game
    {

        public int Id { get; set; }


        [Display(Name = "Тип игры")]
        public GameType GameType { get; set; }


        [Display(Name = "Сложность игры")]
        public GameMode GameMode { get; set; }


        [Display(Name = "Вопрос")]
        public List<Question> Questions { get; set; }

        [Display(Name = "Игрок")]
        public List<Player> Players { get; set; }

        [Display(Name = "Дата")]
        public DateTime DateTime { get; set; }

        public CookieUser User { get; set; }

    }
}

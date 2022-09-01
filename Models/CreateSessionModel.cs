using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Test_OP_Web.Models
{
    public class CreateSessionModel
    {

        [Required(ErrorMessage = "Не указан вариант")]
        [Range(1, 25,ErrorMessage ="Введите вариант от 1 до 25")]
        public int NumVar { get; set; }

        [Required(ErrorMessage = "Не указао имя")]
        public string Name { get; set; }
    }
}

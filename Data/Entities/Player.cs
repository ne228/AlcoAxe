using System.ComponentModel.DataAnnotations;

namespace AlcoAxe.Data
{
    public class Player
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя игрока")]
        public string Name { get; set; }

        
    }
}
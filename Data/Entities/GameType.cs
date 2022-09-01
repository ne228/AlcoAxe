using System.ComponentModel.DataAnnotations;

namespace AlcoAxe.Data
{
    public class GameType
    {

        public int Id { get; set; }
        
        [Display(Name="Тип игры")]
        public string NameGameType { get; set; }
    }
}
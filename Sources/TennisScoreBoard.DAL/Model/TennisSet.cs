using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisScoreBoard.EF.Model
{
    public class TennisSet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ICollection<Game> Games { get; set; }
        public TennisPlayer Winner { get; set; }

        public TennisSet()
        {
            Games = new List<Game>
            {
                new Game()
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TennisScoreBoard.EF
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

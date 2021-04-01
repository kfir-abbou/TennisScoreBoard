using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisScoreBoard.EF.Model
{
    public class GameScores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        public Game Game { get; set; }

        public int ScoringPlayer { get; set; } // first = 0, second = 1
    }
}

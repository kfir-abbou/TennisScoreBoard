using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TennisScoreBoard.EF
{
    public class GameScores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Game Game { get; set; }

        public int ScoringPlayer { get; set; } // first = 0, second = 1
    }
}

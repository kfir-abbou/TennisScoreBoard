using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TennisScoreBoard.EF
{
    public class TennisMatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public TennisPlayer Winner { get; set; }
        public ICollection<TennisSet> Sets { get; set; }
        public TennisPlayer FirstPlayer { get; set; }
        public TennisPlayer SecondPlayer { get; set; }
        public bool IsOver { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisScoreBoard.EF
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; }

        public TennisPlayer Winner { get; private set; }

        public void SetGameWinner(TennisPlayer player)
        {
            Winner = player ?? throw new ArgumentException("Player is null ");
        }
    }
}
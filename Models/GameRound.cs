using System.Data;

namespace MemeMatch.Models
{
    public class GameRound
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int PromptId { get; set; }

        public int SelectedMemeId { get; set; }

        public bool IsCorrect { get; set; }

        public int Score { get; set; }

        public DateTime PlayedAt { get; set; } = DateTime.Now;

        public User User { get; set; } = null!;
    }
}

using Microsoft.AspNetCore.Identity;

namespace MemeMatch.Models
{
    public class User : IdentityUser
    {
        public List<GameRound> GameRounds { get; set; } = new();
    }
}

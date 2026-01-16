using System.ComponentModel.DataAnnotations;

namespace MemeMatch.Models
{
    public class Prompt
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Text { get; set; } = string.Empty;

        public int CorrectMemeId { get; set; }
        public Meme CorrectMeme { get; set; }
    }
}

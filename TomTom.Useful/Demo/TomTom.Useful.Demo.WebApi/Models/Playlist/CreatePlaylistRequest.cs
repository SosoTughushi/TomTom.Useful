using System.ComponentModel.DataAnnotations;

namespace TomTom.Useful.Demo.WebApi.Models.Playlist
{
    public record CreatePlaylistRequest
    {
        [Required]
        public string Title { get; set; } = String.Empty;
    }
}
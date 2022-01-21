using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomTom.Useful.CQRS;
using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Demo.WebApi.Models.Playlist;
using TomTom.Useful.Demo.WebApi.Models;
using TomTom.Useful.Demo.Application;

namespace TomTom.Useful.Demo.WebApi.Controllers
{
    [Route("api/playlists")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistWriter playlistWriter;

        public PlaylistsController(IPlaylistWriter playlistWriter)
        {
            this.playlistWriter = playlistWriter;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistRequest request)
        {
            var context = this.ControllerContext.ToDemoAppContext();
            var result = await this.playlistWriter.Create(request.Title, context);

            return this.Accepted(new AcceptedResponse(result.Id));
        }

        [HttpPost("published")]
        public async Task<IActionResult> PublishPlaylist([FromBody] PublishPlaylistRequest request)
        {
            var context = this.ControllerContext.ToDemoAppContext();

            var result = await this.playlistWriter.Publish(request.PlaylistId, context);

            return this.Accepted(new AcceptedResponse(result.Id));
        }


    }
}

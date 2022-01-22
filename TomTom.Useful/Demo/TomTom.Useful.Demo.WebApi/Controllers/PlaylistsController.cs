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

        [HttpGet("{playlistId}")]
        public IActionResult GetPlaylist([FromRoute] string playlistId)
        {
            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistRequest request)
        {
            var context = this.ControllerContext.ToDemoAppContext();
            var result = await this.playlistWriter.Create(request.Title, context);

            if(result.Success)
            {
                return this.CreatedAtAction(nameof(GetPlaylist), new { playlistId = result.Value }, new { id = result.Value });
            }

            switch(result.Error)
            {
                case CreatePlaylistFailureReason.TitleAlreadyExists:
                    return this.BadRequest("Title Already Exists");

                case CreatePlaylistFailureReason.Unknown:
                default:
                    return this.BadRequest();
            }
        }

        [HttpPost("published")]
        public async Task<IActionResult> PublishPlaylist([FromBody] PublishPlaylistRequest request)
        {
            var context = this.ControllerContext.ToDemoAppContext();

            var result = await this.playlistWriter.Publish(request.PlaylistId, context);

            
            if(result.Success)
            {
                return this.Ok();
            }

            switch(result.Error)
            {
                case PublishPlaylistFailureReason.AlreadyPublished:
                    return this.Conflict();
                case PublishPlaylistFailureReason.Unknown:
                default:
                    return this.BadRequest();
            }
        }


    }
}

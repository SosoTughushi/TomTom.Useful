using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomTom.Useful.CQRS;
using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Demo.WebApi.Models.Playlist;
using TomTom.Useful.Demo.WebApi.Models;
using TomTom.Useful.Demo.Application.Playlist;
using MediatR;
using TomTom.Useful.Demo.Application.Commands.Playlist;
using TomTom.Useful.Demo.Application.Queries.Playlists;

namespace TomTom.Useful.Demo.WebApi.Controllers
{
    [Route("api/playlists")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IMediator mediator;

        public PlaylistsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlaylistModel>))]
        public async Task<IActionResult> GetAll()
        {
            var context = this.ControllerContext.ToDemoAppContext();
            var playlistDtos = await this.mediator.Send(new GetPlaylists(context));

            var model = playlistDtos.Select(MapToModel).ToList();

            return this.Ok(model);
        }

        [HttpGet("{playlistId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaylistModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlaylist([FromRoute] Guid playlistId)
        {
            var context = this.ControllerContext.ToDemoAppContext();
            var playlistDto = await this.mediator.Send(new GetPlaylist(playlistId, context));

            if (playlistDto == null)
            {
                return this.NotFound();
            }

            var model = MapToModel(playlistDto);

            return this.Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistRequest request)
        {
            var context = this.ControllerContext.ToDemoAppContext();
            var command = new CreatePlaylistCommand(request.Title, context);
            var result = await this.mediator.Send(command);

            if (result.Success)
            {
                return this.CreatedAtAction(nameof(GetPlaylist), new { playlistId = result.Value }, new { id = result.Value });
            }

            switch (result.Error)
            {
                case CreatePlaylistFailureReason.TitleAlreadyExists:
                    return this.BadRequest("Title Already Exists");

                case CreatePlaylistFailureReason.Unknown:
                default:
                    return this.BadRequest();
            }
        }

        [HttpPut("{playlistId}/publish")]
        public async Task<IActionResult> PublishPlaylist([FromRoute] Guid playlistId)
        {
            var context = this.ControllerContext.ToDemoAppContext();
            var command = new PublishPlaylistCommand(playlistId, context);
            var result = await this.mediator.Send(command);


            if (result.Success)
            {
                return this.Ok();
            }

            switch (result.Error)
            {
                case PublishPlaylistFailureReason.AlreadyPublished:
                    return this.Conflict();
                case PublishPlaylistFailureReason.Unknown:
                default:
                    return this.BadRequest();
            }
        }

        private static PlaylistModel MapToModel(PlaylistDto playlistDto)
        {
            return new PlaylistModel(playlistDto.Id, playlistDto.Title, playlistDto.IsPublished);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Demo.Application.Playlist
{
    public record PlaylistDto(Guid Id, string Title, bool IsPublished);
}

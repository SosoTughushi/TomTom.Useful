using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Demo.Application.Projections
{
    public record ProjectedPlaylist(Guid Id, Guid OwnerId, string Title, bool IsPublished, long Version);
}

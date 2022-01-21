using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.CQRS
{
    public class CommandBase<TTargetIdentity> : ICommand<TTargetIdentity>
    {
        /// <summary>
        /// Used for serialization and deserialization if necessary
        /// </summary>
        public CommandBase()
        {
        }

        public CommandBase(TTargetIdentity targetIdentity, Guid id, string causedById, string correlationId)
        {
            TargetIdentity = targetIdentity;
            Id = id;
            CorrelationId = correlationId;
            CausedById = causedById;
        }

        public TTargetIdentity TargetIdentity { get; set; }

        public Guid Id { get; set; }

        public string CorrelationId { get; set; }

        public string? CausedById { get; set; }
    }

    public class CreateCommandBase<TTargetIdentity> : ICreateCommand<TTargetIdentity>
    {
        public CreateCommandBase()
        {
        }

        public CreateCommandBase(Guid id, string causedById, string correlationId)
        {
            Id = id;
            CorrelationId = correlationId;
            CausedById = causedById;
        }

        public Guid Id { get; set; } = Guid.Empty;

        public string CorrelationId { get; set; } = string.Empty;

        public string? CausedById { get; set; }

        public TTargetIdentity TargetIdentity { get; } = default;
    }
}

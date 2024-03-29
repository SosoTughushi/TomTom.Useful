﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Messaging;

namespace TomTom.Useful.CQRS
{
    public interface ICommand : IMessage
    {
    }

    public interface ICommand<TTargetIdentity> : ICommand
    {
        TTargetIdentity TargetIdentity { get; }
    }

    public interface ICreateCommand<TargetIdentity> : ICommand<TargetIdentity>
    {
    }

}

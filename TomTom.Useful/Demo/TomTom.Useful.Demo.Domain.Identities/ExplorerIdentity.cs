using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Demo.Domain.Identities
{
    public struct ExplorerIdentity : IEquatable<ExplorerIdentity>, IComparable<ExplorerIdentity>
    {
        public ExplorerIdentity(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public int CompareTo(ExplorerIdentity other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(ExplorerIdentity other)
        {
            return this.Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static implicit operator Guid(ExplorerIdentity err) => err.Value;

        public static implicit operator ExplorerIdentity(Guid err) => new(err);

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not ExplorerIdentity) return false;
            return Equals((ExplorerIdentity)obj);
        }

        public static bool operator ==(ExplorerIdentity left, ExplorerIdentity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExplorerIdentity left, ExplorerIdentity right)
        {
            return !(left == right);
        }
    }
}

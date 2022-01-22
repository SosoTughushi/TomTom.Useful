namespace TomTom.Useful.Demo.Domain.Identities
{
    public struct PlaylistIdentity : IEquatable<PlaylistIdentity>, IComparable<PlaylistIdentity>
    {
        public PlaylistIdentity(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(PlaylistIdentity other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(PlaylistIdentity other)
        {
            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not PlaylistIdentity) return false;
            return Equals((PlaylistIdentity)obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static implicit operator Guid(PlaylistIdentity err) => err.Value;

        public static implicit operator PlaylistIdentity(Guid err) => new(err);


        public static bool operator ==(PlaylistIdentity left, PlaylistIdentity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlaylistIdentity left, PlaylistIdentity right)
        {
            return !(left == right);
        }
    }
}
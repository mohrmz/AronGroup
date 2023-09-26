namespace AronGroup.Models.Base;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public interface IEntity
{
}

public abstract class BaseEntity<TKey> : IEntity where TKey : IEquatable<TKey>
{
    public TKey Id { get; protected set; }

    public override bool Equals(object? obj)
    {
        var compareTo = obj as BaseEntity<TKey>;

        if (ReferenceEquals(this, compareTo))
            return true;

        if (ReferenceEquals(null, compareTo))
            return false;

        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(BaseEntity<TKey> left, BaseEntity<TKey> right)
    {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            return true;

        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(BaseEntity<TKey> left, BaseEntity<TKey> right) => !(left == right);

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * 907) + Id.GetHashCode();
    }

}

public abstract class BaseEntity : BaseEntity<int>
{
}

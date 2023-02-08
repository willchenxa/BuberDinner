using System.Diagnostics.CodeAnalysis;

using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.Host.ValueObjects;

public sealed class HostId : ValueObject
{
    public Guid Value { get; }

    private HostId(Guid value)
    {
        Value = value;
    }

    public static HostId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static HostId Create(string hostId)
    {
        return new(new Guid(hostId));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
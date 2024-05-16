using System;
using Sqids;

namespace BottleSplitter.Services;

public interface ISquidGenerator
{
    string GetSquid();
}

public class SquidGenerator : ISquidGenerator
{
    private readonly Random _random = new();
    private readonly SqidsEncoder<long> _sqidsEncoder = new(new() { MinLength = 10 });

    public string GetSquid()
    {
        return _sqidsEncoder.Encode(_random.NextInt64(0, long.MaxValue));
    }
}

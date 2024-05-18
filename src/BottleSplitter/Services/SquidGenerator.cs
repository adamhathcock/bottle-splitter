using System;
using Speckle.InterfaceGenerator;
using Sqids;

namespace BottleSplitter.Services;

[GenerateAutoInterface]
public class SquidGenerator : ISquidGenerator
{
    private readonly Random _random = new();
    private readonly SqidsEncoder<long> _sqidsEncoder = new(new() { MinLength = 10 });

    public string GetSquid()
    {
        return _sqidsEncoder.Encode(_random.NextInt64(0, long.MaxValue));
    }
}

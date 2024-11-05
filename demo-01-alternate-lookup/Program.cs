using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using Bogus;

using System.Text;

BenchmarkRunner.Run<Benchmarks>();

[MemoryDiagnoser]
public class Benchmarks
{
    [Benchmark(Baseline = true)]
    public int Split_Lookup_With_Strings()
    {
        var foundMatches = 0;

        var set = TestData.Names;

        foreach (var line in TestData.Lines)
        {
            foreach (var name in line.Split(';'))
            {
                if (set.Contains(name))
                    foundMatches++;
            }
        }

        return foundMatches;
    }

    [Benchmark]
    public int Split_Lookup_With_Spans()
    {
        var foundMatches = 0;

        var set = TestData.Names.GetAlternateLookup<ReadOnlySpan<char>>();

        foreach (var line in TestData.Lines)
        {
            var lineSpan = line.AsSpan();
            foreach (var nameRange in lineSpan.Split(';'))
            {
                var nameSpan = lineSpan[nameRange];
                if (set.Contains(nameSpan))
                    foundMatches++;
            }
        }

        return foundMatches;
    }

    // Results:
    //
    // | Method                    | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0      | Allocated  | Alloc Ratio |
    // |-------------------------- |---------:|---------:|---------:|------:|--------:|----------:|-----------:|------------:|
    // | Split_Lookup_With_Strings | 21.21 ms | 0.160 ms | 0.142 ms |  1.00 |    0.01 | 2781.2500 | 23272474 B |       1.000 |
    // | Split_Lookup_With_Spans   | 21.59 ms | 0.380 ms | 0.355 ms |  1.02 |    0.02 |         - |       34 B |       0.000 |
}

public sealed class TestData
{
    public const int LineCount = 10000;

    public const int MinNamesPerLine = 20;
    public const int MaxNamesPerLine = 80;

    public const float NameInclusionProbability = 0.03f;
    
    public static string[] Lines { get; }

    public static HashSet<string> Names { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    static TestData()
    {
        Randomizer.Seed = new Random(6532632);

        var faker = new Faker();

        var lines = new List<string>(LineCount);
        var sb = new StringBuilder();

        for (var i = 0; i < LineCount; i++)
        {
            var nameCount = faker.Random.Int(MinNamesPerLine, MaxNamesPerLine);


            sb.Clear();

            for (var j = 0; j < nameCount; j++)
            {
                var name = faker.Name.LastName();

                if (faker.Random.Float(0, 1) <= NameInclusionProbability)
                    Names.Add(name);

                if (sb.Length > 0)
                    sb.Append(';');

                sb.Append(name);
            }

            var line = sb.ToString();
            lines.Add(line);
        }

        Lines = lines.ToArray();
    }
}
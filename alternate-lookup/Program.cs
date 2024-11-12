BenchmarkRunner.Run<Benchmarks>();

[MemoryDiagnoser]
public class Benchmarks
{
    [Benchmark(Baseline = true)]
    public int Split_Lookup_With_Strings()
    {
        int foundMatches = 0;

        HashSet<string> set = TestData.Names;
        string[] lines = TestData.Lines;

        foreach (string line in lines)
        {
            foreach (string name in line.Split(';'))
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
        int foundMatches = 0;

        #region The Trick
        HashSet<string>.AlternateLookup<ReadOnlySpan<char>> set = TestData.Names.GetAlternateLookup<ReadOnlySpan<char>>();
        #endregion

        foreach (string line in TestData.Lines)
        {
            ReadOnlySpan<char> lineSpan = line.AsSpan();
            foreach (Range nameRange in lineSpan.Split(';'))
            {
                ReadOnlySpan<char> nameSpan = lineSpan[nameRange];
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

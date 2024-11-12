using System.Diagnostics;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Starting...");

        var t1 = CountToAsync(1);
        var t2 = CountToAsync(2);
        var t3 = CountToAsync(3);

        var stopwatch = Stopwatch.StartNew();

        var tasks = new[] { t1, t2, t3 };
        await Task.WhenAll(tasks);

        foreach (var task in tasks)
            Console.WriteLine($"[{stopwatch.Elapsed}] Task {task.Result} is finished.");
    }

    private static async Task<int> CountToAsync(int number)
    {
        for (var i = 0; i < number; i++)
            await Task.Delay(TimeSpan.FromSeconds(1));

        return number;
    }
}
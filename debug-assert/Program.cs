using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("How old are you?");
            var ageText = Console.ReadLine();
            if (string.IsNullOrEmpty(ageText))
                return;

            if (!int.TryParse(ageText, out var age))
                Console.WriteLine("Version isn't valid");
            else
                PrintResults(age);
        }
    }

    private static void PrintResults(int age)
    {
        Debug.Assert(age >= 0);

        var year = DateTime.Now.Year - age;
        Console.WriteLine($"You were born in the year {year}.");
    }
}

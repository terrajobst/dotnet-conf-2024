using System;

internal static class Program
{
    private static readonly List<string> _people =
    [
        "Immo Landwerth",
        "Richard Lander",
        "Jared Parsons",
        "Stephen Toub",
        "David Fowler"
    ];

    private static int _selectedIndex;

    private static void Main(string[] args)
    {
        while (true)
        {
            Render();

            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Escape)
            {
                return;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                _selectedIndex = Math.Max(0, _selectedIndex - 1);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                _selectedIndex = Math.Min(_selectedIndex + 1, _people.Count - 1);
            }
        }
    }

    private static void Render()
    {
        Console.Clear();

        foreach (var person in _people)
        {
            var selected = _people.IndexOf(person) == _selectedIndex;

            if (selected)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkCyan;
            }

            Console.WriteLine(person);
            Console.ResetColor();
        }
    }
}
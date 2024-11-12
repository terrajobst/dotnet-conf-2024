using Bogus;

using System.Text;

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

        Faker faker = new Faker();

        List<string> lines = new List<string>(LineCount);
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < LineCount; i++)
        {
            int nameCount = faker.Random.Int(MinNamesPerLine, MaxNamesPerLine);


            sb.Clear();

            for (int j = 0; j < nameCount; j++)
            {
                string name = faker.Name.LastName();

                if (faker.Random.Float(0, 1) <= NameInclusionProbability)
                    Names.Add(name);

                if (sb.Length > 0)
                    sb.Append(';');

                sb.Append(name);
            }

            string line = sb.ToString();
            lines.Add(line);
        }

        Lines = lines.ToArray();
    }
}
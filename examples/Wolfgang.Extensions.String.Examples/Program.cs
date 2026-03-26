using System;

namespace Wolfgang.Extensions.String.DotNet8.Examples;

internal abstract class Program
{
    private static void Main()
    {
        Left();
        PadCenter();
        ToXxxCase();
    }



        private static void PadCenter()
        {
            Console.WriteLine("\tPadCenter\n\n");

            Console.WriteLine($"\t{"Name".PadCenter(10)}|{"Date".PadCenter(12)}");
            Console.WriteLine("\t----------+------------");
            Console.WriteLine($"\t{"Steve",-10}|{DateTime.Today.ToString("MM/dd/yyyy").PadCenter(12)}");
            Console.WriteLine($"\t{"Jane",-10}|{new DateTime(2012, 1, 2).ToString("M/d/yyyy").PadCenter(12)}");
            Console.WriteLine();
        }


    private static void Left()
    {
        Console.WriteLine("\n");
        Console.ForegroundColor = ConsoleColor.White;

        var s = "A sample string for processing";
        Console.WriteLine($"\tTest string: \"{s}\".\n");

        Console.WriteLine($"\t\tLeft(10): \"{s.Left(10) ?? "<null>"}\"");

        Console.WriteLine($"\t\tLeft(30): \"{s.Left(30) ?? "<null>"}\"");

        Console.WriteLine($"\t\tLeft(0):  \"{s.Left(0) ?? "<null>"}\"");
        Console.WriteLine($"\t\tRight(10): \"{s.Right(10) ?? "<null>"}\"");

        Console.WriteLine($"\t\tRight(30): \"{s.Right(30) ?? "<null>"}\"");

        Console.WriteLine($"\t\tRight(0):  \"{s.Right(0) ?? "<null>"}\"");

        Console.WriteLine("\n");

        s = "";
        Console.WriteLine("\tTest with blank string \"\".\n");

        Console.WriteLine($"\t\tLeft(10): \"{s.Left(10) ?? "<null>"}\"");

        Console.WriteLine($"\t\tRight(10): \"{s.Right(10) ?? "<null>"}\"");
        Console.WriteLine("\n");


        s = null;
        Console.WriteLine("\tTest with null string.\n");

        Console.WriteLine($"\t\tLeft(10): \"{s.Left(10) ?? "<null>"}\"");

        Console.WriteLine($"\t\tRight(10): \"{s.Right(10) ?? "<null>"}\"");
        Console.WriteLine("\n");

        Console.ResetColor();
    }



    private static void ToXxxCase()
    {
        var testString = " A sample string for processing ";

        Console.WriteLine("\n");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Sample string: \"{testString}\"\n");
        Console.WriteLine($"\tToCamelCase: \"{testString.ToCamelCase()}\".\n");
        Console.WriteLine($"\tToKebabCase: \"{testString.ToKebabCase()}\".\n");
        Console.WriteLine($"\tToPascalCase: \"{testString.ToPascalCase()}\".\n");
        Console.WriteLine($"\tToSnakeCase: \"{testString.ToSnakeCase()}\".\n");
        Console.WriteLine($"\tToTitleCase: \"{testString.ToTitleCase()}\".\n");
        
        Console.WriteLine("\n");
    }

}

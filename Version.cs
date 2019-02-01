using System;
using System.Reflection;

namespace packater
{
    public static class Version
    {
        public static string AsString()
        {
            var versionString = Assembly.GetEntryAssembly()
                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                        .InformationalVersion
                        .ToString();
            return $"packater v{versionString}";
        }

        public static void Print()
        {
            var versionString = Version.AsString();
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(versionString);
            for (var i = 0; i < versionString.Length; i++)
            {
                Console.Write('-');
            }
            Console.WriteLine();
            Console.ForegroundColor = originalColor;
        }
    }
}
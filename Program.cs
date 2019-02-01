using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.XPath;

namespace packater
{
    class Program
    {
        static void Main(string[] args)
        {
            Version.Print();
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            Console.WriteLine($"Finding projects in {currentDirectory}");

            var projectPaths = GetProjects(currentDirectory);

            foreach (var file in projectPaths)
            {
                Console.WriteLine(file);
                XPathDocument document = new XPathDocument(file);
                XPathNavigator navigator = document.CreateNavigator();
                XPathNodeIterator nodes = navigator.Select("/Project/ItemGroup/PackageReference");
                while (nodes.MoveNext())
                {
                    var packageName = nodes.Current.GetAttribute("Include", "");
                    UpdatePackage(file, packageName);
                }
            }
        }

        private static string[] GetProjects(string path)
        {
            var enumerationOptions = new EnumerationOptions
            {
                MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = true
            };
            var files = System.IO.Directory.GetFiles(path, "*.csproj", enumerationOptions);
            return files;
        }

        private static void UpdatePackage(string projectPath, string packageName)
        {
            Console.WriteLine($"\tAttempting to update package {packageName}");
            using (var process = new Process())
            {
                //The executable which is recognized by PATH
                process.StartInfo.FileName = $"dotnet";
                //The arguments to pass to the executable
                process.StartInfo.Arguments = $"add {projectPath} package {packageName}";
                //Do not open a new console window
                process.StartInfo.CreateNoWindow = true;
                //Do not open with the default program
                process.StartInfo.UseShellExecute = false;
                //Redirect the standard input and output to this console
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.OutputDataReceived += (sender, @event) =>
                {
                    Console.WriteLine(@event.Data);
                };
                process.ErrorDataReceived += (sender, @event) =>
                {
                    Console.WriteLine(@event.Data);
                };
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }
    }
}
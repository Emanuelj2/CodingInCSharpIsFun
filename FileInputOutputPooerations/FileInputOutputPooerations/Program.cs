using System.IO;
using System.Security.Authentication.ExtendedProtection;

namespace FileInputOutputPooerations
{
    class Program
    {
        static void Main(string[] args)
        {
            // IMPORTANT: Use @ for verbatim strings to avoid escaping backslashes
            string filePath = @"C:\Users\emanu\OneDrive\TestFolder\test.txt";
            
            File.WriteAllText(filePath, "Hello, World!");

            File.AppendAllText(filePath, "\nThis is an appended line.");

            //write multiple lines
            string[] lines = { "First line", "Second line", "Third line" };
            File.AppendAllText(filePath, "\n" + string.Join("\n", lines));

            //reading from a file
            string content = File.ReadAllText(filePath);
            Console.WriteLine(content);

            Console.WriteLine("Reading line by line:");
            //read all lines into an array
            string[] readLines = File.ReadAllLines(filePath);
            foreach (string line in readLines)
            {
                Console.WriteLine(line);
            }

            //chaech if the file exists
            if (File.Exists(filePath))
            {
                Console.WriteLine("File exist!");
            }

            //create a directory
            Directory.CreateDirectory(@"C:\Users\emanu\OneDrive\TestFolder\NewFolder");

            //chech if the directory exists
            if (Directory.Exists(@"C:\Users\emanu\OneDrive\TestFolder\NewFolder"))
            {
                Console.WriteLine("Directory exists!");
                
            }
        }


    }
}



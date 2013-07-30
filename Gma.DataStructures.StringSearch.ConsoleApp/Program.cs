using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gma.DataStructures.StringSearch.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quit = false;
            do
            {
                Console.Write("{0}>", Directory.GetCurrentDirectory());
                var input = Console.ReadLine();
                input = input ?? string.Empty;
                input = input.Trim().ToLower();
                var splitedInput = input.Split(' ').Select(part => part.Trim());
                var command = splitedInput.First();
                var arguments = splitedInput.Skip(1).ToArray();

                switch (command)
                {
                    case "ls":
                    case "dir":
                        ListDirectoryContent(arguments);
                        break;

                    case "quit":
                        quit = true;
                        break;

                    default:
                        PrintSupportedCommands();
                        break;
                }



            } while (!quit);
        }

        private static void PrintSupportedCommands()
        {
            Console.WriteLine("Supported commands are: 'ls [*.txt]', 'cd [dir//name]', 'load [filename.txt]'");
        }

        private static void ListDirectoryContent(string[] arguments)
        {
            string directory;
            try
            {
                directory = arguments.Length == 0 ? Directory.GetCurrentDirectory() : arguments[0];
            } 
            catch(UnauthorizedAccessException accessException)
            {
                Console.WriteLine(accessException.Message);
                return;
            }
            catch(NotSupportedException notSupportedException)
            {
                Console.WriteLine(notSupportedException.Message);
                return;
            }

            string[] files;
            try
            {
                files = Directory.GetFiles(directory);
            } 
            catch(IOException ioException)
            {
                Console.WriteLine(ioException.Message);
                return;
            }
            catch(UnauthorizedAccessException unauthorizedAccessException)
            {
                Console.WriteLine(unauthorizedAccessException.Message);
                return;
            }
            catch(ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
                return;
            }

            foreach (var file in files)
            {
                Console.WriteLine(Path.GetFileName(file));
            }
        }
    }
}

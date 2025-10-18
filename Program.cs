using System;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using NAudio.Wave;

namespace BehringerPolywavConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Introduction
            Console.WriteLine("Polywav converter for Behringer Wing based records.");
            Console.WriteLine("Usage: behringer-polywav-converter <input_folder> <output_folder>");

            // Check if the input arguments are given
            if (args.Length < 2)
            {
                Console.Write("Not enough arguments provided. ");
                Console.WriteLine("Usage: behringer-polywav-converter <input_folder> <output_folder>");
                return;
            }

            string inputDir = args[0];
            string outputDir = args[1];

            ValidateDirectory(directory: inputDir, shouldCreate: false, shouldBeEmpty: false);
            ValidateDirectory(directory: outputDir, shouldCreate: true, shouldBeEmpty: true);
        }

        public static void ValidateDirectory(string directory, bool shouldCreate, bool shouldBeEmpty)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string absolutePathToDirToCreate = $"{currentDir}/{directory}";

            try
            {
                FileAttributes attr = File.GetAttributes(directory);
            }
            catch (FileNotFoundException)
            {
                // Check if the directory exists (or if the given path is a file)
                if (!Directory.Exists(directory))
                {
                    Console.WriteLine($"Directory at \"{absolutePathToDirToCreate}\" does not exist.");

                    // If it doesn't exist and it should be created then we create it here
                    if (shouldCreate)
                    {
                        Console.WriteLine($"Creating new directory at \"{absolutePathToDirToCreate}\"...");
                        Directory.CreateDirectory(directory);
                    }
                }
            }

            // If the directory exists, check if it's empty and ask the user to delete it first.
            if (shouldBeEmpty && Directory.GetFileSystemEntries(directory).Length != 0)
            {
                Console.WriteLine($"Directory at \"{absolutePathToDirToCreate}\" directory is not empty. Please clean the output folder first!");
            }
        }
    }
}

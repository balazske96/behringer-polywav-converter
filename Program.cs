using System;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
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
                Console.WriteLine("Usage: behringer-polywav-converter <input_folder> <output_folder>");
                return;
            }

            string inputDir = args[0];
            string outputDir = args[1];

            // Check if the directory exists (or if the given path is a file)
            try
            {
                FileAttributes attr = File.GetAttributes(outputDir);
            }
            catch (FileNotFoundException)
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                    Console.WriteLine("Output directory does not exist. Creating...");
                }
            }

            // If the directory exists, check if it's empty and ask the user to delete it first.
            if (Directory.GetFileSystemEntries(outputDir).Length != 0)
            {
                Console.WriteLine("Output directory is not empty. Please clean the output folder first!");
            }
        }
    }
}

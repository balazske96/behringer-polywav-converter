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

            // Check for WAV files in the input directory
            var files = Directory.GetFiles(inputDir, "*.wav").OrderBy(f => f).ToList();
            if (files.Count == 0)
            {
                Console.WriteLine($"❌ No WAV files found in {inputDir}");
                return;
            }

            // Read out channel sizes and sample rate from the first file
            using var firstReader = new WaveFileReader(files[0]);
            int channels = firstReader.WaveFormat.Channels;
            int sampleRate = firstReader.WaveFormat.SampleRate;
            Console.WriteLine($"🎚️ Detected {channels} channels at {sampleRate} Hz");
            Console.WriteLine($"⚙️ Processing {files.Count} files...");

            // Allocate writers for each channel
            var writers = new WaveFileWriter[channels];
            for (int channelIndex = 0; channelIndex < channels; channelIndex++)
            {
                string outPath = Path.Combine(outputDir, $"channel_{channelIndex + 1:D2}.wav");
                writers[channelIndex] = new WaveFileWriter(outPath, new WaveFormat(sampleRate, 1));
            }

            // Process each file
            foreach (var file in files)
            {
                Console.WriteLine($"→ {Path.GetFileName(file)}");
                using var reader = new WaveFileReader(file);
                var provider = reader.ToSampleProvider();

                // Define the block size for reading audio samples
                int blockSize = 4096;

                // Create a buffer to hold audio samples for all channels
                // The buffer size is blockSize multiplied by the number of channels
                float[] buffer = new float[blockSize * channels];

                // Variable to store the number of samples read in each iteration
                int read;

                // Read audio samples in chunks until the end of the file
                while ((read = provider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Iterate through the buffer in steps of the number of channels
                    for (int i = 0; i < read; i += channels)
                    {
                        // Write each channel's sample to its corresponding output file
                        for (int c = 0; c < channels; c++)
                        {
                            // Ensure the index is within bounds before writing the sample
                            if (i + c < read)
                                writers[c].WriteSample(buffer[i + c]);
                        }
                    }
                }
            }

            // Clean up and close all writers
            foreach (var w in writers) w.Dispose();
            Console.WriteLine($"✅ Done! Created {channels} WAV files in {outputDir}");
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
                Console.WriteLine($"❌ Directory at \"{absolutePathToDirToCreate}\" directory is not empty. Please clean the output folder first!");
            }
        }
    }
}

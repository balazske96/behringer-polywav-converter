# Behringer Polywav Converter

This project is a tool for processing and converting multi-channel WAV files recorded with the Behringer Wing console. It splits multi-channel WAV files into separate mono WAV files for each channel.

## Requirements

To build and run this project, you need the following:

- **.NET SDK**: Version 8.0 or later. You can download it from [Microsoft's .NET website](https://dotnet.microsoft.com/).
- **NAudio Library**: This project uses the NAudio library for audio processing. The dependency is managed automatically via NuGet.

## How to Build and Run

### 1. Clone the Repository

First, clone this repository to your local machine:
```bash
git clone https://github.com/balazske96/behringer-polywav-converter.git
cd behringer-polywav-converter
```

### 2. Run the Program

Run the program using the following command:
```bash
dotnet run
```

### 3. Build the Project

If you want to create executables for multiple platforms, use the provided `build.sh` script:
```bash
./build.sh
```

This will generate self-contained executables for Windows, macOS (Intel and Apple Silicon), and Linux. The output files will be located in the `build/` directory.

### 4. Using the Program

When you run the program, it will prompt you to:
1. Enter the path to the input folder containing the WAV files.
2. Enter the path to the output folder where the processed files will be saved.

The program will process all WAV files in the input folder and split them into mono WAV files, saving the results in the output folder.

### Example

```plaintext
Polywav converter for Behringer Wing based records.
This program will ask for the input and output folders.

Please enter the path to the input folder: /path/to/input
Please enter the path to the output folder: /path/to/output
Processing file 1 of 3: track1.wav
Progress: 50.00%
Progress: 100.00%
Processing file 2 of 3: track2.wav
...
✅ Done! Created 8 WAV files in /path/to/output
```

## Features

- Splits multi-channel WAV files into separate mono WAV files.
- Provides progress feedback for each file being processed.
- Handles large files efficiently by processing in chunks.

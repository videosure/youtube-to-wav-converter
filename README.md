# youtube-to-wav-converter
A C# console app to download YouTube audio and convert it to WAV.

# YouTube to WAV Converter

A simple C# console application that downloads the audio from a YouTube video and converts it into a high-quality, uncompressed WAV file.

## Getting Started

To run this project, you will need the .NET SDK installed on your machine. You will also need to download and place FFmpeg in the project's execution directory.

### Prerequisites

* [.NET 6 SDK (or later)](https://dotnet.microsoft.com/en-us/download)
* **FFmpeg**

### Installation

1.  **Clone the repository or download the source code.**
2.  **Download FFmpeg:**
    * Go to the official FFmpeg download page: [https://ffmpeg.org/download.html](https://ffmpeg.org/download.html)
    * Download the latest build for your operating system (e.g., Windows).
    * Extract the ZIP file, and from the `bin` folder, copy `ffmpeg.exe`.
3.  **Place FFmpeg:** Paste the `ffmpeg.exe` file into the project's output directory after you build it. In Visual Studio, this is typically `[Your Project Folder]\bin\Debug\netX.0\`.
4.  **Build and Run:** Open the project in Visual Studio and run it.

## Usage

Run the application. You will be prompted to enter a YouTube video URL. Paste the URL and press Enter. The converted `.wav` file will be saved to your computer's default `Downloads` folder.

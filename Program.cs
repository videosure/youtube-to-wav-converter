using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("YouTube to WAV Converter");
        Console.WriteLine("------------------------");
        Console.Write("Enter a YouTube video URL: ");
        string videoUrl = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(videoUrl))
        {
            Console.WriteLine("Invalid URL. Exiting.");
            return;
        }

        try
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(videoUrl);
            string sanitizedTitle = SanitizeFileName(video.Title);

            Console.WriteLine($"Found video: {video.Title}");
            Console.WriteLine("Getting audio stream info...");

            // Get the best audio-only stream
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            var audioStreamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            if (audioStreamInfo == null)
            {
                Console.WriteLine("Could not find an audio stream for this video.");
                return;
            }

            Console.WriteLine($"Downloading audio stream ({audioStreamInfo.Size.MegaBytes:F2} MB)...");

            // --- UPDATED CODE ---
            // Get the path to the user's Downloads folder
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Define file paths
            string tempAudioFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.{audioStreamInfo.Container.Name}");
            string outputWavFilePath = Path.Combine(downloadsPath, $"{sanitizedTitle}.wav");
            // --- END OF UPDATE ---

            // Download the audio stream to a temporary file
            await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, tempAudioFilePath);
            Console.WriteLine("Download complete.");

            // Convert the downloaded audio to WAV using FFmpeg
            Console.WriteLine("Converting to WAV...");
            await ConvertToWavAsync(tempAudioFilePath, outputWavFilePath);

            // Clean up the temporary file
            File.Delete(tempAudioFilePath);

            Console.WriteLine($"Successfully converted video to WAV!");
            Console.WriteLine($"File saved at: {Path.GetFullPath(outputWavFilePath)}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }

    /// <summary>
    /// Uses FFmpeg to convert an input audio file to WAV format.
    /// </summary>
    private static Task ConvertToWavAsync(string inputFilePath, string outputFilePath)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg", // Assumes ffmpeg.exe is in the output directory or system PATH
            Arguments = $"-i \"{inputFilePath}\" \"{outputFilePath}\" -y", // -y overwrites output file if it exists
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        var process = new Process { StartInfo = processStartInfo };
        process.Start();

        // Asynchronously wait for the process to exit
        return process.WaitForExitAsync();
    }

    /// <summary>
    /// Removes characters from a string that are invalid for a file name.
    /// </summary>
    private static string SanitizeFileName(string fileName)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(c, '_');
        }
        return fileName;
    }
}

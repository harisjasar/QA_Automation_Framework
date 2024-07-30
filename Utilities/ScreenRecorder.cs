using System.Diagnostics;

namespace QA_AutomationFramework.Utilities
{
    public static class ScreenRecorder
    {
        private static Process ffmpegProcess;

        public static void StartRecording(string testName, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            string outputPath = Path.Combine(outputFolder, $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.mkv");
            string ffmpegArguments = $"-y -f gdigrab -framerate 30 -i desktop -r 30 {outputPath}";
            ffmpegProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = ffmpegArguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                }
            };
            ffmpegProcess.Start();
        }

        public static void StopRecording()
        {
            if (ffmpegProcess != null && !ffmpegProcess.HasExited)
            {
                try
                {
                    ffmpegProcess.StandardInput.WriteLine("q");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stopping FFmpeg process: {ex.Message}");
                    if (!ffmpegProcess.HasExited)
                    {
                        ffmpegProcess.Kill();
                    }
                }
                finally
                {
                    ffmpegProcess.Dispose();
                }
            }
        }
    }
}

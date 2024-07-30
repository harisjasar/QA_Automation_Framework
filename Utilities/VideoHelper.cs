namespace QA_AutomationFramework.Utilities
{
    public static class VideoHelper
    {
        public static void StartRecording(string testName, string outputFolder)
        {
            ScreenRecorder.StartRecording(testName, outputFolder);
        }

        public static void StopRecording()
        {
            ScreenRecorder.StopRecording();
        }

        public static string FindVideoFile(string testName, string recordingsDirectory)
        {
            string searchPattern = $"{testName}_*.mkv";

            var files = Directory.GetFiles(recordingsDirectory, searchPattern);
            if (files.Length > 0)
            {
                return files[0];
            }
            return null;
        }
    }

}

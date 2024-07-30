namespace QA_AutomationFramework.Configs
{
    public class TestSettings
    {
        public bool IncludeStepScreenshots { get; set; }
        public bool IncludeVideoRecording { get; set; }
        public EmailSettings EmailSettings { get; set; }
    }

    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Email {  get; set; }
        public string AppPassword { get; set; }
    }

}

﻿using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

namespace QA_AutomationFramework
{
    [SetUpFixture]
    public class GlobalSetupTeardown
    {
        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string reportFolder = Path.Combine(AppContext.BaseDirectory, "Reports", "Report");
            Directory.CreateDirectory(reportFolder);
            Directory.CreateDirectory(Path.Combine(reportFolder, "Recordings"));
            var consolidatedJsonReportPath = Path.Combine(reportFolder, "report.json");
            var extent = new ExtentReports();
            var spark = new ExtentSparkReporter(Path.Combine(reportFolder, "report.html"));
            var json = new ExtentJsonFormatter(consolidatedJsonReportPath);
            extent.AttachReporter(spark, json);
            var reportDirectories = Directory.GetDirectories(Path.Combine(AppContext.BaseDirectory, "Reports"), "Run_*");
            var jsonReports = reportDirectories.SelectMany(dir => Directory.GetFiles(dir, "report.json")).ToList();
            foreach (var jsonReport in jsonReports)
            {
                extent.CreateDomainFromJsonArchive(jsonReport);
            }

            foreach (var reportDirectory in reportDirectories)
            {
                var recordingsDirectory = Path.Combine(reportDirectory, "Recordings");
                if (Directory.Exists(recordingsDirectory))
                {
                    var videoFiles = Directory.GetFiles(recordingsDirectory, "*.mkv");
                    foreach (var videoFile in videoFiles)
                    {
                        var destFile = Path.Combine(reportFolder, "Recordings", Path.GetFileName(videoFile));
                        File.Move(videoFile, destFile);
                    }
                }
            }

            extent.Flush();

            foreach (var reportDirectory in reportDirectories)
            {
                Directory.Delete(reportDirectory, true);
            }
        }

    }
}

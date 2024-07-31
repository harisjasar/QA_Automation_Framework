![Workflow Status](https://img.shields.io/github/actions/workflow/status/harisjasar/QA_Automation_Framework/ci.yml?branch=master&label=tests)

# QA Automation Framework

This repository contains a comprehensive QA Automation Framework designed for robust, flexible, and scalable test automation. Below is a brief overview of its features and capabilities.

## Features

### Extent Reports
The framework uses ExtentReports for generating HTML reports. These reports include detailed logs, screenshots, and video recordings of test steps, providing comprehensive insights into test execution. Additionally, the framework generates a JSON file which can be integrated into ReportPortal to track the progress of the test over time.

### Flexible Reporting
Test results and screenshots are embedded in the HTML report, and video recordings are linked. The reports are structured to display before and after screenshots for each test step, and error messages and stack traces are collapsible for better readability. The framework also supports extensibility for adding notifications, such as emailing the report or sending notifications on Slack, or other communication tools.

### Test Report Configurations

This section provides links to different versions of the test reports based on various configurations. Each configuration showcases how the framework behaves under different settings, such as including or excluding step screenshots and video recordings. The table below details the configurations and provides links to view the corresponding test reports.

| Configuration | Report Link |
| --- | --- |
| `IncludeStepScreenshots=false`, `IncludeVideoRecording=false`, Single screenshot at the end of the test | [View Report](http://automation.techlabs.ba/reports/config1/report.html) |
| `IncludeStepScreenshots=false`, `IncludeVideoRecording=false`, Single screenshot at the end of the test / Error message and stack trace included for failed test | [View Report](http://automation.techlabs.ba/reports/config2/report.html) |
| `IncludeStepScreenshots=true`, `IncludeVideoRecording=false`, Single screenshot at the end of the test | [View Report](http://automation.techlabs.ba/reports/config3/report.html) |
| `IncludeStepScreenshots=false`, `IncludeVideoRecording=true`, Single screenshot at the end of the test | [View Report](http://automation.techlabs.ba/reports/config4/report.html) |
| `IncludeStepScreenshots=true`, `IncludeVideoRecording=true`, Single screenshot at the end of the test | [View Report](http://automation.techlabs.ba/reports/config5/report.html) |


### Tag Filtering and Execution
The framework leverages tags which are included in the report as well. This allows for filtering test cases based on tags and executing specific test cases based on tags, such as regression or smoke tests. This feature enhances the flexibility and control over test execution.

### Email OTP Retrieval
The framework includes a feature to retrieve emails from an email provider (in this case, Gmail) and extract One-Time Passwords (OTPs) for further processing. The framework is designed to be flexible, allowing for easy switching to other email providers like MailGun or MailTrap, making it adaptable to different project requirements.

### Video Recording and Screenshots
The framework can record test execution videos and capture screenshots. This is configurable via the `appsettings.json` file. It helps in debugging by providing visual evidence of what occurred during the test run. Regardless of whether the `IncludeStepScreenshots` setting is enabled or disabled, the framework always captures a screenshot at the end of a test case, whether it passes or fails, to ensure there is always a visual log of the final state.

### Descriptive Step Logging
Test cases are written in a descriptive style using the `Step` method, which logs the description in the report. This makes it easy to understand what each step is doing. Here is an example:

```csharp
[Test]
public void ValidOTP_ShouldLogin()
{
    NavigateAndEnterEmail(testData.Email);
    Step("Click Continue on Confirm Email button", () => _loginPage.ClickContinue());
    Step("Extract and Enter OTP", () => _loginPage.EnterOtp(emailService.GetMostRecentOtp()));
    Step("Click Continue button to confirm OTP", () => _loginPage.ClickContinue());
    Step("Verify user info email", () => Assert.That(_loginPage.GetUserInfoEmail(), Is.EqualTo(testData.Email)));
}
```


## Configuration

The framework uses an `appsettings.json` file for configuration. Here's an example configuration:

```json
{
  "TestSettings": {
    "IncludeStepScreenshots": false,
    "IncludeVideoRecording": false,
    "EmailSettings": {
      "Host": "your host imap here",
      "Port": 993,
      "UseSsl": true,
      "Email": "your-email@gmail.com",
      "AppPassword": "your-app-password"
    }
  }
}
```

## Running Tests

Tests can be executed locally or through CI/CD pipelines such as GitHub Actions. Environment-specific configurations can be managed by passing environment variables or using different `appsettings.{environment}.json` files.

### Setting Environment Variables Locally

You can set environment variables directly through the system environment variables:

#### Setting Environment Variables:

1. Open the Windows Search bar and type `Environment Variables`.
2. Select `Edit the system environment variables`.
3. In the System Properties window, click `Environment Variables`.
4. Under `System variables`, click `New` and add `TEST_ENVIRONMENT` with your desired value (e.g., `integration`, `staging`, `production`). Note that currently only `appsettings.integration.json` exists in the repository.

#### Restart Visual Studio:

After setting the environment variable, restart Visual Studio to ensure it picks up the new environment variable.

# Getting Started

To get started with this QA Automation Framework, follow the steps below:

1. **Prerequisites**:
    - Install .NET 8 SDK.
    - Install Visual Studio 2022 version 17.8 or later.

2. **Set up Gmail App Password**:
    - Create an app password for your Gmail account. Follow the instructions provided in this [Google Workspace knowledge article](https://knowledge.workspace.google.com/kb/how-to-create-app-passwords-000009237).

3. **Clone the Repository**:
    - Clone the repository to your local machine using the following command:
      ```sh
      git clone https://github.com/harisjasar/QA_AutomationFramework.git
      ```

4. **Open in Visual Studio**:
    - Open the cloned repository in Visual Studio.

5. **Build the Project**:
    - Build the solution in Visual Studio to restore the dependencies and ensure everything is set up correctly.

6. **Configure `appsettings.json`**:
    - Set the `Host`, `Email`, and `AppPassword` in the `appsettings.json` file located in the `Configs` folder. It should look like this:
      ```json
      {
        "TestSettings": {
          "IncludeStepScreenshots": false,
          "IncludeVideoRecording": false,
          "EmailSettings": {
            "Host": "your-host-imap-here",
            "Port": 993,
            "UseSsl": true,
            "Email": "your-email@gmail.com",
            "AppPassword": "your-app-password"
          }
        }
      }

      Host for Gmail is imap.gmail.com
      ```

7. **Configure `testData.json`**:
    - Set the `Email` to the same email used in `appsettings.json`:
      ```json
      {
          "BaseUrl": "https://fenixshare.anchormydata.com/fenixpyre/s/669ff2910e5caf9f73cd28ea/QA%2520Assignment",
          "Email": "techlabs.automation@gmail.com",
          "InvalidEmail": "techlabs.automationgmail.com"
      }
     ```

8. **Run the Tests**:
    - You can now run the tests using Visual Studio's Test Explorer or by running the following command in the terminal:
      ```sh
      dotnet test
      ```

You're now ready to use the QA Automation Framework!


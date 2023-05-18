# Keylogger

This is a simple keylogger application written in C#. It captures keystrokes on a Windows machine and logs them into a file. It also sends an email containing the logged keystrokes at specified intervals.

## Prerequisites

- .NET Framework
- SMTP server credentials for sending emails

## Getting Started

1. Clone the repository or download the source code files.

2. Open the project in your preferred IDE or text editor.

3. Update the configuration settings in the `App.config` file:
   - `folderName`: Specify the folder name where the log file will be stored.
   - `fileName`: Specify the name of the log file.
   - `smtpHost`: Specify the SMTP server hostname.
   - `smtpPort`: Specify the SMTP server port number.
   - `fromEmail`: Specify the email address from which the logs will be sent.
   - `EmailPassword`: Specify the password for the fromEmail account.
   - `toEmail`: Specify the email address to which the logs will be sent.

4. Build the project to ensure everything is compiled correctly.

5. Run the application.

6. The keylogger will start capturing keystrokes and saving them into a log file located at the specified folder. It will also send an email with the logged keystrokes at specified intervals.

**Note:** The application will only capture keystrokes between 8:00 AM and 7:00 PM on weekdays.

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvement, feel free to open an issue or submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).


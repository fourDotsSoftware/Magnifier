using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Drawing;

namespace Magnifier
{
    class Module
    {
        public static System.Drawing.Color BlueForeColor = System.Drawing.Color.FromArgb(52, 89, 152);

        public static string ApplicationName = "Magnifier 4dots";
        public static string Version = "1.3";

        public static string Ver = "2";

        public static string ShortApplicationTitle = ApplicationName + " V" + Version;
        public static string ApplicationTitle = ShortApplicationTitle + " - 4dots Software";                
        
        public static string DownloadURL = "https://www.4dots-software.com/d/Magnifier/";
        public static string HelpURL = "https://www.4dots-software.com/magnifier/how-to-use.php";
        public static string ProductWebpageURL = "https://www.4dots-software.com/magnifier/";
        public static string BuyURL = "https://www.4dots-software.com/store/buy-magnifier.php";
        public static string VersionURL = "http://cssspritestool.com/versions/magnifier.txt";

        public static string TipText = TranslateHelper.Translate("Great application to magnify Screen Areas !");

        public static List<string> GeneratedTemporaryFiles = new List<string>();

        public static List<string> TempFolders = new List<string>();

        public static string OpenImagesFilter =
            "Image Files (*.png;*.gif;*.jpg;*.jpeg;*.bmp)|*.png;*.gif;*.jpg;*.jpeg;*.bmp|All Files (*.*)|*.*";

        public static string OpenFilesFilter =
    "All Supported Video Files (*.avi;*.mp4;*.mpeg;*.mpg;*.mov;*.mkv;*.flv;*.wmv;*.3gp;*.vob;*.swf;*.3g2;*.asf;*.m2p;*.m2ts;*.m2v;*.m4v;*.mp2;*.ogm;*.ogv;*.qt;*.rm;*.rmvb;*.ts;*.webm)|" +
    "*.avi;*.mp4;*.mpeg;*.mpg;*.mov;*.mkv;*.flv;*.wmv;*.3gp;*.vob;*.swf;*.3g2;*.asf;*.m2p;*.m2ts;*.m2v;*.m4v;*.mp2;" +
    "*.ogm;*.ogv;*.qt;*.rm;*.rmvb;*.ts;*.webm|" +
    "AVI Files (*.avi)|*.avi|MP4 Files (*.mp4)|*.mp4|MPEG Files (*.mpeg;*.mpg)|*.mpeg;*.mpg|MOV Files (*.mov)|*.mov|MKV Files (*.mkv)|*.mkv|" +
    "FLV Files (*.flv)|*.flv|WMV Files (*.wmv)|*.wmv|3GP 3G2 Files (*.3gp;*.3g2)|*.3gp;*.3g2|VOB Files (*.vob)|*.vob|" +
    "SWF Files (*.swf)|*.swf|Quicktime Files (*.qt)|*.qt|Real Media Files (*.rm;*.rmvb)|*.rm;*.rmvb|" +
    "Webm Files (*.webm)|*.webm|Other Video Files (*.asf;*.m2p;*.m2ts;*.m2v;*.m4v;*.mp2;*.ogm;*.ogv;*.ts)|*.asf;" +
    "*.m2p;*.m2ts;*.m2v;*.m4v;*.mp2;*.ogm;*.ogv;*.ts|All Files (*.*)|*.*";

        public static string AcceptableMediaInputPattern = "*.docx;*.txt;*.doc;*.rtf;";

        public static string _TempFolder = string.Empty;

        public static bool ScheduleMode = false;

        public static bool StealthMode = false;

        public static string TempFolder
        {
            get
            {
                if (_TempFolder == string.Empty)
                {
                    string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    _TempFolder = path;

                    return path;
                }
                else
                {
                    return _TempFolder;
                }
            }
            set
            {
                _TempFolder = value;
            }
        }

        public static string _ProfilesFile
= Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + ApplicationName + "\\ffmpeg_profiles.xml";

        public static string ProfilesFile
        {
            get
            {
                if (!System.IO.File.Exists(_ProfilesFile))
                {
                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(_ProfilesFile)))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_ProfilesFile));
                    }

                    System.IO.File.Copy(System.IO.Path.Combine(Application.StartupPath, "ffmpeg_profiles.xml"),
                        _ProfilesFile, true);
                }

                return _ProfilesFile;
            }
        }

        public static string _VideoOptionsFile
        = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + ApplicationName + "\\video_options.xml";

        public static string VideoOptionsFile
        {
            get
            {
                if (!System.IO.File.Exists(_VideoOptionsFile))
                {
                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(_VideoOptionsFile)))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_VideoOptionsFile));
                    }

                    System.IO.File.Copy(System.IO.Path.Combine(Application.StartupPath, "video_options.xml"),
                        _VideoOptionsFile, true);
                }

                return _VideoOptionsFile;
            }
        }

        public static string AppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Batch Document Image Replacer\\";
        public static string CurrentImagesDirectory = "";

        public static string SelectedLanguage = "";
        
        public static string[] args = null;
        public static bool IsCommandLine = false;
        public static bool IsFromWindowsExplorer = false;                        
        
        public static bool DoNotOverwriteFiles = false;
        public static bool AskBeforeOverwrite = false;
        public static bool LeaveSameDateTime = false;

        public static string OutputFilepath = "";

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam,
        int lParam);

        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        public static void WaitNMSeconds(int mseconds)
        {
            if (mseconds < 1) return;
            DateTime _desired = DateTime.Now.AddMilliseconds(mseconds);
            while (DateTime.Now < _desired)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public static bool IsLegalFilename(string name)
        {
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Image ImageFromFile(string path)
        {
            if (!System.IO.File.Exists(path)) return null;

            var bytes = System.IO.File.ReadAllBytes(path);
            var ms = new System.IO.MemoryStream(bytes);
            Image img = null;

            try
            {
                img = Image.FromStream(ms);
                return img;
                /*
                using (var ms = new System.IO.MemoryStream(bytes))
                {
                    var img = Image.FromStream(ms);
                    return img;
                }*/
            }
            catch
            {
                if (ms != null)
                {
                    ms.Dispose();
                    ms = null;
                }

                if (img != null)
                {
                    try
                    {
                        ((Image)img).Dispose();
                        img = null;
                    }
                    catch { }

                }

                bytes = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                return null;
            }

        }


        public static bool RunAdminAction(string args)
        {
            try
            {
                System.Diagnostics.Process pr = new System.Diagnostics.Process();
                pr.StartInfo.FileName = Application.StartupPath + "\\4dotsAdminActions.exe";
                pr.StartInfo.Arguments = args;
                pr.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                pr.Start();

                System.Threading.Thread.Sleep(300);

                while (!pr.HasExited)
                {
                    Application.DoEvents();
                }

                if (pr.ExitCode != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string GetRelativePath(string mainDirPath, string absoluteFilePath)
        {
            string[] firstPathParts = mainDirPath.Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            string[] secondPathParts = absoluteFilePath.Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);

            int sameCounter = 0;
            for (int i = 0; i < Math.Min(firstPathParts.Length,
            secondPathParts.Length); i++)
            {
                if (
                !firstPathParts[i].ToLower().Equals(secondPathParts[i].ToLower()))
                {
                    break;
                }
                sameCounter++;
            }

            if (sameCounter == 0)
            {
                return absoluteFilePath;
            }

            string newPath = String.Empty;
            for (int i = sameCounter; i < firstPathParts.Length; i++)
            {
                if (i > sameCounter)
                {
                    newPath += Path.DirectorySeparatorChar;
                }
                newPath += "..";
            }
            if (newPath.Length == 0)
            {
                newPath = ".";
            }
            for (int i = sameCounter; i < secondPathParts.Length; i++)
            {
                newPath += Path.DirectorySeparatorChar;
                newPath += secondPathParts[i];
            }
            return newPath;
        }

        public static void ShowMessage(string msg)
        {
            if (Module.IsCommandLine)
            {
                Console.WriteLine(TranslateHelper.Translate(msg));
            }
            else
            {
                MessageBox.Show(TranslateHelper.Translate(msg));
            }
        }

        public static DialogResult ShowQuestionDialog(string msg, string caption)
        {
            return MessageBox.Show(TranslateHelper.Translate(msg), TranslateHelper.Translate(caption), MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
        }


        public static void ShowError(Exception ex)
        {
            ShowError("Error", ex);
        }

        public static void ShowError(string msg)
        {
            if (Module.IsCommandLine)
            {
                Console.WriteLine("Error:" + msg);
            }
            else
            {
                /*
                try
                {
                    frmError f = new frmError("Error", msg);
                    f.TopMost = true;
                    f.ShowDialog();
                }
                catch
                {

                }*/
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public static void ShowError(string msg, Exception ex)
        {
            ShowError(msg + "\n\n" + ex.Message);
        }

        public static void ShowError(string msg, string exstr)
        {
            ShowError(msg + "\n\n" + exstr);
        }

        public static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {           
            
        }

        public static DialogResult ShowQuestionDialogYesFocus(string msg, string caption)
        {
            return MessageBox.Show(TranslateHelper.Translate(msg), TranslateHelper.Translate(caption), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }
        

        public static bool IsAcceptableMediaInput(string filepath)
        {
            try
            {
                filepath = filepath.ToLower();
                FileInfo fi = new FileInfo(filepath);

                if (fi.Extension != String.Empty && Module.AcceptableMediaInputPattern.IndexOf(fi.Extension) >= 0)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static int _Modex64 = -1;

        public static bool Modex64
        {
            get
            {
                if (_Modex64 == -1)
                {
                    if (Marshal.SizeOf(typeof(IntPtr)) == 8)
                    {
                        _Modex64 = 1;
                        return true;
                    }
                    else
                    {
                        _Modex64 = 0;
                        return false;
                    }
                }
                else if (_Modex64 == 1)
                {
                    return true;
                }
                else if (_Modex64 == 0)
                {
                    return false;
                }
                return false;
            }
        }

        public static string DownloadPage(string uri)
        {
            try
            {
                WebClient client = new WebClient();

                Stream data = client.OpenRead(uri);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                return s;
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public static String HexConverter(System.Drawing.Color c)
        {
            String rtn = String.Empty;
            try
            {
                rtn = "0x" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
            }
            catch (Exception ex)
            {
                //doing nothing
            }

            return rtn;
        }        

        public static string DecimalToString(decimal dec)
        {
            return DecimalToString(dec, 1);
        }

        public static string DecimalToString(decimal dec, int decimal_places)
        {
            string format = "#0";

            if (decimal_places > 0)
            {
                format += ".";
            }

            for (int k = 0; k < decimal_places; k++)
            {
                format += "0";
            }

            return dec.ToString(format, new System.Globalization.CultureInfo("en-US")).Replace(",", ".");
        }

        public static decimal StringToDecimal(string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;

            int epos = str.LastIndexOf(".");

            if (epos < 0)
            {
                epos = str.LastIndexOf(",");
            }

            if (epos < 0)
            {
                bool ihask = false;

                string sintegral = str;

                if (sintegral.ToLower().IndexOf("k") >= 0)
                {
                    ihask = true;
                }

                int integral_part = int.Parse(sintegral.ToLower().Replace("k", ""));

                return (decimal)integral_part;
            }
            else
            {
                bool ihask = false;

                string sintegral = str.Substring(0, epos);

                if (str.ToLower().IndexOf("k") >= 0)
                {
                    ihask = true;
                }

                int integral_part = int.Parse(sintegral.ToLower().Replace("k", ""));

                string sdecimal = str.Substring(epos + 1, str.Length - epos - 1).ToLower().Replace("k", "");

                int decimal_part = int.Parse(sdecimal);

                decimal d10 = 1;

                for (int k = 0; k < sdecimal.Length; k++)
                {
                    d10 = d10 * 10;
                }

                decimal ddecimal_part = (decimal)decimal_part;

                decimal ddec = ddecimal_part / d10;

                decimal dintegral_part = (decimal)integral_part;

                decimal d = dintegral_part + ddec;

                if (ihask)
                {
                    d = d * 1000;
                }


                return d;
            }
        }

        public static bool IsWindows64Bit
        {
            get
            {
                try
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = System.IO.Path.Combine(Application.StartupPath, "get32or64bit.exe");
                    p.Start();
                    p.WaitForExit();

                    if (p.ExitCode == 64)
                    {
                        return true;
                    }
                    else if (p.ExitCode == 32)
                    {
                        return false;
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool DeleteApplicationSettingsFile()
        {
            try
            {
                string settingsFile = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.Save();

                System.IO.FileInfo fi = new FileInfo(settingsFile);
                fi.Attributes = System.IO.FileAttributes.Normal;
                fi.Delete();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }    
}
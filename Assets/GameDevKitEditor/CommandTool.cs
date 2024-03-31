namespace GameEditor
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Debug = UnityEngine.Debug;

    public class CommandTool
    {
        public static void InstallApk(string apkpath, bool IsTestOnRealDevice)
        {
            //调用adb命令安装apk
#if UNITY_EDITOR_WIN
            string adbPath =
                "C:\\Program Files\\Unity\\Hub\\Editor\\2022.3.17f1c1\\Editor\\Data\\PlaybackEngines\\AndroidPlayer\\SDK\\platform-tools\\adb.exe";
            string installCommand = "adb install -r " + Path.GetFullPath(apkpath);
            string connectCommand = "adb connect 127.0.0.1:21503";
            //先连接再安装,如果是真机测试,则不需要连接
            if (!IsTestOnRealDevice)
            {
                Process.Start("cmd.exe", $"/c {connectCommand}");
            }

            Process.Start("cmd.exe", $"/c {installCommand}");
#elif UNITY_EDITOR_OSX
            string installCommand = "install -r " + Path.GetFullPath(apkpath);
            string cmd =
                "/Applications/Unity/Hub/Editor/2022.3.17f1c1/PlaybackEngines/AndroidPlayer/SDK/platform-tools/adb";
            RunADBOnMac(cmd, $"{installCommand}");
            RunADBOnMac(cmd, "shell am start -n com.nightq.MonopolyRush/com.unity3d.player.UnityPlayerActivity");
#endif
        }


        public static void RunADBOnMac(string cmd, string arguments)
        {
            Process p = new Process();
            p.StartInfo.FileName = cmd; //设定程序名  
            p.StartInfo.Arguments = arguments; //设定程式执行參數  
            p.StartInfo.UseShellExecute = false; //关闭Shell的使用  
            p.StartInfo.RedirectStandardInput = true; //重定向标准输入  
            p.StartInfo.RedirectStandardOutput = true; //重定向标准输出  
            p.StartInfo.RedirectStandardError = true; //重定向错误输出  
            p.StartInfo.CreateNoWindow = true; //设置不显示窗口  
            p.StartInfo.StandardOutputEncoding = Encoding.UTF8; // 指定输出流编码为 UTF-8
            p.StartInfo.StandardErrorEncoding = Encoding.UTF8; // 指定错误输出流编码为 UTF-8
            p.Start();

            string result = p.StandardOutput.ReadToEnd(); // 正确输出
            var errOuput = p.StandardError.ReadToEnd(); // 错误输出
            p.Close();

            UnityEngine.Debug.Log(" sss " + result + " " + errOuput);
            // outputFunc = () => result;
            // errOutputFunc = () => errOuput;
            // return result + "\r\n" + errOuput;
        }

        public static void RunCommandOnWindow(string command, bool autoClose = true)
        {
            Process myProcess = new Process();
            var pStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                CreateNoWindow = true,
                UseShellExecute = false, //必须为false
                RedirectStandardError = true, //必须为true
                RedirectStandardInput = false,
                RedirectStandardOutput = true, //必须为true
            };
            if (autoClose)
            {
                pStartInfo.Arguments = $"/c {command}";
            }
            else
            {
                pStartInfo.Arguments = $"/k {command}";
            }

            myProcess.StartInfo = pStartInfo;
            myProcess.Start();
            if (autoClose)
            {
                // Read the standard error of net.exe and write it on to console.
                if (myProcess.StandardOutput.Peek() != -1)
                {
                    UnityEngine.Debug.Log(myProcess.StandardOutput.ReadToEnd());
                }

                if (myProcess.StandardError.Peek() != -1)
                {
                    Debug.LogError(myProcess.StandardError.ReadToEnd());
                }
            }
        }

        /**
         * windows 会使用cmd执行
         * mac 直接执行
         */
        public static bool RunCommand(string command, string param)
        {
            UnityEngine.Debug.Log($" {command} {param} ");
            Process myProcess = new Process();
            var pStartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false, //必须为false
                RedirectStandardError = true, //必须为true
                RedirectStandardInput = false,
                RedirectStandardOutput = true, //必须为true
            };
#if UNITY_EDITOR_WIN
            pStartInfo.FileName = "cmd.exe";
            pStartInfo.Arguments = $"/k {command} {param}";
#elif UNITY_EDITOR_OSX
            if (command.StartsWith("ossutil"))
            {
                command = "/usr/local/bin/ossutil";
            }

            pStartInfo.FileName = command;
            pStartInfo.Arguments = param;
#endif
            myProcess.StartInfo = pStartInfo;
            myProcess.Start();
            // Read the standard error of net.exe and write it on to console.
            if (myProcess.StandardOutput.Peek() != -1)
            {
                UnityEngine.Debug.Log(myProcess.StandardOutput.ReadToEnd());
            }

            if (myProcess.StandardError.Peek() != -1)
            {
                Debug.LogError(myProcess.StandardError.ReadToEnd());
                return false;
            }

            UnityEngine.Debug.Log($" {command} {param} end ");
            return true;
        }

        public static void RunMultiCommand(params string[] commandParam)
        {
            // 创建一个新的进程
            Process myProcess = new Process();
            var pStartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
            };
            pStartInfo.FileName = "cmd.exe";

            // 构建多条命令字符串，用 \ 分隔
            string commands = string.Join(" & ", commandParam);
            pStartInfo.Arguments = $"/c {commands}";

            // 启动进程
            myProcess.StartInfo = pStartInfo;
            myProcess.Start();

            if (myProcess.StandardOutput.Peek() != -1)
            {
                UnityEngine.Debug.Log(myProcess.StandardOutput.ReadToEnd());
            }

            if (myProcess.StandardError.Peek() != -1)
            {
                UnityEngine.Debug.LogError(myProcess.StandardError.ReadToEnd());
            }

            myProcess.WaitForExit();
        }
    }
}
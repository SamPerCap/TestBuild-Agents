using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace FirstAgent
{
    class BuildAgent
    {
        Thread agentThread;
        bool done = false;
        Process buildProcess;
        private static readonly object buildLock = new object();
        public int Id { get; private set; }
        public string BuildPath { get; private set; }
        public int Time { get; private set; }
        public bool Behaviour { get; set; }


        public BuildAgent(bool beh, int id, string buildPath, int timeInSeconds)
        {
            Id = id;
            Behaviour = beh;
            BuildPath = buildPath;
            Time = timeInSeconds;
            buildProcess = buildAgentLogic(Behaviour, BuildPath);

            agentThread = new Thread(() => Run());
            agentThread.Start();
        }

        public void Cancel()
        {
            if (agentThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            {
                agentThread.Interrupt();
            }
        }

        private void Run()
        {
            done = false;
            while (!done)
            {
                int exitCode = DoBuild();

                if (!done)
                {
                    TimedDelay(Time);
                }
            }
            Cancel();
            Console.WriteLine($"Agent{Id} has stopped");
        }

        public Process buildAgentLogic(bool beh, string BuildPath)
        {
            if (beh)
            {
                return new Process
                {

                    StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "dotnet",
                    Arguments = $" test {BuildPath}"
                }
                };
            }
            else
            {
                return new Process
                {

                    StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,                    
                    FileName = "dotnet",
                    Arguments = $" build {BuildPath}"
                }
                };
            }
            
        }

        private int DoBuild()
        {
            lock (buildLock)
            {
                Console.WriteLine("Agent number" + Id + " is starting...");
                buildProcess.Start();
                buildProcess.WaitForExit();
                writeLogFile();
                done = true;
                return buildProcess.ExitCode;
            }
        }

        private void writeLogFile()
        {
            string filepath = "<path>";

            using (StreamWriter sw = File.AppendText(filepath+$"{Behaviour}_Agent_{Id}.log"))
            {
                sw.WriteAsync($"Time: {buildProcess.StartTime}");
                sw.WriteLine(buildProcess.StandardOutput.ReadToEnd());
                sw.WriteLine();
            }
        }
        private void TimedDelay(int seconds)
        {
            try
            {
                Thread.Sleep(seconds * 1000);
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine($"{Id} Got Interrupted!");
                // Do nothing special - just wake up!
            }
        }
    }
}

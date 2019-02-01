using System;
using System.Collections.Generic;
using System.Threading;


namespace FirstAgent
{
    class MyBuildManager
    {
        List<BuildAgent> operativeAgents = new List<BuildAgent>();
        public string BuildPath { get; private set; }
        public string TestPath { get; private set; }
        public int Time { get; private set; }
        public int ID { get; private set; }
        public bool Behaviour { get; set; }

        Thread awakeningThread;

        public MyBuildManager(bool beh, String path, int time, int qty)
        {
            operativeAgents.Clear();
            Behaviour = beh;
            Time = time;
            BuildPath = path;
            TestPath = path;
            for (int a = 1; a <= qty; a++)
            {
                ID = a;
                if (beh)
                {
                AddAgent(a, path, Time);
                }
                else { 
                AddAgent(a, path, Time);
                }
            }
        }
        private void AddAgent(int ID, String buildpath, int localTime)
        {
            BuildAgent buildAgent = new BuildAgent(Behaviour, ID, buildpath, localTime);
            operativeAgents.Add(buildAgent);
            int agentsSaved = operativeAgents.Count;
            Console.WriteLine("There is actually " + agentsSaved + " operative agents.");
            awakeningThread = new Thread(() => StartAwakeningThread());
            awakeningThread.Start();
        }
        private void StartAwakeningThread()
        {
            foreach (BuildAgent agentStored in operativeAgents)
            {
                agentStored.buildAgentLogic(Behaviour, BuildPath);
            }
            Thread.Sleep(Time*1000);
        }
    }
}

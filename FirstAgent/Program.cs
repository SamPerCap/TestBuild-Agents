using System;
using System.Diagnostics;

namespace FirstAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write in seconds how long should the thread sleep");
            int time = int.Parse(Console.ReadLine());
            Console.WriteLine("How many agents would you like to add?");
            int agentsQuantity = int.Parse(Console.ReadLine());
            Console.WriteLine("Test or Build?");
            string behaviour = Console.ReadLine();
            String buildpath = "<path>";
            String testpath = "<path>";

            // BuildAgent BA = new BuildAgent(buildpath, time);
            if (behaviour.Equals("Test"))
            {
                MyBuildManager MBA = new MyBuildManager(true, testpath, time, agentsQuantity);

            }
            else if (behaviour.Equals("Build"))
            {
                MyBuildManager MBA = new MyBuildManager(false, buildpath, time, agentsQuantity);

            }
            else
            {
                Console.WriteLine("Make sure you write correctly");
            }

            Console.ReadLine();
        }
    }
}

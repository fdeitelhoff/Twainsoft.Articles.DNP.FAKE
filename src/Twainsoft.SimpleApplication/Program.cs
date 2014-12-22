using System;
using Twainsoft.SimpleApplication.Lib;

namespace Twainsoft.SimpleApplication
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("This program needs two input parameters!");
            }
            else
            {
                Console.WriteLine(Calculator.Add(Convert.ToInt32(args[0]), Convert.ToInt32(args[1])));
            }

            Console.ReadLine();
        }
    }
}

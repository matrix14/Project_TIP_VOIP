using System;
using Shared;
namespace Server
{
    public delegate void Notify();
    class Program
    {
        public static event Notify ProcessCompleted;
        static void Main(string[] args)
        {
            Work();
            OnProcessCompleted();
        }

        protected static void OnProcessCompleted() //protected virtual method
        {
            ProcessCompleted?.Invoke();
        }

        public static void Work()
        {
            ProcessCompleted += Program_ProcessCompleted;
        }

        private static void Program_ProcessCompleted()
        {
            Console.WriteLine("Process Completed!");
        }
    }
}

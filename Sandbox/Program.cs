using System;

namespace Sandbox
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                FirebaseRest.RunAsync().Wait();
                FirebaseRepository.RunAsync().Wait();
                Console.WriteLine("Done ......");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadLine();
            }
        }
    }
}
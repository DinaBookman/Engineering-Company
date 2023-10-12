using System;
 

namespace stage0
{
    partial class program
    {
        static void Main(String[] args)
        {
            Welcome9181();
            Welcome1388();
            Console.ReadKey();
        }
        static partial void Welcome1388();
        private static void Welcome9181()
        {
            Console.Write("Enter your name:");
            var a = Console.ReadLine();
            Console.WriteLine($"{a}, welcome to my first console application");
        }
    }
}
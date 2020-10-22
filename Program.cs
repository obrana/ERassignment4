using System;

namespace EFassinment4
{
    class Program
    {
        static void Main(string[] args)
        {
         using var ctx = new NorthWindContext();
         foreach (var category in ctx.Categories)
         {
             Console.WriteLine(category);
         }
        }
    }
}
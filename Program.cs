using System;
using System.Linq;

namespace EFassinment4
{
    class Program
    {
        static void Main(string[] args)
        {
         using var ctx = new NorthWindContext();
         var MaxId = ctx.Categories.Max(x => x.Id);
         ctx.Categories.Add(new Category{ Id = MaxId +1, Name="Testing"});

         ctx.SaveChanges();
         foreach (var category in ctx.Categories)
         {
             Console.WriteLine(category);
         }
        }
    }
}
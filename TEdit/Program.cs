using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEdit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Editor.path = "test.txt";
            Editor.Run();
            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDIDemo.Models
{
    public class MyService : IMyService
    {
        public void PrintTime(string title)
        {
            Console.WriteLine($"====================================");
            Console.WriteLine($"     {title}:{DateTime.Now}");    
            Console.WriteLine($"====================================");
        }
    }
}

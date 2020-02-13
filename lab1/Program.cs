using System;

namespace lab1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Серебряков Артем ИУ5-34Б";

            var a = parseParam(args.Length > 0 ? args[0] : null, "A");
            var b = parseParam(args.Length > 1 ? args[1] : null, "B");
            var c = parseParam(args.Length > 2 ? args[2] : null, "C");

            Console.ForegroundColor = ConsoleColor.Green;
            var d = b * b - 4 * a * c;
            if (d < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Discriminant < 0; no real roots");
            }
            else if (a == 0)
            {
                if (b == 0)
                {
                    if (c == 0)
                    {
                        Console.WriteLine("Infinity of roots");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No roots"); 
                    }
                }
                else
                {
                    if(-c/b >= 0) {
                        var x1 = Math.Sqrt(-c/b);
                        var x2 = -x1;
                        Console.WriteLine("x1 = {0}, x2 = {1}", x1, x2);
                    }
                    else {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No real roots");
                    }
                }
            }
            else
            {
                var y1 = (-b + Math.Sqrt(d))/(2*a);
                var y2 = (-b - Math.Sqrt(d))/(2*a);
                if(y1 < 0 && y2 < 0) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No real roots");
                    return;
                }
                if (y1 >= 0) {
                    var x1 = Math.Sqrt(y1);
                    var x2 = -x1;
                    Console.WriteLine("x1 = {0}, x2 = {1}", x1, x2);
                }
                if(y2 >= 0 && d != 0) {
                    var x3 = Math.Sqrt(y2);
                    var x4 = -x3;
                    Console.WriteLine("x3 = {0}, x4 = {1}", x3, x4);
                }
            }
            
        }

        private static double parseParam(String param, String paramName)
        {
            double d;
            while (!double.TryParse(param, out d))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error parsing param " + paramName);
                Console.ResetColor();
                Console.WriteLine("Input param " + paramName + " again");
                param = Console.ReadLine();
            }

            return d;
        }
    }
}
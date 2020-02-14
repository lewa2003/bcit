using System;
using System.Collections;
using System.Collections.Generic;
using lab3.figures;

namespace lab3
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var rectangle = new Rectangle(10, 3);
            var square = new Square(4);
            var circle = new Circle(3);
            rectangle.Print();
            square.Print();
            circle.Print();
            
            ArrayList arrayList = new ArrayList();
            arrayList.Add(rectangle);
            arrayList.Add(square);
            arrayList.Add(circle);
            Console.WriteLine("Not sorted ArrayList: ");
            foreach (var figure in arrayList)
                Console.WriteLine(figure);
            Console.WriteLine();
            
            arrayList.Sort();
            Console.WriteLine("Sorted ArrayList: ");
            foreach (var figure in arrayList)
                Console.WriteLine(figure);
            Console.WriteLine();

            List<Figure> list = new List<Figure>();
            list.Add(rectangle);
            list.Add(square);
            list.Add(circle);
            Console.WriteLine("Not sorted List: ");
            foreach (var figure in list)
                Console.WriteLine(figure);
            Console.WriteLine();
            
            list.Sort();
            Console.WriteLine("Sorted List: ");
            foreach (var figure in list)
                Console.WriteLine(figure);
            Console.WriteLine();
        }
    }
}
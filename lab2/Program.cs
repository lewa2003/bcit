using lab2.figures;

namespace lab2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var rectangle = new Rectangle(2, 3);
            var square = new Square(4);
            var circle = new Circle(5);
            rectangle.Print();
            square.Print();
            circle.Print();
        }
    }
}
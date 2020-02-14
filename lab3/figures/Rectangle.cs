using System;

namespace lab3.figures
{
    public class Rectangle : Figure, IPrint
    {
        private double _height;
        private double _width;

        public double Height
        {
            get => _height;
            set => _height = value;
        }

        public double Width
        {
            get => _width;
            set => _width = value;
        }

        public Rectangle(double height, double width)
        {
            this._height = height;
            this._width = width;
        }

        public override double Square()
        {
            return _height * _width;
        }

        public override string ToString()
        {
            return "Height = " + this._height + ", Width = " + this._width + ", Square = " + this.Square();
        }

        public void Print()
        {
            Console.WriteLine(this.ToString());
        }
    }
}
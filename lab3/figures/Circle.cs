using System;

namespace lab3.figures
{
    public class Circle : Figure, IPrint
    {
        private double _r;

        public double R
        {
            get => _r;
            set => _r = value;
        }

        public Circle(double r)
        {
            this._r = r;
        }

        public override double Square()
        {
            return Math.PI * this._r * this._r;
        }

        public override string ToString()
        {
            return "Radius = " + this._r + ", Square = " + this.Square();
        }

        public void Print()
        {
            Console.WriteLine(this.ToString());
        }
    }
}
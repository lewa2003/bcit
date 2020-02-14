using System;

namespace lab3.figures
{
    public abstract class Figure : IComparable
    {
        public abstract double Square();
        public abstract override string ToString();
        
        public int CompareTo(object obj) {
            Figure tmp = (Figure)obj;
            if(this.Square() < tmp.Square()) return -1;
            else if(this.Square() == tmp.Square()) return 0;
            else return 1;
        }
    }
}
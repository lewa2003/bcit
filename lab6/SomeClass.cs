using System;

namespace lab6
{
    public class SomeClass
    {
        public String StringField { get; set; }
        [SomeAttribute(Description = "Some int field")]
        public int IntField { get; set; }

        public SomeClass()
        {
        }

        public SomeClass(String a, int b)
        {
            StringField = a;
            IntField = b;
        }

        public void Print()
        {
            Console.WriteLine($"String: {StringField} Int: {IntField} ");
        }

        public String Concatenate(String a, String b)
        {
            return a + b;
        }

    }
}
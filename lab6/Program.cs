using System;
using System.Reflection;

namespace lab6
{
    class Program
    {
        delegate String MyDelegate(String a, int b);

        static String Sum(String a, int b)
        {
            return a + b;
        }

        static void DelegateAsParam(String message, String a, int b, MyDelegate myDelegate)
        {
            String result = myDelegate(a, b);
            Console.WriteLine(message + result);
        }

        static void CommonDelegateAsParam(String message, String a, int b, Func<String, int, String> commonDelegate)
        {
            String result = commonDelegate(a, b);
            Console.WriteLine(message + result);
        }

        public static void Main(string[] args)
        {
            String a = "String";
            int b = 3;
            Console.WriteLine("First part:\n");

            DelegateAsParam("Delegate as param : ", a, b, Sum);

            DelegateAsParam("Lambda func", a, b, (x, y) => { return "Lambda func delegate: " + y + x; });

            CommonDelegateAsParam("Common delegate based on method: ", a, b, Sum);

            CommonDelegateAsParam("Common delegate based on lamda: ", a, b,
                (x, y) => { return "Lambda func delegate: " + y + x; });

            Console.WriteLine("Second part:\n");

            Type t = typeof(SomeClass);
            
            Console.WriteLine("Type " + t.FullName + " inherits from " + t.BaseType.FullName);
            
            Console.WriteLine("\nConstructors:");
            foreach (var i in t.GetConstructors())
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("\nProperties:");
            foreach (var i in t.GetProperties())
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("\nMethods:");
            foreach (var i in t.GetMethods())
            {
                Console.WriteLine(i);
            }
            
            Console.WriteLine("\nPropeties with attribute:");
            foreach (var i in t.GetProperties())
            {
                object attrObj;
                if (GetPropertyAttribute(i, typeof(SomeAttribute), out attrObj))
                {
                    SomeAttribute attr = attrObj as SomeAttribute;
                    Console.WriteLine(i.Name + " - " + attr.Description);
                }
            }

            Console.WriteLine("\nCall method:");
            SomeClass someClass = new SomeClass("not String", -1);
            t.InvokeMember("Print", BindingFlags.InvokeMethod, null, someClass, null);
            
        }
        
        public static bool GetPropertyAttribute(PropertyInfo checkType, Type attrType, out object attr)
        {
            bool Result = false;
            attr = null;

            var isAttribute = checkType.GetCustomAttributes(attrType, false);
            if (isAttribute.Length > 0)
            {
                Result = true;
                attr = isAttribute[0];
            }

            return Result;
        }
    }
}
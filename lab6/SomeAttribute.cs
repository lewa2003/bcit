using System;

namespace lab6
{
    public class SomeAttribute : Attribute
    {
        public String Description { get; set; }
        public SomeAttribute() {}
        public SomeAttribute(string DescriptionParam) {
            Description = DescriptionParam;
        }
    }
}
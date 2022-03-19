using System;
using System.Collections.Generic;
using System.Text;

namespace lab2
{
    class Teacher : Person
    {
        public Teacher(string name, int age) : base(name, age)
        {
           
        }
        override public string ToString()
        {
            return "Teacher: " + base.ToString() + "\n";
        }
    }
}

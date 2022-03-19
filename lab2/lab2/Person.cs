using System;
using System.Collections.Generic;
using System.Text;

namespace lab2
{
    class Person
    {
        public Person(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
        protected string name;
        protected int age;
        public string Name
        {
            get { return name; }
        }
        public int Age
        {
            get { return age; }
        }

        public bool Equals(Person obj)
        {
            return obj.name == this.name && obj.age == this.age;
        }

        override public string ToString()
        {
            return this.name + "(" + this.age.ToString() + " y.o.)";
        }
    }
}

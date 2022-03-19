using System;
using System.Collections.Generic;
using System.Text;

namespace lab2
{
    class Classroom
    {
        private string name;
        private Person[] persons;

        public string Name
        {
            get { return name; }
        }

        public Classroom(string name, Person[] persons)
        {
            this.name = name;
            this.persons = persons;
        }
        override public string ToString()
        {
            string result = "Classroom: " + this.name + "\n\n";
            foreach(var p in persons)
            {
                result += p.ToString() + "\n";
            }
            return result;
        }
    }
}

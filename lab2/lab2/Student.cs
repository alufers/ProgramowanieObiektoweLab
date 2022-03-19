using System;
using System.Collections.Generic;
using System.Text;

namespace lab2
{
    class Student : Person
    {
        private string group;
        private List<Task> tasks = new List<Task>();

        public string Group
        {
            get
            {
                return group;
            }
        }
        public Student(string name, int age, string group) : base(name, age)
        {
            this.group = group;
        }

        public Student(string name, int age, string group, List<Task> tasks) : base(name, age)
        {
            this.group = group;
            this.tasks = tasks;
        }

        public void AddTask(string taskName, TaskStatus taskStatus)
        {
            this.tasks.Add(new Task(taskName, taskStatus));
        }

        public void UpdateTask(int index, TaskStatus taskStatus)
        {
            this.tasks[index].Status = taskStatus;
        }

        public string RenderTasks(String prefix = "\t")
        {
            string result = "";
            foreach(var t in this.tasks)
            {
                result += prefix + t.ToString() + "\n";
            }
            return result;
        }

        public override string ToString()
        {
            return "Student: " + base.ToString() + "\nGroup: " + this.group + "\nTasks:\n" + this.RenderTasks();
        }
        
        public bool Equals(Student other)
        {
           return base.Equals(other) && this.group == other.group && SequenceEquals(this.tasks, other.tasks);
        }
        private bool SequenceEquals(List<Task> a, List<Task> b)
        {
            if (a.Count != b.Count) return false;
            for(int i = 0; i < a.Count; i++)
            {
                if(!a[i].Equals(b[i]))
                {
                    return false;
                }
            }
            return true;
        }

    }
}

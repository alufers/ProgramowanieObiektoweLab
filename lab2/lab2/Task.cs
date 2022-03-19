using System;
using System.Collections.Generic;
using System.Text;

namespace lab2
{
    enum TaskStatus
    {
        Waiting,
        Progress,
        Done,
        Rejected
    }
    class Task
    {
        private string name;
        private TaskStatus status;
        public string Name
        {
            get { return name; }
        }
        public TaskStatus Status
        {
            get { return status; }
            set { this.status = value; }
        }

        public Task(string name, TaskStatus status)
        {
            this.name = name;
            this.status = status;
        }

        override public string ToString()
        {
            return this.name + " [" + this.status.ToString() + "]";
        }

        public bool Equals(Task other)
        {
            return this.name == other.name && this.status == other.status;
        }
    }
}

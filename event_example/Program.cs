using System;

public class Program
{
    public static void Main(string[] args)
    {
        Couter couter = new Couter();

        couter.Changed += (object sender, ChangedEventArgs e) => // <----------- handled event
        {
            Console.WriteLine("Changed event occured: value=" + e.Value);
        };

        couter.Add(+1);
        couter.Add(+1);
        couter.Add(+1);

        couter.Add(-1);
        couter.Add(-1);
        couter.Add(-1);
    }
}



public class Couter
{
    private int value;

    public event EventHandler<ChangedEventArgs> Changed; // <------------------- event declaration

    protected virtual void OnChanged(ChangedEventArgs e) // <------------------- wraps event (allows to raise the event)
    {
        EventHandler<ChangedEventArgs> handler = this.Changed;

        if (handler != null)
            handler(this, e);
    }

    public void Add(int step)
    {
        this.value += step;

        this.OnChanged(new ChangedEventArgs(this.value)); // <------------------- runs the event
    }
}

public class ChangedEventArgs : EventArgs
{
    public int Value { get; set; }

    public ChangedEventArgs(int value)
    {
        this.Value = value;
    }
}

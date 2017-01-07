using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class State<Context>
{
    private Context context;

    public State(Context context)
    {

    }

    abstract public void UpdateState();
}
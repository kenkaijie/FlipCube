using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void CallbackHandler();

public interface ISpawnable
{
    // callbacks
    void SetOnCreationCompleteHandler(CallbackHandler callback);
}

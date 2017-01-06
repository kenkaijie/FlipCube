using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void EventCallback(EventCallbackError returnCode);

public enum EventCallbackError
{
    NOERROR,
    ERROR_INCORRECT_PLAYER_STATE,
    ERROR_FAILED_TO_FINISH_MOVE,
    ERROR_GENERAL
}
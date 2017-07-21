using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Components
{
    public enum SwitchState :
        byte
    {
        SwitchedOff,
        SwitchingOn,
        SwitchedOn,
        SwitchingOff
    }
}

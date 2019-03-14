using System;
using System.Collections.Generic;
using System.Text;

namespace BaseTCPServerBindings
{
    public enum ServerPackets
    {
        SConnectionOk = 1, SStringMessage = 2,
    }

    public enum ClientPackets
    {
        CThankYou = 1, CStringMessage = 2,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPong
{
    public interface IInputController
    {
        event EventHandler KeyUpPush;
        event EventHandler KeyDownPush;
        event EventHandler KeyUpRelease;
        event EventHandler KeyDownRelease;

    }
}

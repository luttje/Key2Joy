using System;
using System.Runtime.InteropServices;

namespace Key2Joy.LowLevelInput
{
    // Source: https://stackoverflow.com/a/34384189
    [StructLayout(LayoutKind.Sequential)]
    public struct LowLevelKeyboardInputEvent
    {
        /// <summary>
        /// A virtual-key code. The code must be a value in the range 1 to 254.
        /// </summary>
        public int VirtualCode;

        /// <summary>
        /// A hardware scan code for the key. 
        /// </summary>
        public int HardwareScanCode;

        /// <summary>
        /// The extended-key flag, event-injected Flags, context code, and transition-state flag. This member is specified as follows. An application can use the following values to test the keystroke Flags. Testing LLKHF_INJECTED (bit 4) will tell you whether the event was injected. If it was, then testing LLKHF_LOWER_IL_INJECTED (bit 1) will tell you whether or not the event was injected from a process running at lower integrity level.
        /// 
        /// - LLKHF_EXTENDED
        /// (KF_EXTENDED >> 8)
        /// Test the extended-key flag.
        ///
        /// - LLKHF_LOWER_IL_INJECTED
        /// 0x00000002
        /// Test the event-injected (from a process running at lower integrity level) flag.
        ///
        /// - LLKHF_INJECTED
        /// 0x00000010
        /// Test the event-injected (from any process) flag.
        ///
        /// - LLKHF_ALTDOWN
        /// (KF_ALTDOWN >> 8)
        /// Test the context code.
        ///
        /// - LLKHF_UP
        /// (KF_UP >> 8)
        /// Test the transition-state flag. 
        /// </summary>
        public int Flags;

        /// <summary>
        /// The time stamp stamp for this message, equivalent to what GetMessageTime would return for this message.
        /// </summary>
        public int TimeStamp;

        /// <summary>
        /// Additional information associated with the message. 
        /// </summary>
        public IntPtr AdditionalInformation;
    }
}

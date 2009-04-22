﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessHacker
{
    public interface IProvider
    {
        bool Busy { get; }
        bool CreateThread { get; set; }
        bool Enabled { get; set; }
        bool UseInvoke { get; set; }
        void RunOnce();
        void RunOnceAsync();
        void InterlockedExecute(Delegate action, params object[] args);
        void InterlockedExecute(Delegate action, int timeout, params object[] args);
        void Wait();
        void Wait(int timeout);
    }
}

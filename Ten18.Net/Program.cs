﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using Ten18.Async;
using Ten18.Interop;

namespace Ten18
{
    public sealed class Program : IDisposable
    {
        public Program()
        {
            mTask = Flow();
        }

        public void Tick()
        {
            mSingleThreadedConstraint.Verify();
            mTaskScheduler.Tick();
        }

        public void Drain()
        {
            Tick();
        }

        private async Task Flow()
        {
            using (var window = new Window("Ten18"))
            {
                window.MakeFullScreen();

                while (!window.HasClosed)
                {
                    await mTaskScheduler.Yield();
                }
            }
        }

        public void Dispose()
        {
            mDebugTextWriter.Dispose();
        }

        private DebugTextWriter mDebugTextWriter = new DebugTextWriter();
        private SingleThreadedConstraint mSingleThreadedConstraint = SingleThreadedConstraint.Create();
        private CoroutineScheduler mTaskScheduler = new CoroutineScheduler();
        private Task mTask;
    }
}

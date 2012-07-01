// 
// Copyright (c) 2004-2011 Jaroslaw Kowalski <jaak@jkowalski.net>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

#if NETFX_CORE

using Windows.System.Threading;

namespace System.Configuration
{
}

namespace System
{
    public class LocalizableAttribute : Attribute { }
}

namespace System.Threading
{
    internal delegate void TimerCallback(Object state);

    internal sealed class Timer : IDisposable
    {
        private ThreadPoolTimer timer;

        public Timer(TimerCallback callback, Object state, TimeSpan dueTime, TimeSpan period)
        {
            if (period.Milliseconds == -1)
            {
                timer = ThreadPoolTimer.CreateTimer(t => callback(state), dueTime);
            }
            else
            {
                var startedAt = DateTimeOffset.Now;
                timer = ThreadPoolTimer.CreatePeriodicTimer(t =>
                {
                    if (DateTimeOffset.Now - startedAt < dueTime)
                    {
                        return;
                    }

                    callback(state);
                }, period);
            }
        }

        public void Dispose()
        {
            var theTimer = Interlocked.Exchange(ref timer, null);

            if (theTimer != null)
            {
                theTimer.Cancel();
            }
        }
    }
}

namespace System.IO
{


}

#endif
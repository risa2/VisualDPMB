using System;
using System.Windows.Threading;

namespace VisualDPMB
{
	class SimpleTimer
	{
		private DispatcherTimer timer;
		public SimpleTimer(TimeSpan time, EventHandler callback)
		{
			timer=new DispatcherTimer();
			timer.Interval=time;
			timer.Tick+=callback;
			timer.Start();
		}
        public void Stop()
        {
            timer.Stop();
        }
	}
}

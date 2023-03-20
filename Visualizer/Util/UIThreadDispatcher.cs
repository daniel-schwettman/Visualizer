using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace Visualizer.Util
{
    public sealed class UIThreadDispatcher
    {
        public static readonly UIThreadDispatcher Instance = new UIThreadDispatcher();

        private DispatcherTimer _timer;
        private ConcurrentQueue<Action> _queue;

        private UIThreadDispatcher()
        {
        }

        /// <summary>
        /// Perform startup logic (create queue and start timer)
        /// </summary>
        public void Initialize()
        {
            this._queue = new ConcurrentQueue<Action>();

            this._timer = new DispatcherTimer();
            this._timer.Start();
            this._timer.Tick += _timer_Tick;
            this._timer.IsEnabled = true;
            this._timer.Interval = TimeSpan.FromMilliseconds(50);
        }

        /// <summary>
        /// Handles the timer tick. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _timer_Tick(object sender, EventArgs e)
        {
            Action action = null;

            // Check the queue for any pending actions and if they exist, execute them
            while (this._queue.TryDequeue(out action) == true)
            {
                action();
            }
        }

        /// <summary>
        /// Enqueues an action into the queue for processing on the UI thread
        /// </summary>
        /// <param name="action">The action to execute on the UI thread</param>
        public void BeginInvoke(Action action)
        {
            this._queue.Enqueue(action);
        }
    }
}

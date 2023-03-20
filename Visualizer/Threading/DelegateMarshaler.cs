using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Visualizer.Threading
{
	public sealed class DelegateMarshaler
	{
		private SynchronizationContext _synchronizationContext;

		private DelegateMarshaler(SynchronizationContext synchronizationContext)
		{
			this._synchronizationContext = synchronizationContext;
		}

		public static DelegateMarshaler Create()
		{
			if (ReferenceEquals(SynchronizationContext.Current, null))
			{
				throw new InvalidOperationException("No SynchronizationContext exists for the current thread.");
			}
			return new DelegateMarshaler(SynchronizationContext.Current);
		}

		public void BeginInvoke(Action action)
		{
			if (this.IsMarshalRequired == false)
			{
				// already on the target thread, just invoke delegate directly
				action();
			}
			else
			{
				// marshal the delegate call to the target thread
				this._synchronizationContext.Post(delegate { action(); }, null);
			}
		}

		public void BeginInvoke<T>(Action<T> action, T arg)
		{
			if (this.IsMarshalRequired == false)
			{
				// already on the target thread, just invoke delegate directly
				action(arg);
			}
			else
			{
				// marshal the delegate call to the target thread
				this._synchronizationContext.Post(delegate { action(arg); }, null);
			}
		}

		private bool IsMarshalRequired =>
			!ReferenceEquals(this._synchronizationContext, SynchronizationContext.Current);
	}
}

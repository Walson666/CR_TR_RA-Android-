// dnSpy decompiler from Assembly-CSharp.dll class: FSM
using System;
using System.Collections.Generic;

public class FSM
{
	public class Object<T, K> where T : FSM.Object<T, K>
	{
		public Object()
		{
			this.timeSource = Singleton<TimeManager>.Instance.MasterSource;
		}

		public Object(TimeSource source)
		{
			this.timeSource = source;
		}

		public K PrevState
		{
			get
			{
				return this.prevState.key;
			}
		}

		public K State
		{
			get
			{
				return this.state.key;
			}
			set
			{
				this.prevState = this.state;
				if (this.prevState != null)
				{
					this.prevState.onExit(this as T, this.timeSource.TotalTime);
				}
				FSM.State<T, K> state;
				if (this.states.TryGetValue(value, out state))
				{
					this.state = state;
					this.state.onEnter(this as T, this.timeSource.TotalTime);
				}
				else
				{
					this.state = null;
				}
			}
		}

		public TimeSource TimeSource
		{
			get
			{
				return this.timeSource;
			}
			set
			{
				this.timeSource = value;
			}
		}

		public void AddState(K key, FSM.Object<T, K>.Function onEnter, FSM.Object<T, K>.Function onExec, FSM.Object<T, K>.Function onExit)
		{
			FSM.State<T, K> state = new FSM.State<T, K>();
			state.key = key;
			state.onEnter = onEnter;
			state.onExec = onExec;
			state.onExit = onExit;
			this.states.Add(key, state);
		}

		public void Update()
		{
			if (this.state == null)
			{
				return;
			}
			this.state.onExec(this as T, this.timeSource.TotalTime);
		}

		protected TimeSource timeSource;

		protected Dictionary<K, FSM.State<T, K>> states = new Dictionary<K, FSM.State<T, K>>();

		protected FSM.State<T, K> state;

		protected FSM.State<T, K> prevState;

		public delegate void Function(T self, float time);
	}

	public class State<T, K> where T : FSM.Object<T, K>
	{
		public K key;

		public FSM.Object<T, K>.Function onEnter;

		public FSM.Object<T, K>.Function onExec;

		public FSM.Object<T, K>.Function onExit;
	}
}

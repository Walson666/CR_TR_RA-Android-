// dnSpy decompiler from Assembly-CSharp.dll class: FiniteStateMachine
using System;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
	private void Awake()
	{
		this.state = new FiniteStateMachine.StateType();
		this.state.id = 0;
		this.state.onEnterMessage = "OnFollowCharaEnter";
		this.state.onExecMessage = "OnFollowCharaExec";
		this.state.onExitMessage = "OnFollowCharaExit";
	}

	public int PrevState
	{
		get
		{
			return this.fsmObject.PrevState;
		}
	}

	public int State
	{
		get
		{
			return this.fsmObject.State;
		}
		set
		{
			this.fsmObject.State = value;
		}
	}

	protected void Start()
	{
		this.fsmObject = new FiniteStateMachine.FSMObject(base.gameObject);
		this.fsmObject.AddState(this.state.id, new FSM.Object<FiniteStateMachine.FSMObject, int>.Function(this.state.onEnter), new FSM.Object<FiniteStateMachine.FSMObject, int>.Function(this.state.onExec), new FSM.Object<FiniteStateMachine.FSMObject, int>.Function(this.state.onExit));
		this.fsmObject.State = this.startState;
	}

	private void LateUpdate()
	{
		this.fsmObject.Update();
	}

	private FiniteStateMachine.StateType state;

	private int startState;

	protected FiniteStateMachine.FSMObject fsmObject;

	public enum UpdateFunction
	{
		Update,
		LateUpdate,
		FixedUpdate
	}

	public class FSMObject : FSM.Object<FiniteStateMachine.FSMObject, int>
	{
		public FSMObject(GameObject _go)
		{
			this.go = _go;
		}

		public GameObject go;
	}

	[Serializable]
	public class StateType
	{
		public void onEnter(FiniteStateMachine.FSMObject fsmObject, float time)
		{
			fsmObject.go.SendMessage(this.onEnterMessage, time, SendMessageOptions.RequireReceiver);
		}

		public void onExec(FiniteStateMachine.FSMObject fsmObject, float time)
		{
			fsmObject.go.SendMessage(this.onExecMessage, time, SendMessageOptions.RequireReceiver);
		}

		public void onExit(FiniteStateMachine.FSMObject fsmObject, float time)
		{
			fsmObject.go.SendMessage(this.onExitMessage, time, SendMessageOptions.RequireReceiver);
		}

		public int id;

		public string onEnterMessage;

		public string onExecMessage;

		public string onExitMessage;
	}
}

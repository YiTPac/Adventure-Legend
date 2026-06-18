using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
[Icon("res://Icons/FiniteStateMachineIcon.svg")]
public partial class FiniteStateMachine : Node
{
	[Export] private State initialState;
	public State CurrentState { get; private set; }
	public float StateTime { get; private set; }
	private Node owner;
	private Dictionary<Type, State> states = new Dictionary<Type, State>();
	public override void _Ready()
	{
		owner = GetParent();
		//await this.ToSignal(Owner, SignalName.Ready);
		foreach (var childState in GetChildren())
		{
			if (childState is State state)
			{
				RegisterState(state);
				state.owner = owner;
				state.stateMachine = this;
				state.StateReady();
			}
		}
		if (initialState != null)
		{
			CurrentState = initialState;
			StateTime = 0;
			CurrentState.OnEnter();
		}
		else
		{
			GD.PrintErr("Initial state is not set for the FSM.");
		}
	}
	private void RegisterState(State state)
	{
		var type = state.GetType();
		if (!states.ContainsKey(type))
		{
			states.Add(type, state);
		}
	}
	public void SwitchState<T>() where T : State
	{
		if (states.TryGetValue(typeof(T), out var newState))
		{
			StateTime = 0;
			// if (owner is Player)
			// {
			// 	GD.Print($"{CurrentState.Name} => {newState.Name}");
			// }
			CurrentState?.OnExit();
			CurrentState = newState;
			CurrentState.OnEnter();

		}
		else
		{
			GD.PrintErr($"State of type {typeof(T)} not found in the FSM.");
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		CurrentState?.OnPhysicsUpdate(delta);
		StateTime += (float)delta;
	}
	public override void _Process(double delta)
	{
		CurrentState?.OnProcessUpdate(delta);
		if (Input.IsActionPressed("Test"))
		{
			GD.Print(CurrentState.GetType());
		}
	}
}

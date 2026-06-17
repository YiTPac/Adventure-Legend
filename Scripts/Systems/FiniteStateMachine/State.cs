using Godot;
using System;
using System.Buffers;

[GlobalClass]
[Icon("res://Icons/StateIcon.svg")]
public partial class State : Node
{
	public Node owner;
	public FiniteStateMachine stateMachine;
	public virtual void StateReady(){}
	public virtual void OnEnter() { }
	public virtual void OnExit() { }
	public virtual void OnPhysicsUpdate(double delta) { }
	public virtual void OnProcessUpdate(double delta) { }
}

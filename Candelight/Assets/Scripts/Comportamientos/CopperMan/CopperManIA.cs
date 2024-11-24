using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.BehaviourTrees;

public class CopperManIA : BehaviourRunner
{
	
	
	protected override BehaviourGraph CreateGraph()
	{
        FSM CopperManFSMLvel1 = new();
		BehaviourTree CopperManFSCalmBTLevel2 = new();
        BehaviourTree CopperManAngryBTLevel2 = new();

        SubsystemAction Angry_action = new(CopperManAngryBTLevel2, true, ExecutionInterruptOptions.Stop);
		State Angry = CopperManFSMLvel1.CreateState(Angry_action);

        SubsystemAction Calm_action = new(CopperManFSCalmBTLevel2, true, ExecutionInterruptOptions.Stop);
		State Calm = CopperManFSMLvel1.CreateState(Calm_action);

        ConditionPerception CalmToAngryTransition_perception = new()
        {
            onInit = CalmToAngryTransitionInit,
            onCheck = CalmToAngryTransitionCheck,
            onReset = CalmToAngryTransitionReset
        };
        StateTransition CalmToAngryTransition = CopperManFSMLvel1.CreateTransition(Calm, Angry, CalmToAngryTransition_perception, statusFlags: StatusFlags.None);

        ConditionPerception AngryToCalmTransition_perception = new()
        {
            onInit = AngryToCalmTransitionInit,
            onCheck = AngryToCalmTransitionCheck,
            onReset = AngryToCalmTransitionReset
        };
        StateTransition AngryToCalmTransition = CopperManFSMLvel1.CreateTransition(Angry, Calm, AngryToCalmTransition_perception, statusFlags: StatusFlags.None);

        FunctionalAction Move_action = new()
        {
            onStarted = MoveStart,
            onUpdated = MoveUpdate,
            onStopped = MoveStop
        };
        LeafNode Move = CopperManFSCalmBTLevel2.CreateLeafNode(Move_action);

        FunctionalAction MeleAttack_action = new()
        {
            onStarted = MeleAttackStart,
            onUpdated = MeleAttackUpdate,
            onStopped = MeleAttackStop
        };
        LeafNode MeleAttack = CopperManFSCalmBTLevel2.CreateLeafNode(MeleAttack_action);
		
		ConditionNode PlayerAtRange = CopperManFSCalmBTLevel2.CreateDecorator<ConditionNode>(MeleAttack);
		
		SelectorNode SelectorNode = CopperManFSCalmBTLevel2.CreateComposite<SelectorNode>(false, Move, PlayerAtRange);
		SelectorNode.IsRandomized = false;
		
		LoopNode InfiniteLoop = CopperManFSCalmBTLevel2.CreateDecorator<LoopNode>(SelectorNode);
		InfiniteLoop.Iterations = -1;

        FunctionalAction Flee_action = new()
        {
            onStarted = FleeStart,
            onUpdated = FleeUpdate,
            onStopped = FleeStop
        };
        LeafNode Flee = CopperManAngryBTLevel2.CreateLeafNode(Flee_action);
		
		ConditionNode CanFlee = CopperManAngryBTLevel2.CreateDecorator<ConditionNode>(Flee);
		
		ConditionNode PlayerNear = CopperManAngryBTLevel2.CreateDecorator<ConditionNode>(CanFlee);

        FunctionalAction RangedAttack_action = new()
        {
            onStarted = RangedAttackStart,
            onUpdated = RangedAttackUpdate,
            onStopped = RangedAttackStop
        };
        LeafNode RangedAttack = CopperManAngryBTLevel2.CreateLeafNode(RangedAttack_action);
		
		PlayerAtRange = CopperManAngryBTLevel2.CreateDecorator<ConditionNode>(RangedAttack);
		
		SelectorNode = CopperManAngryBTLevel2.CreateComposite<SelectorNode>(false, PlayerNear, PlayerAtRange, Move);
		SelectorNode.IsRandomized = false;
		
		InfiniteLoop = CopperManAngryBTLevel2.CreateDecorator<LoopNode>(SelectorNode);
		InfiniteLoop.Iterations = -1;
		
		return CopperManFSMLvel1;
	}
	
	private void CalmToAngryTransitionInit()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean CalmToAngryTransitionCheck()
	{
		throw new System.NotImplementedException();
	}
	
	private void CalmToAngryTransitionReset()
	{
		throw new System.NotImplementedException();
	}
	
	private void AngryToCalmTransitionInit()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean AngryToCalmTransitionCheck()
	{
		throw new System.NotImplementedException();
	}
	
	private void AngryToCalmTransitionReset()
	{
		throw new System.NotImplementedException();
	}
	
	private void MoveStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status MoveUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void MoveStop()
	{
		throw new System.NotImplementedException();
	}
	
	private void MeleAttackStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status MeleAttackUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void MeleAttackStop()
	{
		throw new System.NotImplementedException();
	}
	
	private void FleeStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status FleeUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void FleeStop()
	{
		throw new System.NotImplementedException();
	}
	
	private void RangedAttackStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status RangedAttackUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void RangedAttackStop()
	{
		throw new System.NotImplementedException();
	}
}

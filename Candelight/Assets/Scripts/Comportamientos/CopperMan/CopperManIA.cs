using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.BehaviourTrees;
using Enemy;

public class CopperManIA : BehaviourRunner
{
	[SerializeField] private EnemyController _enemyController;

	protected override BehaviourGraph CreateGraph()
	{
        FSM CopperManMoodFSM = new();
		BehaviourTree CopperManAngryBT = new();
        BehaviourTree CopperManCalmBT = new();

        SubsystemAction CalmState_action = new(CopperManCalmBT);
		State CalmState = CopperManMoodFSM.CreateState("CalmState", CalmState_action);

        SubsystemAction AngryState_action = new(CopperManAngryBT);
		State AngryState = CopperManMoodFSM.CreateState("AngryState", AngryState_action);

        ConditionPerception CalmToAngryTransition_perception = new()
        {
            onInit = CalmToAngryTransitionInit,
            onCheck = CalmToAngryTransitionCheck,
            onReset = CalmToAngryTransitionReset
        };
        StateTransition CalmToAngryTransition = CopperManMoodFSM.CreateTransition("CalmToAngryTransition", CalmState, AngryState, CalmToAngryTransition_perception, statusFlags: StatusFlags.None);

        ConditionPerception AngryToCalmTransition_perception = new()
        {
            onInit = AngryToCalmTransitionInit,
            onCheck = AngryToCalmTransitionCheck,
            onReset = AngryToCalmTransitionReset
        };
        StateTransition AngryToCalmTransition = CopperManMoodFSM.CreateTransition("AngryToCalmTransition", AngryState, CalmState, AngryToCalmTransition_perception, statusFlags: StatusFlags.None);

        FunctionalAction AngryFlee_action = new()
        {
            onStarted = AngryFleeStart,
            onUpdated = AngryFleeUpdate,
            onStopped = AngryFleeStop
        };
        LeafNode AngryFlee = CopperManAngryBT.CreateLeafNode("AngryFlee", AngryFlee_action);
		
		ConditionNode AngryCanFlee = CopperManAngryBT.CreateDecorator<ConditionNode>("AngryCanFlee", AngryFlee);
		
		ConditionNode AngryPlayerNear = CopperManAngryBT.CreateDecorator<ConditionNode>("AngryPlayerNear", AngryCanFlee);

        FunctionalAction AngryRangedAttack_action = new()
        {
            onStarted = AngryRangedAttackStart,
            onUpdated = AngryRangedAttackUpdate,
            onStopped = AngryRangedAttackStop
        };
        LeafNode AngryRangedAttack = CopperManAngryBT.CreateLeafNode("AngryRangedAttack", AngryRangedAttack_action);
		
		ConditionNode AngryPlayerAtRange = CopperManAngryBT.CreateDecorator<ConditionNode>("AngryPlayerAtRange", AngryRangedAttack);

        FunctionalAction AngryMove_action = new()
        {
            onStarted = AngryMoveStart,
            onUpdated = AngryMoveUpdate,
            onStopped = AngryMoveStop
        };
        LeafNode AngryMove = CopperManAngryBT.CreateLeafNode("AngryMove", AngryMove_action);
		
		SelectorNode AngrySelector = CopperManAngryBT.CreateComposite<SelectorNode>("AngrySelector", false, AngryPlayerNear, AngryPlayerAtRange, AngryMove);
		AngrySelector.IsRandomized = false;
		
		LoopNode AngryInfiniteLoop = CopperManAngryBT.CreateDecorator<LoopNode>("AngryInfiniteLoop", AngrySelector);
		AngryInfiniteLoop.Iterations = -1;

        FunctionalAction CalmMeleAttack_action = new()
        {
            onStarted = CalmMeleAttackStart,
            onUpdated = CalmMeleAttackUpdate,
            onStopped = CalmMeleAttackStop
        };
        LeafNode CalmMeleAttack = CopperManCalmBT.CreateLeafNode("CalmMeleAttack", CalmMeleAttack_action);
		
		ConditionNode CalmPlayerAtRange = CopperManCalmBT.CreateDecorator<ConditionNode>("CalmPlayerAtRange", CalmMeleAttack);

        FunctionalAction CalmMove_action = new()
        {
            onStarted = CalmMoveStart,
            onUpdated = CalmMoveUpdate,
            onStopped = CalmMoveStop
        };
        LeafNode CalmMove = CopperManCalmBT.CreateLeafNode("CalmMove", CalmMove_action);
		
		SelectorNode CalmSelector = CopperManCalmBT.CreateComposite<SelectorNode>("CalmSelector", false, CalmPlayerAtRange, CalmMove);
		CalmSelector.IsRandomized = false;
		
		LoopNode CalmInfiniteLoop = CopperManCalmBT.CreateDecorator<LoopNode>("CalmInfiniteLoop", CalmSelector);
		CalmInfiniteLoop.Iterations = -1;
		
		return CopperManMoodFSM;
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
	
	private void AngryFleeStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status AngryFleeUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void AngryFleeStop()
	{
		throw new System.NotImplementedException();
	}
	
	private void AngryRangedAttackStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status AngryRangedAttackUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void AngryRangedAttackStop()
	{
		throw new System.NotImplementedException();
	}
	
	private void AngryMoveStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status AngryMoveUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void AngryMoveStop()
	{
		throw new System.NotImplementedException();
	}
	
	private void CalmMeleAttackStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status CalmMeleAttackUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void CalmMeleAttackStop()
	{
		throw new System.NotImplementedException();
	}
	
	private void CalmMoveStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status CalmMoveUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void CalmMoveStop()
	{
		throw new System.NotImplementedException();
	}
}

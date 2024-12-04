using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.BehaviourTrees;

public class NPCBehaviourRunner : BehaviourRunner
{
	[SerializeField] private NPCActions m_NPCActions;
	
	protected override void Init()
	{
		m_NPCActions = GetComponent<NPCActions>();
		
		base.Init();
	}
	
	protected override BehaviourGraph CreateGraph()
	{
		BehaviourTree AldeanoBT = new BehaviourTree();
		
		FunctionalAction ObjetivoRandom_action = new FunctionalAction(m_NPCActions.setRandomTarget);
		//ObjetivoRandom_action.action = m_NPCActions.setRandomTarget;
		LeafNode ObjetivoRandom = AldeanoBT.CreateLeafNode(ObjetivoRandom_action);

        //ConditionPerception EstaEnObjetivo_perception = new ConditionPerception(); //
		//EstaEnObjetivo_perception.onCheck = m_NPCActions.hasArrived; //
        ConditionNode EstaEnObjetivo = AldeanoBT.CreateDecorator<ConditionNode>(ObjetivoRandom); //
        EstaEnObjetivo.SetPerception(new ConditionPerception(m_NPCActions.hasArrived)); //

        FunctionalAction Moverse_action = new FunctionalAction(m_NPCActions.move);
		//Moverse_action.action = m_NPCActions.move;
		LeafNode Moverse = AldeanoBT.CreateLeafNode(Moverse_action);
		
		SequencerNode Sequence = AldeanoBT.CreateComposite<SequencerNode>(false, EstaEnObjetivo, Moverse);
		Sequence.IsRandomized = false;
		
		LoopNode InfLoop = AldeanoBT.CreateDecorator<LoopNode>(Sequence);
		InfLoop.Iterations = -1;

        AldeanoBT.SetRootNode(InfLoop); //

        return AldeanoBT;
	}
}

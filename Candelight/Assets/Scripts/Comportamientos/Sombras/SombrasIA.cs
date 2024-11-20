using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.BehaviourTrees;

namespace Comportamientos.Sombra
{
	public class SombrasIA : BehaviourRunner
	{
		
		
		protected override BehaviourGraph CreateGraph()
		{
			FSM Sombra_Comportamiento_FSM = new FSM();
			BehaviourTree Disparar_Proyectiles_BT = new BehaviourTree();
			
			SimpleAction Orbitar_action = new SimpleAction();
			State Orbitar = Sombra_Comportamiento_FSM.CreateState("Orbitar", Orbitar_action);
			
			SubsystemAction Disparar_Proyectiles_action = new SubsystemAction(Disparar_Proyectiles_BT);
			State Disparar_Proyectiles = Sombra_Comportamiento_FSM.CreateState("Disparar Proyectiles", Disparar_Proyectiles_action);
			
			ConditionPerception EquipadoFuego_perception = new ConditionPerception();
			EquipadoFuego_perception.onCheck = () => false;
			StateTransition equipadoFuego = Sombra_Comportamiento_FSM.CreateTransition("EquipadoFuego", Orbitar, Disparar_Proyectiles, EquipadoFuego_perception, statusFlags: StatusFlags.Success);
			
			ConditionPerception equipadofuego_perception = new ConditionPerception();
			equipadofuego_perception.onCheck = () => false;
			StateTransition equipadofuego = Sombra_Comportamiento_FSM.CreateTransition("Equipadofuego", Disparar_Proyectiles, Orbitar, equipadofuego_perception, statusFlags: StatusFlags.Failure);
			
			SimpleAction Alejarse_action = new SimpleAction();
			LeafNode Alejarse = Disparar_Proyectiles_BT.CreateLeafNode("Alejarse", Alejarse_action);
			
			SimpleAction Disparar_action = new SimpleAction();
			LeafNode Disparar = Disparar_Proyectiles_BT.CreateLeafNode("Disparar", Disparar_action);
			
			SequencerNode Sequence_Disparar_Proyectiles = Disparar_Proyectiles_BT.CreateComposite<SequencerNode>("Sequence Disparar Proyectiles", false, Alejarse, Disparar);
			Sequence_Disparar_Proyectiles.IsRandomized = false;
			
			return Sombra_Comportamiento_FSM;
		}
	}
}

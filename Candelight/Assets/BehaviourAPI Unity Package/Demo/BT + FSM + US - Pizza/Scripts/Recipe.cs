using System.Collections.Generic;
using UnityEngine;

namespace BehaviourAPI.UnityToolkit.Demos
{
	[CreateAssetMenu(menuName = "BehaviourAPI/Demos/Pizza/Recipe", fileName = "recipe")]
	public class Recipe : ScriptableObject
	{
		public string RecipeName;
		public List<Ingredient> ingredients;
	}

}
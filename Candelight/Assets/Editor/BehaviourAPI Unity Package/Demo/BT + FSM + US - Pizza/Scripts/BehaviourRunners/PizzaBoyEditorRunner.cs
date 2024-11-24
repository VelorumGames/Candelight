using BehaviourAPI.Core;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using BehaviourAPI.UtilitySystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourAPI.UnityToolkit.Demos
{
    public class PizzaBoyEditorRunner : EditorBehaviourRunner
    {
        [SerializeField] Ingredient _pizzaMass;
        [SerializeField] Ingredient _tomato;
        [SerializeField] List<Recipe> _allRecipes;
        [SerializeField] float _timeToAddIngredient;
        [SerializeField] Transform _pizzaTransform, _table, _oven, _handsHandler, _tableHandler;

        [SerializeField] Pizza _pizza;
        [SerializeField] RecipePaper _recipePaper;

        List<UtilityNode> m_ChooseRecipeActions = new List<UtilityNode>();

        NavMeshAgent _agent;

        // utility variables
        int _pizzasCreated = 0;
        int _peperoniUsed = 0;

        Recipe _currentRecipe;
        int _currentIngredient = 0;
        float _lastIngredientAddedTime = 0f;

        protected override void Init()
        {
            _agent = GetComponent<NavMeshAgent>();
            base.Init();
        }

        protected override void ModifyGraphs(Dictionary<string, BehaviourGraph> graphMap, Dictionary<string, PushPerception> pushPerceptionMap)
        {
            var graph = graphMap["Recipe US"];
            m_ChooseRecipeActions.Add(graph.FindNode<UtilityNode>("ham and cheese"));
            m_ChooseRecipeActions.Add(graph.FindNode<UtilityNode>("vegetarian"));
            m_ChooseRecipeActions.Add(graph.FindNode<UtilityNode>("hawaiian"));
        }

        // Escribe la receta
        public void CreateRecipe_0() => CreateRecipe(0);

        public void CreateRecipe_1() => CreateRecipe(1);

        public void CreateRecipe_2() => CreateRecipe(2);
        public void CreateRecipe(int id)
        {
            _pizza.SetHandler(_tableHandler);
            _agent.SetDestination(_table.position);
            _pizzasCreated += 1;
            if (id == 0) _peperoniUsed += 1;
            _currentRecipe = _allRecipes[id];
            _currentIngredient = 0;
            _recipePaper.SetRecipe(_currentRecipe);
            _recipePaper.Show();

            UpdateUtilities();
        }

        private void UpdateUtilities()
        {
            foreach (var utilityNode in m_ChooseRecipeActions)
            {
                utilityNode.UpdateUtility(true);
            }
        }

        // Espera a que el personaje se haya colocado en la mesa
        public Status RecipeCreated()
        {
            return (Vector3.Distance(transform.position, _table.position) < 0.3f) ? Status.Success : Status.Running;
        }

        // Cuando termina la acción:
        public void CreateRecipeCompleted()
        {
            transform.LookAt(_pizzaTransform);
            _recipePaper.Hide();
        }

        // Añade un elemento a la pizza
        public void PutMass() => PutIngredient(_pizzaMass);

        public void PutTomato() => PutIngredient(_tomato);

        public void PutIngredient(Ingredient ingredient)
        {
            _pizza.AddIngredient(ingredient);
            _lastIngredientAddedTime = Time.time;
        }

        // Espera un tiempo
        public Status WaitToPutIngredient()
        {
            return Time.time > _lastIngredientAddedTime + _timeToAddIngredient ? Status.Success : Status.Running;
        }

        // Añade el siguiente ingrediente de la receta
        public void PutNextTopping()
        {
            _lastIngredientAddedTime = Time.time;
            PutIngredient(_currentRecipe.ingredients[_currentIngredient]);
            _currentIngredient++;
        }

        // Espera un tiempo y devuelve success si ya ha puesto todos los ingredientes.
        public Status CheckToppings()
        {
            if (Time.time > _lastIngredientAddedTime + _timeToAddIngredient)
            {
                return _currentIngredient == _currentRecipe.ingredients.Count ? Status.Success : Status.Failure;
            }
            else
                return Status.Running;
        }

        // Acción de hornear la pizza
        public void BakePizza()
        {
            _pizza.SetHandler(_handsHandler);
            _agent.SetDestination(_oven.position);
        }

        // Espera a que el personaje llegue al horno
        public Status pizzaBaked()
        {
            return Vector3.Distance(transform.position, _oven.position) < 0.5f ? Status.Success : Status.Running;
        }

        // Cuando la acción de hornear la pizza acaba, se borra la receta y se destruye la pizza
        public void BakedActionCompleted()
        {
            _pizza.Clear();
            _recipePaper.Clear();
        }

        public float PizzaFactor() => _pizzasCreated % 10;

        public float PeperoniFactor() => _peperoniUsed % 4;

    }

}
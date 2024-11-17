using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.BehaviourTrees;
using BehaviourAPI.StateMachines;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using BehaviourAPI.UtilitySystems;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace BehaviourAPI.UnityToolkit.Demos
{
    public class PizzaBoyRunner : BehaviourRunner
    {
        [SerializeField] Ingredient _pizzaMass;
        [SerializeField] Ingredient _tomato;
        [SerializeField] List<Recipe> _allRecipes;
        [SerializeField] float _timeToAddIngredient;
        [SerializeField] Transform _pizzaTransform, _table, _oven, _handsHandler, _tableHandler;

        [SerializeField] Pizza _pizza;
        [SerializeField] RecipePaper _recipePaper;

        NavMeshAgent _agent;
        BSRuntimeDebugger _debugger;

        // utility variables
        int _pizzasCreated = 0;
        int _peperoniUsed = 0;

        Recipe _currentRecipe;
        int _currentIngredient = 0;
        float _lastIngredientAddedTime = 0f;

        protected override void Init()
        {
            _debugger = GetComponent<BSRuntimeDebugger>();
            _agent = GetComponent<NavMeshAgent>();
            base.Init();
        }

        protected override BehaviourGraph CreateGraph()
        {
            var bt = new BehaviourTree();
            var us = CreateLookRecipeUtilitySystem();
            var fsm = CreateMakePizzaFSM();

            var recipeAction = bt.CreateLeafNode("recipe action", new SubsystemAction(us));
            var makePizzaAction = bt.CreateLeafNode("make pizza action", new SubsystemAction(fsm));
            var bakeAction = bt.CreateLeafNode("bake pizza", new FunctionalAction(BakePizza, pizzaBaked, BakedActionCompleted));

            var seq = bt.CreateComposite<SequencerNode>("pizza seq", false, recipeAction, makePizzaAction, bakeAction);
            var root = bt.CreateDecorator<LoopNode>("loop", seq).SetIterations(-1);
            bt.SetRootNode(root);

            _debugger.RegisterGraph(bt, "main");
            _debugger.RegisterGraph(us, "recipe us");
            _debugger.RegisterGraph(fsm, "make pizza fsm");

            return bt;
        }

        private FSM CreateMakePizzaFSM()
        {
            var fsm = new FSM();

            var massState = fsm.CreateState("mass", new FunctionalAction(() => PutIngredient(_pizzaMass), WaitToPutIngredient));
            var tomatoState = fsm.CreateState("tomato", new FunctionalAction(() => PutIngredient(_tomato), WaitToPutIngredient));
            var toppingState = fsm.CreateState("topping", new FunctionalAction(PutNextTopping, CheckToppings));

            fsm.CreateTransition("mass putted", massState, tomatoState, statusFlags: StatusFlags.Finished);
            fsm.CreateTransition("tommato putted", tomatoState, toppingState, statusFlags: StatusFlags.Finished);
            fsm.CreateTransition("next topping", toppingState, toppingState, statusFlags: StatusFlags.Failure);
            fsm.CreateExitTransition("pizza completed", toppingState, Status.Success, statusFlags: StatusFlags.Success);
            return fsm;
        }

        UtilitySystem CreateLookRecipeUtilitySystem()
        {
            var us = new UtilitySystem();

            var pizzafactor = us.CreateVariable("pizzas", () => _pizzasCreated % 10, 10, 0);
            var pepperoniFactor = us.CreateVariable("peperoni_used", () => _peperoniUsed % 4, 4, 0);

            var peperoniSumFactor = us.CreateFusion<WeightedFusionFactor>("peperoni", pizzafactor, pepperoniFactor)
                .SetWeights(0.6f, 0.4f);

            var pointList = new List<CurvePoint>();
            pointList.Add(new CurvePoint(0.0f, 1f));
            pointList.Add(new CurvePoint(0.2f, 0.5f));
            pointList.Add(new CurvePoint(0.4f, 0.1f));
            pointList.Add(new CurvePoint(0.6f, 0.4f));
            pointList.Add(new CurvePoint(0.8f, 0.2f));
            pointList.Add(new CurvePoint(1.0f, 0.0f));
            var vegetarianFactor = us.CreateCurve<PointedCurveFactor>("vegetarian", pizzafactor).SetPoints(pointList);

            var hawaiianFactor = us.CreateCurve<ExponentialCurveFactor>("hawaiian", pizzafactor).SetExponent(.7f);

            var peperoniAction = us.CreateAction("choose ham and cheese", peperoniSumFactor,
                new FunctionalAction(() => CreateRecipe(0), RecipeCreated, CreateRecipeCompleted), finishOnComplete: true);
            var vegetarianAction = us.CreateAction("choose vegetarian", vegetarianFactor,
                new FunctionalAction(() => CreateRecipe(1), RecipeCreated, CreateRecipeCompleted), finishOnComplete: true);
            var hawaiianAction = us.CreateAction("choose hawaiian", hawaiianFactor,
                new FunctionalAction(() => CreateRecipe(2), RecipeCreated, CreateRecipeCompleted), finishOnComplete: true);
            return us;
        }

        // Escribe la receta
        void CreateRecipe(int id)
        {
            _pizza.SetHandler(_tableHandler);
            _agent.SetDestination(_table.position);
            _pizzasCreated += 1;
            if (id == 0) _peperoniUsed += 1;
            _currentRecipe = _allRecipes[id];
            _currentIngredient = 0;
            _recipePaper.SetRecipe(_currentRecipe);
            _recipePaper.Show();
        }

        // Espera a que el personaje se haya colocado en la mesa
        public Status RecipeCreated()
        {
            return (Vector3.Distance(transform.position, _table.position) < 0.3f) ? Status.Success : Status.Running;
        }

        // Cuando termina la acción:
        void CreateRecipeCompleted()
        {
            transform.LookAt(_pizzaTransform);
            _recipePaper.Hide();
        }

        // Añade un elemento a la pizza
        void PutIngredient(Ingredient ingredient)
        {
            _pizza.AddIngredient(ingredient);
            _lastIngredientAddedTime = Time.time;
        }

        // Espera un tiempo
        Status WaitToPutIngredient()
        {
            return Time.time > _lastIngredientAddedTime + _timeToAddIngredient ? Status.Success : Status.Running;
        }

        // Añade el siguiente ingrediente de la receta
        void PutNextTopping()
        {
            _lastIngredientAddedTime = Time.time;
            PutIngredient(_currentRecipe.ingredients[_currentIngredient]);
            _currentIngredient++;
        }

        // Espera un tiempo y devuelve success si ya ha puesto todos los ingredientes.
        Status CheckToppings()
        {
            if (Time.time > _lastIngredientAddedTime + _timeToAddIngredient)
            {
                return _currentIngredient == _currentRecipe.ingredients.Count ? Status.Success : Status.Failure;
            }
            else
                return Status.Running;
        }

        // Acción de hornear la pizza
        void BakePizza()
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
        void BakedActionCompleted()
        {
            _pizza.Clear();
            _recipePaper.Clear();
        }
    }

}
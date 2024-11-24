using UnityEngine;

namespace BehaviourAPI.UnityToolkit.Demos
{
    public class Pizza : MonoBehaviour
    {
        float _height;

        public void AddIngredient(Ingredient ingredient)
        {
            //Debug.Log($"Adding ingredient: {ingredient.Name}");
            var ingredientItem = Instantiate(ingredient.prefab, transform);
            ingredientItem.transform.Translate(Vector3.up * _height);
            _height += ingredient.height;
        }

        public void SetHandler(Transform tf)
        {
            transform.parent = tf;
            transform.localPosition = Vector3.zero;
        }

        public void Clear()
        {
            int childs = transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
            _height = 0f;
        }
    }
}
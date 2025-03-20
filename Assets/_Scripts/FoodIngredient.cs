using System;
using UnityEngine;

namespace _Scripts
{
    [Serializable]
    public class FoodIngredient
    {
        [SerializeField] string name;
        [SerializeField] string amount;
        [SerializeField] bool isAvailable;
    }
}
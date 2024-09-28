using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()] //Commented out to prevent creation of RecipeListSO in the project folder
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> RecipeSOList;
}

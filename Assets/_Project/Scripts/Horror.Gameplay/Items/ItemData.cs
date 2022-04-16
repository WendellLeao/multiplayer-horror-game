using UnityEngine;

namespace Horror.Gameplay.Items
{
    [CreateAssetMenu(menuName = "Items/Item Data", fileName = "NewItemData")]
    public sealed class ItemData : ScriptableObject
    {
        public GameObject Prefab;
    }
}
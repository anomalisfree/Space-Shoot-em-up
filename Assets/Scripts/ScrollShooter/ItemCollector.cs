using System;
using ScrollShooter.Data;
using UnityEngine;

namespace ScrollShooter
{
    public class ItemCollector : MonoBehaviour
    {
        public Action<ItemType> GETItemAction;
        public void GetItem(ItemType itemType)
        {
            GETItemAction?.Invoke(itemType);
        }
    }
}

using EXILED.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace CallEscape
{
    public class SetRoleOnSpawn : MonoBehaviour
    {
        private float Timer = 0.0f;
        private ReferenceHub PlayerHub;

        public List<ItemType> RemoveItems = new List<ItemType>();
        public List<ItemType> AddItems = new List<ItemType>();
        public float MaxHealth = 0;

        public void Start()
        {
            PlayerHub = Player.GetPlayer(gameObject);
        }

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > 0.2f)
            {
                foreach (ItemType itemType in RemoveItems)
                {
                    for (int i = 0; i < PlayerHub.inventory.items.Count; i++)
                    {
                        if (PlayerHub.inventory.items[i].id == itemType)
                            PlayerHub.inventory.items.Remove(PlayerHub.inventory.items[i]);
                    }
                }
                foreach (ItemType itemType in AddItems)
                {
                    PlayerHub.AddItem(itemType);
                }
                if (MaxHealth != 0)
                {
                    PlayerHub.SetMaxHealth(MaxHealth);
                    PlayerHub.SetHealth(MaxHealth);
                }
                Destroy(GetComponent<SetRoleOnSpawn>());
            }
        }
    }
}
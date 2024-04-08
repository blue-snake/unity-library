using System.Collections.Generic;
using BlueSnake.Container;
using BlueSnake.Utils;
using UnityEngine;

public class ItemRegistry : StandaloneSingleton<ItemRegistry> {

    private bool loadFromResources = true;
    private string resourcesPath = "Items/";

    private readonly Dictionary<string, Item> items = new();

    public ItemRegistry() {
        if (loadFromResources) {
            foreach (Item item in Resources.LoadAll<Item>(resourcesPath)) {
                RegisterItem(item);
            }
        }
    }

    public bool TryGetItem(string id, out Item item) {
        return items.TryGetValue(id, out item);
    }

    public Item GetItem(string id) {
        return items[id];
    }

    public bool ExistsItem(string id) {
        return items.ContainsKey(id);
    }

    public void RegisterItem(Item item) {
        item.id = item.id.ToLower();
        items[item.id] = item;
    }

    public void UnregisterItem(string id) {
        items.Remove(id);
    }

    public Dictionary<string, Item>.ValueCollection GetItems() {
        return items.Values;
    }
}
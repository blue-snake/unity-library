using System.Collections.Generic;
using BlueSnake.Container;
using BlueSnake.Utils;
using UnityEngine;

public class ItemRegistry : Singleton<ItemRegistry> {
    
    [Header("Items")]
    [SerializeField]
    private List<Item> registeredItems;

    [Header("Loading")]
    [SerializeField]
    private bool loadFromResources = true;
    [SerializeField]
    private string resourcesPath = "Items/";

    private readonly Dictionary<string, Item> items = new();

    public override void Awake() {
        base.Awake();
        if (loadFromResources) {
            foreach (Item item in Resources.LoadAll<Item>(resourcesPath)) {
                registeredItems.Add(item);
            }
        }

        foreach (Item item in registeredItems) {
            RegisterItem(item);
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
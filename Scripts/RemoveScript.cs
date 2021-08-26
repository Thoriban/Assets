using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveScript : MonoBehaviour
{

    GameObject itemToRemove;
    public string itemName = "";

    void Start()
    {

    }

    public void SetItemToRemove(GameObject itemToRemove)
    {
        this.itemToRemove = itemToRemove;
    }

    public void DeleteElementFromList(Action<string> removeMethod)
    {
        removeMethod(itemName);
    }

    public void RemoveItem()
    {
        Destroy(itemToRemove);
        Destroy(this.gameObject);
    }

    void Update()
    {

    }
}

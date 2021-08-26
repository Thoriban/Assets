using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOSBattle : MonoBehaviour
{
    public GameObject gridElementPrefab;

    GameObject counterAnchor;

    private void Start()
    {
        counterAnchor = GameObject.Find("counterGrid");
    }

    public void AddButton()
    {
        GameObject counter = GameObject.Instantiate(gridElementPrefab);

        counter.transform.parent = counterAnchor.transform;
    }
}

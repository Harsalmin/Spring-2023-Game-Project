using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private int approval;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPoints(int amount)
    {
        Debug.Log(amount + " points added");
        approval += amount;
    }

    public void RemovePoints(int amount)
    {
        Debug.Log(amount + " points removed");
        approval -= amount;
    }
}

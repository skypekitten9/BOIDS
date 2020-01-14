using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentScript : MonoBehaviour
{
    public float speed;
    public float sight;
    float privateRange;

    private void Start()
    {
        privateRange = sight / 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

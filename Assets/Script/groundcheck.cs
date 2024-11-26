using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundcheck : MonoBehaviour
{
    PlayerLogic logicmovement;

    // Start is called before the first frame update
    void Start()
    {
        logicmovement = this.GetComponentInParent<PlayerLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        logicmovement.groundedchanger();
        Debug.Log("Sentuh Tanah");
    }
}

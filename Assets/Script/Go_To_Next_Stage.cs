using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Go_To_Next_Stage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.attachedRigidbody.velocity = Vector2.zero;
            other.transform.position = new Vector3(-5, 0, 0);
        }
    }
}

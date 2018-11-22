using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloque : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Mario")
        {
            this.gameObject.layer = 8;
        }
        else
        {
            this.gameObject.layer = 13;
        }
    }
}

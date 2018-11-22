using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concha : MonoBehaviour {
    public float fuerzaRebote = 500f;
    public float velocidadConcha;
    public float impulso;
    public bool rebote;
    public Transform posicionRebote;
    public float radioConcha;
    public float radioPieConcha;
    public LayerMask suelo;
    public LayerMask objetoRebote;

    public GameObject Mario;
    public GameObject pieMario;

    public Transform pieConcha;
    public bool enSueloConcha;

    public bool cogeConcha;

    Animator animator;
    Rigidbody2D rb;


	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        velocidadConcha = rb.velocity.x;
        animator.SetFloat("velX", Mathf.Abs(velocidadConcha));
        rebote = Physics2D.OverlapCircle(posicionRebote.position, radioConcha, objetoRebote);
        if(velocidadConcha != 0) {
            impulso = velocidadConcha;
        }
        if (rebote && impulso > 0){
            this.rb.AddForce(new Vector2(-fuerzaRebote, 0));
        }

        if(rebote && impulso < 0){
            this.rb.AddForce(new Vector2(fuerzaRebote, 0));
        }
        //Concha en el suelo
        enSueloConcha = Physics2D.OverlapCircle(pieConcha.position, radioPieConcha, suelo);
        if (enSueloConcha){
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        else {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
	}
}

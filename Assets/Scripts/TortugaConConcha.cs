using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortugaConConcha : MonoBehaviour {
    public GameObject posicionInicial;
    public GameObject posicionFinal;
    public Vector3 inicioTortuga;
    public Vector3 finToruga;
    public Vector3 posicionPisada;
    public Vector3 rango;
    public GameObject Mario;
    public float velTortuga = 0.1f;
    public bool mirandoDerecha = true;
    public bool conConcha = true;
    public bool pisada = false;
    public int contPisadas = 0;

    Animator animator;
    Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (mirandoDerecha)
        {
            inicioTortuga = this.transform.position;
            finToruga = this.transform.position + rango;
            posicionInicial.transform.position = inicioTortuga;
            posicionFinal.transform.position = finToruga;
        }
        else
        {
            inicioTortuga = this.transform.position;
            finToruga = this.transform.position - rango;
            posicionInicial.transform.position = inicioTortuga;
            posicionFinal.transform.position = finToruga;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (mirandoDerecha){
            transform.position = Vector3.MoveTowards(transform.position, posicionFinal.transform.position, velTortuga);
            animator.SetFloat("velX", velTortuga);
            animator.SetBool("conConcha", true);
            if (transform.position == posicionFinal.transform.position) {             
                mirandoDerecha = false;
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        if (!mirandoDerecha){
            transform.position = Vector3.MoveTowards(transform.position, posicionInicial.transform.position, velTortuga);
            animator.SetFloat("velX", velTortuga);
            animator.SetBool("conConcha", true);
            if (transform.position == posicionInicial.transform.position){
                mirandoDerecha = true;
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other){
        if (mirandoDerecha){
            transform.localScale = new Vector3(-1f, 1f, 1f);
            mirandoDerecha = false;
        }else{
            transform.localScale = new Vector3(1f, 1f, 1f);
            mirandoDerecha = true;
        }
        if(Mario.gameObject.GetComponent<Movimiento>().enSuelo == false){
            pisada = true;
            posicionPisada = this.transform.position;
            animator.SetBool("pisada", true);
            conConcha = false;
            animator.SetBool("conConcha", false);
        }
    }
}

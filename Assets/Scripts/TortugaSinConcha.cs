using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortugaSinConcha : MonoBehaviour {
    public float desplazamiento;
    public float desplazamientoFinal;
    public float velDesplazamiento = 1;
    public bool saliendoConcha;
    public bool conConcha;
    public bool pataleo;

    public GameObject posicionInicial;
    public GameObject posicionFinal;
    public GameObject Mario;
    public Vector3 posicionPisada; 
    public Vector3 puntoInicio;
    public Vector3 puntoFinal;
    public Vector3 rango;
    public float velTortuga = 0.03f;
    public bool mirandoDerecha = true;
    public bool pisada;
    

    Animator animator;
    Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if(GetComponent <TortugaConConcha>().mirandoDerecha == true) { mirandoDerecha = true; }
        else { mirandoDerecha = false; }
        pisada = GetComponent<TortugaConConcha>().pisada;
        posicionPisada = GetComponent<TortugaConConcha>().posicionPisada;

	}
	
	// Update is called once per frame
    void FixedUpdate () {
        if (pisada && mirandoDerecha){
            this.transform.localScale = new Vector3(1f, 1f, 1f);
            saliendoConcha = true;
            animator.SetBool("saliendoConcha", true);
            conConcha = false;
            animator.SetBool("conConcha", false);
            desplazamiento = this.transform.position.x;
            desplazamientoFinal = posicionPisada.x + 1f;

            if(desplazamiento < desplazamientoFinal)  {
                this.rb.velocity = new Vector3(velDesplazamiento, rb.velocity.y, 0);
            }else{
                pataleo = true;
                animator.SetBool("pataleo", true);
                saliendoConcha = false;
                animator.SetBool("saliendoConcha", false);
            }
        }
        if (pisada && !mirandoDerecha){
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
            saliendoConcha = true;
            animator.SetBool("saliendoConcha", true);
            conConcha = false;
            animator.SetBool("conConcha", false);
            desplazamiento = this.transform.position.x;
            desplazamientoFinal = posicionPisada.x - 1f;

            if (desplazamiento < desplazamientoFinal)
            {
                this.rb.velocity = new Vector3(-velDesplazamiento, rb.velocity.y, 0);
            }
            else
            {
                pataleo = true;
                animator.SetBool("pataleo", true);
                saliendoConcha = false;
                animator.SetBool("saliendoConcha", false);
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class Movimiento : MonoBehaviour {

	//VARIABLES MOVIEMIENTO
	public float velX = 0.03f;
	public float movX;
	public float posicionActual;
	public bool mirandoDerecha;

	//VARIABLES DE SALTO
	public float fuerzaSalto = 100f;
	public Transform pie;
	public float radioPie = 0.08f;
	public LayerMask suelo;
	public bool enSuelo;

	//VARIABLES AGACHADO
	public bool agachado;

	//VARIABLES MIRAR ARRIBA
	public bool mirarArriba; 

	//VARIABLES CAIDA
	public float caida;
	Rigidbody2D rb;

	//VARIABLES DERRAPE
	public int derrape;
	public int derecha;
	public int izquierda;

	//VARIABLES CORRER
	public bool run;

	//VARIABLES TURBO
	public bool turbo;
	public int contadorTurbo;

	//VARIABLES TURBOSALTO
	public bool turboSalto;

	//VARIABLES CONCHA
	public float patada = 500f;
	public Transform mano;
	public float radioMano = 0.07f;
	public LayerMask concha;
	public bool cogeConcha;
	public GameObject Concha;
	//ANIMACIONES

	Animator animator;

	void Awake(){
		animator = GetComponent <Animator>();
		rb = GetComponent <Rigidbody2D> ();

	}



	// Use this for initialization
	void Start () {

	}

	// FixUpdate is called once 0.02 seconds each frame (This can be modified in edit > Project Settings > Time > Fixed Timestep)
	void FixedUpdate () {

		//MOVIMIENTO HORIZONTAL
		float inputX = Input.GetAxis ("Horizontal"); // Almacena el movimiento en el eje X
		movX = transform.position.x + (inputX * velX);//movX será igual a mi posicion en X + el movimiento en el eje X * velX
		posicionActual=movX;//almacena la posicion actual
		if (!agachado && !mirarArriba) {//Si no estoy agachado y no estoy mirando arriba

			if (inputX > 0) {//Si la velocidad en el eje X es mayor que 0
				transform.position = new Vector3 (movX, transform.position.y, 0);//mi posicion = movX, la posicion que tenga en y, 0
				transform.localScale = new Vector3 (1, 1, 1);//la escala original si me muevo a la derecha
				mirandoDerecha = true;//establece que estoy mirando a la derecha
				animator.SetBool ("derrape", false);
			}

			if (inputX < 0) {//Si la velocidad en el eje X es menor que 0
				transform.position = new Vector3 (movX, transform.position.y, 0);//mi posicion = movX, la posicion que tenga en y, 0
				transform.localScale = new Vector3 (-1, 1, 1);//la escala inversa si me muevo a la izquierda
				mirandoDerecha = false;//Establece que no estoy mirando a la derecha
				animator.SetBool ("derrape", false);
			}
		}

		if (inputX != 0) {
			animator.SetFloat ("velX", 1);// Si me muevo le digo al animador que la velocidad en X es 1
		} else {
			animator.SetFloat ("velX",0);// Si no me muevo le digo al animador que la velocidad en X es 0
		} 

		//SALTO 

		enSuelo = Physics2D.OverlapCircle (pie.position, radioPie, suelo);// Si estoy tocando el suelo enSuelo será true

		if (enSuelo) {//Si estoy en el suelo
			animator.SetBool ("enSuelo", true);//Le digo al animador que estoy en el suelo
			animator.SetBool ("turboSalto",false);
			if(Input.GetKeyDown(KeyCode.C) && !agachado){//Si pulso la tecla C y no estoy agachado
				GetComponent<Rigidbody2D>().AddForce (new Vector2 (0,fuerzaSalto));//Accedo a la velocidad del Rigidbody2D y le añado la fuerza vertical establecida en fuerzaSalto
				animator.SetBool ("enSuelo",false);//Le digo al animador que no estoy en el suelo
			}

		} else {
			animator.SetBool ("enSuelo", false);//Le digo al animador que no estoy en el suelo
		}

		//AGACHARSE

		if (enSuelo && Input.GetKey (KeyCode.DownArrow)) {//Si estoy en el suelo y pulso abajo
			agachado = true;//Establece que estoy agachado
			animator.SetBool ("agachado", true);//Le digo al animador que estoy agachado
		} else {
			agachado = false;//Establece que no estoy agachado
			animator.SetBool ("agachado",false);//Le digo al animador que no estoy agachado
		}

		//MIRAR ARRIBA
		if (inputX == 0) {//Si no me muevo
			if (enSuelo && Input.GetKey (KeyCode.UpArrow)) {//Si estoy en el suelo y pulso arrba
				mirarArriba = true;//Establece que estoy mirando arriba
				animator.SetBool ("mirarArriba", true);//Le digo al animador que estoy mirando arriba
			} else {
				mirarArriba = false;////Establece que no estoy mirando arriba
				animator.SetBool ("mirarArriba", false);//Le digo al animador que no estoy mirando arriba
			}
		}

		//CAIDA
		caida = rb.velocity.y;//Establezco que caida es igual a la velocidad en el eje y

		if (caida != 0 || caida == 0) {//Tanto si caida es igual o distinta de 0 le paso la velocidad al animador
			animator.SetFloat ("velY",caida);
		}

		//DERRAPE

		if(inputX == 0){// Si no me muevo o estoy cambiando de direccion
			StartCoroutine (TiempoEspera());//Llamo a la corutina TiempoEspera()
		} 
		//DERRAPE DERECHA-->IZQUIERDA

		if (inputX > 0.5f) {//Si la velocidad en X es mayor de 0.5
			derecha = 1;//Le doy a derecha un valor de 1
			derrape = 1;//Le doy a derrape un valor de 1
		}


		if (derrape == 1 && Input.GetKey (KeyCode.LeftArrow)) {//Si derrape es 1 y pulso izquierda
			animator.SetBool ("derrape",true);//Le digo al animador que estoy derrapando
			animator.SetBool ("turbo",false);
			StartCoroutine (TiempoEspera ());//Llamo a la corutina TiempoEspera()
			StopCoroutine (Turbo ());
		}

		//DERRAPE IZQUIERDA --> DERECHA

		if (inputX < 0) {//Si la velocidad en X es menor que 0
			izquierda = 1;//Le doy a izquierda un valor de 1
			derrape = -1;//Le doy a derrape un valor de -1
		}


		if (derrape == -1 && Input.GetKey (KeyCode.RightArrow)) {//Si derrape es 1 y pulso derecha
			animator.SetBool ("derrape",true);//Le digo al animador que estoy derrapando
			animator.SetBool ("turbo",false);
			StartCoroutine (TiempoEspera ());//Llamo a la corutina TiempoEspera()
		}


		//CORRER

		if (inputX != 0) {//Si me estoy moviendo
			if (Input.GetKey (KeyCode.X)) {//y pulso X
				run = true;//Establezco que estoy corriendo
				velX = 0.06f;//Establezco que velX ahora vale 0.06
				animator.SetBool ("run",true);//Le digo al animador que estoy corriendo

			} else {//Si me estoy moviendo pero no pulso X
				velX = 0.03f;//Establezco que velX vale 0.03
				run = false;// Establezco que no estoy corriendo
				animator.SetBool ("run",false);	//Le digo al animador que no estoy corriendo
				contadorTurbo=0;//Reinicio el contador de turbo
			}

		}else{//Si no me estoy moviendo
			//Reestablezco los valores del código y del animador por defecto
			run = false;
			animator.SetBool ("run",false);
			animator.SetBool ("derrape", false);
			contadorTurbo=0;//Reinicio el contador de turbo
		}

		//TURBO
		if (Input.GetKey (KeyCode.X) && run == true && enSuelo) {//Si estoy pulsando X, estoy corriendo y estoy en el suelo
			StartCoroutine (Turbo ());//Llamo a la corrutina Turbo()
		}
		else{//Si no, reestablezco los valores de codigo y animador por defecto
			turbo = false;
			animator.SetBool ("turbo",false);
			StopAllCoroutines ();//Detengo las corrutinas
		}

		//TURBOSALTO
		if (inputX > 0 || inputX < 0) {//Si me estoy moviendo
			if (turbo && Input.GetKeyDown (KeyCode.C)) {//Si turbo está activado y pulso C
				animator.SetBool ("turboSalto", true);//Le digo al animador que active turbosalto
			}
		}

		//CONCHA
		cogeConcha = Physics2D.OverlapCircle (mano.position,radioMano,concha);//Cuando la mano entre en el radio de colision con la concha

		if(cogeConcha && mirandoDerecha){//Si estoy en el radio de colisión y estoy mirando a la derecha
			if (Input.GetKey (KeyCode.X)) {//Si pulso la tecla X
				Concha.transform.parent = this.transform;//Hago que la concha sea hija de Mario para que tengan el mismo movimiento
				Concha.GetComponent<Rigidbody2D> ().gravityScale = 0;//Desactivo la gravedad de la concha
				Concha.GetComponent<Rigidbody2D> ().isKinematic = true;//Activo kinematic para que no le afecten las fuerzas
			} else {//Si no estoy pulsando X
				Concha.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (patada, 0));//Le añado una fuerza al eje X para que simule la patada a la concha
				Concha.transform.parent = null;//La concha pasa a ser padre si no lo era
				Concha.GetComponent<Rigidbody2D> ().gravityScale = 3;//Activo la gravedad de la concha y la establezco en 3
				Concha.GetComponent<Rigidbody2D> ().isKinematic = false;//Desactivo kinematic para que le vuelvan a afectar las fuerzas
			}
		}

		if (cogeConcha && !mirandoDerecha) {//Si estoy en el radio de colisión y no estoy mirando a la derecha
			if (Input.GetKey (KeyCode.X)) {//Si pulso la tecla X
				Concha.transform.parent = this.transform;//Hago que la concha sea hija de Mario para que tengan el mismo movimiento
				Concha.GetComponent<Rigidbody2D> ().gravityScale = 0;//Desactivo la gravedad de la concha
				Concha.GetComponent<Rigidbody2D> ().isKinematic = true;//Activo kinematic para que no le afecten las fuerzas
			} else {//Si no estoy pulsando X
				Concha.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (patada*(-1), 0));//Le añado una fuerza al eje X para que simule la patada a la concha
				Concha.transform.parent = null;//La concha pasa a ser padre si no lo era
				Concha.GetComponent<Rigidbody2D> ().gravityScale = 3;//Activo la gravedad de la concha y la establezco en 3
				Concha.GetComponent<Rigidbody2D> ().isKinematic = false;//Desactivo kinematic para que le vuelvan a afectar las fuerzas
			}
		}

	}
	//FIN FIXUPDATE

	public IEnumerator TiempoEspera(){//Funcion para que haya un tiempo de espera desde que cambio de dirección hasta que reestablezcan los valores
		yield return new WaitForSeconds (0.3f);//Espero 0.3 "segundos"
		derrape = 0;//Reestablezco derrape
		derecha = 0;//Restablezco derecha
		izquierda = 0;//Reestablezco izquierda
		animator.SetBool ("derrape", false);//Le digo al animador que ya no estoy derrapando

	}

	public IEnumerator Turbo(){//Funcion para activar el turbo
		while (contadorTurbo<=15){//Mientras contador turbo sea menor que 20
			yield return new WaitForSeconds (1.5f);//espera 1.5 "segundos"
			contadorTurbo++;//Sumo 1 al contador turbo
		}
		turbo = true;//Activo turbo
		animator.SetBool ("turbo",true);//Le digo al animador que estoy en turbo
		velX = 0.12f;//Establezco la velocidad en X a 0.12f
		StopAllCoroutines ();//Detengo la corrutina
	}
}

using System.Collections;
using UnityEngine;

public class playrScript : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad;
    [SerializeField] private float FuerzaSalto;
    private Rigidbody2D rigidBody;
    private bool MirandoDerecha = true;

    [Header("Deteccion de Suelo Nueva")]
    [SerializeField] bool enSuelo;
    [SerializeField] LayerMask CapaSuelo;
    [SerializeField] private Vector2 dimensionesCaja = new Vector2(0.5f, 0.1f);
    [SerializeField] private float desplazamientoY = -0.8f;
    private bool ejecutandoCoyote;

    // 1. VARIABLE NUEVA PARA TU ANIMATOR
    private Animator animator;

    [Header("Sistema de disparos")]
    [SerializeField] private balasPooling BP;
    public float LaserOffSet;
    private float cooldownDisparo;
    [SerializeField] public float TiempoEntreDisparos = 0.15f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        // 2. BUSCAMOS EL ANIMATOR AL INICIAR
        animator = GetComponent<Animator>();

        cooldownDisparo = 0;
    }

    void Update()
    {
        disparo();
        ProcesarMovimiento();
        ControlarSalto();
    }

    private void disparo()
    {
        // Eliminamos todo el sistema de temporizador (Time.deltaTime) 
        // para que no haya que esperar absolutamente nada entre clics.

        if (Input.GetKeyDown(KeyCode.V))
        {
            GameObject Dis = BP.Aparicion();

            Vector3 direccion = MirandoDerecha ? Vector3.right : Vector3.left;
            Dis.transform.position = transform.position + direccion * LaserOffSet + new Vector3(0, 0.9f, 0);

            Rigidbody2D rbDisparo = Dis.GetComponent<Rigidbody2D>();
            if (rbDisparo != null)
            {
                rbDisparo.linearVelocity = direccion * 15;
            }

            Dis.transform.localScale = new Vector3(MirandoDerecha ? 1 : -1, 1, 1);
        }
    }

    void ProcesarMovimiento()
    {
        float inputMovimiento = Input.GetAxisRaw("Horizontal");
        rigidBody.linearVelocity = new Vector2(inputMovimiento * velocidad, rigidBody.linearVelocity.y);
        GestionarOrientacion(inputMovimiento);

        // 3. ¡AQUÍ SE ACTIVA TU ANIMACIÓN!
        if (animator != null)
        {
            // Si el input es diferente de 0, significa que se está moviendo -> true
            // Si el input es 0, está quieto -> false
            bool seEstaMoviendo = (inputMovimiento != 0);
            animator.SetBool("corriendo", seEstaMoviendo);
        }
    }

    void GestionarOrientacion(float inputMovimiento)
    {
        if ((inputMovimiento > 0 && !MirandoDerecha) || (inputMovimiento < 0 && MirandoDerecha))
        {
            MirandoDerecha = !MirandoDerecha;
            Vector3 escala = transform.localScale;
            escala.x *= -1;
            transform.localScale = escala;
        }
    }

    void ControlarSalto()
    {
        Vector2 posicionCaja = (Vector2)transform.position + new Vector2(0, desplazamientoY);
        Collider2D colisionSuelo = Physics2D.OverlapBox(posicionCaja, dimensionesCaja, 0f, CapaSuelo);

        if (colisionSuelo != null)
        {
            enSuelo = true;
            ejecutandoCoyote = false;
        }
        else if (enSuelo && !ejecutandoCoyote)
        {
            StartCoroutine(coyotetime());
        }

        if (enSuelo && Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, FuerzaSalto);
            enSuelo = false;
        }
    }

    IEnumerator coyotetime()
    {
        ejecutandoCoyote = true;
        yield return new WaitForSeconds(0.2f);
        if (ejecutandoCoyote)
        {
            enSuelo = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector2 posicionCaja = (Vector2)transform.position + new Vector2(0, desplazamientoY);
        Gizmos.DrawWireCube(posicionCaja, dimensionesCaja);
    }
}
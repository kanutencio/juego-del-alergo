using System.Collections;
using UnityEngine;

public class playrScript : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad;
    [SerializeField] private float FuerzaSalto;
    private Rigidbody2D rigidBody;
    bool MirandoDerecha;
    [SerializeField] bool enSuelo;
    [SerializeField] LayerMask CapaSuelo;


    [Header("Sistema de disparos")]
    [SerializeField] private balasPooling BP;
    public float LaserOffSet;
    public float TiempoEntreDisparos = 0.15f;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        disparo();
        ProcesarMovimiento();
        ControlarSalto();
    }


    private void disparo()
    {
        if (TiempoEntreDisparos > 0)
        {
            TiempoEntreDisparos -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.V) && TiempoEntreDisparos < 0){
            GameObject Dis = BP.Aparicion();

            Vector3 direccion = MirandoDerecha ? Vector3.right : Vector3.left;
            Dis.transform.position = transform.position + direccion * LaserOffSet + new Vector3(0, 0.9f, 0);

            Rigidbody2D rbDisparo = Dis.GetComponent<Rigidbody2D>();
            if (rbDisparo != null)
            {
                rbDisparo.linearVelocity = direccion * 15;
            }

            if (!MirandoDerecha)
            {
                Dis.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                Dis.transform.localScale = new Vector3(1, 1, 1);
            }

            TiempoEntreDisparos = 0.15f;
        }
        
    }

    void ProcesarMovimiento()
    {
        float inputMovimiento = Input.GetAxis("Horizontal");
        rigidBody.linearVelocity = new Vector2(inputMovimiento * velocidad, rigidBody.linearVelocity.y);
        GestionarOrientacion(inputMovimiento);
    }
    void GestionarOrientacion(float inputMovimiento)
    {
        if ((MirandoDerecha == true && inputMovimiento < 0) || (MirandoDerecha == false && inputMovimiento > 0))
        {
            MirandoDerecha = !MirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
    void ControlarSalto()
    {
        RaycastHit2D raycastSueloCentro = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, 0.4f, CapaSuelo);
        RaycastHit2D raycastSueloIzquierda = Physics2D.Raycast(new Vector2(transform.position.x + 0.25f, transform.position.y), Vector2.down, 0.4f, CapaSuelo);
        RaycastHit2D raycastSueloDerecha = Physics2D.Raycast(new Vector2(transform.position.x - 0.25f, transform.position.y), Vector2.down, 0.4f, CapaSuelo);



        if (raycastSueloCentro.collider != null || raycastSueloIzquierda.collider != null || raycastSueloDerecha.collider != null)
        {
            enSuelo = true;
        }
        else
        {
            StartCoroutine(coyotetime());
        }


        if (enSuelo && Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, FuerzaSalto);
        }
    }
    IEnumerator coyotetime()
    {
        yield return new WaitForSeconds(0.2f);
        enSuelo = false;
    }
}

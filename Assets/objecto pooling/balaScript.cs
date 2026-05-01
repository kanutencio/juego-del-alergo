using UnityEngine;

public class balaScript : MonoBehaviour
{
    [SerializeField] private float VelocidadBala; // la velocidad a la que ira la bala
    private Rigidbody2D rb;  //componente rigidbody de la bala

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>(); //cuando es activada buscara su comonente rigidbody2d
        rb.linearVelocity = Vector2.right * VelocidadBala; // esto es para que la bala se mueva hacia la derecha con una velocidad asignada 

    }

    //para cuando se colisione con un objeto con el tag (pared)

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("pared"))
        {
            gameObject.SetActive(false);
        }
    }
}

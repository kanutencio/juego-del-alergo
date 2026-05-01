using UnityEngine;

public class scriptCamara : MonoBehaviour
{
    public Transform jugador;

    private float yFija;
    private float zFija;
    public float offsetX;

    void Start()
    {
        yFija = transform.position.y;
        zFija = transform.position.z;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(jugador.position.x+offsetX,yFija,zFija);
    }
}

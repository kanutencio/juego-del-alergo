using System.Collections.Generic;
using UnityEngine;

public class balasPooling : MonoBehaviour
{
    [SerializeField] private GameObject prefBala;
    [SerializeField] private int NBalas;
    [SerializeField] private List<GameObject> ListaBalas;
    void Start()
    {
        for (int i = 0; i < NBalas; i++)
        {
            GameObject NewBala = Instantiate(prefBala);
            NewBala.SetActive(false);
            ListaBalas.Add(NewBala);
            NewBala.transform.parent = transform;
        }
    }

    public GameObject Aparicion()
    {
        for (int i = 0; i < ListaBalas.Count; i++)
        {
            if (!ListaBalas[i].activeSelf)
            {
                ListaBalas[i].SetActive(true);
                return ListaBalas[i];
            }
        }

        GameObject nuevaBala = Instantiate(prefBala);
        nuevaBala.SetActive(true);
        nuevaBala.transform.parent = transform;
        ListaBalas.Add(nuevaBala);

        return nuevaBala;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class ObjectDestroy : MonoBehaviour
{
    public float cubeSize = 0.2f;
    public int cubesInRow = 5;
    float cubesPivotDistance;
    Vector3 cubesPivot;
    public double deadZone = 0.04;

    // Añadimos las variables que faltan
    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;

    // Variable para controlar cuánto tiempo permanecen los fragmentos
    public float destructionDelay = 3f;

    // Variable para evitar múltiples explosiones
    private bool hasExploded = false;

    void Start()
    {
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    void Update()
    {
        if (transform.position.y < deadZone && !hasExploded)
        {
            explode();
        }
    }

    public void explode()
    {
        // Prevenir múltiples explosiones
        hasExploded = true;

        gameObject.SetActive(false);
        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    createPiece(x, y, z);
                }
            }
        }

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }

        // Destruir el objeto original después de la explosión
        Destroy(gameObject, 0.1f);
    }

    void createPiece(int x, int y, int z)
    {
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        // Destruir el fragmento después de un tiempo
        Destroy(piece, destructionDelay);
    }
}
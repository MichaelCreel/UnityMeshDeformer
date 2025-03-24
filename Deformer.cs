using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deformer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Deform(Vector3 force, GameObject deformingObject, ContactPoint[] contactPos, float deformResistance)
    {

    }

    public void DeformVertex(Vector3 force, Collider collider, ContactPoint contact, float deformResistance)
    {
        Debug.Log("Deformer Ran");
        Vector3 vertexPos = collider.ClosestPointOnBounds(contact.point);

        vertexPos += force/deformResistance;

        Vector3 closestPoint = collider.ClosestPoint(contact.point); //TODO Find vertex nearest to contact point.

        Debug.Log(closestPoint);
    }
}

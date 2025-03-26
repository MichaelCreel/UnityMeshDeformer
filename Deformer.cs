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

    public void DeformVertex(Vector3 force, MeshCollider collider, Mesh mesh, ContactPoint contact, float deformResistance, float buffer)
    {
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        vertices = mesh.vertices;

        Vector3 vertexPos = collider.ClosestPoint(contact.point);

        vertexPos += force / deformResistance / -10000;

        Vector3 closestPoint = collider.ClosestPoint(contact.point);

        for (int i = 0; i < vertices.Length; i++)
        {
            if (Mathf.Abs(Vector3.Distance(vertices[i], closestPoint)) < buffer)
            {
                vertices[i] = vertexPos;
                mesh.vertices = vertices;
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                collider.sharedMesh = mesh;
                break;
            }
        }
    }
}

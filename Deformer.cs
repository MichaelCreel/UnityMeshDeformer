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

    public void Deform(MeshCollider collider, Mesh mesh, ContactPoint[] contacts, float deformResistance, float buffer, float radius)
    {
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        vertices = mesh.vertices;

        foreach (ContactPoint contact in contacts)
        {
            Vector3 vertexPos = collider.ClosestPoint(contact.point);

            //vertexPos += force / deformResistance / -10000;
            Vector3 change = contact.impulse / Time.fixedDeltaTime / deformResistance / -10000;
            vertexPos += change;

            Vector3 closestPoint = collider.ClosestPoint(contact.point);

            for (int i = 0; i < vertices.Length; i++)
            {
                float distance = Mathf.Abs(Vector3.Distance(vertices[i], closestPoint));
                if (Mathf.Abs(Vector3.Distance(vertices[i], closestPoint)) < buffer)
                {
                    vertices[i] = vertexPos;
                } else if (distance < radius)
                {
                    vertices[i] += change * Mathf.Pow(radius - distance, 2) / Mathf.Pow(radius, 2);
                }
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        collider.sharedMesh = mesh;
    }

    public void DeformVertex(MeshCollider collider, Mesh mesh, ContactPoint contact, float deformResistance, float buffer, float radius)
    {
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        vertices = mesh.vertices;

        Vector3 vertexPos = collider.ClosestPoint(contact.point);

        Vector3 change = contact.impulse / Time.fixedDeltaTime / deformResistance / -10000;
        vertexPos += change;

        Vector3 closestPoint = collider.ClosestPoint(contact.point);

        for (int i = 0; i < vertices.Length; i++)
        {
            float distance = Mathf.Abs(Vector3.Distance(vertices[i], closestPoint));
            if (distance < buffer)
            {
                vertices[i] = vertexPos;
                mesh.vertices = vertices;
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                collider.sharedMesh = mesh;
                break;
            } else if (distance < radius)
            {
                vertices[i] += change * Mathf.Pow(radius - distance, 2) / Mathf.Pow(radius, 2);
                mesh.vertices = vertices;
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                collider.sharedMesh = mesh;
                break;
            }
        }
    }
}

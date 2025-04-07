using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deformer : MonoBehaviour
{
    public void Deform(MeshCollider collider, Mesh mesh, ContactPoint[] contacts, float deformResistance, float buffer, float radius, int multiplier)
    {
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        vertices = mesh.vertices;

        foreach (ContactPoint contact in contacts)
        {
            Vector3 vertexPos = collider.ClosestPoint(contact.point);

            Vector3 closestPoint = vertexPos * (float)multiplier;

            Vector3 change = contact.impulse / Time.fixedDeltaTime / deformResistance / -10000;
            vertexPos += change;

            for (int i = 0; i < vertices.Length; i++)
            {
                float distance = Vector3.Distance(vertices[i], closestPoint);
                if (distance < buffer)
                {
                    //Debug.Log(distance); //Use for tuning distance in CollisionChecker script on your mesh. Only shows distances less than distance.
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
        collider.sharedMesh = null;
        collider.sharedMesh = mesh;
    }

    public void DeformVertex(MeshCollider collider, Mesh mesh, ContactPoint contact, float deformResistance, float buffer, float radius)
    {
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        vertices = mesh.vertices;

        Vector3 vertexPos = collider.ClosestPoint(contact.point);

        Vector3 closestPoint = vertexPos;

        Vector3 change = contact.impulse / Time.fixedDeltaTime / deformResistance / -10000;
        vertexPos += change;

        for (int i = 0; i < vertices.Length; i++)
        {
            float distance = Vector3.Distance(vertices[i], closestPoint);
            if (distance < buffer)
            {
                //Debug.Log(distance); //Use for tuning distance in CollisionChecker script on your mesh. Only shows distances less than distance.
                vertices[i] = vertexPos;
                mesh.vertices = vertices;
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                collider.sharedMesh = null;
                collider.sharedMesh = mesh;
                break;
            } else if (distance < radius)
            {
                vertices[i] += change * Mathf.Pow(radius - distance, 2) / Mathf.Pow(radius, 2);
                mesh.vertices = vertices;
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                collider.sharedMesh = null;
                collider.sharedMesh = mesh;
                break;
            }
        }
    }
}

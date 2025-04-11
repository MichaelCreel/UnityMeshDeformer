using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Deformer : MonoBehaviour
{
    public void Deform(MeshCollider collider, Collision collision, Mesh mesh, ContactPoint[] contacts, float deformResistance, float buffer, float radius, int multiplier, float bufferCut, float outformRadius)
    {
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        vertices = mesh.vertices;

        foreach (ContactPoint contact in contacts)
        {
            //Vector3 vertexPos = collider.ClosestPoint(contact.point);
            Vector3 closP = collider.ClosestPoint(contact.point);

            Vector3 scale = collider.gameObject.transform.localScale;

            Vector3 vertexPos = new Vector3(closP.x / scale.x, closP.y / scale.y, closP.z / scale.z);

            Vector3 closestPoint = vertexPos;
            Vector3 change = contact.impulse / Time.fixedDeltaTime / deformResistance / -10000 * (float)multiplier;
            //vertexPos += change;
            vertexPos += new Vector3(change.x / scale.x, change.y / scale.y, change.z / scale.z);

            for (int i = 0; i < vertices.Length; i++)
            {
                float distance = Vector3.Distance(vertices[i], closestPoint);
                if (distance < buffer && distance > bufferCut)
                {
                    //Debug.Log(distance); //Use for tuning distance in CollisionChecker script on your mesh. Only shows distances less than distance.
                    vertices[i] = vertexPos;
                } else if (distance < radius)
                {
                    vertices[i] += change * Mathf.Pow(radius - distance, 2) / Mathf.Pow(radius, 2);
                } else if (distance < outformRadius)
                {
                    vertices[i] += change * Mathf.Sin(radius-distance) / 2;
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

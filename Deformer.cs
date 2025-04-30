using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Deformer : MonoBehaviour
{
    //Deform requires the collision from the deforming object
    //Deforming Collider, Collision, Deforming Mesh, Contact Points, Resistance to Deforming, Maximum Distance for Full Deforming, Radius for Functional Deforming, Deforming Multiplier, Minimum Distance for Deforming, Radius for Outward Deforming (Push Out & Up)
    public void Deform(MeshCollider collider, Collision collision, Mesh mesh, ContactPoint[] contacts, float deformResistance, float buffer, float radius, int multiplier, float bufferCut, float outformRadius)
    {
        Vector3 rotation = collider.gameObject.transform.rotation.eulerAngles;

        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        vertices = mesh.vertices;

        Vector3 scale = collider.gameObject.transform.localScale;

        foreach (var vertex in vertices)
        {
            Vector3 positionBase = new Vector3(vertex.x * scale.x, vertex.y * scale.y, vertex.z * scale.z);
            Debug.DrawLine(positionBase + collider.gameObject.transform.position, positionBase + new Vector3(0, 0, 0.2f) + collider.gameObject.transform.position, Color.red, 30f);
        }

        foreach (ContactPoint contact in contacts)
        {
            Vector3 closP = ClosestPoint(contact.point, vertices);

            Vector3 vertexPos = new Vector3(closP.x / scale.x, closP.y / scale.y, closP.z / scale.z);

            Vector3 closestPoint = vertexPos;
            Vector3 change = contact.impulse / Time.fixedDeltaTime / deformResistance / -10000 * (float)multiplier;
            vertexPos += new Vector3(change.x / scale.x, change.y / scale.y, change.z / scale.z);

            for (int i = 0; i < vertices.Length; i++)
            {
                float distance = Vector3.Distance(vertices[i], closestPoint);
                if (distance < buffer && distance > bufferCut)
                {
                    //Debug.Log(distance); //Use for tuning distance in CollisionChecker script on your mesh. Only shows distances less than distance.
                    vertices[i] = vertexPos;
                }
                else if (distance < radius)
                {
                    vertices[i] += change * Mathf.Pow(radius - distance, 2) / Mathf.Pow(radius, 2);
                }
                else if (distance < outformRadius)
                {
                    vertices[i] += change * Mathf.Sin(radius - distance) / 4;
                }
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        collider.sharedMesh = null;
        collider.sharedMesh = mesh;
    }

    //DeformVertex requires the collision from the deforming object
    //Deforming Collider, Collision, Deforming Mesh, Contact Points, Resistance to Deforming, Maximum Distance for Full Deforming, Radius for Functional Deforming, Deforming Multiplier, Minimum Distance for Deforming, Radius for Outward Deforming(Push Out & Up)
    public void DeformVertex(MeshCollider collider, Collision collision, Mesh mesh, ContactPoint contact, float deformResistance, float buffer, float radius, int multiplier, float bufferCut, float outformRadius)
    {
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        vertices = mesh.vertices;

        Vector3 closP = ClosestPoint(contact.point, vertices);

        Vector3 scale = collider.gameObject.transform.localScale;

        Vector3 vertexPos = new Vector3(closP.x / scale.x, closP.y / scale.y, closP.z / scale.z);

        Vector3 closestPoint = vertexPos;
        Vector3 change = contact.impulse / Time.fixedDeltaTime / deformResistance / -10000 * (float)multiplier;
        vertexPos += new Vector3(change.x / scale.x, change.y / scale.y, change.z / scale.z);

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
            }
            else if (distance < radius)
            {
                vertices[i] += change * Mathf.Pow(radius - distance, 2) / Mathf.Pow(radius, 2);
                mesh.vertices = vertices;
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                collider.sharedMesh = null;
                collider.sharedMesh = mesh;
                break;
            }
            else if (distance < outformRadius)
            {
                vertices[i] += change * Mathf.Sin(radius - distance) / 2;
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                collider.sharedMaterial = null;
                collider.sharedMesh = mesh;
                break;
            }
        }
    }

    private Vector3 ClosestPoint(Vector3 contactPoint, Vector3[] vertices)
    {
        Vector3 point = vertices[0];
        float distance = Vector3.Distance(contactPoint, vertices[0]);
        for (int i = 1; i < vertices.Length; i++)
        {
            float temp = Vector3.Distance(contactPoint, vertices[i]);
            if (temp < distance)
            {
                distance = temp;
                point = vertices[i];
            }
        }
        Debug.Log(point);
        return point;
    }
}
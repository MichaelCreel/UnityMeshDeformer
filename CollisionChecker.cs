using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField] private float deformResistance = 100;

    private MeshCollider collider;

    private Mesh mesh;

    private void Start()
    {
        collider = GetComponent<MeshCollider>();

        mesh = GetComponent<MeshFilter>().mesh;
    }

    public void SetDeformResistance(float resistance)
    {
        deformResistance = resistance;
    }

    public void OnCollisionEnter(Collision collision)
    {
        /*Debug.Log(collision.contactCount);
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 force = contact.impulse / Time.fixedDeltaTime;
            if (force.magnitude > 5000)
            {
                FindObjectOfType<Deformer>().DeformVertex(force, collider, mesh, contact, deformResistance, 0.5f);
            }
        }*/

        if ((collision.impulse / Time.fixedDeltaTime).magnitude > 5000)
        {
            Debug.Log(collision.contactCount);
            FindObjectOfType<Deformer>().Deform(collider, mesh, collision.contacts, deformResistance, 0.5f, collision.impulse.magnitude / Time.fixedDeltaTime / 1000000f);
        }
    }
}

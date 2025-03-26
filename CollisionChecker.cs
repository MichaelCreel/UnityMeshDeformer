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

    public void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;

        foreach (ContactPoint contact in contactPoints)
        {
            Vector3 force = contact.impulse / Time.fixedDeltaTime;
            Debug.Log(force.magnitude);
            if (force.magnitude > 5000)
            {
                FindObjectOfType<Deformer>().DeformVertex(force, collider, mesh, contact, deformResistance, 0.5f);
            }
        }
    }
}

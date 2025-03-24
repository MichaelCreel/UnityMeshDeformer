using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField] private float deformResistance;

    private Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;

        foreach (ContactPoint contact in contactPoints)
        {
            Vector3 force = contact.impulse / Time.fixedDeltaTime;
            if (force.magnitude > 5)
            {
                FindObjectOfType<Deformer>().DeformVertex(force, collider, contact, deformResistance);
            }
        }
    }
}

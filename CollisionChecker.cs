using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField] private float deformResistance = 100;

    [SerializeField] private float distance = 0.5f;

    [SerializeField] private int multiplier = 1;

    [SerializeField] private float radiusMultiplier = 1;

    [SerializeField] private float distanceCut = 0f;

    private Mesh mesh;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    public void SetDeformResistance(float resistance)
    {
        deformResistance = resistance;
    }

    public void SetDeformDistance(float length)
    {
        distance = length;
    }

    public void OnCollisionEnter(Collision collision)
    {

        if ((collision.impulse / Time.fixedDeltaTime).magnitude > 5000)
        {
            FindObjectOfType<Deformer>().Deform(GetComponent<MeshCollider>(), collision, mesh, collision.contacts, deformResistance, distance, collision.impulse.magnitude * radiusMultiplier / Time.fixedDeltaTime / 10000f / deformResistance, multiplier, distanceCut);
        }
    }
}

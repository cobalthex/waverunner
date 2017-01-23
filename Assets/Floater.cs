using UnityEngine;
using System.Collections;

public class Floater : MonoBehaviour
{
    /// <summary>
    /// Where the equilibrium point is on the object
    /// </summary>
    public float EquilibriumHeight;

    public Vector3 buoyancyCentreOffset;
    public float bounceDamp;

    float waterLevel = 0;

    Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 actionPoint = body.worldCenterOfMass + buoyancyCentreOffset;
        float forceFactor = 1f - ((actionPoint.y - waterLevel) / EquilibriumHeight);

        if (forceFactor > 0f)
        {
            Vector3 uplift = -Physics.gravity * (forceFactor - body.velocity.y * bounceDamp) * body.mass;
            body.AddForceAtPosition(uplift, actionPoint);
        }
    }
}

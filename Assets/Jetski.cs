using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetski : MonoBehaviour
{
    public Material Skybox;

    Rigidbody body;

    Camera followCam;
    ParticleSystem exhaust;

    public float ThrottleForce = 40;
    public float LateralDrag = 0.4f;

    Transform ThrottlePoint;

    bool isCollidingWithWater = false;

    bool defaultFog = RenderSettings.fog;
    Color defaultFogColor = RenderSettings.fogColor;
    float defaultFogDensity = RenderSettings.fogDensity;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        ThrottlePoint = transform.FindChild("ThrottlePoint");
        followCam = transform.FindChild("Camera").GetComponent<Camera>();
        exhaust = transform.FindChild("Exhaust").GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isCollidingWithWater = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isCollidingWithWater = false;
        }
    }

    void FixedUpdate()
    {
        var speed = Vector3.Magnitude(body.velocity);

        var throttleInput = Input.GetAxis("Throttle");
        if (throttleInput > 0 && isCollidingWithWater)
        {
            body.AddForceAtPosition(
                transform.forward * throttleInput * ThrottleForce,
                ThrottlePoint.position,
                ForceMode.Acceleration
            );
        }
        else if (throttleInput != 0 && (Physics.Raycast(body.centerOfMass, -transform.up, 5, 8) || isCollidingWithWater))
        {
            body.AddForceAtPosition(
                transform.forward * throttleInput * (ThrottleForce / 5),
                ThrottlePoint.position,
                ForceMode.Acceleration
            );
        }

        var isForward = Mathf.Sign(Vector3.Dot(body.velocity, transform.forward));
        //emit exhaust if moving forward
        if (isForward > 0)
            exhaust.Emit((int)(speed / 10));

        //lateral tilt of craft
        var tiltInput = Input.GetAxis("Tilt");
        if (tiltInput != 0)
            body.AddRelativeTorque(0, tiltInput * (speed * isForward) / 6, -tiltInput * 5, ForceMode.Acceleration);

        //lateral drag(this could be handled via collision detection with waves)
        var lateralDot = Vector3.Dot(transform.right, body.velocity);
        body.AddForce(-(transform.right * lateralDot * LateralDrag), ForceMode.Impulse);

        //keep upright
        Quaternion q = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        float dot = Vector3.Dot(transform.up, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * (4 + 15f * (1 - dot)));

        //underwater settings
        if (followCam.transform.position.y <= 0)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0, 0.4f, 0.7f, 0.6f);
            RenderSettings.fogDensity = 0.04f;
            RenderSettings.skybox = null;
        }
	    else
        {
		    RenderSettings.fog = defaultFog;
		    RenderSettings.fogColor = defaultFogColor;
		    RenderSettings.fogDensity = defaultFogDensity;
		    RenderSettings.skybox = Skybox;
	    }
    }
}

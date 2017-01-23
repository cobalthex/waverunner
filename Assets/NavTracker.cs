using UnityEngine;

public class NavTracker : MonoBehaviour
{
    int currentNavIndex = 0;
    public Transform CurrentNav { get; private set; }

    public Transform Tracking;
    public float TrackingRadius = 6;

    Transform trackerArrow;

    void Start()
    {
        CurrentNav = transform.GetChild(currentNavIndex);
        CurrentNav.localScale *= 5;
        trackerArrow = GameObject.Find("TrackerArrow").transform;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(Tracking.position, CurrentNav.position) < 10)
        {
            CurrentNav.localScale /= 5;
            currentNavIndex = (currentNavIndex + 1) % transform.childCount;
            CurrentNav = transform.GetChild(currentNavIndex);
            CurrentNav.localScale *= 5;
        }

        trackerArrow.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight * 0.85f, 8));
        var fwd = CurrentNav.position - Tracking.position;
        trackerArrow.rotation = Quaternion.Lerp(trackerArrow.rotation, Quaternion.LookRotation(fwd, Tracking.up) * Quaternion.Euler(90, 0, 0), 0.5f);
    }
}

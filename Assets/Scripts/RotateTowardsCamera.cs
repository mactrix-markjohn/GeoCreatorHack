using UnityEngine;

// To use this script, attach it to the GameObject that you would like to rotate towards another game object.
// After attaching it, go to the inspector and drag the GameObject you would like to rotate towards into the target field.
// Move the target around in the scene view to see the GameObject continuously rotate towards it.
public class RotateTowardsCamera : MonoBehaviour
{
    // The target marker.
    private Transform target;
    public float offSet = 180f;
    public bool inverted = false;
    private void Start()
    {
        if (Camera.main.transform)
        {
            target = Camera.main.transform;
        }
        
        if (!target)
        {
            target = FindObjectOfType<Camera>().transform;
        }
    }
    void Update()
    {
        Vector3 relativePos = target.position - transform.position;
        Quaternion LookAtRotation = Quaternion.LookRotation(relativePos);
        float resultAngleY = LookAtRotation.eulerAngles.y + offSet;
        if (inverted)
        {
            resultAngleY = resultAngleY * -1;
        }
        Quaternion LookAtRotationOnly_Y = Quaternion.Euler(transform.rotation.eulerAngles.x, resultAngleY, transform.rotation.eulerAngles.z);
        transform.rotation = LookAtRotationOnly_Y;
    }
}
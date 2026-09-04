using UnityEngine;

public class MirrorNode : MonoBehaviour
{
    // Replaced ReceiveLight and LateUpdate logic entirely
    public void Reflect(Vector3 hitPoint, Vector3 incomingDir, Vector3 surfaceNormal, int bounceCount)
    {
        Vector3 reflectDir = Vector3.Reflect(incomingDir, surfaceNormal);

        // Ask the pool for a beam instead of using a dedicated one
        LightBeam bounceBeam = BeamManager.Instance.GetBeam();

        // Pass bounceCount + 1 to track recursion depth
        bounceBeam.Shoot(hitPoint, reflectDir, bounceCount + 1);
    }
}
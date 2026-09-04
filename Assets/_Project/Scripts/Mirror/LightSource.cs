using UnityEngine;

public class LightSource : MonoBehaviour
{
    private void Update()
    {
        // Continuously shoot light forward starting at bounce 0
        LightBeam initialBeam = BeamManager.Instance.GetBeam();
        initialBeam.Shoot(transform.position, transform.forward, 0);
    }
}
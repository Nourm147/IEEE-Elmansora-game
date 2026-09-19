using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    [SerializeField] private List<TriggerEvent> ActiveSpikes;

    private void Awake()
    {
        ActiveSpikes = GetComponentsInChildren<TriggerEvent>().ToList();
    }

    public void EnableSpikes()
    {
        foreach (var spike in ActiveSpikes)
        {
            spike.transform.GetComponent<Collider>().enabled = true;
        }
    }
    public void DesableSpikes()
    {
        foreach (var spike in ActiveSpikes)
        {
            spike.transform.GetComponent<Collider>().enabled = false;
        }
    }
}

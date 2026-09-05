using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class BeamManager : MonoBehaviour
{
    public static BeamManager Instance { get; private set; }

    [Header("Pool Settings")]
    [Tooltip("Assign your LightBeam prefab here")]
    public LightBeam beamPrefab;
    [Tooltip("Maximum allowed bounces to prevent infinite mirror loops")]
    public int maxBounces = 20;

    private List<LightBeam> _beamPool = new List<LightBeam>();
    private int _activeBeamsThisFrame = 0;

    public UnityEvent onFinishEvent;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    private void LateUpdate()
    {
        for (int i = _activeBeamsThisFrame; i < _beamPool.Count; i++)
        {
            _beamPool[i].TurnOff();
        }

        _activeBeamsThisFrame = 0;
    }

    public LightBeam GetBeam()
    {
        if (_activeBeamsThisFrame >= _beamPool.Count)
        {
            LightBeam newBeam = Instantiate(beamPrefab, transform);
            _beamPool.Add(newBeam);
        }

        LightBeam beam = _beamPool[_activeBeamsThisFrame];
        _activeBeamsThisFrame++;
        return beam;
    }

    public void FinishPuzzle()
    {
        onFinishEvent.Invoke();
    }
}
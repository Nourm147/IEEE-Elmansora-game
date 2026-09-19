using Unity.Cinemachine;
using UnityEngine;

public class InteractionCameraController : MonoBehaviour
{
    [Header("Camera References")]
    [Tooltip("The camera to enable during interaction.")]
    public CinemachineCamera interactionCamera;

    [Tooltip("The main player camera to disable during interaction. Defaults to Camera.main if null.")]
    public GameObject player;

    private BaseInteractable _interactable;

    private void Awake()
    {
        _interactable = GetComponent<BaseInteractable>();

        if (interactionCamera != null)
        {
            interactionCamera.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (_interactable != null)
        {
            _interactable.onSelectEnter.AddListener(OnSelectEnter);
            _interactable.onSelectExit.AddListener(OnSelectExit);
        }
    }

    private void OnDisable()
    {
        if (_interactable != null)
        {
            _interactable.onSelectEnter.RemoveListener(OnSelectEnter);
            _interactable.onSelectExit.RemoveListener(OnSelectExit);
        }
    }

    private void OnSelectEnter()
    {
        if (player != null)
        {
            player.SetActive(false);
        }

        if (interactionCamera != null)
        {
            interactionCamera.gameObject.SetActive(true);
        }
    }

    private void OnSelectExit()
    {
        if (interactionCamera != null)
        {
            interactionCamera.gameObject.SetActive(false);
        }

        if (player != null)
        {
            player.SetActive(true);
        }
    }
}
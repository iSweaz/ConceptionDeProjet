using PurrNet;
using UnityEngine;

public class NI_SpawnSettings : NetworkIdentity
{
    [SerializeField] private Camera cam;
    [SerializeField] private AudioListener audioListener;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        if (isOwner)
        {
            cam = GetComponentInChildren<Camera>();
            audioListener = GetComponentInChildren<AudioListener>();
            Debug.Log($"ActiveSelf: {gameObject.activeSelf}, ActiveInHierarchy: {gameObject.activeInHierarchy}");

            cam.enabled = true;
            audioListener.enabled = true;

            Debug.Log("[NI_SpawnSettings] Caméra activée pour le client propriétaire");
        }
    }
}

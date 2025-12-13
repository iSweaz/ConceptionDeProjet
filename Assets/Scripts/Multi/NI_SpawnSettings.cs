using PurrNet;
using UnityEngine;

public class NI_SpawnSettings : NetworkIdentity
{
    [SerializeField] private Camera cam;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private NI_Selection selection;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        if (isOwner)
        {
            cam = GetComponentInChildren<Camera>();
            audioListener = GetComponentInChildren<AudioListener>();
            selection = GetComponentInChildren<NI_Selection>();

            cam.enabled = true;
            audioListener.enabled = true;
            selection.enabled = true;
        }
    }
}

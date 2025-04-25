using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FirePebbleOnActivate : MonoBehaviour
{
    public GameObject pebble;         // You can rename 'bullet' to 'pebble' if you want
    public Transform spawnPoint;
    public float fireSpeed = 20;

    void Start()
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabbable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabbable.activated.AddListener(PebbleBullet);
    }

    public void PebbleBullet(ActivateEventArgs arg)
    {
        GameObject spawnedPebble = Instantiate(pebble);
        spawnedPebble.transform.position = spawnPoint.position;
        spawnedPebble.GetComponent<Rigidbody>().linearVelocity = spawnPoint.forward * fireSpeed;
        Destroy(spawnedPebble, 5);
    }
}

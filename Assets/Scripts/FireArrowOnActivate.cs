 using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireArrowOnActivate : MonoBehaviour
{
    public GameObject arrow;
    public Transform spawnPoint;
    public float fireSpeed = 20;

    void Start()
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabbable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabbable.activated.AddListener(FireArrow);
    }

    public void FireArrow(ActivateEventArgs arg)
    {
        GameObject spawnedArrow = Instantiate(arrow);
        spawnedArrow.transform.position = spawnPoint.position;
        spawnedArrow.GetComponent<Rigidbody>().linearVelocity = spawnPoint.forward*fireSpeed;
        Destroy(spawnedArrow, 5);
    }

}
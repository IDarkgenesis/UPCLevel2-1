using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;
    [SerializeField] CharacterController player;
    private void OnTriggerEnter(Collider other)
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        player.transform.position = respawnPoint.position;
        Debug.Log("Enter");

        if (controller != null)
        {
            controller.enabled = true;
        }
    }
}

using ARPG.Dialogue;
using Assets.Scripts;
using UnityEngine;

namespace ARPG.Player
{
    public class PlayerIntersact : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (!GameManager.Instance.blackBoard.GetBool(GameManager.Instance.isInteractString) && Input.GetKeyDown(KeyCode.F))
            {
                if (other.CompareTag("Interactable"))
                {
                    Debug.Log("Interact");
                    other.GetComponent<NPC>().OnTriggerByPlayer();
                }
            }
        }
    }
}
using UnityEngine;

public class BuildingInteraction : MonoBehaviour
{
    public GameObject buildingUI; // ����Canvas�µ�Panel

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buildingUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buildingUI.SetActive(false);
        }
    }
}

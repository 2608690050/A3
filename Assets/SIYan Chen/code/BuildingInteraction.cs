using UnityEngine;

public class BuildingInteraction : MonoBehaviour
{
    public GameObject buildingUI; // ÍÏÈëCanvasÏÂµÄPanel

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

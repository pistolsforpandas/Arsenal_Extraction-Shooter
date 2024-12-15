using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {

       if (playerController.isDead == false)
       {
         if (Input.GetKeyDown(KeyCode.Tab))
         {
            if (playerController.canMove == true)
            {
                playerController.canMove = false;
            }
            else
            {
                playerController.canMove = true;
            }

            InventoryPanel.SetActive(!InventoryPanel.activeSelf);
         }
       }
    }
}

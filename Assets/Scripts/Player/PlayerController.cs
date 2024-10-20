using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : NetworkBehaviour
{
    public RectTransform healthBar;
    
    public Camera mainCamera;  // Reference to the camera
    public Transform tankBody; // Reference to the tank body for rotation

    public Vector3 directionToMouse;
    
    private static List<GameObject> tanks = new List<GameObject>();

    public Text txtScore;

    [SyncVar(hook = nameof(OnScoreChanged))]
    public int count;
    private void Awake()
    {
        if(mainCamera == null)
            mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        count = 0;
        txtScore.text = count.ToString();
    }
    

    void Update()
    {
        RotateTowardsMouse();
    }


    private void OnScoreChanged(int oldValue, int newValue)
    {
        txtScore.text = "";
        txtScore.text = count.ToString();
    }
    void RotateTowardsMouse()
    {
        if(!isLocalPlayer) return;
        // Get the mouse position in screen space
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert the mouse position to world space
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, mainCamera.transform.position.y - tankBody.position.y));

        // Calculate the direction to the mouse
        directionToMouse = mouseWorldPosition - tankBody.position;
        directionToMouse.y = 0; // Lock the rotation to the XZ plane if it's a 3D tank

        // Rotate the tank to face the mouse direction
        if (directionToMouse != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToMouse);
            tankBody.rotation = Quaternion.Slerp(tankBody.rotation, targetRotation, Time.deltaTime * 10f);  // Slerp for smooth rotation
        }
    }

    public override void OnStartClient()
    {
        tanks.Add(gameObject);
        if (tanks.Count > 1)
        {
            healthBar.anchorMin = new Vector2(1, 1);
            healthBar.anchorMax = new Vector2(1, 1);
            healthBar.anchoredPosition3D = new Vector3(-200, -25, 0);
            healthBar.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    public override void OnStopClient()
    {
        tanks.Remove(gameObject);
        
    }

    public static List<GameObject> GetTanks()
    {
        return tanks;
    }
}

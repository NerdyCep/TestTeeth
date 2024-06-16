using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float cameraSpeed = 5.0f; 
    public float rotationSpeed = 2.0f; 

    private bool isRightMouseButtonHeld = false;

    void Start()
    {
        
    }

    void Update()
    {

       if (Input.GetMouseButtonDown(1))
        {
            isRightMouseButtonHeld = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRightMouseButtonHeld = false;
        }

         if (isRightMouseButtonHeld)
        {
           if (isRightMouseButtonHeld)
        {
            float moveHorizontal = Input.GetAxis("Horizontal"); // Клавиши A и D или стрелки влево и вправо
            float moveVertical = Input.GetAxis("Vertical"); // Клавиши W и S или стрелки вверх и вниз

            Vector3 direction = transform.forward * moveVertical + transform.right * moveHorizontal;
            transform.position += direction * cameraSpeed * Time.deltaTime;

            // Вращение камеры
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 rotation = new Vector3(-mouseY, mouseX, 0.0f) * rotationSpeed;
            transform.eulerAngles += rotation;
        }
        }



    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class ObjectRotate: MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;
    public bool isDragging = false;
    public bool canRotate = true;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Полная остановка вращения при нажатии пробела
        if (Input.GetKeyDown(KeyCode.Space))
        {
            canRotate = false;
            rb.angularVelocity = Vector3.zero; // Останавливаем вращение
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            canRotate = true;
        }
    }

    void FixedUpdate()
    {
        if (canRotate && isDragging)
        {
            float x = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
            float y = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;

            rb.AddTorque(Vector3.down * x);
            rb.AddTorque(Vector3.right * y);
        }
    }
}
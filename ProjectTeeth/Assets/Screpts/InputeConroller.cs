using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputeConroller : MonoBehaviour
{
    private Camera mainCamera;
    private List<GameObject> selectedObjects;
    private List<Vector3> offsets;
    private List<Vector3> initialPositions; // Для хранения исходных позиций
    private List<Material[]> originalMaterials;
    private bool isDragging = false;
    private Plane dragPlane;

    public Material outlineMaterial;

    void Start()
    {
        mainCamera = Camera.main;
        selectedObjects = new List<GameObject>();
        offsets = new List<Vector3>();
        originalMaterials = new List<Material[]>();
        initialPositions = new List<Vector3>(); // Инициализация списка исходных позиций
    }

    void Update()
    {
       // Проверка нажатия левой кнопки мыши и удержания клавиши Ctrl для выбора объектов
        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        GameObject hitObject = hit.collider.gameObject;
                        if (selectedObjects.Contains(hitObject))
                        {
                            // Объект уже выбран, снимаем выделение
                            RemoveSelectedObject(hitObject);
                        }
                        else if (selectedObjects.Count < 3)
                        {
                            // Новый объект для выбора
                            AddSelectedObject(hitObject);
                        }
                    }
                }
            }
        }

        // Начало перетаскивания объектов
        if (Input.GetMouseButtonDown(1) && selectedObjects.Count > 0)
        {
            isDragging = true;
            SetDraggingState(true);
        }

        // Перемещение объектов при удержании правой кнопки мыши
        if (isDragging && Input.GetMouseButton(1) && selectedObjects.Count > 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            float dist;
            Plane dragPlane = new Plane(Vector3.up, Vector3.zero); // Например, плоскость Y=0
            if (dragPlane.Raycast(ray, out dist))
            {
                Vector3 point = ray.GetPoint(dist);
                for (int i = 0; i < selectedObjects.Count; i++)
                {
                    selectedObjects[i].transform.position = point + offsets[i];
                }
            }
        }

        // Отпускание объектов при отпускании правой кнопки мыши
        if (Input.GetMouseButtonUp(1))
        {
            if (isDragging)
            {
                isDragging = false;
                SetDraggingState(false);
                
                // Снимаем выделение после перетаскивания
                ClearSelectedObjects();
            }
        }
    }

    private void AddSelectedObject(GameObject obj)
    {
        selectedObjects.Add(obj);

        // Устанавливаем смещение от курсора до объекта
        Ray camToObjectRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        float dist;
        Plane dragPlane = new Plane(Vector3.up, obj.transform.position); // Например, плоскость Y=0
        dragPlane.Raycast(camToObjectRay, out dist);
        offsets.Add(obj.transform.position - camToObjectRay.GetPoint(dist));

        // Сохраняем оригинальные материалы и применяем обводку
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterials.Add(renderer.materials);
            Material[] newMaterials = new Material[renderer.materials.Length + 1];
            System.Array.Copy(renderer.materials, newMaterials, renderer.materials.Length);
            newMaterials[renderer.materials.Length] = outlineMaterial;
            renderer.materials = newMaterials;
        }

        // Отключаем вращение для выбранного объекта
        ObjectRotate objectRotate = obj.GetComponent<ObjectRotate>();
        if (objectRotate != null)
        {
            objectRotate.canRotate = false;
        }
    }

    private void RemoveSelectedObject(GameObject obj)
    {
        int index = selectedObjects.IndexOf(obj);
        if (index >= 0)
        {
            selectedObjects.RemoveAt(index);

            // Восстанавливаем оригинальные материалы
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null && index < originalMaterials.Count)
            {
                renderer.materials = originalMaterials[index];
            }

            // Включаем вращение для объекта
            ObjectRotate objectRotate = obj.GetComponent<ObjectRotate>();
            if (objectRotate != null)
            {
                objectRotate.canRotate = true;
            }

            // Удаляем из списков смещений и материалов
            offsets.RemoveAt(index);
            originalMaterials.RemoveAt(index);
        }
    }

    private void SetDraggingState(bool state)
    {
        foreach (GameObject obj in selectedObjects)
        {
            ObjectRotate objectRotate = obj.GetComponent<ObjectRotate>();
            if (objectRotate != null)
            {
                objectRotate.isDragging = state;
            }
        }
    }

  
    public void ClearSelectedObjects()
    {
        // Очистка списков выбранных объектов и их исходных позиций
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            Renderer renderer = selectedObjects[i].GetComponent<Renderer>();
            if (renderer != null && i < originalMaterials.Count)
            {
                renderer.materials = originalMaterials[i];
            }

            ObjectRotate objectRotate = selectedObjects[i].GetComponent<ObjectRotate>();
            if (objectRotate != null)
            {
                objectRotate.canRotate = true;
            }
        }

        selectedObjects.Clear();
        offsets.Clear();
        originalMaterials.Clear();
    }
}

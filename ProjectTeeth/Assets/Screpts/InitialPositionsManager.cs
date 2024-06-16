using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InitialPositionsManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectInitialPosition
    {
        public Transform transform;
        public Vector3 initialPosition;
    }

    public List<ObjectInitialPosition> objectsInitialPositions = new List<ObjectInitialPosition>();

    void Start()
    {
        // Заполнение начальных положений объектов в списке
        foreach (var obj in objectsInitialPositions)
        {
            obj.initialPosition = obj.transform.position;
        }
    }

    public void ResetObjectPositions()
    {
        // Возврат объектов в их начальные положения
        foreach (var obj in objectsInitialPositions)
        {
            obj.transform.position = obj.initialPosition;
        }
    }
}
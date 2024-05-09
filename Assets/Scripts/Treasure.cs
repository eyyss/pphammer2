using UnityEngine;

public class Treasure : MonoBehaviour, ICollectable
{
    public void Collect()
    {
        // toplanacak hazine sayısını bir azalt
        Destroy(gameObject);
    }
}


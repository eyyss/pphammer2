using UnityEngine;

public class Treasure : MonoBehaviour, ICollectable
{
    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void Collect()
    {
        // toplanacak hazine sayısını bir azalt
        gameManager.AddTreasure();
        Destroy(gameObject);
    }
}


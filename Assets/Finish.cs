using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] public GameObject victoryUI;

    private void Start()
    {
        GameManager.ProblemsCleared += OnProblemsCleared;
    }

    void OnProblemsCleared()
    {
        gameObject.SetActive(true);
    }
}

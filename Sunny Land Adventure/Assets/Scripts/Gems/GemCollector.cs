using UnityEngine;

public class GemCollector : MonoBehaviour
{
    public static GemCollector instance;
    [SerializeField] private int gems;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGem()
    {
        gems++;
    }
}

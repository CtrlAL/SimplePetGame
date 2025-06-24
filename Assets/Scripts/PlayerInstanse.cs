using UnityEngine;

public class PlayerInstanse : MonoBehaviour
{
    private static GameObject _instance;
    public static GameObject Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindWithTag("Player");
            }

            return _instance;
        }
    }
}

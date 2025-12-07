using UnityEngine;

public class References
{
    public static T GetRef<T>(GameObject host, GameObject obj, T instance)
    {
        if(obj == null)
        {
            Debug.LogWarning("Assign references to " + host.name);
            return instance;
        }
        return obj.GetComponent<T>();
    }
}

using UnityEngine;

public class PathBuilder : MonoBehaviour
{

    [HideInInspector]
    public Path path;

    public void CreatePath()
    {
        path = new Path(transform.position);
    }

}

// ShowGoldenPath
using UnityEngine;
using UnityEngine.AI;

public class PathCreator : MonoBehaviour
{
    [SerializeField]
    private const float UpdateTime = 0.01f;
    public Transform target;
    public NavMeshPath path;
    private float elapsed = 0.0f;
    [SerializeField]
    private bool ShouldDebug = true;

    public bool ShouldCreatePath;

    void Start()
    {
        path = new NavMeshPath();
        elapsed = 0.0f;
    }

    void Update()
    {
        // Update the way to the goal every second.
        if(ShouldCreatePath)
        {
            CreatePath();
        }

    }

    private void CreatePath()
    {
        elapsed += Time.deltaTime;
        if (elapsed > UpdateTime)
        {
            elapsed -= UpdateTime;
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        }
        if (ShouldDebug)
        {
            DrawPath();
        }
    }

    private void DrawPath()
    {
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
    }
}
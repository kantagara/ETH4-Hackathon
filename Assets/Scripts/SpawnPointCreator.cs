using UnityEngine;
using UnityEngine.AI;

public class SpawnPointCreator : MonoBehaviour
{
    [SerializeField] private SpawnPoint spawnPoint;
    [SerializeField] private float distance;
    [SerializeField] private int count;

    [ContextMenu("Create")]
    public void CreateCircle()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    
        float radius = distance / 2f;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angleInDegrees = angleStep * i;
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        
            // Calculate position on the circle
            float x = Mathf.Cos(angleInRadians) * radius;
            float z = Mathf.Sin(angleInRadians) * radius;

        
            // Instantiate the prefab at the calculated position
            var obj = Instantiate(spawnPoint, new Vector3(x, 0f, z) + transform.position, Quaternion.identity, transform);
            if(NavMesh.SamplePosition(obj.transform.position, out var hit, 300, NavMesh.AllAreas))
                obj.transform.position = hit.position;
          
        }
    
    }
}

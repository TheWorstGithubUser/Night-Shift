using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;


public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject Player;
    public float fov;
    private Mesh mesh;
    Vector3 origin;
    private float startingAngle;
    private PolygonCollider2D polygonCollider;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Player.transform.position;
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void LateUpdate()
    {
        //set values
        fov = 40f;
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        float viewDistance = 7f;


        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector2[] colliderPoints = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;
        colliderPoints[0] = new Vector2(origin.x, origin.y);

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++)
        {
            //array for mesh
            Vector3 vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            //array for collider
            Vector2 colliderVertex =  new Vector2(vertex.x, vertex.y);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);

            if (raycastHit2D.collider == null)
            {
                //no hit
            }
            else
            {
                //hit
                vertex = raycastHit2D.point;
                colliderVertex = new Vector2(vertex.x, vertex.y);
            }

            vertices[vertexIndex] = vertex;
            colliderPoints[vertexIndex] = colliderVertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
        polygonCollider.SetPath(0, colliderPoints);


    }

    void FixedUpdate() 
    {
        origin = Player.transform.position;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    //will be referenced in player class
    public void SetAimDirection(float angleOfRotation)
    {
        startingAngle = angleOfRotation + (fov / 2f);
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{    
    [SerializeField]
    private Transform player;
    [SerializeField]
    private string nameOfFogLayer;
    [SerializeField]
    private Material fogMaterial;
    [SerializeField]
    private float radius;
    private int layerIndexofFogOfWar;
    private LayerMask layerMaskForRayCast;
    
    private GameObject fogOfWarPlane;

    private float radiusSquared { get { return radius * radius; } }
    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;

    
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        layerIndexofFogOfWar = LayerMask.NameToLayer(nameOfFogLayer);
        layerMaskForRayCast = LayerMask.GetMask( new string[] { nameOfFogLayer} );
        fogOfWarPlane = LargePlane(fogMaterial, layerIndexofFogOfWar);        
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // Ray r = new Ray(transform.position, player.position - transform.position);
        Ray r = new Ray(transform.position, player.position - transform.position);
        //RaycastHit hit;
        //if (Physics.Raycast(r, out hit, 1000, fogLayer, QueryTriggerInteraction.Collide))
        if (Physics.Raycast(r, out hit, 1000f, layerMaskForRayCast, QueryTriggerInteraction.Collide))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);
            
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = fogOfWarPlane.transform.TransformPoint(vertices[i]);
                float distance = Vector3.SqrMagnitude(v - hit.point);
                if (distance < radiusSquared)
                {
                    float alpha = Mathf.Min(colors[i].a, distance / radiusSquared);
                    colors[i].a = alpha;
                }
            }
            UpdateColor();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(hit.point, 1f);
    }

    void Initialize()
    {
        mesh = fogOfWarPlane.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];
        for (int i=0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        UpdateColor();
    }

    void UpdateColor()
    {
        mesh.colors = colors;
    }

    public GameObject LargePlane(Material material, int layer)
    {
        return MakePlaneThisSize(100f, 100f, material, layer);
    }

    public GameObject MakePlaneThisSize(float width, float height, Material material, int layer)
    {
        GameObject gameObject = new GameObject("FogOfWarPlane");
        gameObject.layer = layer;
        MeshFilter meshFilter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        MeshCollider meshCollider = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;

        Mesh modelMesh = GetMeshWithVectorsAndTriangles(width, height, 100, 100);
        meshFilter.mesh = modelMesh;
        meshRenderer.material = material;
        modelMesh.RecalculateBounds();
        modelMesh.RecalculateNormals();
       // meshCollider.convex = true;
        meshCollider.sharedMesh = modelMesh;
        gameObject.transform.Rotate(90f, 0, 0);
        gameObject.transform.position = new Vector3(50, 10, 50);
        return gameObject;
    }

    //From https://github.com/doukasd/Unity-Components/blob/master/ProceduralPlane/Assets/Scripts/Procedural/ProceduralPlane.cs
    public Mesh GetMeshWithVectorsAndTriangles(float width, float height, int xSegments = 100, int ySegments = 100)
    {
        Vector2 topLeftOffset = Vector2.zero;
        Vector2 topRightOffset = Vector2.zero;
        Vector2 bottomLeftOffset = Vector2.zero;
        Vector2 bottomRightOffset = Vector2.zero;

        //create the mesh
        Mesh modelMesh = new Mesh();
        modelMesh.name = "ProceduralPlaneMesh";

        //calculate how many vertices we need
        int numVertexColumns = xSegments + 1;
        int numVertexRows = ySegments + 1;

        //calculate sizes
        int numVertices = numVertexColumns * numVertexRows;
        int numUVs = numVertices;                   //always
        int numTris = xSegments * ySegments * 2;    //fact
        int trisArrayLength = numTris * 3;          //3 places in the array for each tri

        // log the number of tris
        //Debug.Log ("Plane has " + trisArrayLength/3 + " tris");

        //initialize arrays
        Vector3[] Vertices = new Vector3[numVertices];
        Vector2[] UVs = new Vector2[numUVs];
        int[] Tris = new int[trisArrayLength];

        //precalculate increments
        float xStep = width / xSegments;
        float yStep = height / ySegments;
        float uvStepH = 1.0f / xSegments;   // place UVs evenly
        float uvStepV = 1.0f / ySegments;
        float xOffset = -width / 2f;        // this offset means we want the pivot at the center
        float yOffset = -width / 2f;        // same as above

        for (int j = 0; j < numVertexRows; j++)
        {
            for (int i = 0; i < numVertexColumns; i++)
            {
                // calculate some weights for the "keystone" vertex pull
                // for some reason this doesn't work
                // TODO: fix this to cache values and make it faster
                //float bottomLeftWeight = ((numVertexColumns-1)-i)/(numVertexColumns-1) * ((numVertexRows-1)-j)/(numVertexRows-1);

                // position current vertex
                // these offsets are too ridiculous to even try to explain
                // ok trying: basically each vertex we drag is affected by the offsets on the 4 courners but
                // the weight of that effect is linearly inverse analogous to the distance from that corner
                Vertices[j * numVertexColumns + i] = new Vector3(i * xStep + xOffset
                        + bottomLeftOffset.x * ((numVertexColumns - 1) - i) / (numVertexColumns - 1) * ((numVertexRows - 1) - j) / (numVertexRows - 1)
                        + bottomRightOffset.x * i / (numVertexColumns - 1) * ((numVertexRows - 1) - j) / (numVertexRows - 1)
                        + topLeftOffset.x * ((numVertexColumns - 1) - i) / (numVertexColumns - 1) * j / (numVertexRows - 1)
                        + topRightOffset.x * i / (numVertexColumns - 1) * j / (numVertexRows - 1),
                    j * yStep + yOffset
                        + bottomLeftOffset.y * ((numVertexColumns - 1) - i) / (numVertexColumns - 1) * ((numVertexRows - 1) - j) / (numVertexRows - 1)
                        + bottomRightOffset.y * i / (numVertexColumns - 1) * ((numVertexRows - 1) - j) / (numVertexRows - 1)
                        + topLeftOffset.y * ((numVertexColumns - 1) - i) / (numVertexColumns - 1) * j / (numVertexRows - 1)
                        + topRightOffset.y * i / (numVertexColumns - 1) * j / (numVertexRows - 1),
                    0f);

                //calculate UVs
                UVs[j * numVertexColumns + i] = new Vector2(i * uvStepH, j * uvStepV);

                //create the tris				
                if (j == 0 || i >= numVertexColumns - 1)
                {
                    continue;
                }
                else
                {
                    // For every vertex we draw 2 tris in this for-loop, therefore we need 2*3=6 indices in the Tris array
                    int baseIndex = (j - 1) * xSegments * 6 + i * 6;

                    //1st tri - below and in front
                    Tris[baseIndex + 0] = j * numVertexColumns + i;
                    Tris[baseIndex + 1] = j * numVertexColumns + i + 1;
                    Tris[baseIndex + 2] = (j - 1) * numVertexColumns + i;

                    //2nd tri - the one it doesn't touch
                    Tris[baseIndex + 3] = (j - 1) * numVertexColumns + i;
                    Tris[baseIndex + 4] = j * numVertexColumns + i + 1;
                    Tris[baseIndex + 5] = (j - 1) * numVertexColumns + i + 1;
                }
            }
        }

        // assign vertices, uvs and tris
        modelMesh.Clear();
        modelMesh.vertices = Vertices;
        modelMesh.uv = UVs;
        modelMesh.triangles = Tris;
        return modelMesh;

    }

    public Mesh GetMeshWithVectorsAndTriangles(float width, float height)
    {
        Mesh modelMesh = new Mesh();
        modelMesh.vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(width,0,0),
            new Vector3(width,0,height),
            new Vector3(0,0,height)
        };

        modelMesh.uv = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1)
        };
        modelMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        return modelMesh;
    }

}

using UnityEngine;

public class PBDIntegration : MonoBehaviour
{
    // Parameters for PBD simulation
    public float timestep = 0.01f;
    public int iterations = 10;
    public float stiffness = 0.5f;
    public float maxDistance = 1.5f; // Maximum distance for stretching constraint

    // Mesh and particle data
    private Mesh mesh;
    private Vector3[] particles;
    private Vector3[] velocities;
    private float[] invertedMasses;
    private MeshCollider meshCollider;
    private int selectedVertex = -1;

    void Start()
    {
        // Initialize mesh and particle data
        mesh = GetComponent<MeshFilter>().mesh;
        particles = mesh.vertices;
        velocities = new Vector3[particles.Length];
        invertedMasses = new float[particles.Length];

        // Set inverted masses (for simplicity, assume uniform mass distribution)
        float totalMass = 1.0f;
        for (int i = 0; i < invertedMasses.Length; i++)
        {
            invertedMasses[i] = 1.0f / (particles.Length * totalMass);
        }

        // Initialize mesh collider
        meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }

        // Disable mesh collider for raycasting (enable it when not dragging)
        meshCollider.enabled = false;
    }

    void Update()
    {
        HandleInput();
        SimulatePBD();
        UpdateMesh();
        UpdateMeshCollider();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MeshCollider collider = hit.collider as MeshCollider;
                if (collider != null && collider.sharedMesh == mesh)
                {
                    selectedVertex = FindClosestVertex(hit.point);
                    meshCollider.enabled = false; // Disable collider for dragging
                }
            }
        }
        else if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            selectedVertex = -1;
            meshCollider.enabled = true; // Enable collider after dragging
        }

        if (selectedVertex != -1 && Input.GetMouseButton(0)) // Dragging selected vertex
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            particles[selectedVertex] = newPos;
            ApplyConstraints(selectedVertex);
        }
    }

    void SimulatePBD()
    {
        for (int i = 0; i < iterations; i++)
        {
            // Apply external forces (not included in this simplified example)

            // Predict positions
            // for (int j = 0; j < particles.Length; j++)
            // {
            //     velocities[j] += invertedMasses[j] * Physics.gravity * timestep;
            //     particles[j] += velocities[j] * timestep;
            // }

            // Resolve constraints
            SolveConstraints();
        }
    }

    void SolveConstraints()
    {
        // Stretching constraints
        for (int i = 0; i < particles.Length; i++)
        {
            ApplyConstraints(i);
        }

        // Bending constraints (not implemented in this example)
    }

    void ApplyConstraints(int vertexIndex)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (i != vertexIndex && Vector3.Distance(particles[i], particles[vertexIndex]) > maxDistance)
            {
                particles[i] = particles[vertexIndex] + (particles[i] - particles[vertexIndex]).normalized * maxDistance;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.vertices = particles;
        mesh.RecalculateNormals();
    }

    void UpdateMeshCollider()
    {
        meshCollider.sharedMesh = mesh;
    }

    int FindClosestVertex(Vector3 point)
    {
        float minDist = Mathf.Infinity;
        int closestVertex = -1;

        for (int i = 0; i < particles.Length; i++)
        {
            float dist = Vector3.Distance(particles[i], point);
            if (dist < minDist)
            {
                minDist = dist;
                closestVertex = i;
            }
        }

        return closestVertex;
    }
}

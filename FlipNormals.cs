using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class inverts the sharders and normal face of the sphere object
 **/
public class FlipNormals : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Gets the mesh
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        // Establishes the normals (faces)
        Vector3[] normals = mesh.normals;
        // Goes through each normal value and inverts it (flip it 180 degrees)
        for (int index = 0; index < normals.Length; index ++)
        {
            normals[index] = -1 * normals[index];
        }

        // Sets the normals
        mesh.normals = normals;

        // For each triangle in the mesh
        // Swap the vertices of each triangle with the opposite side to flip the triangle
        for (int index = 0; index < mesh.subMeshCount; index ++)
        {
            int[] triangle = mesh.GetTriangles(index);
            for (int index2 = 0; index2 < triangle.Length; index2 += 3)
            {
                int temp = triangle[index2];
                triangle[index2] = triangle[index2 + 1];
                triangle[index2 + 1] = temp;
            }
            // Set the triangles back into the mesh
            mesh.SetTriangles(triangle, index);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class generates the mesh for the main map
**/
public class MainMeshGenerator : MonoBehaviour
{
    // Declares and initialized variables
	public SquareGrid squareGrid;
	List<Vector3> vertices;
	List<int> triangles;
    // Maps all the triangles in the mesh to the index of the vertices that formed it
    Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
	// List of integer list to store all of the outlines
	List<List<int>> outlines = new List<List<int>>();
	// A list of vertices that have been checked/outlined already
	HashSet<int> checkedVertices = new HashSet<int>();
	// The height of the wall
	public float wallHeight;
	// The mesh filter
	public MeshFilter walls;
	public GameObject floorPlane;


    /**
     * Call this method to generate the mesh
    **/
	public void GenerateMesh(int[,] map, float squareSize)
	{
		triangleDictionary.Clear();
		outlines.Clear();
		checkedVertices.Clear();


        // Declare and initializes variables
		squareGrid = new SquareGrid(map, squareSize);
		vertices = new List<Vector3>();
		triangles = new List<int>();

        // Passes through every square in the grid
		for (int xIndex = 0; xIndex < squareGrid.squares.GetLength(0); xIndex++)
		{
			for (int zIndex = 0; zIndex < squareGrid.squares.GetLength(1); zIndex++)
			{
                // Uses triangles to draw/connect points within each square
				drawTriangles(squareGrid.squares[xIndex, zIndex]);
			}
		}

        // Creates a new mesh
		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
        // Sets the vertices and triangles
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();

		CreateWallMesh();
		floorPlane.transform.position = new Vector3(0, -wallHeight, 0);

		MeshCollider wallTriMesh = this.gameObject.AddComponent<MeshCollider>();
		wallTriMesh.sharedMesh = null;

		var newMesh = CombineMeshes(new List<Mesh> { mesh, walls.sharedMesh });
		wallTriMesh.sharedMesh = newMesh;
	}

	/**
	 * Combines the meshes of walls and floors for optimization
	**/
	private Mesh CombineMeshes(List<Mesh> meshes)
	{
		var combine = new CombineInstance[meshes.Count];
		for (int i = 0; i < meshes.Count; i++)
		{
			combine[i].mesh = meshes[i];
			combine[i].transform = transform.localToWorldMatrix;
		}

		var mesh = new Mesh();
		mesh.CombineMeshes(combine);
		return mesh;
	}

	/**
     * Creates the mesh for the wall
     **/
	void CreateWallMesh()
    {
		CalculateMeshOutlines();

		List<Vector3> wallVertices = new List<Vector3>();
		List<int> wallTriangles = new List<int>();
		Mesh wallMesh = new Mesh();

        foreach (List<int> outline in outlines)
        {
            for (int index = 0; index < outline.Count - 1; index ++)
            {
				int startIndex = wallVertices.Count;
                // Adds the left vertex
				wallVertices.Add(vertices[outline[index]]);
                // Adds the right vertex
				wallVertices.Add(vertices[outline[index + 1]]);
				// Adds the bottom left vertex
				wallVertices.Add(vertices[outline[index]] - Vector3.up * wallHeight);
				// Adds the bottom right vertex
				wallVertices.Add(vertices[outline[index + 1]] - Vector3.up * wallHeight);

				wallTriangles.Add(startIndex);
				wallTriangles.Add(startIndex + 2);
				wallTriangles.Add(startIndex + 3);

				wallTriangles.Add(startIndex + 3);
				wallTriangles.Add(startIndex + 1);
				wallTriangles.Add(startIndex);
			}
        }
		wallMesh.vertices = wallVertices.ToArray();
		wallMesh.triangles = wallTriangles.ToArray();

		walls.mesh = wallMesh;
    }

    /**
     * Call this method to draw the mesh using triangles
     * @param  Square  square  The square that will be drawn using triangles
    **/
	void drawTriangles(Square square)
	{
        // Different triangles will be drawn depending on which vertices are active
		switch (square.configuration)
		{
            // 0 points are active
			case 0:
				break;
			// 1 point is active
			case 1:
				MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
				break;
			case 2:
				MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
				break;
			case 4:
				MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
				break;
			case 8:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
				break;
			// 2 points are active
			case 3:
				MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
				break;
			case 6:
				MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
				break;
			case 9:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
				break;
			case 12:
				MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
				break;
			case 5:
				MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
				break;
			case 10:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
				break;
			// 3 point are active
			case 7:
				MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
				break;
			case 11:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
				break;
			case 13:
				MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
				break;
			case 14:
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
				break;
			// 4 points are active
            // None of the vertices here can be outline vertices
			case 15:
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
				checkedVertices.Add(square.topLeft.vertexIndex);
				checkedVertices.Add(square.topRight.vertexIndex);
				checkedVertices.Add(square.bottomLeft.vertexIndex);
				checkedVertices.Add(square.bottomRight.vertexIndex);
				break;
		}

	}

    /**
     * Connects the points of the triangles
     * @param  Node[]  points  The active points within the square
    **/
	void MeshFromPoints(params Node[] points)
	{
        // Adds the given points to an array
		AssignVertices(points);
        // If there are three points 1 triangle is drawn
		if (points.Length >= 3)
			CreateTriangle(points[0], points[1], points[2]);
        // If there are four points 2 triangles are drawn
		if (points.Length >= 4)
			CreateTriangle(points[0], points[2], points[3]);
        // If there are five points 3 triangles are drawn
		if (points.Length >= 5)
			CreateTriangle(points[0], points[3], points[4]);
        // If there are six points 4 triangles are drawn
		if (points.Length >= 6)
			CreateTriangle(points[0], points[4], points[5]);
	}

    /**
     * Add the points of the square to an array
     * @param  Node[]  points  the points that will be added to the array
    **/
	void AssignVertices(Node[] points)
	{
        // Goes through the array
		for (int i = 0; i < points.Length; i++)
		{
            // If the point is not yet active
			if (points[i].vertexIndex == -1)
			{
                // Activate it and set the vertex index to the current array count
				points[i].vertexIndex = vertices.Count;
                // Adds the array to the point to the vertices array
				vertices.Add(points[i].position);
			}
		}
	}

    /**
     * Draws the triangle given three vertices
     * @param  Node  aNode  the first node of the triangle
     * @param  Node  bNode  the second node of the triangle
     * @param  Node  cNode  the third node of the triangle
    **/
	void CreateTriangle(Node aNode, Node bNode, Node cNode)
	{
		triangles.Add(aNode.vertexIndex);
		triangles.Add(bNode.vertexIndex);
		triangles.Add(cNode.vertexIndex);

		Triangle triangle = new Triangle(aNode.vertexIndex, bNode.vertexIndex, cNode.vertexIndex);
		AddTriangleToDictionary(triangle.vertexIndexA, triangle);
		AddTriangleToDictionary(triangle.vertexIndexB, triangle);
		AddTriangleToDictionary(triangle.vertexIndexC, triangle);
	}

    /**
     * The square grid class
    **/
	public class SquareGrid
	{
        // Declare and initialize variables
		public Square[,] squares;
        // Creates a grid of squares to be used for the mesh
		public SquareGrid(int[,] map, float squareSize)
		{
            // The maximum values along the width and depth of the map
			int nodeCountX = map.GetLength(0);
			int nodeCountZ = map.GetLength(1);
            // The max size of the map
			float mapWidth = nodeCountX * squareSize;
			float mapDepth = nodeCountZ * squareSize;

            // Creates a new array containing all of the control nodes
			ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountZ];

            // Passes through the map
			for (int xIndex = 0; xIndex < nodeCountX; xIndex++)
			{
				for (int zIndex = 0; zIndex < nodeCountZ; zIndex++)
				{
                    // Fills the array with control nodes
					Vector3 pos = new Vector3(-mapWidth / 2 + xIndex * squareSize + squareSize / 2, 0, -mapDepth / 2 + zIndex * squareSize + squareSize / 2);
					controlNodes[xIndex, zIndex] = new ControlNode(pos, map[xIndex, zIndex] == 1, squareSize);
				}
			}

            // Creates new array of squares
			squares = new Square[nodeCountX - 1, nodeCountZ - 1];
            // Fills the array with squares
			for (int x = 0; x < nodeCountX - 1; x++)
			{
				for (int y = 0; y < nodeCountZ - 1; y++)
				{
					squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
				}
			}

		}
	}

    /**
     * The square class
     * This class holds 8 nodes (4 of which are control nodes)
    **/
	public class Square
	{
        // Declare and initialize variables
		public ControlNode topLeft, topRight, bottomRight, bottomLeft;
		public Node centreTop, centreRight, centreBottom, centreLeft;
		public int configuration;

		/**
         * Class constructor
         * @param  ControlNode  topLeftControl      the top left control node
         * @param  ControlNode  topRightControl     the top right control node
         * @param  ControlNode  bottomRightControl  the bottom right control node
         * @param  ControlNode  bottomLeftControl   the bottom left control node
        **/
		public Square(ControlNode topLeftControl, ControlNode topRightControl, ControlNode bottomRightControl, ControlNode bottomLeftControl)
		{
			topLeft = topLeftControl;
			topRight = topRightControl;
			bottomRight = bottomRightControl;
			bottomLeft = bottomLeftControl;

			centreTop = topLeft.right;
			centreRight = bottomRight.above;
			centreBottom = bottomLeft.right;
			centreLeft = bottomLeft.above;

            // Sets the configuration depending on the number and types of active nodes
			if (topLeft.active)
				configuration += 8;
			if (topRight.active)
				configuration += 4;
			if (bottomRight.active)
				configuration += 2;
			if (bottomLeft.active)
				configuration += 1;
		}

	}

    /**
     * The Node class
    **/
	public class Node
	{
        // Declare and initialize variables
		public Vector3 position;
		public int vertexIndex = -1;

        /**
         * Class constructor
         * @param  Vector3  vectorPosition the position of the vector
        **/
		public Node(Vector3 vectorPosition)
		{
			position = vectorPosition;
		}
	}

    /**
     * The ControlNode class
     * This class extend the Node class
    **/
	public class ControlNode : Node
	{
        // Declare and initializes the variables
		public bool active;
		public Node above, right;

        /**
         * Class constructor
         * @param  Vector3  vectorPos   The position of the vector
         * @param  bool     activity    Whether or not the node has been activated
         * @param  float    squareSize  The size of the square
        **/
		public ControlNode(Vector3 vectorPos, bool activity, float squareSize) : base(vectorPos)
		{
			active = activity;
			above = new Node(position + Vector3.forward * squareSize / 2f);
			right = new Node(position + Vector3.right * squareSize / 2f);
		}

	}

    /**
     * This struct holds 3 variables
    **/
    struct Triangle
    {
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;
		int[] vertices;

		/**
         * The class constructor
         * @param  int  aValue  The index of the first vertex
         * @param  int  bValue  The index of the second vertex
         * @param  int  cValue  The index of the third vertex
        **/
		public Triangle(int aValue, int bValue, int cValue)
        {
			vertexIndexA = aValue;
			vertexIndexB = bValue;
			vertexIndexC = cValue;
			vertices = new int[3];

			vertices[0] = aValue;
			vertices[1] = bValue;
			vertices[2] = cValue;
        }

        /**
         * Determines whether or not the current triangle contains a given vertex
         * @param  int  vertexIndex  The index that will be checked
         * @return bool              True if the triangles contains the vertex, false if not
         **/
        public bool Contains(int vertexIndex)
        {
			return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
        }

        /**
         * Indexer for the struct
        **/
        public int this[int index]
        {
            get
            {
				return vertices[index];
            }
        }
    }

    /**
     * Adds the triangle to the dictionary
     * @param  int       vertexIndexKey  The key that the triangle will be mapped to
     * @param  Triangle  triangle        The triangle that will be added to the dictionary
    **/
    void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
    {
        if (triangleDictionary.ContainsKey(vertexIndexKey))
        {
			triangleDictionary[vertexIndexKey].Add(triangle);
        }
        else
        {
			List<Triangle> triangleList = new List<Triangle>();
			triangleList.Add(triangle);
			triangleDictionary.Add(vertexIndexKey, triangleList);
        }
    }

    /**
     * Gets the number of vertexes forming a triangle in the outlined edge
     * @param  int  vertexIndex  The index of the vertex that will be checked
     * @return                   -1 if no shared vertex that forms a triangle is found
    **/
    int GetConnectedOutlineVertex(int vertexIndex)
    {
		List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];
        for (int index = 0; index < trianglesContainingVertex.Count; index ++)
        {
			Triangle triangle = trianglesContainingVertex[index];
            for (int index2 = 0; index2 < 3; index2 ++)
            {
				int vertexB = triangle[index2];
				if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
				{
					if (IsOutlineEdge(vertexIndex, vertexB))
					{
						return vertexB;
					}
				}
            }
        }
		return -1;
    }

    /**
     * Determines whether or not the triangle is an outline triangle
     * @param  int  vertexA  The first vertex
     * @param  int  vertexB  The second vertex
     * @return bool          True if the vertex is an outline edge, false if not
    **/
    bool IsOutlineEdge(int vertexA, int vertexB)
    {
		// If vertex A and vertex B only have 1 common triangle, then the triangle is an edge
		List<Triangle> trianglesContainingVertexA = triangleDictionary[vertexA];
		int sharedTriangleCount = 0;

        for (int index = 0; index < trianglesContainingVertexA.Count; index ++)
        {
            if (trianglesContainingVertexA[index].Contains(vertexB))
            {
				sharedTriangleCount++;
                if (sharedTriangleCount > 1)
                {
					break;
                }
            }
        }
		return sharedTriangleCount == 1;
    }

    /**
     * Loops around the map to make check all of the vertices outlining the borders/walls
     **/
    void CalculateMeshOutlines ()
    {
        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex ++)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
				int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                if (newOutlineVertex != -1)
                {
					checkedVertices.Add(vertexIndex);

					List<int> newOutline = new List<int>();
					newOutline.Add(vertexIndex);
					outlines.Add(newOutline);
					FollowOutline(newOutlineVertex, outlines.Count - 1);
					outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }

    void FollowOutline(int vertexIndex, int outlineIndex)
    {
		outlines[outlineIndex].Add(vertexIndex);
		checkedVertices.Add(vertexIndex);
		int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

        if (nextVertexIndex != -1)
        {
			FollowOutline(nextVertexIndex, outlineIndex);
        }
    }
}

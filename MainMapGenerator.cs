using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Specialized;

/**
 * This class generates the Main map
**/
public class MainMapGenerator : MonoBehaviour
{
    /**
     * Declaration and initialization of variables
    **/
    // The width and depth of the map (measured in cell size)
	public int width;
	public int depth;
    public int wallHeight;
    // The seed
	public string seed;
    // Whether or not to generate a new random seed
	public bool useRandomSeed;
    // Fill percentage of the map with a range between 0 and 100 (used for cellular automaton)
	[Range(0, 100)]
	public int fillPercent;
	// The number of desired passes
	public int passes;
    // The map is a 2D array of cells (cellular automaton)
	int[,] map;
    // The size/thickness of the border
    public int borderSize;
    // The minimum size of a room
    public int roomThresholdSize;
    // The floor
    public GameObject floorPlane;
    // The sky
    public GameObject skyBox;
    // Keeps a copy of the unmodified depthchart that will be used to spawn in objects and the player
    int[,] spawnMap;
    // Spawn percentage
    [Range(0f, 1f)]
    public float spawnPercent;
    // The array containing all of the items that will be spawn into the world
    public GameObject[] spawnItems;
    // An array containing all of the high profile objects to spawn
    public GameObject[] importantItems;
    // A list of all available spawn positions for high profile objects
    List<int> xIndexPositions;
    List<int> zIndexPositions;
    // The player
    public GameObject player;
    // The first portal
    public GameObject firstPortal;
    // The second portal
    public GameObject secondPortal;
    // The third portal
    public GameObject thirdPortal;
    // The fourth portal
    public GameObject fourthPortal;

    // The portal Manager
    public GameObject portalManager;

    // The "aura"/light FX of the portal
    public GameObject aura;

    public GameObject easle;
    public List<int> xIndexPositionHigh;
    public List<int> zIndexPositionHigh;

    /**
     * This method is called once before the first frame update
    **/
    void Start()
	{
		GenerateMap();
        float scaleY = this.transform.localScale.y;
        floorPlane.transform.localScale = new Vector3(width/10, scaleY, depth/10);
        ProceduralSpawn();
        float radius = (float) Math.Sqrt((width * width) + (depth * depth));
        skyBox.transform.localScale = new Vector3(radius, radius, radius);
    }

    /**
     * Checks the map to find any available 3x3 spaces to spawn important objects
     **/
    void CheckMap3x3()
    {
        xIndexPositions = new List<int>();
        zIndexPositions = new List<int>();
        xIndexPositionHigh = new List<int>();
        zIndexPositionHigh = new List<int>();
        // Goes through the map to check if there are available space of 9 tiles (3x3 tiles space)
        for (int xIndexLoop = 1; xIndexLoop < width - 1; xIndexLoop++)
        {
            for (int zIndexLoop = 1; zIndexLoop < depth - 1; zIndexLoop++)
            {
                if (map[xIndexLoop, zIndexLoop] == 0)
                {
                    if (map[xIndexLoop - 1, zIndexLoop] == 0
                        && map[xIndexLoop + 1, zIndexLoop] == 0
                        && map[xIndexLoop, zIndexLoop - 1] == 0
                        && map[xIndexLoop, zIndexLoop + 1] == 0
                        && map[xIndexLoop - 1, zIndexLoop - 1] == 0
                        && map[xIndexLoop + 1, zIndexLoop + 1] == 0
                        && map[xIndexLoop - 1, zIndexLoop + 1] == 0
                        && map[xIndexLoop + 1, zIndexLoop - 1] == 0)
                    {
                        xIndexPositions.Add(xIndexLoop);
                        zIndexPositions.Add(zIndexLoop);
                    }
                }
                else
                {
                    if (map[xIndexLoop - 1, zIndexLoop] == 1
                        && map[xIndexLoop + 1, zIndexLoop] == 1
                        && map[xIndexLoop, zIndexLoop - 1] == 1
                        && map[xIndexLoop, zIndexLoop + 1] == 1
                        && map[xIndexLoop - 1, zIndexLoop - 1] == 1
                        && map[xIndexLoop + 1, zIndexLoop + 1] == 1
                        && map[xIndexLoop - 1, zIndexLoop + 1] == 1
                        && map[xIndexLoop + 1, zIndexLoop - 1] == 1)
                    {
                        xIndexPositionHigh.Add(xIndexLoop);
                        zIndexPositionHigh.Add(zIndexLoop);
                    }
                }
            }
        }
    }

    /**
     * Spawns all of the objects and the player onto the map
    **/
    void ProceduralSpawn()
    {
        // Uses the seed to generate a pseudo-random map
        System.Random pseudoRNG = new System.Random(seed.GetHashCode());

        CheckMap3x3();

        // spawns environment objects on second layer first
        // Generates the position and rotation that the object will spawn in
        int spawnPositionIndexEasle = (pseudoRNG.Next(0, xIndexPositionHigh.Count));
        int xIndexEasle = xIndexPositionHigh[spawnPositionIndexEasle];
        int zIndexEasle = zIndexPositionHigh[spawnPositionIndexEasle];
        Vector3 spawnPositionEasle = new Vector3(-width / 2 + xIndexEasle, 0f, -depth / 2 + zIndexEasle);
        Vector3 spawnRotationEasle = new Vector3(0, pseudoRNG.Next(0, 359), 0);
        // Sets the tile in the map as occupied
        spawnMap[xIndexEasle, zIndexEasle] = 2;
        spawnMap[xIndexEasle - 1, zIndexEasle] = 2;
        spawnMap[xIndexEasle + 1, zIndexEasle] = 2;
        spawnMap[xIndexEasle, zIndexEasle - 1] = 2;
        spawnMap[xIndexEasle, zIndexEasle + 1] = 2;
        spawnMap[xIndexEasle - 1, zIndexEasle - 1] = 2;
        spawnMap[xIndexEasle + 1, zIndexEasle + 1] = 2;
        spawnMap[xIndexEasle - 1, zIndexEasle + 1] = 2;
        spawnMap[xIndexEasle + 1, zIndexEasle - 1] = 2;
        // Spawns the easle on the second layer
        easle.transform.position = spawnPositionEasle;
        easle.transform.rotation = Quaternion.Euler(spawnRotationEasle);

        for (int xIndexLoop = 0; xIndexLoop < width; xIndexLoop++)
        {
            for (int zIndexLoop = 0; zIndexLoop < depth; zIndexLoop++)
            {
                if (spawnMap[xIndexLoop, zIndexLoop] == 1)
                {
                    // Calculates the spawn percent (should an object be spawned at position [x,z]?)
                    int spawnMultiplier = (int)(1 / spawnPercent);
                    // Generates a yes or no response
                    int spawnIndexGeneral = pseudoRNG.Next(0, spawnItems.Length * spawnMultiplier);
                    // If yes for spawn
                    if (spawnIndexGeneral < spawnItems.Length)
                    {
                        // Spawn object at position [x,z]
                        Vector3 spawnPositionGeneral = new Vector3(-width / 2 + xIndexLoop, 0, -depth / 2 + zIndexLoop);
                        Vector3 spawnRotationGeneral = new Vector3(0, pseudoRNG.Next(0, 359), 0);

                        // Spawns the object at the position with a random rotation
                        Instantiate(spawnItems[spawnIndexGeneral], spawnPositionGeneral, Quaternion.Euler(spawnRotationGeneral));
                        spawnMap[xIndexLoop, zIndexLoop] = 2;
                    }
                }
            }
        }

        // After that elements on the second layer are generated
        CheckMap3x3();

        // Spawns the player at the position of the first portal
        // Generates the position and rotation that the object will spawn in
        int spawnPositionIndex = (pseudoRNG.Next(0, xIndexPositions.Count));
        int xIndex = xIndexPositions[spawnPositionIndex];
        int zIndex = zIndexPositions[spawnPositionIndex];
        Vector3 spawnPositionPortal = new Vector3(-width / 2 + xIndex, -wallHeight, -depth / 2 + zIndex);
        Vector3 spawnPositionPlayer = new Vector3(-width / 2 + xIndex, -wallHeight + 0.5f, -depth / 2 + zIndex);
        Vector3 spawnRotation = new Vector3(0, pseudoRNG.Next(0, 359), 0);

        // Sets the tile in the map as occupied
        spawnMap[xIndex, zIndex] = 1;
        spawnMap[xIndex - 1, zIndex] = 1;
        spawnMap[xIndex + 1, zIndex] = 1;
        spawnMap[xIndex, zIndex - 1] = 1;
        spawnMap[xIndex, zIndex + 1] = 1;
        spawnMap[xIndex - 1, zIndex - 1] = 1;
        spawnMap[xIndex + 1, zIndex + 1] = 1;
        spawnMap[xIndex - 1, zIndex + 1] = 1;
        spawnMap[xIndex + 1, zIndex - 1] = 1;

        // Spawns the first Portal
        firstPortal.transform.position = spawnPositionPortal;
        firstPortal.transform.rotation = Quaternion.Euler(spawnRotation);

        aura.transform.position = spawnPositionPortal;
        aura.transform.rotation = Quaternion.Euler(spawnRotation);

        // Spawns the player
        player.transform.position = spawnPositionPlayer;
        //player.transform.rotation = Quaternion.Euler(spawnRotation);


        // Checks the map for 3x3 spaces
        CheckMap3x3();
        // Generates the position and rotation that the object will spawn in
        int spawnPositionIndex2 = (pseudoRNG.Next(0, xIndexPositions.Count));
        int xIndex2 = xIndexPositions[spawnPositionIndex2];
        int zIndex2 = zIndexPositions[spawnPositionIndex2];
        Vector3 spawnPositionPortal2 = new Vector3(-width / 2 + xIndex2, -wallHeight, -depth / 2 + zIndex2);
        Vector3 spawnRotation2 = new Vector3(0, pseudoRNG.Next(0, 359), 0);
        // Sets the tile in the map as occupied
        spawnMap[xIndex2, zIndex2] = 1;
        spawnMap[xIndex2 - 1, zIndex2] = 1;
        spawnMap[xIndex2 + 1, zIndex2] = 1;
        spawnMap[xIndex2, zIndex2 - 1] = 1;
        spawnMap[xIndex2, zIndex2 + 1] = 1;
        spawnMap[xIndex2 - 1, zIndex2 - 1] = 1;
        spawnMap[xIndex2 + 1, zIndex2 + 1] = 1;
        spawnMap[xIndex2 - 1, zIndex2 + 1] = 1;
        spawnMap[xIndex2 + 1, zIndex2 - 1] = 1;
        // Spawns the second Portal
        secondPortal.transform.position = spawnPositionPortal2;
        secondPortal.transform.rotation = Quaternion.Euler(spawnRotation2);

        // Checks the map for 3x3 spaces
        CheckMap3x3();
        // Generates the position and rotation that the object will spawn in
        int spawnPositionIndex3 = (pseudoRNG.Next(0, xIndexPositions.Count));
        int xIndex3 = xIndexPositions[spawnPositionIndex3];
        int zIndex3 = zIndexPositions[spawnPositionIndex3];
        Vector3 spawnPositionPortal3 = new Vector3(-width / 2 + xIndex3, -wallHeight, -depth / 2 + zIndex3);
        Vector3 spawnRotation3 = new Vector3(0, pseudoRNG.Next(0, 359), 0);
        // Sets the tile in the map as occupied
        spawnMap[xIndex3, zIndex3] = 1;
        spawnMap[xIndex3 - 1, zIndex3] = 1;
        spawnMap[xIndex3 + 1, zIndex3] = 1;
        spawnMap[xIndex3, zIndex3 - 1] = 1;
        spawnMap[xIndex3, zIndex3 + 1] = 1;
        spawnMap[xIndex3 - 1, zIndex3 - 1] = 1;
        spawnMap[xIndex3 + 1, zIndex3 + 1] = 1;
        spawnMap[xIndex3 - 1, zIndex3 + 1] = 1;
        spawnMap[xIndex3 + 1, zIndex3 - 1] = 1;
        // Spawns the second Portal
        thirdPortal.transform.position = spawnPositionPortal3;
        thirdPortal.transform.rotation = Quaternion.Euler(spawnRotation3);

        // Checks the map for 3x3 spaces
        CheckMap3x3();
        // Generates the position and rotation that the object will spawn in
        int spawnPositionIndex4 = (pseudoRNG.Next(0, xIndexPositions.Count));
        int xIndex4 = xIndexPositions[spawnPositionIndex4];
        int zIndex4 = zIndexPositions[spawnPositionIndex4];
        Vector3 spawnPositionPortal4 = new Vector3(-width / 2 + xIndex4, -wallHeight, -depth / 2 + zIndex4);
        Vector3 spawnRotation4 = new Vector3(0, pseudoRNG.Next(0, 359), 0);
        // Sets the tile in the map as occupied
        spawnMap[xIndex4, zIndex4] = 1;
        spawnMap[xIndex4 - 1, zIndex4] = 1;
        spawnMap[xIndex4 + 1, zIndex4] = 1;
        spawnMap[xIndex4, zIndex4 - 1] = 1;
        spawnMap[xIndex4, zIndex4 + 1] = 1;
        spawnMap[xIndex4 - 1, zIndex4 - 1] = 1;
        spawnMap[xIndex4 + 1, zIndex4 + 1] = 1;
        spawnMap[xIndex4 - 1, zIndex4 + 1] = 1;
        spawnMap[xIndex4 + 1, zIndex4 - 1] = 1;
        // Spawns the second Portal
        fourthPortal.transform.position = spawnPositionPortal4;
        fourthPortal.transform.rotation = Quaternion.Euler(spawnRotation4);

        // Spawns the important items such as portals
        for (int numberOfItemsToSpawn = 0; numberOfItemsToSpawn < importantItems.Length; numberOfItemsToSpawn ++)
        {
            CheckMap3x3();

            // Generates the position and rotation that the object will spawn in
            int spawnIndex = (pseudoRNG.Next(0, xIndexPositions.Count));
            int xPosition = xIndexPositions[spawnIndex];
            int zPosition = zIndexPositions[spawnIndex];
            Vector3 spawnPositionImportant = new Vector3(-width / 2 + xPosition, -wallHeight, -depth / 2 + zPosition);
            Vector3 spawnRotationImportant = new Vector3(0, pseudoRNG.Next(0, 359), 0);

            // Sets the tile in the map as occupied
            spawnMap[xPosition, zPosition] = 1;
            spawnMap[xPosition - 1, zPosition] = 1;
            spawnMap[xPosition + 1, zPosition] = 1;
            spawnMap[xPosition, zPosition - 1] = 1;
            spawnMap[xPosition, zPosition + 1] = 1;
            spawnMap[xPosition - 1, zPosition - 1] = 1;
            spawnMap[xPosition + 1, zPosition + 1] = 1;
            spawnMap[xPosition - 1, zPosition + 1] = 1;
            spawnMap[xPosition + 1, zPosition - 1] = 1;

            // Spawns the object at the position with a random rotation
            Instantiate(importantItems[numberOfItemsToSpawn], spawnPositionImportant, Quaternion.Euler(spawnRotationImportant));
        }

        // Spawns the general items such as trees and rocks
        for (int xIndexLoop = 0; xIndexLoop < width; xIndexLoop++)
        {
            for (int zIndexLoop = 0; zIndexLoop < depth; zIndexLoop++)
            {
                if (spawnMap[xIndexLoop, zIndexLoop] == 0)
                {
                    // Calculates the spawn percent (should an object be spawned at position [x,z]?)
                    int spawnMultiplier = (int) (1 / spawnPercent);
                    // Generates a yes or no response
                    int spawnIndexGeneral = pseudoRNG.Next(0, spawnItems.Length * spawnMultiplier);
                    // If yes for spawn
                    if (spawnIndexGeneral < spawnItems.Length)
                    {
                        // Spawn object at position [x,z]
                        Vector3 spawnPositionGeneral = new Vector3(-width / 2 + xIndexLoop, -wallHeight, -depth / 2 + zIndexLoop);
                        Vector3 spawnRotationGeneral = new Vector3(0, pseudoRNG.Next(0, 359), 0);

                        // Spawns the object at the position with a random rotation
                        Instantiate(spawnItems[spawnIndexGeneral], spawnPositionGeneral, Quaternion.Euler(spawnRotationGeneral));
                    }
                }
            }
        }

        portalManager.GetComponent<PortalManager>().SetInitialPortals();
    }

    /**
     * Call this method to initialize the map
    **/
    void GenerateMap()
	{
        // Creates a new blank map
		map = new int[width, depth];
        // Gives each cell in the map a depth value
		FillMap();
        // Passes over the map and refines the cells to give a more "natural" appearance
		for (int index = 0; index < passes; index++)
		{
			PassAndRefine();
		}

        // Removes any rooms that are too small
        ProcessMap();

        // Stores a copy of the map that will be used to spawn in objects
        spawnMap = new int[width, depth];
        spawnMap = map;

        // Initializes the array that will contain the borders of the map
        int[,] mapBorder = new int[width + borderSize * 2, depth + borderSize * 2];
        // Loops through the array
		for (int xIndex = 0; xIndex < mapBorder.GetLength(0); xIndex++)
		{
			for (int zIndex = 0; zIndex < mapBorder.GetLength(1); zIndex++)
			{
                // If the current index is within the map set the border map to the border size
                if (xIndex >= borderSize && xIndex < width + borderSize && zIndex >= borderSize && zIndex < depth + borderSize)
                {
					mapBorder[xIndex, zIndex] = map[xIndex - borderSize, zIndex - borderSize];
                }
                // Otherwise the index is inside a bordered area and will be set to 1
                else
                {
					mapBorder[xIndex, zIndex] = 1;
                }
			}
		}

	    // Initializes and generates the mesh of the map using the depth data
		MainMeshGenerator mesh = GetComponent<MainMeshGenerator>();
		mesh.GenerateMesh(mapBorder, 1);
	}

    /**
     * Call this method to give each cell in the map a depth value
    **/
	void FillMap()
	{
        // Creates a new seed using the system time if a new seed is desired
		if (useRandomSeed)
		{
			seed = System.DateTime.Now.ToString();
		}

        // Uses the seed to generate a pseudo-random map
		System.Random pseudoRNG = new System.Random(seed.GetHashCode());

        // Passes through the map
		for (int xIndex = 0; xIndex < width; xIndex++)
		{
			for (int zIndex = 0; zIndex < depth; zIndex++)
			{
                // If the cell is at the edge of the map it must be a wall (have an depth value of 1)
				if (xIndex == 0 || xIndex == width - 1 || zIndex == 0 || zIndex == depth - 1)
				{
					map[xIndex, zIndex] = 1;
				}
                // All other cells can have any random depth value
				else
				{
					map[xIndex, zIndex] = (pseudoRNG.Next(0, 100) < fillPercent) ? 1 : 0;
				}
			}
		}
	}

    /**
     * Call this method to refine the map and make it "smoother"
    **/
	void PassAndRefine()
	{
        // Passes through the map
		for (int xIndex = 0; xIndex < width; xIndex++)
		{
			for (int zIndex = 0; zIndex < depth; zIndex++)
			{
                // Retrieves the values of neighbouring cells
				int neighbourWallTiles = GetSurroundingWallCount(xIndex, zIndex);
                // The current cell is converted to a wall if there are more than 4 wall cells adjacent to it
				if (neighbourWallTiles > 4)
					map[xIndex, zIndex] = 1;
                // The current cell is converted to an empty cell if there are less than 4 wall cells adjacent to it
				else if (neighbourWallTiles < 4)
					map[xIndex, zIndex] = 0;

			}
		}
	}

    /**
     * Call this method to retrieve the depth value of adjacent cells
     * @param  int  xPosition  The x position of the cell
     * @param  int  zPosition  The z position of the cell
     * @return int  wallCount  The number of walls adjacent to the current cell
    **/
	int GetSurroundingWallCount(int xPosition, int zPosition)
	{
        // Initializes variable
		int wallCount = 0;
        // Passes through all cells in radius (including self)
		for (int neighbourX = xPosition - 1; neighbourX <= xPosition + 1; neighbourX++)
		{
			for (int neighbourZ = zPosition - 1; neighbourZ <= zPosition + 1; neighbourZ++)
			{
                // If this cell is within the maps bounds
				if (IsInMap(neighbourX, neighbourZ))
				{
                    // If this cell is not the cell that is currently being checked
					if (neighbourX != xPosition || neighbourZ != zPosition)
					{
                        // Adds the value of the cell to wallcount
						wallCount += map[neighbourX, neighbourZ];
					}
				}
                // Add 1 to the count of wall (forcing the cells around the edge to be walls
				else
				{
					wallCount++;
				}
			}
		}
		return wallCount;
	}

    /**
     * Stores the x and y coordinates of the tiles in the depth grid
    **/
    struct TileCoordinates
    {
		public int tileXCoordinate;
		public int tileZCoordinate;


        /**
         * Struct Constructor
         * @param  int  xCoord  The x coordinate of the tile
         * @param  int  yCoord  The y coordiante of the tile
        **/
        public TileCoordinates(int xCoord, int zCoord)
        {
			tileXCoordinate = xCoord;
			tileZCoordinate = zCoord;
        }
    }

    /**
     * Checks if the coordinates are within the map's bounds
     * @param  int  xCoord  The x coordinate of the tile
     * @param  int  zCoord  The z coordinate of the tile
    **/
	bool IsInMap(int xCoord, int zCoord)
    {
		return xCoord >= 0 && xCoord < width && zCoord >= 0 && zCoord < depth;
    }

    /**
     * Returns a list of the all the tiles that form a selected region
     * @param  int  startXCoord  The x coordinate of the starting tile
     * @param  int  startYCoord  The y coordinate of the starting tile
    **/
    List<TileCoordinates> GetRegionTiles(int startXCoord, int startZCoord)
    {
        // The list of tile coordinates
		List<TileCoordinates> tiles = new List<TileCoordinates>();
        // Creates a new grids where 1 and 0 represent whether or not the tile has been checked
		int[,] flags = new int[width, depth];
		int tileType = map[startXCoord, startZCoord];

        // Ques the process and sets the start tile to "has been checked"
		Queue<TileCoordinates> queue = new Queue<TileCoordinates>();
		queue.Enqueue(new TileCoordinates(startXCoord, startZCoord));
		flags[startXCoord, startZCoord] = 1;
        // Loops through the que
        while (queue.Count > 0)
        {
            // Sets the tile
			TileCoordinates tile = queue.Dequeue();
			tiles.Add(tile);

            // Checks the neighbouring tiles
            for (int xIndex = tile.tileXCoordinate -1; xIndex <= tile.tileXCoordinate + 1; xIndex ++)
            {
                for (int zIndex = tile.tileZCoordinate - 1; zIndex <= tile.tileZCoordinate + 1; zIndex ++)
                {
                    // Makes sure the tile being checked is within map bounds
					if (IsInMap(xIndex, zIndex) && zIndex == tile.tileZCoordinate || xIndex == tile.tileXCoordinate)
                    {
                        // Makes sure the tile is not already checked and is of the same type
                        if(flags[xIndex,zIndex] == 0 && map[xIndex, zIndex] == tileType)
                        {
                            // Adds tile
							flags[xIndex, zIndex] = 1;
							queue.Enqueue(new TileCoordinates(xIndex, zIndex));
                        }
                    }
                }
            }
        }
		return tiles;
    }

    /**
     * Returns all of the tiles of a certain type
     * @param  int  tileType  The type of tile to be checked for (wall or space)
    **/
    List<List<TileCoordinates>> GetRegion(int tileType)
    {
        // The list of lists containing the tiles
        List<List<TileCoordinates>> regions = new List<List<TileCoordinates>>();
        // Creates a new grids where 1 and 0 represent whether or not the tile has been checked
        int[,] flags = new int[width, depth];

        // Goes through every part of the map
        for (int xIndex = 0; xIndex < width; xIndex ++)
        {
            for (int zIndex = 0; zIndex < depth; zIndex ++)
            {
                if (flags[xIndex, zIndex] == 0 && map[xIndex,zIndex] == tileType)
                {
                    // Checks all the tiles in the new region
                    List<TileCoordinates> newRegion = GetRegionTiles(xIndex, zIndex);
                    // Adds the new region to the list of regions
                    regions.Add(newRegion);
                    foreach (TileCoordinates tile in newRegion)
                    {
                        // Sets each tile in the region as "looked at"
                        flags[tile.tileXCoordinate, tile.tileZCoordinate] = 1;
                    }
                }
            }
        }
        return regions;
    }

    /**
     * Process the map to remove any rooms that are below the room threshold size
    **/
    void ProcessMap()
    {
        // Retrieves all of the space regions on the map
        List<List<TileCoordinates>> spaceRegions = GetRegion(0);
        List<Room> remainingRooms = new List<Room>();

        // Checks each of the regions on the map
        foreach(List<TileCoordinates> spaceRegion in spaceRegions)
        {
            // Removes the rooms that are too small (below the room threshold size)
            if (spaceRegion.Count < roomThresholdSize)
            {
                foreach (TileCoordinates tile in spaceRegion)
                {
                    map[tile.tileXCoordinate, tile.tileZCoordinate] = 1;
                }
            }
            // Otherwise add it to the list of remaining rooms
            else
            {
                remainingRooms.Add(new Room(spaceRegion, map));
            }
        }
        remainingRooms.Sort();
        remainingRooms[0].isMain = true;
        remainingRooms[0].isAccessibleFromMain = true;
        ConnectClosestRooms(remainingRooms);
    }

    /**
     * Connects the rooms that are the closest to one another
     * @param  List<Room>  allRooms  The list of all the rooms to connect
    **/
    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessFromMain = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessFromMain)
        {
            foreach (Room room in allRooms)
            {
                if (room.isAccessibleFromMain)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        // Stores the closest distance
        int closestDistance = 0;
        // Stores the tiles that result in the closest distance
        TileCoordinates bestTileA = new TileCoordinates();
        TileCoordinates bestTileB = new TileCoordinates();
        // Stores the rooms that result in the cloeses distance
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        // Possible connection
        bool possibleConnectionFound = false;
        // All the rooms will be connected to 1 other room
        foreach (Room roomA in roomListA)
        {
            if (!forceAccessFromMain)
            {
                // Reset variable
                possibleConnectionFound = false;
                if (roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }
            foreach (Room roomB in roomListB)
            {
                // Skip if the two rooms are the same
                if (roomA == roomB || roomA.IsConnectedRoom(roomB))
                {
                    continue;
                }
                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA ++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        TileCoordinates tileA = roomA.edgeTiles[tileIndexA];
                        TileCoordinates tileB = roomB.edgeTiles[tileIndexB];
                        // Compares distance between two rooms
                        // uses A^2 + B^2
                        int distanceBetweenRooms = (int)(Math.Pow(tileA.tileXCoordinate - tileB.tileXCoordinate, 2) + Math.Pow(tileA.tileZCoordinate - tileB.tileZCoordinate, 2));
                        // If the new distance is the closest or there is no possible connection yet found
                        if (distanceBetweenRooms < closestDistance || !possibleConnectionFound)
                        {
                            // Remember the information
                            closestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }
            if (possibleConnectionFound && !forceAccessFromMain)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }
        if (possibleConnectionFound && forceAccessFromMain)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }

        // If there are still rooms that are not connected to the main room, force a connection to main room
        if (!forceAccessFromMain)
        {
            ConnectClosestRooms(allRooms, true);
        }
    }

    /**
     * Creates a path between room A and room B
     * @param  Room             roomA  The first room
     * @param  Room             roomB  The second room
     * @param  TileCoordinates  tileA  The best tile in room A
     * @param  TileCoordinates  tileB  The best tile in room B
    **/
    void CreatePassage(Room roomA, Room roomB, TileCoordinates tileA, TileCoordinates tileB)
    {
        Room.ConnectRooms(roomA, roomB);

        List<TileCoordinates> line = GetLine(tileA, tileB);
        foreach (TileCoordinates coord in line)
        {
            DrawCircle(coord, 1);
        }
    }

    /**
     * Draws a circle to carve out a path which will connect two rooms
     * @param  TileCoordinates  coordinate  The position of the circle
     * @param  int              radius      The radius of the circle
     **/
    void DrawCircle(TileCoordinates coordiate, int radius)
    {
        // For all of the points
        for (int x = -radius; x <= radius; x ++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                // If the point is inside the radius of the circle
                if (x*x + z*z <= radius*radius)
                {
                    int drawX = coordiate.tileXCoordinate + x;
                    int drawZ = coordiate.tileZCoordinate + z;
                    if (IsInMap(drawX, drawZ))
                    {
                        map[drawX, drawZ] = 0;
                    }
                }
            }
        }
    }

    /**
     * Gets the cells on the grid that form the line which connects two rooms
     * @param   TileCoordinates        start  The start point of the line
     * @param   TileCoordinates        end    The end point of the line
     * @return  List<TileCoordinates>  line   A list containing all of the coordinates on the line
    **/
    List<TileCoordinates> GetLine(TileCoordinates start, TileCoordinates end)
    {
        List<TileCoordinates> line = new List<TileCoordinates>();

        int x = start.tileXCoordinate;
        int z = start.tileZCoordinate;

        // The change in position
        int dx = end.tileXCoordinate - start.tileXCoordinate;
        int dz = end.tileZCoordinate - start.tileZCoordinate;
        // Whether or not the line is moving forward or backwards
        bool inverted = false;
        // Positive if positive increment and negative if negative increment
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dz);
        // Shortest and longest displacement
        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dz);
        // If the longer line is shorter, than the line is backwards
        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dz);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dz);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int index = 0; index < longest; index ++)
        {
            // Adds the line to the list
            line.Add(new TileCoordinates(x, z));
            // Otherwise move along the line
            if (inverted)
            {
                z += step;
            }
            else
            {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    z += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }

    /**
     * Stores all of the information needed for a room
    **/
    class Room : IComparable<Room>
    {
        // A list of all the tiles in the room
        public List<TileCoordinates> tiles;
        // A list of tiles that form the edge of the room
        public List<TileCoordinates> edgeTiles;
        // A list of the rooms connected to the room
        public List<Room> connectedRooms;
        // The number of tiles in the room
        public int roomSize;
        // Whether or not the room is accessible from the main room
        public bool isAccessibleFromMain;
        // Whether or not this room is the main room
        public bool isMain;

        /**
         * Empty class constructor
        **/
        public Room()
        {
        }

        /**
         * Class constructor
         * @param  List<TileCoordinates>  roomTile  The tiles in the room
         * @param  int[,]                 map       The map in which the room lies
        **/
        public Room(List<TileCoordinates> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<TileCoordinates>();
            // Looks for the edge tiles in the room
            foreach (TileCoordinates tile in tiles)
            {
                for (int xIndex = tile.tileXCoordinate - 1; xIndex <= tile.tileXCoordinate + 1; xIndex ++)
                {
                    for (int zIndex = tile.tileZCoordinate - 1; zIndex <= tile.tileZCoordinate + 1; zIndex++)
                    {
                        if (xIndex == tile.tileXCoordinate || zIndex == tile.tileZCoordinate)
                        {
                            if (map[xIndex, zIndex] == 1)
                            {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        /**
         * Lists the connected rooms to one anothers
         * @param  Room  roomA  The first room
         * @param  Room  roomB  The second room
         **/
        public static void ConnectRooms(Room roomA, Room roomB)
        {
            if (roomA.isAccessibleFromMain)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if(roomB.isAccessibleFromMain)
            {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        /**
         * Checks if two rooms are connected
         * @param  Room  otherRoom  The other room to check
        **/
        public bool IsConnectedRoom(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }

        /**
         * Comparator that allows us to sort by room sizes
        **/
        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }

        /**
         * Set rooms to be accessible from main rooms
         **/
        public void SetAccessibleFromMainRoom()
        {
            if (!isAccessibleFromMain)
            {
                isAccessibleFromMain = true;
                foreach (Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }
    }
}

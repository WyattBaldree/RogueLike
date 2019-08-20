using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public static class Pathfinding
{
    public static Node[,] toPlayerMap;

    public static Node[,] toGoldMap;

    public static Node[,] fleePlayerMap;

    public static float[,] costMap;

    //Make an unvisited list for all nodes that still need to be calculated.
    static List<Node> unvisitedList;


    enum InitialMapType { };
    // Make initial map
    static Node[,] GeneratInitialMap(List<Vector2> startList)
    {
        //MapController mapController = GameController.mapC;
        Vector2 screenResInUnits = GetGameController().ScreenResInUnits;

        unvisitedList = new List<Node>();

        //Make an array of nodes for the map
        Node[,] nodeArray = new Node[(int)screenResInUnits.x, (int)screenResInUnits.y];

        for (int i = 0; i < screenResInUnits.x; i++)
        {
            for (int j = 0; j < screenResInUnits.y; j++)
            {
                //If this is one of our starting nodes, isStart = true, else isStart = false
                bool isStart = false;
                foreach (Vector2 v in startList)
                {
                    if (v.x == i && v.y == j)
                    {
                        isStart = true;
                        break;
                    }
                }

                //If this is a blocking object, don't add it to the unvisited list
                if (costMap[i, j] == float.MaxValue)
                {
                    nodeArray[i, j] = new Node(i, j, float.MaxValue);
                    nodeArray[i, j].visited = true;

                }
                //If this is a starting node, put it at the beginning of the unvisited list and give it 0 distance.
                else if (isStart)
                {
                    nodeArray[i, j] = new Node(i, j, 0);
                    unvisitedList.Insert(0, nodeArray[i, j]);

                }
                //If this is a regular node, put it at the end of the unvisited list and give it MAX distance. 
                else
                {
                    nodeArray[i, j] = new Node(i, j, float.MaxValue);
                    unvisitedList.Insert(unvisitedList.Count, nodeArray[i, j]);
                }
            }
        }

        return nodeArray;
    }

    /// <summary>
    /// Generate a map to the player
    /// </summary>
    static public void GenerateToPlayerMap()
    {
        toPlayerMap = Dijkstra((int)GetUnitController().player.transform.position.x, (int)GetUnitController().player.transform.position.y);
    }

    /// <summary>
    /// Generate a map to all dropped gold
    /// </summary>
    static public void GenerateToGoldMap()
    {
        List<Vector2> goldPositions = new List<Vector2>();
        foreach (Gold g in Gold.goldList)
        {
            if (g.dropped)
            {
                goldPositions.Add(new Vector2(g.transform.position.x, g.transform.position.y));
            }
        }

        toGoldMap = Dijkstra(goldPositions);
    }
    /// <summary>
    /// Generate a map that flees from the player.
    /// </summary>
    static public void GenerateFleePlayerMap()
    {
        fleePlayerMap = GenerateFleeMap((int)GetUnitController().player.transform.position.x, (int)GetUnitController().player.transform.position.y);
    }

    //Generate A flees map running from x,y
    public static Node[,] GenerateFleeMap(int x, int y)
    {
        //Make a vector and place it into a list so we can pass it to the overloaded function
        Vector2 v = new Vector2(GetUnitController().player.transform.position.x, GetUnitController().player.transform.position.y);
        List<Vector2> vList = new List<Vector2>();
        vList.Add(v);
        //call the overloaded function
        return GenerateFleeMap(vList);
    }

    //Generate a Flee map for running away from startList
    static Node[,] GenerateFleeMap(List<Vector2> startList)
    {
        Node[,] newMap = Dijkstra(startList, GenerateCostMap(CostMapType.distance));

        newMap = MapMultiply(newMap, -1.3f);

        float[,] newWalkingCostMap = GenerateCostMap(CostMapType.walking);

        newMap = MapCostAdd(newMap, newWalkingCostMap);
        //newMap = MapFix(newMap);
        //newMap = MapFlip(newMap, -1.25f);

        newMap = SetUpMap(newMap);
        return Dijkstra(new List<Vector2>(), newWalkingCostMap, newMap);
    }

    static Node[,] SetUpMap(Node[,] initialNodeList)
    {
        //MapController mapController = GameController.mapC;
        Vector2 screenResInUnits = GetGameController().ScreenResInUnits;

        //Make an array of nodes for the map
        Node[,] nodeArray = MapCopy(initialNodeList);

        unvisitedList = new List<Node>();

        for (int i = 0; i < screenResInUnits.x; i++)
        {
            for (int j = 0; j < screenResInUnits.y; j++)
            {
                //If this is a blocking object, don't add it to the unvisited list
                if (costMap[i, j] == float.MaxValue)
                {
                    nodeArray[i, j].visited = true;
                }
                //If this is a starting node, put it at the beginning of the unvisited list and give it 0 distance.
                else
                {
                    nodeArray[i, j].visited = false;
                    SortNode(unvisitedList, nodeArray[i, j]);
                }
            }
        }

        return nodeArray;
    }

    // Dijkstra(int x, int y) Generates a Djikstra map leading to x, y
    public static Node[,] Dijkstra(int x, int y, float[,] costMapParam = null, Node[,] initialMap = null)
    {
        //Make a vector and place it into a list so we can pass it to the overloaded function
        Vector2 v = new Vector2(GetUnitController().player.transform.position.x, GetUnitController().player.transform.position.y);
        List<Vector2> vList = new List<Vector2>();
        vList.Add(v);
        //call the overloaded function
        return Dijkstra(vList, costMapParam, initialMap);
    }

    // Dijkstra(List<Vector2> startList) Generates a Djikstra map leading to each of the points in startList.
    public static Node[,] Dijkstra(List<Vector2> startList, float[,]costMapParam = null, Node[,]initialMap = null)
    {
        //Get a cost array from the current map. In the future, this will be 
        //called when a wall is broken or the map otherwise changes
        if (costMapParam == null) costMap = GenerateCostMap();
        else costMap = costMapParam;

        if (initialMap == null) initialMap = GeneratInitialMap(startList);

        //Make an array of nodes for the map
        Node[,] nodeArray = initialMap;




        //At this point we a 2d array of nodes where starting nodes 
        //have 0 distance and other nodes have infinite distance;

        //we have an unvisitedList that contains all unvisited nodes
        //It must remain sorted by node distance.

        //blocking objects (walls) are considered already visited and are not
        //added to the unvisted list.


        //MapController mapController = GameController.mapC;
        while (unvisitedList.Count > 0)
        {
            //Get the lowest distance node.
            Node node = unvisitedList[0];

            //Try to Calculate each of its neighbors
            //LEFT
            if (node.pos.x > 0)
            {
                Node leftNode = nodeArray[(int)node.pos.x - 1, (int)node.pos.y];
                if (leftNode.visited == false)
                {
                    // If left neighbor has not been visited, calculate the node
                    Calculate(node.distance, leftNode, costMap, unvisitedList, false);
                }
            }
            //RIGHT
            if (node.pos.x < GetGameController().ScreenResInUnits.x -1)
            {
                Node rightNode = nodeArray[(int)node.pos.x + 1, (int)node.pos.y];
                if (rightNode.visited == false)
                {
                    // If right neighbor has not been visited, calculate the node
                    Calculate(node.distance, rightNode, costMap, unvisitedList, false);
                }
            }
            //UP
            if (node.pos.y < GetGameController().ScreenResInUnits.y -1)
            {
                Node upNode = nodeArray[(int)node.pos.x, (int)node.pos.y + 1];
                if (upNode.visited == false)
                {
                    // If up neighbor has not been visited, calculate the node
                    Calculate(node.distance, upNode, costMap, unvisitedList, false);
                }
            }
            //DOWN
            if (node.pos.y > 0)
            {
                Node downNode = nodeArray[(int)node.pos.x, (int)node.pos.y - 1];
                if (downNode.visited == false)
                {
                    // If down neighbor has not been visited, calculate the node
                    Calculate(node.distance, downNode, costMap, unvisitedList, false);
                }
            }

            //UP LEFT
            if (node.pos.y < GetGameController().ScreenResInUnits.y - 1 && node.pos.x > 0)
            {
                Node upLeftNode = nodeArray[(int)node.pos.x - 1, (int)node.pos.y + 1];
                if (upLeftNode.visited == false)
                {
                    // If left neighbor has not been visited, calculate the node
                    Calculate(node.distance, upLeftNode, costMap, unvisitedList, true);
                }
            }
            //UP RIGHT
            if (node.pos.y < GetGameController().ScreenResInUnits.y - 1 && node.pos.x < GetGameController().ScreenResInUnits.x - 1)
            {
                Node upRightNode = nodeArray[(int)node.pos.x + 1, (int)node.pos.y + 1];
                if (upRightNode.visited == false)
                {
                    // If right neighbor has not been visited, calculate the node
                    Calculate(node.distance, upRightNode, costMap, unvisitedList, true);
                }
            }
            //DOWN LEFT
            if (node.pos.y > 0 && node.pos.x > 0)
            {
                Node downLeftNode = nodeArray[(int)node.pos.x - 1, (int)node.pos.y - 1];
                if (downLeftNode.visited == false)
                {
                    // If up neighbor has not been visited, calculate the node
                    Calculate(node.distance, downLeftNode, costMap, unvisitedList, true);
                }
            }
            //DOWN RIGHT
            if (node.pos.y > 0 && node.pos.x < GetGameController().ScreenResInUnits.x - 1)
            {
                Node downRightNode = nodeArray[(int)node.pos.x + 1, (int)node.pos.y - 1];
                if (downRightNode.visited == false)
                {
                    // If down neighbor has not been visited, calculate the node
                    Calculate(node.distance, downRightNode, costMap, unvisitedList, true);
                }
            }

            //This node has been visited.
            node.visited = true;
            unvisitedList.Remove(node);
        }

        //Print out the 2d node array distances
        if (GetGameController() && GetGameController().debug)
        {
            for (int i = 0; i < GetGameController().ScreenResInUnits.x; i++)
            {
                String s = "";
                for (int j = 0; j < GetGameController().ScreenResInUnits.y; j++)
                {
                    s += nodeArray[i, j].distance.ToString() + "\t";

                }
                Debug.Log(s);
            }
        }
        
        //return our playermap
        return nodeArray;
    }

    //Calculate the distance for a neighboring node
    static void Calculate(float baseDistance, Node neighborNode, float[,] costArray, List<Node> unvisitedList, bool diagonal = false)
    {

        if (costArray[(int)neighborNode.pos.x, (int)neighborNode.pos.y] == float.MaxValue || baseDistance == float.MaxValue)
        {
            return;
        }

        float newDistance = 1;
        newDistance *= (diagonal) ? 1.414f : 1f;
        newDistance += costArray[(int)neighborNode.pos.x, (int)neighborNode.pos.y];
        newDistance += baseDistance;
        
        // ^^ Do we add the cost before or after multiplying for diagonals??????

        if (costArray[(int)neighborNode.pos.x, (int)neighborNode.pos.y] != float.MaxValue && newDistance < neighborNode.distance)
        {
            neighborNode.distance = newDistance;


            SortNode(unvisitedList, neighborNode);
        }
    }





    public static Node[,] MapMultiply(Node[,] map, float coefficient)
    {
        //Map
        Node[,] newMap = MapCopy(map);

        for (int i = 0; i < GetGameController().ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GetGameController().ScreenResInUnits.y; j++)
            {
                if(newMap[i, j].distance != float.MaxValue)
                {
                    newMap[i, j].distance *= coefficient;
                }
            }
        }

        return newMap;
    }

    public static Node[,] MapAdd(Node[,] map, float a)
    {
        //Map
        Node[,] newMap = MapCopy(map);

        for (int i = 0; i < GetGameController().ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GetGameController().ScreenResInUnits.y; j++)
            {
                if (newMap[i, j].distance != float.MaxValue)
                {
                    newMap[i, j].distance += a;
                }
            }
        }

        return newMap;
    }

    public static Node[,] MapAdd(Node[,] map1, Node[,] map2)
    {
        //Map
        Node[,] newMap = MapCopy(map1);

        for (int i = 0; i < GetGameController().ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GetGameController().ScreenResInUnits.y; j++)
            {
                if (newMap[i, j].distance != float.MaxValue && map2[i, j].distance != float.MaxValue)
                {
                    newMap[i, j].distance += map2[i, j].distance;
                }
            }
        }

        return newMap;
    }

    private static Node[,] MapFlip(Node[,] map, float coefficient)
    {

        if(coefficient > -1)
        {
            coefficient = -1;
        }

        //Map
        Node[,] newMap = MapCopy(map);

        newMap = MapMultiply(newMap, coefficient);

        for (int i = 0; i < GetGameController().ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GetGameController().ScreenResInUnits.y; j++)
            {
                if (newMap[i, j].distance != float.MaxValue)
                {
                    newMap[i, j].distance += (costMap[i, j] * -coefficient);
                    newMap[i, j].distance += costMap[i, j];
                }
            }
        }

        return newMap;
    }

    private static Node[,] MapCostAdd(Node[,] map, float[,] costMapParam)
    {

        //Map
        Node[,] newMap = MapCopy(map);

        for (int i = 0; i < GetGameController().ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GetGameController().ScreenResInUnits.y; j++)
            {
                if (newMap[i, j].distance != float.MaxValue)
                {
                    newMap[i, j].distance += costMapParam[i,j];
                }
            }
        }

        return newMap;
    }

    //After you multiply the map by a negative number, readd twice the cost of each cell to "negate" everything except the cost.
    private static Node[,] MapFix(Node[,] map)
    {
        //Map
        Node[,] newMap = MapCopy(map);

        for (int i = 0; i < GetGameController().ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GetGameController().ScreenResInUnits.y; j++)
            {
                if (newMap[i, j].distance != float.MaxValue)
                {
                    newMap[i, j].distance += 2*costMap[i,j];
                }
            }
        }

        return newMap;
    }

    private static Node[,] MapCopy(Node[,] map)
    { 

        Node[,] newMap = new Node[map.GetLength(0), map.GetLength(1)];

        for (int i = 0; i < GetGameController().ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GetGameController().ScreenResInUnits.y; j++)
            {
                newMap[i, j] = new Node((int)map[i, j].pos.x, (int)map[i, j].pos.y, map[i, j].distance, map[i, j].visited);
            }
        }
        return newMap;
    }

    //Places a node into the list in the correct spot according to distance.
    private static void SortNode(List<Node> list, Node node)
    {
        list.Remove(node);
        var index = list.BinarySearch(node);
        if (index < 0) index = ~index;
        list.Insert(index, node);
    }


    public enum CostMapType{ distance, flying, walking, burrowing };
    public static float[,] GenerateCostMap(CostMapType costMapType = CostMapType.walking)
    {
        //MapController mapController = GameController.mapC;

        float[,] costArray = new float[(int)GetGameController().ScreenResInUnits.x, (int)GetGameController().ScreenResInUnits.y];

        //Loop through all nodes
        for (int i = 0; i < GetGameController().ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GetGameController().ScreenResInUnits.y; j++)
            {
                Wall2 thisWall = GetWallController().GetWall(new Vector2Int(i, j));
                //We pathfind differently depending on what costmaptype we are using (i.e. flying creatures ignore floors)
                switch (costMapType)
                {

                    case CostMapType.walking:
                        //if there is a wall set the node cost its cost
                        if (thisWall != null)
                        {
                            costArray[i, j] = thisWall.GetPathfindingCost();
                        }
                        //otherwise no cost
                        else
                        {
                            costArray[i, j] = 0;
                        }
                        break;
                    case CostMapType.distance:
                        //if there is a wall set the node cost to max value
                        if (thisWall != null)
                        {
                            if(thisWall.GetPathfindingCost() != float.MaxValue)
                            {
                                costArray[i, j] = 0;
                            }
                            else
                            {
                                costArray[i, j] = float.MaxValue;
                            }
                        }
                        //otherwise no cost
                        else
                        {
                            costArray[i, j] = 0;
                        }
                        break;
                    case CostMapType.flying:
                        Debug.Log("flying is unimplimented behavior");
                        break;
                    case CostMapType.burrowing:
                        Debug.Log("burrowing is unimplimented behavior");
                        break;
                }
            }
        }
        costMap = costArray;
        return costArray;
    }

    public static Path GeneratePath()
    {
        return new Path();
    }
}



public class Path
{
    private List<Node> nodes = new List<Node>();
    

}

public class Node : IComparable
{
    public Vector2 pos;
    public float distance;
    public bool visited = false;

    public Node(int x, int y, float d, bool v = false)
    {
        pos = new Vector2(x, y);
        distance = d;
        visited = v;
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        Node node = obj as Node;
        if (node.distance > distance)
        {
            return -1;
        }
        else if (node.distance < distance)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}

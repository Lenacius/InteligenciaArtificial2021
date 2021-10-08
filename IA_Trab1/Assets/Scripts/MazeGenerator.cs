using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Node
{
    public List<Node> neighbor = new List<Node>();
    public int value = 0;
    public bool visited = false;
    public Vector2Int position = new Vector2Int(0, 0);

    public Node(int newValue, Vector2Int newPosition)
    {
        value = newValue;
        position = newPosition;
    }
    public void AddNeighbor(Node newNeighbor)
    {
        if (newNeighbor.value <= 0)
            neighbor.Add(newNeighbor);
    }
}

public class Graph
{
    public List<Node> nodeList = new List<Node>();
    public Node SNode = null;
    public Node ENode = null;

    public void BuildGraph(int width, int depth)
    {
        for(int y = 0; y < depth; y++)
        {
            for(int x = 0; x < width; x++)
            {
                //x+1
                if (x + 1 < width)
                    nodeList[x + y * width].AddNeighbor(nodeList[(x+1) + y * width]);
                //y-1
                if (y - 1 >= 0)
                    nodeList[x + y * width].AddNeighbor(nodeList[x + (y-1) * width]);
                //x-1
                if (x - 1 >= 0)
                    nodeList[x + y * width].AddNeighbor(nodeList[(x - 1) + y * width]);
                //y+1
                if (y + 1 < depth)
                    nodeList[x + y * width].AddNeighbor(nodeList[x + (y+1) * width]);
                //if (x - 1 >= 0 && x + 1 < width && y - 1 >= 0 && y + 1 < depth)
            }
        }
    }
    public bool Explore(int[,] maze, Node current) {
        Node path;
        if (current == null)
            path = SNode;
        else
            path = current;

        path.visited = true;
        for(int x = 0; x < path.neighbor.Count; x++)
        {
            Node next = path.neighbor[x];
            if (next == ENode)
            {
                next.value = path.value - 1;
                maze[next.position.x, next.position.y] = path.value - 1;
                return true;
            }
            else if (!next.visited)
            {
                next.value = path.value - 1;
                maze[next.position.x, next.position.y] = path.value - 1;
                Explore(maze, next);
            }
        }
        return false;
    }
    public void FindLines(Vector2Int start, Vector2Int end)
    {
        FindStart(start);
        FindEnd(end);
    }
    void FindStart(Vector2Int start)
    {
        for (int x = 0; x < nodeList.Count; x++)
        {
            if (SNode == null)
            {
                if (nodeList[x].position == start)
                {
                    SNode = nodeList[x];
                    break;
                }
            }
        }
    }
    void FindEnd(Vector2Int end)
    {
        for (int x = 0; x < nodeList.Count; x++)
        {
            if (ENode == null)
            {
                if (nodeList[x].position == end)
                {
                    ENode = nodeList[x];
                    break;
                }
            }
        }
    }
}

public class MazeGenerator : MonoBehaviour
{
    int mazeWidth = 22;
    int mazeDepth = 19;

    int[,] maze = new int[19, 22] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                    { 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                                    { 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1 },
                                    { 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
                                    { 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1 },
                                    { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1 },
                                    { 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                                    { 1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1 },
                                    { 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1 },
                                    { 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1 },
                                    { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1 },
                                    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 0, 1, 1, 1, 1 },
                                    { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
                                    { 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1 },
                                    { 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1 },
                                    { 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 1 },
                                    { 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1 },
                                    { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1 },
                                    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } };

    public List<GameObject> tileset = new List<GameObject>();

    float tileWidth = 3.0f;
    float tileDepth = 3.0f;

    float xi = -25.0f;
    float zi = 25.0f;

    public GameObject Character;

    public Vector2Int StartPosition;
    public Vector2Int EndPosition;

    Graph mazeGraph = new Graph();

    public List<Node> path = new List<Node>();
    public int pathStep = 0;
    public float timer = 3.0f;
    public float timerSpeed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze();
        //printNodes();
        mazeGraph.BuildGraph(mazeWidth, mazeDepth);
        //printNodes();
        mazeGraph.FindLines(StartPosition, EndPosition);
        mazeGraph.Explore(maze, null);
        var sr = File.CreateText("MyFile");
        for (int i = 0; i < mazeDepth; i++) //z
        {
            for (int j = 0; j < mazeWidth; j++) //x
            {
                sr.Write(maze[i, j] + ";");
            }
            sr.WriteLine();
        }
        sr.Close();
        redoMaze();
        //printNodes();
        FindPath(null);
        Character.transform.position = new Vector3(xi + StartPosition.x * tileWidth, 3.0f, zi - StartPosition.y * tileDepth);
    }

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.Space))
        if(timer <= 0)
        {
            timer = timerSpeed;
            if(path.Count != pathStep)
                Move();
        }
            //Move();

    }

    void GenerateMaze()
    {
        for (int i = 0; i < mazeDepth; i++) //z
        {
            for (int j = 0; j < mazeWidth; j++) //x
            {
                //VisualMaze
                GameObject tilePrefab = tileset[maze[i, j]];
                Vector3 p = tilePrefab.transform.position;
                p.x = xi + j * tileWidth;
                p.z = zi - i * tileDepth;

                GameObject newTile = Instantiate(tilePrefab, p, Quaternion.identity) as GameObject;
                newTile.transform.parent = gameObject.transform;
                if (maze[i, j] == 1)
                {
                    newTile.GetComponent<Renderer>().material.color = Color.black;
                    newTile.transform.position += new Vector3(0, 3.0f, 0);
                }
                if(i == EndPosition.x && j == EndPosition.y)
                {
                    newTile.GetComponent<Renderer>().material.color = Color.yellow;
                }

                //MazeGraph
                Node newNode = new Node(maze[i,j], new Vector2Int(i, j));
                mazeGraph.nodeList.Add(newNode);
            }
        }
    }
    
    void printNodes()
    {
        for (int x = 0; x < mazeWidth * mazeDepth; x++)
        {
            Node auxx = mazeGraph.nodeList[x];
            print(auxx.value + " " + auxx.position);
            //for (int y = 0; y < auxx.neighbor.Count; y++)
            //{
            //    Node auxy = auxx.neighbor[y];
            //    print("--" + auxy.value + " " + auxy.position);
            //}
        }
    }

    void redoMaze()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < mazeDepth; i++) //z
        {
            for (int j = 0; j < mazeWidth; j++) //x
            {
                //VisualMaze
                GameObject tilePrefab = tileset[0];
                Vector3 p = tilePrefab.transform.position;
                p.x = xi + j * tileWidth;
                p.z = zi - i * tileDepth;

                GameObject newTile = Instantiate(tilePrefab, p, Quaternion.identity) as GameObject;
                newTile.transform.parent = gameObject.transform;
                if (maze[i, j] == 1)
                {
                    newTile.GetComponent<Renderer>().material.color = Color.black;
                    newTile.transform.position += new Vector3(0, 3.0f, 0);
                }
                else
                {
                    newTile.GetComponent<Renderer>().material.color = new Color(1.0f - (mazeGraph.nodeList[j + i * mazeWidth].value * -1) / 100.0f,
                                                                                0.0f,
                                                                                0.0f);
                }

                if (i == EndPosition.x && j == EndPosition.y)
                {
                    newTile.GetComponent<Renderer>().material.color = Color.yellow;
                }
            }
        }


    }

    void FindPath(Node end)
    {
        if (end == null)
            end = mazeGraph.ENode;

        path.Add(end);

        print(end.value + "|" + end.position);
        for(int x = 0; x < end.neighbor.Count; x++)
        {
            if (end.neighbor[x].value == 0)
            {
                print("FIM");
                break;
            }

            if (end.neighbor[x].value > end.value)
                FindPath(end.neighbor[x]);
        }
    }
    
    void Move()
    {
        pathStep++;
        Character.transform.position = new Vector3(xi + path[path.Count - pathStep].position.y * tileWidth, 3.0f,zi - path[path.Count - pathStep].position.x * tileDepth);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Swift_Blade.Level.Door;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum NodeType
{
    //default
    Exp,
    
    //event
    Event,
    
    //level Up
    Point,
    Challenge,
    Store,
    
    //boss
    Boss,
    
    None,
}

[Serializable]
public class Node
{
    public NodeType nodeType;
    public string nodeName;
    private Door doorPrefab;

    public void SetPortalPrefab(Door prefab)
    {
        doorPrefab = prefab;
    }

    public Door GetPortalPrefab()
    {
        return doorPrefab;
    }
}

[Serializable]
public class NodeDictionary : IEnumerable<Node>
{
    private Dictionary<NodeType, List<Node>> nodeList;
    
    public NodeDictionary(Node[] nodes)
    {
        nodeList = new Dictionary<NodeType, List<Node>>();

        foreach (var item in nodes)
        {
            if (!nodeList.ContainsKey(item.nodeType))
            {
                nodeList[item.nodeType] = new List<Node>();
            }
            
            nodeList[item.nodeType].Add(item);
        }
    }

    public string this[NodeType type] => nodeList[type][Random.Range(0 , nodeList[type].Count)].nodeName;
    
    private bool IsValidScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
            return false;
        
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string scenePath = System.IO.Path.GetFileNameWithoutExtension(path);
                        
            if (scenePath == sceneName)
                return true;
        }
        return false;
    }

    public Node GetRandomNode(NodeType nodeType)
    {
        if (nodeList.ContainsKey(nodeType) && nodeList[nodeType].Count > 0)
        {
            List<Node> nodes = nodeList[nodeType];

            if (nodes.Count == 1 && nodes[0].nodeName == SceneManager.GetActiveScene().name)
            {
                return nodes[0];
            }
            
            Node selectedNode;
            do
            {
                selectedNode = nodes[Random.Range(0, nodes.Count)];
            } while (selectedNode.nodeName == SceneManager.GetActiveScene().name);
                
            if (IsValidScene(selectedNode.nodeName))
                return selectedNode;

            Debug.LogError($"{selectedNode.nodeName}는 sceneList에 없습니다!");
            return null;
        }

        Debug.LogError("유효하지 않은 Node에 접근하고 있습니다.");
        return null;
    }

    public List<NodeType> GetNodeTypes(int currentNodeIndex)
    {
        List<NodeType> nodes = new List<NodeType>();
        
        if (currentNodeIndex % 3 == 0)
        {
            nodes.Add(NodeType.Boss);
        }
        else if (currentNodeIndex % 2 == 0)
        {
            nodes.Add(NodeType.Point);
            nodes.Add(NodeType.Store);
            nodes.Add(NodeType.Challenge);
        }
        else
        {
            int random = Random.Range(0,100);
            if (random < 40)
                nodes.Add(NodeType.Event);            
            
            nodes.Add(NodeType.Exp);            
        }
        
        return nodes;
    }

    public IEnumerator<Node> GetEnumerator()
    {
        foreach (var item  in nodeList)
        {
            foreach (var item2 in item.Value)
            {
                yield return item2;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

namespace Swift_Blade.Level
{
    [CreateAssetMenu(fileName = "NodeList", menuName = "SO/Scene/NodeList")]
    public class NodeList : ScriptableObject
    {
        [SerializeField] private Node[] nodelist;
        private NodeDictionary nodeDictionary;
        
        [SerializeField] private Door.Door expDoor;
        [SerializeField] private Door.Door eventDoor;
        [SerializeField] private Door.Door storeDoor;
        [SerializeField] private Door.Door pointDoor;
        [SerializeField] private Door.Door challengeDoor;
        [SerializeField] private Door.Door bossDoor;
        
        [Header("Chest")]
        [SerializeField] private Chest chest;
        
        private int currentNodeIndex = 0;
         
        private void OnEnable()
        {
            currentNodeIndex = 0;
            nodeDictionary = new NodeDictionary(nodelist);

            foreach (var item in nodeDictionary)
            {
                switch (item.nodeType)
                {
                    case NodeType.Exp:
                        item.SetPortalPrefab(expDoor);
                        break;
                    case NodeType.Event:
                        item.SetPortalPrefab(eventDoor);
                        break;
                    case NodeType.Point:
                        item.SetPortalPrefab(pointDoor);
                        break;
                    case NodeType.Challenge:
                        item.SetPortalPrefab(challengeDoor);
                        break;
                    case NodeType.Store:
                        item.SetPortalPrefab(storeDoor);
                        break;
                    case NodeType.Boss:
                        item.SetPortalPrefab(bossDoor);
                        break;
                    case NodeType.None:
                        break;
                }
            }
            
        }
        
        public Node[] GetNode()
        {
            List<NodeType> nodeTypes = nodeDictionary.GetNodeTypes(++currentNodeIndex);
            
            Node[] nodes = new Node[nodeTypes.Count];
            
            for (int i = 0; i < nodeTypes.Count; i++)
            {
                nodes[i] = nodeDictionary.GetRandomNode(nodeTypes[i]);
            }

            return nodes;
        }

        public string GetNodeName(NodeType nodeType)
        {
            return nodeDictionary[nodeType];
        }

        public Chest GetChest()
        {
            return chest;
        }
    }
}

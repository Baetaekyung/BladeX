using System.Collections.Generic;
using Swift_Blade.Level.Door;
using System.Collections;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine;
using System;
using System.Linq;

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
    
    Rest,
    
    None,
}

[Serializable]
public class Node
{
    public NodeType nodeType;
    public string nodeName;
    private Door doorPrefab;
    private byte appearCount = 0;
    
    public void SetPortalPrefab(Door prefab)
    {
        doorPrefab = prefab;
    }
    
    public Door GetPortalPrefab()
    {
        ++appearCount;
        return doorPrefab;
    }
    
    public byte GetAppearCount() {return appearCount;}
}

[Serializable]
public class NodeDictionary : IEnumerable<Node>
{
    private Dictionary<NodeType, List<Node>> nodeList;
    private bool canAppearSpecialNode = true;
    private const byte APPEAR_SPECIAL_NODE_PERCENT = 16;//100 / 6 = 16.xxx
    
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
        Debug.LogError("Scene List에 Scene 없습니다.");
        return false;
    }
    
    public Node GetRandomNode(NodeType nodeType)
    {
        if (nodeList.ContainsKey(nodeType) && nodeList[nodeType].Count > 0)
        {
            List<Node> nodes = nodeList[nodeType];
            
            nodes = nodes.OrderBy(x => x.GetAppearCount()).ToList();
            int minValue = nodes[0].GetAppearCount();
            nodes.RemoveAll(x => x.GetAppearCount() > minValue); 
            
            if (nodes.Count == 1 && nodes[0].nodeName == SceneManager.GetActiveScene().name)
            {
                return nodes[0];
            }
            
            Node selectedNode  = nodes[Random.Range(0, nodes.Count)];
                            
            if (IsValidScene(selectedNode.nodeName))
                return selectedNode;
            
            Debug.LogError($"{selectedNode.nodeName}은(는) sceneList에 존재하지 않습니다!");
            return null;
        }
        
        Debug.LogError("유효하지 않은 NodeType이거나, 해당 타입의 Node가 존재하지 않습니다.");
        return null;
    }
    
    public List<NodeType> GetNodeTypes(int currentNodeIndex)
    {
        List<NodeType> nodeTypes = new List<NodeType>();
        
        if (currentNodeIndex % 6 == 0)
        {
            nodeTypes.Add(NodeType.Boss);
        }
        else if (canAppearSpecialNode && Random.Range(0,100) < currentNodeIndex * APPEAR_SPECIAL_NODE_PERCENT)
        {
            canAppearSpecialNode = false;
            int randomNode =  Random.Range(0, 4);
            switch (randomNode)
            {
                case 1:nodeTypes.Add(NodeType.Point);
                    break;
                case 2 : nodeTypes.Add(NodeType.Store);
                    break;
                case 3 : nodeTypes.Add(NodeType.Challenge);
                    break;
            }
        }
        else
        {
            nodeTypes.Add(NodeType.Exp);         
        }
        
        return nodeTypes;
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

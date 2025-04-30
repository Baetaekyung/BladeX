using System.Collections.Generic;
using Swift_Blade.Level.Door;
using System.Collections;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine.Serialization;

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
public class NodeDictionary : IEnumerable<List<Node>>
{
    private Dictionary<NodeType, List<Node>> nodeList;

    private List<NodeType> specialNodeTypes;
    
    private bool canFirstAppearSpecialNode = true;
    private bool canSecondAppearSpecialNode = true;
    
    private const byte APPEAR_SPECIAL_NODE_PERCENT = 16;//100 / 6 = 16.xxx
    
    public NodeDictionary(Node[] nodes)
    {
        nodeList = new Dictionary<NodeType, List<Node>>();
        specialNodeTypes = new List<NodeType>();
        InitSpecialNodes();
        
        foreach (var item in nodes)
        {
            if (!nodeList.ContainsKey(item.nodeType))
            {
                nodeList[item.nodeType] = new List<Node>();
            }
            
            nodeList[item.nodeType].Add(item);
        }
    }
    
    #region Priavte
    private bool CanAppearSpecialNode() => canSecondAppearSpecialNode || canFirstAppearSpecialNode;
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
        
    private Node SelectRandomNode(List<Node> nodes)
    {
        nodes = nodes.OrderBy(x => x.GetAppearCount()).ToList();
        int minValue = nodes[0].GetAppearCount();
        nodes.RemoveAll(x => x.GetAppearCount() > minValue); 
        
        if (nodes.Count == 1 && nodes[0].nodeName == SceneManager.GetActiveScene().name)
        {
            return nodes[0];
        }
        
        return nodes[Random.Range(0, nodes.Count)];
    }

    #endregion

    #region Public

    public void Add(Node newNode)
    {
        nodeList[newNode.nodeType].Add(newNode);
    }
    
    public string this[NodeType type] => nodeList[type][Random.Range(0 , nodeList[type].Count)].nodeName;
    
    public void InitSpecialNodes()
    {
        specialNodeTypes.Clear();
        
        specialNodeTypes.Add(NodeType.Challenge);
        specialNodeTypes.Add(NodeType.Point);
        specialNodeTypes.Add(NodeType.Event);
        specialNodeTypes.Add(NodeType.Store);
    }
    
    public Node GetRandomNode(NodeType nodeType)
    {
        if (nodeList.ContainsKey(nodeType) && nodeList[nodeType].Count > 0)
        {
            List<Node> nodes = nodeList[nodeType];
            Node selectedNode = SelectRandomNode(nodes);
                                        
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
        
        if (currentNodeIndex % 7 == 0)
        {
            canFirstAppearSpecialNode = true;
            canSecondAppearSpecialNode = true;
            
            nodeTypes.Add(NodeType.Rest);
        }
        else if (currentNodeIndex % 6 == 0)
        {
            nodeTypes.Add(NodeType.Boss);
        }
        else if (CanAppearSpecialNode() && Random.Range(0,100) < currentNodeIndex * APPEAR_SPECIAL_NODE_PERCENT)
        {
            if (canFirstAppearSpecialNode)
                canFirstAppearSpecialNode = false;
            else
                canSecondAppearSpecialNode = false;
            
            
            NodeType nodeType = specialNodeTypes[Random.Range(0, specialNodeTypes.Count)];
            specialNodeTypes.Remove(nodeType);
                        
            nodeTypes.Add(nodeType);
            nodeTypes.Add(NodeType.Exp);         
        }
        else
        {
            nodeTypes.Add(NodeType.Exp);         
        }
                
        return nodeTypes;
    }
        
    public IEnumerator<List<Node>> GetEnumerator()
    {
        return nodeList.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
    
}

namespace Swift_Blade.Level
{
    [CreateAssetMenu(fileName = "NodeList", menuName = "SO/Scene/NodeList")]
    public class NodeList : ScriptableObject
    {
        int stageCount = 0;
        
        [SerializeField] private Node[] nodelist;
        private NodeDictionary nodeDictionary;
        
        [SerializeField] private Door.Door expDoor;
        [SerializeField] private Door.Door eventDoor;
        [SerializeField] private Door.Door storeDoor;
        [SerializeField] private Door.Door pointDoor;
        [SerializeField] private Door.Door challengeDoor;
        [SerializeField] private Door.Door bossDoor;
        [SerializeField] private Door.Door restDoor;
        
        private int currentNodeIndex = 0;

        private readonly StringBuilder stageName = new StringBuilder();
        
        public void RestNodeIndex()
        {
            nodeDictionary.InitSpecialNodes();
            currentNodeIndex = 0;
        }
                
        private void OnEnable()
        {
            nodeDictionary = new NodeDictionary(nodelist);
            RestNodeIndex();
            
            stageCount = 0;
            stageName.Clear();
            stageName.Append("Stage_");
            
            foreach (var node in nodeDictionary)
            {
                foreach (var item in node)
                {
                    AssignDoor(item);
                }
            }
        }
        
        private void AssignDoor(Node item)
        {
           
            switch (item.nodeType)
            {
                case NodeType.Exp:
                    ++stageCount;
                    item.nodeName = stageName.Append(stageCount).ToString();
                    item.SetPortalPrefab(expDoor);
                    stageName.Remove(stageName.Length - stageCount.ToString().Length, stageCount.ToString().Length);
                    
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
                case NodeType.Rest:
                    item.SetPortalPrefab(restDoor);
                    break;
                case NodeType.None:
                    break;
            }
        }
        
        public Node[] GetNode()
        {
            List<NodeType> nodeTypes = nodeDictionary.GetNodeTypes(++currentNodeIndex);
            Node[] nodes = new Node[nodeTypes.Count];
            
            ++currentNodeIndex;
            
            for (int i = 0; i < nodeTypes.Count; i++)
            {
                nodes[i] = nodeDictionary.GetRandomNode(nodeTypes[i]);
                //Debug.Log($"Next Door is {nodes[i]}");
            }
            
            return nodes;
        }

        public string GetNodeName(NodeType nodeType)
        {
           
            return nodeDictionary[nodeType];
        }
               
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
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
public struct Node
{
    public NodeType nodeType;
    public string nodeName;
}

[Serializable]
public class NodeDictionary
{
    private Dictionary<NodeType, List<string>> nodeList;

    public NodeDictionary(Node[] nodes)
    {
        nodeList = new Dictionary<NodeType, List<string>>();

        foreach (var item in nodes)
        {
            if (!nodeList.ContainsKey(item.nodeType))
            {
                nodeList[item.nodeType] = new List<string>();
            }
            
            nodeList[item.nodeType].Add(item.nodeName);
        }
    }

    public string this[NodeType nodeType]
    {
        get
        {
            if (nodeList.ContainsKey(nodeType))
            {
                return nodeList[nodeType][Random.Range(0, nodeList[nodeType].Count)];
            }
            Debug.LogError("유효하지 않은 Node에 접근하고 있습니다.");
            return string.Empty;
        }
        set
        {
            if (nodeList.ContainsKey(nodeType))
            {
                nodeList[nodeType].Add(value);
            }
            else
            {
                nodeList[nodeType] = new List<string> { value };
            }
        }
    }
}

namespace Swift_Blade.Level
{
    [CreateAssetMenu(fileName = "NodeList", menuName = "SO/Scene/NodeList")]
    public class NodeList : ScriptableObject
    {
        [SerializeField] private Node[] nodelist;
        private NodeDictionary nodeDictionary;
        
        private void OnEnable()
        {
            nodeDictionary = new NodeDictionary(nodelist);
        }
        
        public string GetNode(NodeType nodeType)
        {
            //get random node            
            return nodeDictionary[nodeType];
        }
        
    }
}

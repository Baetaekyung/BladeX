using System;
using System.Collections.Generic;
using Swift_Blade.Level.Portal;
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
    Challange,
    Store,
    
    //boss
    Boss,
    
    None,
}

[Serializable]
public struct Node
{
    public NodeType nodeType;
    public Portal portalPrefab;
    public string nodeName;
}

[Serializable]
public class NodeDictionary
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

            Debug.LogError("sceneName이 sceneList에 없습니다!");
            return default;
        }

        Debug.LogError("유효하지 않은 Node에 접근하고 있습니다.");
        return default;
    }

    public List<NodeType> GetNodeTypes(int currentNodeIndex)
    {
        List<NodeType> nodes = new List<NodeType>();

        if (currentNodeIndex >= 10)
        {
            nodes.Add(NodeType.Boss);
        }
        else if (currentNodeIndex >= 5)
        {
            nodes.Add(NodeType.Point);
            nodes.Add(NodeType.Store);
            nodes.Add(NodeType.Challange);
        }
        else
        {
            int random = Random.Range(0,100);
            if (random < 20)
                nodes.Add(NodeType.Event);            
            
            nodes.Add(NodeType.Exp);            
        }
        
        return nodes;
    }
}

namespace Swift_Blade.Level
{
    [CreateAssetMenu(fileName = "NodeList", menuName = "SO/Scene/NodeList")]
    public class NodeList : ScriptableObject
    {
        [SerializeField] private Node[] nodelist;
        private NodeDictionary nodeDictionary;

        private static int CURRENT_NODE_INDEX = 0;
        
        private void OnEnable()
        {
            CURRENT_NODE_INDEX = 0;
            
            nodeDictionary = new NodeDictionary(nodelist);
        }
        
        public Node[] GetNode()
        {
            List<NodeType> nodeTypes = nodeDictionary.GetNodeTypes(CURRENT_NODE_INDEX);
            Node[] nodes = new Node[nodeTypes.Count];
            
            //Debug.Log(nodes.Length);
            
            for (int i = 0; i < nodeTypes.Count; i++)
            {
                nodes[i] = nodeDictionary.GetRandomNode(nodeTypes[i]);
            }

            return nodes;
        }
    }
}

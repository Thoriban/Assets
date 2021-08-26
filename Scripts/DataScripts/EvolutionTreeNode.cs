using System;
using System.Collections.Generic;

[Serializable]
public class EvolutionTreeNode
{
    public string pokemon { get; set; }
    public string methodToReachNode { get; set; }
    public int numRequirementToReachNode { get; set; }
    public EvolutionTreeNode[] evolutionsFromNode { get; set; }
    public List<EvolutionTreeNode> evolutionsFromNodeList { get; set; }

    public EvolutionTreeNode()
    {
        evolutionsFromNode = new EvolutionTreeNode[1];
        evolutionsFromNodeList = new List<EvolutionTreeNode>();
    }

    public EvolutionTreeNode AddNodesToTree(string pokemonName, KeyValuePair<string, string> pokemonEvolution)
    {
        EvolutionTreeNode evolutionTreeNode = new EvolutionTreeNode();

        if (pokemonEvolution.Key == "this")
        {
            evolutionTreeNode.pokemon = pokemonName;
        }
        else
        {
            evolutionTreeNode.pokemon = pokemonEvolution.Key;
        }

        evolutionTreeNode.methodToReachNode = pokemonEvolution.Value;
        evolutionTreeNode.numRequirementToReachNode = 0;
        evolutionTreeNode.evolutionsFromNode = new EvolutionTreeNode[1];
        evolutionTreeNode.evolutionsFromNodeList = new List<EvolutionTreeNode>();

        return evolutionTreeNode;
    }
}

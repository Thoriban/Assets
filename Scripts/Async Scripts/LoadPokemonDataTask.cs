using System.Collections.Generic;

public class LoadPokemonDataTask : ThreadedJob
{
    public string[] columns;
    public string appDataPath;

    protected override void ThreadFunction()
    {
        if (columns[0] != string.Empty)
        {
            Pokemon pokemon = new Pokemon();
            pokemon.name = columns[0];
            pokemon.dexNumber = int.Parse(columns[1]);
            pokemon.generation = "Generation " + columns[2];
            pokemon.evolutionStage = int.Parse(columns[3]);

            pokemon.eggGroup1 = columns[4];
            pokemon.eggGroup2 = columns[5];

            pokemon.type1 = columns[6];
            pokemon.type2 = columns[7];

            //Evolution Tree
            //Baby Stage
            bool hasBabyStage = false;
            pokemon.evolutionTree = GetFirstNode(pokemon.name, columns[8], columns[10], out hasBabyStage);

            if (hasBabyStage)
            {
                Dictionary<string, string> basicPokemon = new Dictionary<string, string>();

                basicPokemon.Add(columns[10], columns[9]);

                if (columns[16] != "")
                {
                    basicPokemon.Add(columns[16], columns[15]);

                    if (columns[18] != "")
                    {
                        basicPokemon.Add(columns[18], columns[17]);
                    }
                }

                foreach (KeyValuePair<string, string> basic in basicPokemon)
                {
                    EvolutionTreeNode basicStageTreeNode = (AddNodesToTree(pokemon.name, basic));

                    Dictionary<string, string> stage1Pokemon = new Dictionary<string, string>();

                    if (columns[12] != "")
                    {
                        stage1Pokemon.Add(columns[12], columns[11]);

                        if (columns[20] != "")
                        {
                            stage1Pokemon.Add(columns[20], columns[19]);

                            if (columns[22] != "")
                            {
                                stage1Pokemon.Add(columns[22], columns[21]);

                                if (columns[24] != "")
                                {
                                    stage1Pokemon.Add(columns[24], columns[23]);

                                    if (columns[26] != "")
                                    {
                                        stage1Pokemon.Add(columns[26], columns[25]);

                                        if (columns[28] != "")
                                        {
                                            stage1Pokemon.Add(columns[28], columns[27]);

                                            if (columns[30] != "")
                                            {
                                                stage1Pokemon.Add(columns[30], columns[29]);

                                                if (columns[32] != "")
                                                {
                                                    stage1Pokemon.Add(columns[32], columns[31]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (KeyValuePair<string, string> stage1 in stage1Pokemon)
                    {
                        EvolutionTreeNode Stage1TreeNode = AddNodesToTree(pokemon.name, stage1);

                        Dictionary<string, string> stage2Pokemon = new Dictionary<string, string>();

                        if (columns[14] != "")
                        {
                            stage2Pokemon.Add(columns[14], columns[13]);

                            if (columns[34] != "")
                            {
                                stage2Pokemon.Add(columns[34], columns[33]);
                            }
                        }

                        foreach (KeyValuePair<string, string> stage2 in stage2Pokemon)
                        {
                            EvolutionTreeNode Stage2TreeNode = AddNodesToTree(pokemon.name, stage2);

                            Stage1TreeNode.evolutionsFromNodeList.Add(Stage2TreeNode);//add to stage 1 node
                        }

                        Stage1TreeNode.evolutionsFromNode = Stage1TreeNode.evolutionsFromNodeList.ToArray();

                        basicStageTreeNode.evolutionsFromNodeList.Add(Stage1TreeNode);//add to basic node
                    }

                    basicStageTreeNode.evolutionsFromNode = basicStageTreeNode.evolutionsFromNodeList.ToArray();

                    pokemon.evolutionTree.evolutionsFromNodeList.Add(basicStageTreeNode);// add to baby node
                }

                pokemon.evolutionTree.evolutionsFromNode = pokemon.evolutionTree.evolutionsFromNodeList.ToArray();
            }
            else
            {
                Dictionary<string, string> stage1Pokemon = new Dictionary<string, string>();

                if (columns[12] != "")
                {
                    stage1Pokemon.Add(columns[12], columns[11]);

                    if (columns[20] != "")
                    {
                        stage1Pokemon.Add(columns[20], columns[19]);

                        if (columns[22] != "")
                        {
                            stage1Pokemon.Add(columns[22], columns[21]);

                            if (columns[24] != "")
                            {
                                stage1Pokemon.Add(columns[24], columns[23]);

                                if (columns[26] != "")
                                {
                                    stage1Pokemon.Add(columns[26], columns[25]);

                                    if (columns[28] != "")
                                    {
                                        stage1Pokemon.Add(columns[28], columns[27]);

                                        if (columns[30] != "")
                                        {
                                            stage1Pokemon.Add(columns[30], columns[29]);

                                            if (columns[32] != "")
                                            {
                                                stage1Pokemon.Add(columns[32], columns[31]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (KeyValuePair<string, string> stage1 in stage1Pokemon) //key = pokemon, value = method
                {
                    EvolutionTreeNode Stage1TreeNode = AddNodesToTree(pokemon.name, stage1);

                    Dictionary<string, string> stage2Pokemon = new Dictionary<string, string>();

                    if (columns[14] != "")
                    {
                        stage2Pokemon.Add(columns[14], columns[13]);

                        if (columns[34] != "")
                        {
                            stage2Pokemon.Add(columns[34], columns[33]);
                        }
                    }

                    foreach (KeyValuePair<string, string> stage2 in stage2Pokemon)
                    {
                        EvolutionTreeNode Stage2TreeNode = AddNodesToTree(pokemon.name, stage2);

                        Stage1TreeNode.evolutionsFromNodeList.Add(Stage2TreeNode);//add to stage 1 node
                    }

                    Stage1TreeNode.evolutionsFromNode = Stage1TreeNode.evolutionsFromNodeList.ToArray();

                    pokemon.evolutionTree.evolutionsFromNodeList.Add(Stage1TreeNode);//add to basic node
                }

                foreach (EvolutionTreeNode node in pokemon.evolutionTree.evolutionsFromNodeList) //Stage 1 array for stage 2 Pokemon
                {
                    node.evolutionsFromNode = node.evolutionsFromNodeList.ToArray();
                }

                pokemon.evolutionTree.evolutionsFromNode = pokemon.evolutionTree.evolutionsFromNodeList.ToArray(); //Basic stage array for stage 1 Pokemon
            }

            pokemon.Save(appDataPath + "/Data/");
        }
    }

    protected override void OnFinished()
    {
        base.OnFinished();
    }

    public EvolutionTreeNode GetFirstNode(string pokemonName, string babyPokemon, string basicPokemon, out bool hasBabyStage)
    {
        EvolutionTreeNode evolutionTreeNode = new EvolutionTreeNode();

        if (babyPokemon != "")
        {
            if (babyPokemon == "this")
            {
                evolutionTreeNode.pokemon = pokemonName;
            }
            else
            {
                evolutionTreeNode.pokemon = babyPokemon;
            }

            hasBabyStage = true;
        }
        else
        {
            if (basicPokemon == "this")
            {
                evolutionTreeNode.pokemon = pokemonName;
            }
            else
            {
                evolutionTreeNode.pokemon = basicPokemon;
            }

            hasBabyStage = false;
        }

        evolutionTreeNode.methodToReachNode = "";
        evolutionTreeNode.numRequirementToReachNode = 0;
        evolutionTreeNode.evolutionsFromNode = new EvolutionTreeNode[1];
        evolutionTreeNode.evolutionsFromNodeList = new List<EvolutionTreeNode>();

        return evolutionTreeNode;
    }

    EvolutionTreeNode AddNodesToTree(string pokemonName, KeyValuePair<string, string> pokemonEvolution)
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

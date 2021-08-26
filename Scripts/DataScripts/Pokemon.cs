using System;

[Serializable]
public class Pokemon
{
    public string name { get; set; } // pokemon name
    public int dexNumber { get; set; } // pokedex number
    public string generation { get; set; } // generation the pokemon was added to
    public int evolutionStage { get; set; } // stage in the pokemon's evolution (0 = baby, 1 = stage 1, 2 = stage 2, 3 = stage 3

    //Breeding
    public string eggGroup1 { get; set; }
    public string eggGroup2 { get; set; }

    //Elemental Types
    public string type1 { get; set; }
    public string type2 { get; set; }

    //Evolution Tree
    public EvolutionTreeNode evolutionTree { get; set; }

    //Game Data
    public string[] availableGames { get; set; } //games it is huntable in
    public RouteList[] availableLocations { get; set; }
    public string availableInGo { get; set; }
    public string availableShinyInGo { get; set; }

    //Shiny Hunting Methods
    public string[] availableMethods { get; set; } //Method name    
    public float bestOdds { get; set; } //percentage
    public string[] bestMethods { get; set; } //list of methods with the joint best odds
    public string[] bestGames { get; set; } //list of games with the joint best odds

    //Alternate Forms
    public string alolanForm { get; set; }
    public string galarianForm { get; set; }
    public string[] alternateForms { get; set; } //alternate Form appearances.
    public bool hasMegaEvolution { get; set; }
    public string[] megaEvolutionForms { get; set; }
    public bool hasGigantamaxForm { get; set; }

    public Pokemon()
    {
        availableGames = new string[1];
        availableMethods = new string[1];
        availableLocations = new RouteList[1];
        bestMethods = new string[1];
        bestGames = new string[1];
        alternateForms = new string[1];
        megaEvolutionForms = new string[1];
        evolutionTree = new EvolutionTreeNode();
    }

    public Pokemon(string name, int dexNumber, string generation, int evolutionStage, string eggGroup1,
        string eggGroup2, string type1, string type2, EvolutionTreeNode evolutionTree, string[] availableGames,
        string availableInGo, string availableShinyInGo, string[] availableMethods, 
        RouteList[] availableLocations, float bestOdds, string[] bestMethods, string[] bestGames,
        string alolanForm, string galarianForm, string[] alternateForms, bool hasMegaEvolution,
        string[] megaEvolutionForms, bool hasGigantamaxForm)
    {
        this.name = name;
        this.dexNumber = dexNumber;
        this.generation = generation;
        this.evolutionStage = evolutionStage;

        this.eggGroup1 = eggGroup1;
        this.eggGroup2 = eggGroup2;

        this.type1 = type1;
        this.type2 = type2;

        this.evolutionTree = evolutionTree;

        this.availableGames = availableGames;
        this.availableLocations = availableLocations;
        this.availableInGo = availableInGo;
        this.availableShinyInGo = availableShinyInGo;

        this.availableMethods = availableMethods;
        this.bestMethods = bestMethods;
        this.bestGames = bestGames;
        this.bestOdds = bestOdds;

        this.alolanForm = alolanForm;
        this.galarianForm = galarianForm;
        this.alternateForms = alternateForms;
        this.hasMegaEvolution = hasMegaEvolution;
        this.megaEvolutionForms = megaEvolutionForms;
        this.hasGigantamaxForm = hasGigantamaxForm;
    }

    public void Save()
    {
        if (name.Contains(":"))
        {
            name = name.Replace(":", string.Empty);
        }

        DataControl.Save(name, DataControl.DataType.Pokemon, this);
    }

    public void Save(string filePath)
    {
        if (name != null)
        {
            if (name.Contains(":"))
            {
                name = name.Replace(":", string.Empty);
            }

            //LogSave();

            DataControl.Save(name, DataControl.DataType.Pokemon, this, filePath);
        }
    }

    void LogSave()
    {
        Log.Write("############# SAVE POKEMON ############");
        Log.Write("Pokemon: " + name);
        Log.Write("Dex Number: " + dexNumber);
        Log.Write("Generation:" + generation);
        Log.Write("Evolution Stage:" + evolutionStage);
        Log.Write("Egg Group 1:" + eggGroup1);
        Log.Write("Egg Group 2:" + eggGroup2);
        Log.Write("Type 1:" + type1);
        Log.Write("Type 2:" + type2);

        Log.Write("Evolution Tree:");
        try
        {
            if (evolutionTree.evolutionsFromNode.Length != 0)
            {
                if (evolutionTree.evolutionsFromNode[0].pokemon != "")
                {
                    foreach (EvolutionTreeNode node in evolutionTree.evolutionsFromNode)
                    {
                        string evolutionTreeText = string.Empty;
                        evolutionTreeText += evolutionTree.pokemon;
                        evolutionTreeText += " -> " + node.methodToReachNode + " -> " + node.pokemon;

                        if (node.evolutionsFromNode.Length != 0)
                        {
                            if (node.evolutionsFromNode[0].pokemon != "")
                            {
                                foreach (EvolutionTreeNode node2 in evolutionTree.evolutionsFromNode)
                                {
                                    evolutionTreeText += " -> " + node2.methodToReachNode + " -> " + node2.pokemon;

                                    Log.Write(evolutionTreeText);
                                    evolutionTreeText += evolutionTree.pokemon + " -> " + node.methodToReachNode + " -> " + node.pokemon;
                                }
                            }
                        }

                        Log.Write(evolutionTreeText);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Write("ERROR: SAVE: " + ex.Message);
            Log.Write(ex.StackTrace);
            throw;
        }

        Log.Write("Games:");

        foreach (string game in availableGames)
        {
            Log.Write("• " + game);
        }

        Log.Write("Available Locations:");

        foreach (RouteList location in availableLocations)
        {
            Log.Write("• " + location.gameName);
            for (int i = 0; i < location.routeNames.Length; i++)
            {
                Log.Write("  ○ " + location.routeNames[i]);
                Log.Write("  ○ " + location.encounterTypes[i]);
                Log.Write("  ○ " + location.encounterDescriptions[i]); 
            }
        }

        Log.Write("Available Methods:");

        foreach (string method in availableMethods)
        {
            Log.Write("• " + method);
        }

        Log.Write("Best Odds:" + bestOdds);

        Log.Write("Best Methods:");

        foreach (string method in bestMethods)
        {
            Log.Write("• " + method);
        }

        Log.Write("Best Games:");

        foreach (string game in bestGames)
        {
            Log.Write("• " + game);
        }

        if (hasMegaEvolution)
        {
            Log.Write("Has Mega Evolution");
        }

        if (hasGigantamaxForm)
        {
            Log.Write("Has Gigantamax Form");
        }

        Log.Write("Alternate Forms:");

        if (alolanForm != "")
        {
            Log.Write("Alolan Variant = " + alolanForm);
        }

        if (galarianForm != "")
        {
            Log.Write("Galarian Variant = " + galarianForm);
        }

        foreach (string form in alternateForms)
        {
            Log.Write("• " + form);
        }

        foreach (string form in megaEvolutionForms)
        {
            Log.Write("• " + form);
        }

    }
}

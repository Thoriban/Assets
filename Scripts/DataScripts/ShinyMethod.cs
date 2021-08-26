using System;
using System.Collections.Generic;

[Serializable]
public class ShinyMethod
{
    public string methodName { get; set; }
    public string game { get; set; }
    public string methodType { get; set; }
    public bool effectedByShinyCharm { get; set; }
    public int[] baseOddsKeys { get; set; }  //encounter requirement modifier, odds (percentage)
    public float[] baseOddsValues { get; set; }  //encounter requirement modifier, odds (percentage)
    public int[] shinyCharmOddsKeys { get; set; } //encounter requirement modifier, odds (percentage)
    public float[] shinyCharmOddsValues { get; set; } //encounter requirement modifier, odds (percentage)
    public bool automaticallyApplied { get; set; }
    public float[] chanceOfBeingApplied { get; set; }
    public string[] availablePokemon { get; set; }
    public string description { get; set; }

    public List<float> chancesOfBeingApplied;
    public Dictionary<int, float> baseOdds;
    public Dictionary<int, float> shinyCharmOdds;

    public ShinyMethod()
    {
        chanceOfBeingApplied = new float[1];
        chancesOfBeingApplied = new List<float>();
        baseOdds = new Dictionary<int, float>();
        //baseOddsKeys = new List<int>();
        //baseOddsValues = new List<float>();
        //shinyCharmOddsKeys = new List<int>();
        //shinyCharmOddsValues = new List<float>();
        shinyCharmOdds = new Dictionary<int, float>();
        //availablePokemon = new List<string>();
    }

    //public ShinyMethod(string methodName, string game, string methodType, bool effectedByShinyCharm,
    //    List<int> baseOddsKeys, List<float> baseOddsValues, List<int> shinyCharmOddsKeys,
    //    List<float> shinyCharmOddsValues, bool automaticallyApplied, float chanceOfBeingApplied,
    //    List<string> availablePokemon, string description)
    public ShinyMethod(string methodName, string game, string methodType, bool effectedByShinyCharm,
        int[] baseOddsKeys, float[] baseOddsValues, int[] shinyCharmOddsKeys,
        float[] shinyCharmOddsValues, bool automaticallyApplied, float[] chanceOfBeingApplied,
        string[] availablePokemon, string description)
    {
        this.methodName = methodName;
        this.game = game;
        this.methodType = methodType;
        this.effectedByShinyCharm = effectedByShinyCharm;
        this.baseOddsKeys = baseOddsKeys;
        this.baseOddsValues = baseOddsValues;
        this.shinyCharmOddsKeys = shinyCharmOddsKeys;
        this.shinyCharmOddsValues = shinyCharmOddsValues;
        this.automaticallyApplied = automaticallyApplied;
        this.chanceOfBeingApplied = chanceOfBeingApplied;
        this.availablePokemon = availablePokemon;
        this.description = description;

        baseOdds = new Dictionary<int, float>();

        for (int i = 0; i < baseOddsKeys.Length; i++)
        {
            baseOdds.Add(baseOddsKeys[i], baseOddsValues[i]);
        }

        shinyCharmOdds = new Dictionary<int, float>();

        for (int i = 0; i < shinyCharmOddsKeys.Length; i++)
        {
            shinyCharmOdds.Add(shinyCharmOddsKeys[i], shinyCharmOddsValues[i]);
        }
    }

    public string ConvertDecimalToFraction(float odds)
    {
        float denom = 1f / odds;

        if (odds == 0)
        {
            return "";
        }

        if (denom == 1)
        {
            return "Static Shiny Pokemon";
        }
        else if (denom > 8192.5f)
        {
            return "ERROR: INCORRECT ODDS ENTERED: Chance is lower original Gen 1 - 5 full odds";
        }
        else
        {
            denom += 0.5f;
            return "1/" + (int)denom;
        }
    }

    public string ConvertDecimalToPercentage(float odds)
    {
        float percentage = odds * 100;
        return "" + percentage.ToString("0.####") + "%";
    }

    public void Save()
    {
        //LogSave();
        DataControl.Save(methodName, DataControl.DataType.HuntingMethod, this);
    }

    public void Save(string filePath, string gameName)
    {
        //LogSave();
        DataControl.Save(methodName, DataControl.DataType.HuntingMethod, this, filePath, gameName);
    }

    void LogSave()
    {
        Log.Write("######### SAVE METHOD #########");
        Log.Write("Method name = " + methodName);
        Log.Write("Game = " + game);
        Log.Write("Method Type = " + methodType);
        Log.Write("Effected By Shiny Charm = " + effectedByShinyCharm);
        Log.Write("Base Odds:");
        Log.Write("Encounters | Odds");

        if (baseOddsKeys != null && baseOddsValues != null)
        {
            for (int i = 0; i < baseOddsKeys.Length; i++)
            {
                Log.Write("" + baseOddsKeys[i] + " | " + baseOddsValues[i]);
            }
        }

        Log.Write("Shiny Charm Odds:");
        Log.Write("Encounters | Odds");

        if (shinyCharmOddsKeys != null && shinyCharmOddsValues != null)
        {
            for (int i = 0; i < shinyCharmOddsKeys.Length; i++)
            {
                Log.Write("" + shinyCharmOddsKeys[i] + " | " + shinyCharmOddsValues[i]);
            }
        }

        Log.Write("Automatically Applied = " + automaticallyApplied);
        Log.Write("Chance of Being Applied = " + chanceOfBeingApplied);

        Log.Write("Available Pokemon:");

        if (availablePokemon != null)
        {
            for (int i = 0; i < availablePokemon.Length; i++)
            {
                Log.Write("• " + availablePokemon[i]);
            }
        }

        Log.Write("Description = " + description);
        Log.Write("");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMethodDataTask : ThreadedJob
{
    public string[] columns;
    public string appDataPath;
    public string[] methodFilePaths;

    protected override void ThreadFunction()
    {
        if (columns[0] != string.Empty && columns[0] != null && columns[0].Length != 0)
        {
            ShinyMethod method = new ShinyMethod();

            foreach (string path in methodFilePaths)
            {
                if (path.Contains("/" + columns[0] + ".dat"))
                {
                    method = DataControl.LoadShinyHuntingMethod(path);
                    break;
                }
            }

            method.methodName = columns[0];
            method.game = columns[1];
            method.methodType = columns[2];
            method.effectedByShinyCharm = ImportScript.CheckCellForBoolean(columns[3]);
            method.automaticallyApplied = ImportScript.CheckCellForBoolean(columns[4]);
            method.chancesOfBeingApplied = new List<float>();
            method.chancesOfBeingApplied.Add(ImportScript.CheckCellForFloat(columns[5]));
            method.chanceOfBeingApplied = method.chancesOfBeingApplied.ToArray();

            int encountersRequired = int.Parse(columns[6]);
            float baseOdds = ImportScript.CheckCellForFloat(columns[7]);
            float shinyCharmOdds = ImportScript.CheckCellForFloat(columns[10]);

            if (baseOdds != 0)
            {
                if (method.baseOdds.ContainsKey(encountersRequired))
                {
                    method.baseOdds[encountersRequired] = baseOdds;
                }
                else
                {
                    method.baseOdds.Add(encountersRequired, baseOdds);
                }

                List<int> baseOddsKeys = new List<int>();
                List<float> baseOddsValues = new List<float>();

                foreach (KeyValuePair<int, float> odds in method.baseOdds)
                {
                    baseOddsKeys.Add(odds.Key);
                    baseOddsValues.Add(odds.Value);
                }

                method.baseOddsKeys = baseOddsKeys.ToArray();
                method.baseOddsValues = baseOddsValues.ToArray();
            }
            else
            {
                method.baseOddsKeys = new int[1];
                method.baseOddsValues = new float[1];
            }

            if (shinyCharmOdds != 0f)
            {
                if (method.shinyCharmOdds.ContainsKey(encountersRequired))
                {
                    method.shinyCharmOdds[encountersRequired] = shinyCharmOdds;
                }
                else
                {
                    method.shinyCharmOdds.Add(encountersRequired, shinyCharmOdds);
                }

                List<int> shinyCharmOddsKeys = new List<int>();
                List<float> shinyCharmOddsValues = new List<float>();

                foreach (KeyValuePair<int, float> odds in method.shinyCharmOdds)
                {
                    shinyCharmOddsKeys.Add(odds.Key);
                    shinyCharmOddsValues.Add(odds.Value);
                }

                method.shinyCharmOddsKeys = shinyCharmOddsKeys.ToArray();
                method.shinyCharmOddsValues = shinyCharmOddsValues.ToArray();
            }
            else
            {
                method.shinyCharmOddsKeys = new int[1];
                method.shinyCharmOddsValues = new float[1];
            }

            method.description = columns[13];

            method.description = method.description.Replace("@", ",");

            List<string> availablePokemon = new List<string>();

            for (int i = 14; i < columns.Length; i++)
            {
                if (columns[i] != string.Empty)
                {
                    availablePokemon.Add(columns[i]);
                }
            }

            method.availablePokemon = availablePokemon.ToArray();

            method.Save(appDataPath + "/Resources/", columns[1]);
        }
    }

    protected override void OnFinished()
    {
        base.OnFinished();
    }
}

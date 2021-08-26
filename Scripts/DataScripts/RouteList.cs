using System;
using System.Collections.Generic;

public enum EncounterType
{
    WildEncounter = 1,
    FixedEncounter = 2,
    StarterChoice = 3,
    Choice = 4,
    Gift = 5,
    Purchase = 6,
    Trade = 7
}

[Serializable]
public class RouteList
{
    public string gameName { get; set; }

    public string[] routeNames { get; set; }

    public int[] encounterTypes { get; set; }

    public string[] encounterDescriptions { get; set; }

    public RouteList()
    {
        gameName = "";
        routeNames = new string[1];
        encounterTypes = new int[1];
        encounterDescriptions = new string[1];
    }

    public RouteList AddRoutList(string gameName, List<string> routeNames, List<int> encounterTypes, List<string> encounterDescriptions)
    {
        this.gameName = gameName;
        this.routeNames = routeNames.ToArray();
        this.encounterTypes = encounterTypes.ToArray();
        this.encounterDescriptions = encounterDescriptions.ToArray();

        return this;
    }

    public static RouteList[] ToArray(List<RouteList> list)
    {
        RouteList[] array = new RouteList[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            array[i] = new RouteList();
        }

        for (int i = 0; i < list.Count; i++)
        {
            array[i].gameName = list[i].gameName;
            array[i].routeNames = list[i].routeNames;
            array[i].encounterTypes = list[i].encounterTypes;
            array[i].encounterDescriptions = list[i].encounterDescriptions;
        }

        return array;
    }
}

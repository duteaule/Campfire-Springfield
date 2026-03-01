using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    public Transform player;
    public int chunkSize = 16;
    public int viewRadius = 2;
    Dictionary<Vector2Int, GameObject> loaded = new Dictionary<Vector2Int, GameObject>();

    public int iron=0;
    public int cadmium=0;
    public int copper=0;
    public int titanium=0;
    public int gold=0;

    public int ironCost=2;
    public int cadmiumCost=4;
    public int copperCost=8;
    public int titaniumCost=16;
    public int goldCost=32;

    public Text ironText;   //amount in inventory on screen
    public Text copperText;
    public Text goldText;
    public Text titaniumText;
    public Text cadmiumText;
    public Text moneyText;

    public Text ironTextC;   //cost in shop
    public Text copperTextC;
    public Text goldTextC;
    public Text titaniumTextC;
    public Text cadmiumTextC;

    public int money = 0;

    public int dynamiteCost;
    public int extractorCost;

    public int dynamite;
    public int extractor;

    public Text extractorText;
    public Text extractorTextCost;
    public Text dynamiteText;
    public Text dynamiteTextCost;

    void Start()
    {
        ironCost = 2;
        cadmiumCost = 4;
        copperCost = 8;
        titaniumCost = 16;
        goldCost = 32;

        dynamiteCost = 64;
        extractorCost = 16;

        extractor = 1;
        dynamite = 0;

        ironTextC.text = ironCost.ToString();
        copperTextC.text = copperCost.ToString();
        goldTextC.text = goldCost.ToString();
        titaniumTextC.text = titaniumCost.ToString();
        cadmiumTextC.text = cadmiumCost.ToString();
    }
    void Update()
    {

        ironText.text = iron.ToString();
        copperText.text = copper.ToString();
        goldText.text = gold.ToString();
        titaniumText.text = titanium.ToString();
        cadmiumText.text = cadmium.ToString();
        moneyText.text = money.ToString();


        dynamiteTextCost.text = dynamiteCost.ToString();
        extractorTextCost.text = extractorCost.ToString();
        dynamiteText.text = dynamite.ToString();
        extractorText.text = extractor.ToString();





        Vector2 p = player.position;
        int cx = Mathf.FloorToInt(p.x / chunkSize);
        int cy = Mathf.FloorToInt(p.y / chunkSize);

        var keep = new HashSet<Vector2Int>();

        for (int x = cx - viewRadius; x <= cx + viewRadius; x++)
            for (int y = cy - viewRadius; y <= cy + viewRadius; y++)
            {
                var key = new Vector2Int(x, y);
                keep.Add(key);
                if (!loaded.ContainsKey(key))
                    loaded[key] = CreateChunk(x, y);
            }

        // unload far chunks
        var toRemove = new List<Vector2Int>();
        foreach (var kv in loaded)
            if (!keep.Contains(kv.Key))
            {
                Destroy(kv.Value);
                toRemove.Add(kv.Key);
            }
        for (int i = 0; i < toRemove.Count; i++)
            loaded.Remove(toRemove[i]);
    }

    GameObject CreateChunk(int cx, int cy)
    {
        var go = new GameObject($"Chunk_{cx}_{cy}");
        go.transform.position = new Vector3(cx * chunkSize, cy * chunkSize, 0f);
        return go;
    }

    public void sellIrons()
    {
        money += ironCost*iron;
        Debug.Log(ironCost+"ironCost");
        iron = 0;
    }
    public void sellCoppers()
    {

        money += copperCost * copper;
        copper = 0;
    }
    public void sellGolds()
    {

        money += goldCost * gold;
        gold = 0;
    }
    public void sellTitaniums()
    {

        money += titaniumCost * titanium;
        titanium = 0;
    }
    public void sellCadmiums()
    {

        money += cadmiumCost * cadmium;
        cadmium = 0;
    }
    public void sellAlls()
    {
        int num = 0;
        num += ironCost * iron;
        iron = 0;
        num += copperCost * copper;
        copper = 0;
        num += goldCost * gold;
        gold = 0;
        num += titaniumCost * titanium;
        titanium = 0;
        num += cadmiumCost * cadmium;
        cadmium = 0;
        money += num;
    }
    public void buyDynamite()
    {
        if ( money >= dynamiteCost )
        {
            money -= dynamiteCost;
            dynamite++;
        }
    }
    public void buyExtractor()
    {
        if (money >= extractorCost)
        {
            money -= extractorCost;
            extractorCost += 16;
            extractor++;
        }
    }



    public void gainResources(int resourceType)
    {
        if(resourceType == 0)
        {
            iron++;
        }
        else if(resourceType == 1)
        {
            cadmium++;
        }
        else if(resourceType == 2)
        {
            copper++;
        }
        else if(resourceType == 3)
        {
            titanium++;
        }
        else if(resourceType == 4)
        {
            gold++;
        }
    }


}

/*
    Basically a character sheet + dice to make calculations + game status.
    Persists a wide range of data to a csv file until JSON or SQL storage are
    made available.
*/
using Godot;
using System;
using System.Collections.Generic;

public class StatsManager {
    public enum Archetypes{ // Types of characters
        None,
        One,
        Two,
        Three,
        EnemyOne,
        EnemyTwo
    };

    public enum Effects{ // Perks and status conditions
        None,
        Bleed
    };

    public enum Stats{
        // ICEPAWS Attributes
        Intelligence,
        Charisma,
        Endurance,
        Perception,
        Agility,
        Willpower,
        Strength,

        // DERIVED stats
        Health,
        HealthMax,
        HealthRegenDelay,
        HealthRegenAmount,
        Stamina,
        StaminaMax,
        StaminaRegenDelay,
        StaminaRegenAmount,
        Mana,
        ManaMax,
        ManaRegenDelay,
        ManaRegenAmount,
        Speed,
        UnarmedDamage,
        DamageResistance,
        DamageThreshold,

        // GameState
        LastEncounter,
        CurrentNode,
        CurrentLevel,
        LastNode,
        NodeInProgress,
        Victory
    };

    public enum Abilities{
        None,
        Unarmed,
        DoubleJump
    };

    public enum Facts{
        None,
        Name
    };

    System.Collections.Generic.Dictionary<Stats, int> baseStats;
    System.Collections.Generic.Dictionary<Stats, int> statBuffs;
    System.Collections.Generic.Dictionary<Effects, int> effects;
    System.Collections.Generic.Dictionary<Abilities, int> abilities;

    System.Collections.Generic.Dictionary<Facts, string> facts;

    public StatsManager(){
        baseStats = new System.Collections.Generic.Dictionary<Stats, int>();
        statBuffs = new System.Collections.Generic.Dictionary<Stats, int>();
        abilities = new System.Collections.Generic.Dictionary<Abilities, int>();
        effects = new System.Collections.Generic.Dictionary<Effects, int>();
        facts = new System.Collections.Generic.Dictionary<Facts, string>();
    }

    public void Init(Archetypes archetype){
        switch(archetype){
            case Archetypes.None:
                break;
            case Archetypes.One:
                break;
            case Archetypes.Two:
                break;
            case Archetypes.Three:
                break;
        }
        baseStats.Add(Stats.CurrentLevel, -1);
        baseStats.Add(Stats.LastNode, -1);
    }


    // Returns a stat before buffs are applied
    public int GetBaseStat(Stats stat){
        if(baseStats.ContainsKey(stat) && !StatIsDerived(stat)){
            return baseStats[stat];
        }
        else if(baseStats.ContainsKey(stat)){
            return GetDerivedStat(stat);
        }
        return 0;
    }

    public void SetBaseStat(Stats stat, int value){
        if(baseStats.ContainsKey(stat)){
            baseStats[stat] = value;
            return;
        }
        baseStats.Add(stat, value);
    }

    // Returns a stat derived from other stats
    public int GetDerivedStat(Stats stat){
        switch(stat){
            case Stats.HealthMax:
                break;
            case Stats.HealthRegenAmount:
                break;
            case Stats.HealthRegenDelay:
                break;
            case Stats.StaminaMax:
                break;
            case Stats.StaminaRegenAmount:
                break;
            case Stats.StaminaRegenDelay:
                break;
            case Stats.ManaMax:
                break;
            case Stats.ManaRegenAmount:
                break;
            case Stats.ManaRegenDelay:
                break;
            case Stats.Speed:
                break;
            case Stats.UnarmedDamage:
                break;
            case Stats.DamageResistance:
                break;
            case Stats.DamageThreshold:
                break;
        }
        return 0;
    }

    public int GetStatBuff(Stats stat){
        if(statBuffs.ContainsKey(stat)){
            return statBuffs[stat];
        }
        return 0;
    }

    public void SetStatBuff(Stats stat, int value){
        if(statBuffs.ContainsKey(stat)){
            statBuffs[stat] = value;
            return;
        }
        statBuffs.Add(stat, value);
    }

    public int GetStat(Stats stat){
        return GetBaseStat(stat) + GetStatBuff(stat);
    }

    public string GetFact(Facts fact){
        if(facts.ContainsKey(fact)){
            return facts[fact];
        }
        return "";
    }

    public bool StatIsDerived(Stats stat){
        List<Stats> derived = new List<Stats>{
            Stats.HealthMax,
            Stats.StaminaMax,
            Stats.ManaMax,
            Stats.Speed,
            Stats.UnarmedDamage
        };

        if(derived.IndexOf(stat) != -1){
            return true;
        }
        return false;
    }

    public bool HasPerk(Effects effect){
        if(effects.ContainsKey(effect)){
            return true;
        }

        return false;
    }

    public void SetEffect(Effects effect, int value){
        if(effects.ContainsKey(effect)){
            effects[effect] = value;
            return;
        }
        effects.Add(effect, value);
    }

    public void SetAbility(Abilities ability, int value){
        if(abilities.ContainsKey(ability)){
            abilities[ability] = value;
            return;
        }
        abilities.Add(ability, value);
    }

    public void SetFact(Facts fact, string value){
        if(facts.ContainsKey(fact)){
            facts[fact] = value;
            return;
        }
        facts.Add(fact, value);
    }    

    public int EffectsLevel(Effects effect){
        if(effects.ContainsKey(effect)){
            return effects[effect];
        }
        return 0;
    }

    public bool HasAbility(Abilities ability){
        if(abilities.ContainsKey(ability)){
            return true;
        }

        return false;
    }

    public System.Collections.Generic.Dictionary<int, string[]> GetRows(){
        System.Collections.Generic.Dictionary<int, string[]> ret;
        ret = new System.Collections.Generic.Dictionary<int, string[]>();
        int i = 0;
        foreach(Stats key in baseStats.Keys){
            ret.Add(i, new List<string>{"stats", "" + key, "" + baseStats[key]}.ToArray());
            i++;
        }

        foreach(Stats key in statBuffs.Keys){
            ret.Add(i, new List<string>{"buffs", "" + key, "" + statBuffs[key]}.ToArray());
            i++;
        }

        foreach(Effects key in effects.Keys){
            ret.Add(i, new List<string>{"effects", "" + key, "" + effects[key]}.ToArray());
            i++;
        }

        foreach(Abilities key in abilities.Keys){
            ret.Add(i, new List<string>{"abilities", "" + key, "" + abilities[key]}.ToArray());
            i++;
        }

        foreach(Facts key in facts.Keys){
            ret.Add(i, new List<string>{"facts", "" + key, facts[key]}.ToArray());
            i++;
        }

        return ret;

    }

    public static StatsManager FromRows(System.Collections.Generic.Dictionary<int, string[]> rows){
        StatsManager ret = new StatsManager();
        foreach(int key in rows.Keys){
            ret.LoadRow(rows[key]);
        }
        return ret;
    }

    public void LoadRow(string[] row){
        string type = row[0];
        string key = row[1];
        string value = row[2];

        switch(type){
            case "stats":
                LoadStat(key, value);
                break;
            case "buffs":
                LoadBuff(key, value);
                break;
            case "effects":
                LoadEffect(key, value);
                break;
            case "abilities":
                LoadAbility(key, value);
                break;
            case "facts":
                LoadFact(key, value);
                break;
        }
    }

    public void LoadStat(string key, string value){
        Stats keyStat = (Stats) Enum.Parse(typeof(Stats), key, true);
        int valueInt = Util.ToInt(value);
        SetBaseStat(keyStat, valueInt);
    }

    public void LoadBuff(string key, string value){
        Stats keyStat = (Stats) Enum.Parse(typeof(Stats), key, true);
        int valueInt = Util.ToInt(value);
        SetStatBuff(keyStat, valueInt);   
    }

    public void LoadEffect(string key, string value){
        Effects keyEffect = (Effects) Enum.Parse(typeof(Effects), key, true);
        int valueInt = Util.ToInt(value);
        SetEffect(keyEffect, valueInt);
    }

    public void LoadAbility(string key, string value){
        Abilities keyEffect = (Abilities) Enum.Parse(typeof(Abilities), key, true);
        int valueInt = Util.ToInt(value);
        SetAbility(keyEffect, valueInt);
    }

    public void LoadFact(string key, string value){
        Facts keyFact = (Facts) Enum.Parse(typeof(Facts), key, true);
        SetFact(keyFact, value);
    }

    public static Archetypes Archetype(string characterName){
        switch(characterName){
            case "fred":
                return Archetypes.One;
                break;
            case "velma":
                return Archetypes.Two;
                break;
            case "scoob":
                return Archetypes.Three;
                break;
            case "old man rivers":
                return Archetypes.EnemyOne;
                break;
            case "old man jenkins":
                return Archetypes.EnemyTwo;
                break;
        }
        GD.Print("StatsManager.Archetypes: Invalid character name " + characterName);
        return Archetypes.None;
    }
}
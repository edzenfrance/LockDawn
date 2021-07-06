using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI;

public static class EmeraldAICombatManager
{
    /// <summary>
    /// This external script handles the generation of all Emerald AI attacks both ranged and melee.
    /// </summary>
    public static void GenerateMeleeAttack (EmeraldAISystem EmeraldComponent)
    {       
        EmeraldComponent.CurrentMeleeAttackType = EmeraldAISystem.CurrentMeleeAttackTypes.StationaryAttack;

        if (EmeraldComponent.MeleeAttacks.Count > 0)
        {
            if (EmeraldComponent.MeleeAttackPickType == EmeraldAISystem.MeleeAttackPickTypeEnum.Odds) //Pick an ability by using the odds for each ability
            {
                List<float> OddsList = new List<float>();
                for (int i = 0; i < EmeraldComponent.MeleeAttacks.Count; i++)
                {
                    OddsList.Add(EmeraldComponent.MeleeAttacks[i].AttackOdds);
                }
                int OddsIndex = (int)GenerateProbability(OddsList.ToArray());
                EmeraldComponent.MeleeAttacksListIndex = OddsIndex;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.MeleeAttacks[OddsIndex].AttackAnimaition;
            }
            else if (EmeraldComponent.MeleeAttackPickType == EmeraldAISystem.MeleeAttackPickTypeEnum.Order) //Pick an ability by going through the list in order
            {
                EmeraldComponent.MeleeAttacksListIndex = EmeraldComponent.MeleeAttackIndex;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.MeleeAttacks[EmeraldComponent.MeleeAttackIndex].AttackAnimaition;
                EmeraldComponent.MeleeAttackIndex++;
                if (EmeraldComponent.MeleeAttackIndex == EmeraldComponent.MeleeAttacks.Count)
                {
                    EmeraldComponent.MeleeAttackIndex = 0;
                }
            }
            else if (EmeraldComponent.MeleeAttackPickType == EmeraldAISystem.MeleeAttackPickTypeEnum.Random) //Pick a random ability from the list
            {
                int RandomIndex = Random.Range(0, EmeraldComponent.MeleeAttacks.Count);
                EmeraldComponent.MeleeAttacksListIndex = RandomIndex;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.MeleeAttacks[RandomIndex].AttackAnimaition;
            }
        }
    }

    public static void GenerateMeleeRunAttack(EmeraldAISystem EmeraldComponent)
    {
        EmeraldComponent.CurrentMeleeAttackType = EmeraldAISystem.CurrentMeleeAttackTypes.RunAttack;

        if (EmeraldComponent.MeleeRunAttacks.Count > 0)
        {
            if (EmeraldComponent.MeleeRunAttackPickType == EmeraldAISystem.MeleeRunAttackPickTypeEnum.Odds) //Pick an ability by using the odds for each ability
            {
                List<float> OddsList = new List<float>();
                for (int i = 0; i < EmeraldComponent.MeleeRunAttacks.Count; i++)
                {
                    OddsList.Add(EmeraldComponent.MeleeRunAttacks[i].AttackOdds);
                }
                int OddsIndex = (int)GenerateProbability(OddsList.ToArray());
                EmeraldComponent.MeleeRunAttacksListIndex = OddsIndex;
                EmeraldComponent.CurrentRunAttackAnimationIndex = EmeraldComponent.MeleeRunAttacks[OddsIndex].AttackAnimaition;
            }
            else if (EmeraldComponent.MeleeRunAttackPickType == EmeraldAISystem.MeleeRunAttackPickTypeEnum.Order) //Pick an ability by going through the list in order
            {
                EmeraldComponent.MeleeRunAttacksListIndex = EmeraldComponent.MeleeRunAttackIndex;
                EmeraldComponent.CurrentRunAttackAnimationIndex = EmeraldComponent.MeleeRunAttacks[EmeraldComponent.MeleeRunAttackIndex].AttackAnimaition;
                EmeraldComponent.MeleeRunAttackIndex++;
                if (EmeraldComponent.MeleeRunAttackIndex == EmeraldComponent.MeleeRunAttacks.Count)
                {
                    EmeraldComponent.MeleeRunAttackIndex = 0;
                }
            }
            else if (EmeraldComponent.MeleeRunAttackPickType == EmeraldAISystem.MeleeRunAttackPickTypeEnum.Random) //Pick a random ability from the list
            {
                int RandomIndex = Random.Range(0, EmeraldComponent.MeleeRunAttacks.Count);
                EmeraldComponent.MeleeRunAttacksListIndex = RandomIndex;
                EmeraldComponent.CurrentRunAttackAnimationIndex = EmeraldComponent.MeleeRunAttacks[RandomIndex].AttackAnimaition;
            }
        }
    }

    public static void GenerateOffensiveAbility (EmeraldAISystem EmeraldComponent)
    {
        if (EmeraldComponent.OffensiveAbilities.Count > 0)
        {
            if (EmeraldComponent.OffensiveAbilityPickType == EmeraldAISystem.OffensiveAbilityPickTypeEnum.Odds) //Pick an ability by using the odds for each ability
            {
                List<float> OddsList = new List<float>();
                for (int i = 0; i < EmeraldComponent.OffensiveAbilities.Count; i++)
                {
                    OddsList.Add(EmeraldComponent.OffensiveAbilities[i].AbilityOdds);
                }
                int OddsIndex = (int)GenerateProbability(OddsList.ToArray());
                EmeraldComponent.m_EmeraldAIAbility = EmeraldComponent.OffensiveAbilities[OddsIndex].OffensiveAbility;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.OffensiveAbilities[OddsIndex].AbilityAnimaition;
            }
            else if (EmeraldComponent.OffensiveAbilityPickType == EmeraldAISystem.OffensiveAbilityPickTypeEnum.Order) //Pick an ability by going through the list in order
            {
                EmeraldComponent.m_EmeraldAIAbility = EmeraldComponent.OffensiveAbilities[EmeraldComponent.OffensiveAbilityIndex].OffensiveAbility;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.OffensiveAbilities[EmeraldComponent.OffensiveAbilityIndex].AbilityAnimaition;
                EmeraldComponent.OffensiveAbilityIndex++;
                if (EmeraldComponent.OffensiveAbilityIndex == EmeraldComponent.OffensiveAbilities.Count)
                {
                    EmeraldComponent.OffensiveAbilityIndex = 0;
                }
            }
            else if (EmeraldComponent.OffensiveAbilityPickType == EmeraldAISystem.OffensiveAbilityPickTypeEnum.Random) //Pick a random ability from the list
            {
                int RandomIndex = Random.Range(0, EmeraldComponent.OffensiveAbilities.Count);
                EmeraldComponent.m_EmeraldAIAbility = EmeraldComponent.OffensiveAbilities[RandomIndex].OffensiveAbility;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.OffensiveAbilities[RandomIndex].AbilityAnimaition;
            }
        }
    }

    public static void GenerateSupportAbility (EmeraldAISystem EmeraldComponent)
    {
        if (EmeraldComponent.SupportAbilities.Count > 0)
        {
            if (EmeraldComponent.SupportAbilityPickType == EmeraldAISystem.SupportAbilityPickTypeEnum.Odds) //Pick an ability by using the odds for each ability
            {
                List<float> OddsList = new List<float>();
                for (int i = 0; i < EmeraldComponent.SupportAbilities.Count; i++)
                {
                    OddsList.Add(EmeraldComponent.SupportAbilities[i].AbilityOdds);
                }
                int OddsIndex = (int)GenerateProbability(OddsList.ToArray());
                EmeraldComponent.m_EmeraldAIAbility = EmeraldComponent.SupportAbilities[OddsIndex].SupportAbility;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.SupportAbilities[OddsIndex].AbilityAnimaition;
            }
            else if (EmeraldComponent.SupportAbilityPickType == EmeraldAISystem.SupportAbilityPickTypeEnum.Order) //Pick an ability by going through the list in order
            {
                EmeraldComponent.m_EmeraldAIAbility = EmeraldComponent.SupportAbilities[EmeraldComponent.SupportAbilityIndex].SupportAbility;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.SupportAbilities[EmeraldComponent.SupportAbilityIndex].AbilityAnimaition;
                EmeraldComponent.SupportAbilityIndex++;
                if (EmeraldComponent.SupportAbilityIndex == EmeraldComponent.SupportAbilities.Count)
                {
                    EmeraldComponent.SupportAbilityIndex = 0;
                }
            }
            else if (EmeraldComponent.SupportAbilityPickType == EmeraldAISystem.SupportAbilityPickTypeEnum.Random) //Pick a random ability from the list
            {
                int RandomIndex = Random.Range(0, EmeraldComponent.SupportAbilities.Count);
                EmeraldComponent.m_EmeraldAIAbility = EmeraldComponent.SupportAbilities[RandomIndex].SupportAbility;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.SupportAbilities[RandomIndex].AbilityAnimaition;
            }
        }
    }

    public static void GenerateSummoningAbility(EmeraldAISystem EmeraldComponent)
    {
        if (EmeraldComponent.SummoningAbilities.Count > 0)
        {
            if (EmeraldComponent.SummoningAbilityPickType == EmeraldAISystem.SummoningAbilityPickTypeEnum.Odds) //Pick an ability by using the odds for each ability
            {
                List<float> OddsList = new List<float>();
                for (int i = 0; i < EmeraldComponent.SummoningAbilities.Count; i++)
                {
                    OddsList.Add(EmeraldComponent.SummoningAbilities[i].AbilityOdds);
                }
                int OddsIndex = (int)GenerateProbability(OddsList.ToArray());
                EmeraldComponent.m_EmeraldAIAbility = EmeraldComponent.SummoningAbilities[OddsIndex].SummoningAbility;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.SummoningAbilities[OddsIndex].AbilityAnimaition;
            }
            else if (EmeraldComponent.SummoningAbilityPickType == EmeraldAISystem.SummoningAbilityPickTypeEnum.Order) //Pick an ability by going through the list in order
            {
                EmeraldComponent.m_EmeraldAIAbility = EmeraldComponent.SummoningAbilities[EmeraldComponent.SummoningAbilityIndex].SummoningAbility;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.SummoningAbilities[EmeraldComponent.SummoningAbilityIndex].AbilityAnimaition;
                EmeraldComponent.SummoningAbilityIndex++;
                if (EmeraldComponent.SummoningAbilityIndex == EmeraldComponent.SummoningAbilities.Count)
                {
                    EmeraldComponent.SummoningAbilityIndex = 0;
                }
            }
            else if (EmeraldComponent.SummoningAbilityPickType == EmeraldAISystem.SummoningAbilityPickTypeEnum.Random) //Pick a random ability from the list
            {
                int RandomIndex = Random.Range(0, EmeraldComponent.SummoningAbilities.Count);
                EmeraldComponent.m_EmeraldAIAbility = EmeraldComponent.SummoningAbilities[RandomIndex].SummoningAbility;
                EmeraldComponent.CurrentAnimationIndex = EmeraldComponent.SummoningAbilities[RandomIndex].AbilityAnimaition;
            }
        }
    }

    public static float GenerateProbability(float[] probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Component to hold properties of the Economy state and generic NPC answer templates.
//https://docs.google.com/document/d/1N2e_JC98oNHS8fSQ9jtK2RaHWgUFOOyF6bJpNgzt_GM/edit?usp=sharing

public class Economy
{
	public enum EconomyState
	{
		Overheating,
		GlobalRecession
	}

	public enum NEERState
	{
		Neutral,
		Stronger,
		Weaker
	}


    public enum GameStage //To determine what topic should the player be finding about
    {
        Stage1,
        Stage2,
        Stage3
    }


    public static EconomyState mEconomyState = EconomyState.Overheating;
    public static NEERState mNEERState = NEERState.Neutral;
    public static GameStage mStage = GameStage.Stage1;

	private static readonly string[] sRatePositive = { "increasing", "rising", "going up" };
	private static readonly string[] sRateNegative = { "decreasing", "falling", "going down" };

	public static string GetImportPriceRates()
	{
		switch (mEconomyState)
		{
			case EconomyState.Overheating:
				return GetRatePositive();
			case EconomyState.GlobalRecession:
				return GetRateNegative();
			default:
				return null;
		}
	}

	public static string GetInflationRates()
	{
		switch (mEconomyState)
		{
			case EconomyState.Overheating:
				return GetRatePositive();
			case EconomyState.GlobalRecession:
				return GetRateNegative();
			default:
				return null;
		}
	}

	public static string GetGDPRates()
	{
		switch (mEconomyState)
		{
			case EconomyState.Overheating:
				return GetRatePositive();
			case EconomyState.GlobalRecession:
				return GetRateNegative();
			default:
				return null;
		}
	}

	static string GetRatePositive()
	{
		return sRatePositive[Random.Range(0, sRatePositive.Length)];
	}

	static string GetRateNegative()
	{
		return sRateNegative[Random.Range(0, sRateNegative.Length)];
	}
}

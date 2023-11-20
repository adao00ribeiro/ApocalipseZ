using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct StructUser
{
	public int id;
	public string username;
	public string password;
	public string email;
	public int maxslot;
	public int dp;
	public int ap;


}

[System.Serializable]
public class User
{
	[SerializeField] private int Id;
	[SerializeField] private string Name;
	[SerializeField] private string Email;
	[SerializeField] private int Maxslot;
	//moeda global
	[SerializeField] private int CoinAp;
	//moeda conquistada dentro do jogo
	[SerializeField] private int CoinDp;
	public string personagemSelecionado;

	public User(int id, string name, string email, int maxslot, int coinap, int coindp)
	{
		Id = id;
		Name = name;
		Email = email;
		Maxslot = maxslot;
		CoinAp = coinap;
		CoinDp = coindp;

	}

	internal string GetName()
	{
		return Name;
	}

	internal int GetCoinAp()
	{
		return CoinAp;

	}

	internal int GetCoinDp()
	{
		return CoinDp;

	}
}
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
	[SerializeField] private int Dp;
	[SerializeField] private int Ap;

	public string personagemSelecionado;

	public User(int id, string name, string email, int maxslot, int dp, int ap)
	{
		Id = id;
		Name = name;
		Email = email;
		Maxslot = maxslot;
		Dp = dp;
		Ap = ap;
	}

	internal string GetName()
	{
		return Name;
	}
}
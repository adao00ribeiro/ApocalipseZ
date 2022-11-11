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
	[SerializeField] private int id;
	[SerializeField] private string username;
	[SerializeField] private string password;
	[SerializeField] private string email;
	[SerializeField] private int maxslot;
	[SerializeField] private int dp;
	[SerializeField] private int ap;

	public List<InfoPersonagem> PersonagensCriados;
	public InfoPersonagem personagemSelecionado;
	public List<int> ListIDCharacterComprados = new List<int>();
	public List<int> ListIDSkinsComprados = new List<int>();

	public User ( StructUser structuser )
	{
		id = structuser.id;
		username = structuser.username;
		password = structuser.password;
		email = structuser.email;
		maxslot = structuser.maxslot;
		dp = structuser.dp;
		ap = structuser.ap;
	}
	public int Id { get => id; set => id = value; }
	public string Username { get => username; set => username = value; }
	public string Password { get => password; set => password = value; }
	public string Email { get => email; set => email = value; }
	public int Maxslot { get => maxslot; set => maxslot = value; }
	public int Dp { get => dp; set => dp = value; }
	public int Ap { get => ap; set => ap =  value ; }
}
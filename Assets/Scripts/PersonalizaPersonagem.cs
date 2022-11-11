using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Categorias
{
	CABELO,
	CAMISETAS,
	LUVAS,
	CALCA,    
	SAPATOS
}
public struct Equipado
{
   
	public int Cabelos;
	public int Camisetas;
	public int Luvas;
	public int Calcas;
	public int Sapatos;
}
[System.Serializable]
public struct PersonalizaEquipamentos{
   
	public GameObject[] item;
	public int idItemSetado;
	public void Ativa(int id)
	{
		idItemSetado = id;
		Desativa();
		item[id].SetActive(true);
	}
	public void Desativa()
	{
		for (int i = 0; i < item.Length; i++)
		{
			item[i].SetActive(false);
		}
	}
}
[System.Serializable]
public struct InfoPersonagem
{
	public int IDPrefab;
	public int idCabelos;
	public int idCamisetas;
	public int idLuvas;
	public int idCalcas;
	public int idSapatos;

	internal void teste()
	{
		IDPrefab = 4;
		idCabelos = 2;
		idCamisetas = 0;
		idLuvas = 0;
		idCalcas = 0;
		idSapatos = 0;
	}
}

public class PersonalizaPersonagem : MonoBehaviour
{
	public int idprefab;
	public PersonalizaEquipamentos Cabelos;
	public PersonalizaEquipamentos Camisetas;
	public PersonalizaEquipamentos Luvas;
	public PersonalizaEquipamentos Calcas;
	public PersonalizaEquipamentos Sapatos;

	public Equipado equipado;
	private void Start()
	{
		SetaItem(Categorias.CABELO,0);
		SetaItem(Categorias.CAMISETAS, 0);
		SetaItem(Categorias.LUVAS, 0);
		SetaItem(Categorias.CALCA, 0);
		SetaItem(Categorias.SAPATOS, 0);
	}

	public void SetaItem(Categorias categoria,int id)
	{
		switch (categoria)
		{
		case Categorias.CABELO:
			equipado.Cabelos = id;
			SetCabelo(id);
			break;
		case Categorias.CAMISETAS:
			equipado.Camisetas = id;
			SetCamiseta(id);
			break;
		case Categorias.LUVAS:
			equipado.Luvas = id;
			SetLuvas(id);
			break;
		case Categorias.CALCA:
			equipado.Calcas = id;
			SetCalca(id);
			break;
		case Categorias.SAPATOS:
			equipado.Sapatos = id;
			SetSapatos(id);
			break;
		default:
			break;
		}
	}
	private void SetCabelo(int id)
	{
		Cabelos.Ativa(id);
	}
	private void SetCamiseta(int id)
	{
		Camisetas.Ativa(id);
	}
	private void SetLuvas(int id)
	{
		Luvas.Ativa(id);
	}
	private void SetCalca(int id)
	{
		Calcas.Ativa(id);
	}
	private void SetSapatos(int id)
	{
		Sapatos.Ativa(id);
	}

	public InfoPersonagem GetInfoPersonagem()
	{
		InfoPersonagem novo = new InfoPersonagem();
		novo.IDPrefab = idprefab;
		novo.idCabelos =    Cabelos.idItemSetado;
		novo. idCamisetas = Camisetas.idItemSetado;
		novo. idLuvas =     Luvas.idItemSetado;
		novo. idCalcas =    Calcas.idItemSetado;
		novo.idSapatos = Sapatos.idItemSetado;

		return novo;
	}
}
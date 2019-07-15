﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Investissement : MonoBehaviour
{
    [SerializeField] Text joueur;
    [SerializeField] Text central;
    [SerializeField] GameObject verts;
    [SerializeField] GameObject rouges;
    [SerializeField] Image decideur;
    [SerializeField] GameObject canvas_jetons;
    [SerializeField] GameObject canvas_choix_cartes;
    [SerializeField] GameObject canvas_fin;

    short publicID = 1016;
    short nextID = 1015;

    // Start is called before the first frame update
    void Start()
    {
        JoueurStatic.Client.RegisterHandler(publicID, OnPublicReceived);
        JoueurStatic.Client.RegisterHandler(nextID, OnWaitReceived);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        if (JoueurStatic.Langue == "FR")
        {
            joueur.text = "Joueur " + JoueurStatic.Numero;
            central.text = "Investissements";
        } else
        {
            joueur.text = "Player " + JoueurStatic.Numero;
            central.text = "Investments";
        }

        for (int i = 0; i < rouges.transform.childCount; i++)
        {
            rouges.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (JoueurStatic.IsPrive)
        {
            decideur.sprite = Resources.Load<Sprite>(JoueurStatic.Langue + "/Decideurs/DecideurPrive");
            decideur.gameObject.SetActive(true);
            central.gameObject.SetActive(false);
            int nb = 5;
            if (JoueurStatic.NbJoueurs == 6)
                nb = 6;
            for (int i = 0; i < nb; i++)
            {
                verts.transform.GetChild(i).gameObject.SetActive(true);
            }
        } else
        {
            central.gameObject.SetActive(true);
            decideur.gameObject.SetActive(false);
            for (int i = 0; i < verts.transform.childCount; i++)
            {
                verts.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void OnPublicReceived(NetworkMessage netMsg)
    {
        if (JoueurStatic.IsPublic)
        {
            central.gameObject.SetActive(false);
            decideur.sprite = Resources.Load<Sprite>(JoueurStatic.Langue + "/Decideurs/DecideurPublic");
            decideur.gameObject.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                verts.transform.GetChild(i).gameObject.SetActive(true);
            }

            int nb = 1;
            if (JoueurStatic.NbJoueurs == 6)
                nb = 2;

            for (int j = 0; j < nb; j++)
            {
                rouges.transform.GetChild(j).gameObject.SetActive(true);
            }
        } else
        {
            central.gameObject.SetActive(true);
            decideur.gameObject.SetActive(false);
            for (int i = 0; i < verts.transform.childCount; i++)
            {
                verts.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void OnWaitReceived(NetworkMessage netMsg)
    {
        string msg = netMsg.ReadMessage<MyStringMessage>().s;
        if (msg.Equals("next"))
        {
            canvas_jetons.SetActive(false);
            canvas_choix_cartes.SetActive(true);
        }
        else
        {
            canvas_jetons.SetActive(false);
            canvas_fin.SetActive(true);
        }
    }
}
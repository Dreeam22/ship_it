using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

static public class SaveSystem
{
    static public void Save()
    {

        BinaryFormatter bf = new BinaryFormatter(); //Fonction qui permet de serialiser
        string path = Path.Combine(Application.persistentDataPath, "player.fun"); 

        FileStream file = File.Create(path); //Crée le fichier (Donc réécrit si déjà présent)
        bf.Serialize(file, Storydata.ships); //Met la list Ships dans le fichier
        file.Close(); //ferme l'édieur de fichier
    }

    static public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "player.fun");
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open); //ouvre le fichier
            Storydata.ships = (List<int>)bf.Deserialize(file); //Attribue les données dans la liste de ships
            file.Close();
        }
        else
        {
            Debug.Log("Save file not found in" + path);           
        }
    }
}

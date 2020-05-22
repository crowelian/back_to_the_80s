using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Serializable so we can see this in the inspector (when used as an array item)
[Serializable]
public class HighscoreItem
{

    public int id;
    public string name;
    public int score;

    
}

[Serializable]
public class HighscoreItemArray {
    public HighscoreItem[] highscores;
}

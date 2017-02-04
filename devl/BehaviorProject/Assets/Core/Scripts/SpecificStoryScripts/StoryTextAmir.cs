using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StoryTextAmir : MonoBehaviour {

    public string[] sequence1 = { "Amir, Benjamin, and Caleb are worried about their lost friend Imran. They venture into the wilderness to find their lost friend." };
    public string[] sequence2 = { "Only one of the men return to their village." };
    public string[] sequence3 = { "Although the four are lifelong friends, their families do not get along because of religious differences." };
    public string[] sequence4 = { "One family hates the family of the survivor. This family accuses the survivor of murdering their member." };
    public string[] sequence5 = { "The survivor family, offended by the accusation, argues with the accusing family and insults their deceased." };
    public string[] sequence6 = { "The two families plot to murder the survivor. They do so successfully." };
    public string[] sequence7 = { "The accusing family demands that the survivor go back to the wild to find the bodies of their deceased. " };
    public string[] sequence8 = { "He complies, but does not return." };

    

    
    public Dictionary<string, string[]> texts;

    void Start()
    {

        texts = new Dictionary<string, string[]>();
        texts.Add("sequence1", sequence1);
        texts.Add("sequence2", sequence2);
        texts.Add("sequence3", sequence3);
        texts.Add("sequence4", sequence4);
        texts.Add("sequence5", sequence5);
        texts.Add("sequence6", sequence6);
        texts.Add("sequence7", sequence7);
        texts.Add("sequence8", sequence8);
    }

    public Dictionary<string, string[]> getTextDictionary() { 
        return this.texts; }
}

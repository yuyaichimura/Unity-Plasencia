using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;

public class NPCUtils {

    #region Members
    private static Dictionary<string, Condition> mConditions;
    private static Dictionary<string, Modifier> mModifier;
    private static Dictionary<string, NPCEvent> mNPCEvents;
    private static Dictionary<string, Context> mContexts;
    #endregion

    public static void LoadBehaviors() {
        try {
            TextAsset ta = (TextAsset)Resources.Load("behaviors");
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(ta.text);
            XmlNodeList nodes = xml.SelectNodes("/behaviors/*");
            foreach (XmlNode node in nodes) {
                XmlNodeList children = node.ChildNodes;
                switch (node.Name) {
                    case "events":
                        foreach (XmlNode eventNode in children) {
                            if(eventNode.Name == "event")
                                ProcessNPCEvent(eventNode);
                        }
                        break;
                    case "conditions":
                        foreach (XmlNode conditionNode in children) {
                            if(conditionNode.Name == "condition")
                                ProcessCondition(conditionNode);
                        }
                        break;
                    case "modifiers":
                        foreach (XmlNode modifierNode in children) {
                            if (modifierNode.Name == "modifier")
                                ProcessModifier(modifierNode);

                        }
                        break;
                    case "contexts":
                        foreach (XmlNode contextNode in children) {
                            if (contextNode.Name == "context")
                                ProcessContext(contextNode);
                        }
                        break;
                }
            }
        }
        catch (Exception e) {
            Debug.Log("Failed to find or parse behaviors.xml - " + e.Message);
        }
    }

    #region Implementation
    private static void ProcessNPCEvent(XmlNode pNode) {
        try {
            XmlNodeList children = pNode.ChildNodes;
            Dictionary<string, Condition> conditions = new Dictionary<string, Condition>();
            string eventName = pNode["name"].InnerText;
            string behavior = pNode["behavior"].InnerText;
            NPCEventPriority priority = NPCEventPriority.Individual; // default value
            switch (pNode["priority"].InnerText) {
                case "Individual":
                    priority = NPCEventPriority.Individual;
                    break;
                case "Social":
                    priority = NPCEventPriority.Social;
                    break;
                case "User": case "UserCommand":
                    priority = NPCEventPriority.UserCommand;
                    break;
                case "SelfPreservation": case "Preservation":
                    priority = NPCEventPriority.SelfPreservation;
                    break;
            }
            NPCEventType type = NPCEventType.NEUTRAL; // default value
            switch (pNode["type"].InnerText) {
                case "Neutral":
                    type = NPCEventType.NEUTRAL;
                    break;
                case "Global":
                    type = NPCEventType.GLOBAL;
                    break;
                case "Targeted":
                    type = NPCEventType.TARGETED;
                    break;
            }

            List<Modifier> mods = new List<Modifier>();

            /* TODO - handle conditions and modifiers */
            foreach (XmlNode n in children) {
                if (n.Name == "condition") {
                    if (NPCController.mAvailableConditions.ContainsKey(n.InnerText)) {
                        Condition c = NPCController.mAvailableConditions[n.InnerText];
                        // conditions.Add(c.Name, c);
                        conditions.Add(n.InnerText, c);
                    }
                    else { Debug.Log("Undefined Condition: " + n.InnerText + " - for event: " + eventName); }
                }
                else if (n.Name == "modifier") {
                    if (NPCController.mAvailableModifiers.ContainsKey(n.InnerText)) {
                        mods.Add(NPCController.mAvailableModifiers[n.InnerText]);
                        // e.AddModifier(NPCController.mAvailableModifiers[n.InnerText]);
                    }
                    else { Debug.Log("Undefined Modifier: " + n.InnerText + " for event: " + eventName); }
                }
            }
            /* -- */

            float f = -1f;
            bool random = false;
            if (pNode["random"] != null) {
                try {
                    f = float.Parse(pNode["random"].InnerText);
                    random = true;
                }
                catch (Exception ex) { }
            }

            NPCEvent e = null;
            if (!random) e = new NPCEvent(eventName, NPCController.mAvailableActions[behavior], conditions, priority, type);
            else {
                e = new NPCEvent(eventName, NPCController.mAvailableActions[behavior], conditions, priority, type, f);
            }

            foreach (Modifier m in mods) { // this is non-sensical but it will be like this for now
                e.AddModifier(m);
            }

            if (pNode["participants"] != null) {
                try {
                    e.Participants = int.Parse(pNode["participants"].InnerText);
                }
                catch (Exception ex) { }
            }

            if (!NPCController.mAvailableEvents.ContainsKey(e.Name)) {
                NPCController.mAvailableEvents.Add(e.Name, e);
            }
            else throw new Exception("duplicated event: " + e.Name);
        } catch (Exception e) {
            Debug.Log("Error instantiating XML-defined NPCEvent - "+e.Message);
            return;
        }
    }

    private static void ProcessCondition(XmlNode pNode) {
        Condition c = new Condition();
        try {
            c.Name = pNode["name"].InnerText;
            // there are many many ways to make this more elegant and sensical but now, is not the time.
            switch (pNode["type"].InnerText) {
                case "bool":
                    c.Type = typeof(bool);
                    break;
                case "NPC":
                case "NPCController":
                    c.Type = typeof(NPCController);
                    break;
                case "float":
                    c.Type = typeof(float);
                    break;
                case "string":
                    c.Type = typeof(string);
                    break;
            }
            if (pNode["targetvalue"] != null) {
                string val = pNode["targetvalue"].InnerText;
                try {
                    c.Target = float.Parse(val);
                }
                catch (Exception e) {
                    try {
                        try {
                            bool v = Boolean.Parse(val);
                            c.TargetValue = v;
                        }
                        catch (Exception ex2) {
                            c.TargetValue = val;
                        }
                    }
                    catch (Exception ex) { c.TargetValue = null; }
                }
            }
            if (pNode["targeted"] != null) {
                try {
                    c.IsTargeted = Boolean.Parse(pNode["targeted"].InnerText);
                }
                catch (Exception e) { c.IsTargeted = false; }
            }
            if (pNode["trait"] != null) {
                c.Trait = pNode["trait"].InnerText;
                c.Matching = true;
                c.TargetValue = true;
                if (pNode["invert"] != null) {
                    c.Inverted = Boolean.Parse(pNode["invert"].InnerText);
                }
            }
            string key = pNode.Attributes["id"].Value;
            if (pNode["mandatory"] != null) {
                try {
                    c.Mandatory = Boolean.Parse(pNode["mandatory"].InnerText);
                }
                catch (Exception e) { }
            }

            XmlNodeList children = pNode.ChildNodes;
            foreach (XmlNode n in children) {
                if (n.Name == "condition") {
                    // this is a nested condition
                    c.NestedConditions.Add(NPCController.mAvailableConditions[n.InnerText]);
                }
            }

            if (!NPCController.mAvailableConditions.ContainsKey(key)) {
                NPCController.mAvailableConditions.Add(key, c);
            }
            else {
                // string key = pNode.Attributes["id"].Value;
                key = c.IsTargeted ? c.Name : c.Name + "-Self";
                if (!NPCController.mAvailableConditions.ContainsKey(key)) NPCController.mAvailableConditions.Add(key, c);
                else throw new Exception("Duplicated Condition KEY in XML: " + c.Name);
            }
            
        }
        catch (Exception e) {
            Debug.Log("Error instantiating XML-defined Condition: " + c.Name);
            return;
        }
    }

    private static void ProcessModifier(XmlNode pNode) {
        Modifier m = null;
        try {

            string keyName = pNode.Attributes["name"].Value;
            string modName = pNode["trait"].InnerText;
            float value = float.Parse(pNode["value"].InnerText);
            ModifierType type = ModifierType.INCREMENTOR; // default value
            switch (pNode["type"].InnerText) {
                case "increment":
                    type = ModifierType.INCREMENTOR;
                    break;
                case "decrement":
                    type = ModifierType.DECREMENTOR;
                    break;
                case "neutral":
                    type = ModifierType.NEUTRAL;
                    break;
            }
            m = new Modifier(modName, type, value);
            bool persistent;
            if (pNode["persistent"] != null) {
                persistent = Boolean.Parse(pNode["persistent"].InnerText);
                m.Persistent = persistent;
            }
            NPCController.mAvailableModifiers.Add(keyName, m);
        }
        catch (Exception e) {
            Debug.Log("Error instantiating XML-defined Modifier: " + m.Name);
            return;
        }
    }

    private static void ProcessContext(XmlNode pNode) {
        Context c = new Context();
        try {
            c.Name = pNode["name"].InnerText;
            XmlNodeList children = pNode.ChildNodes;
            foreach (XmlNode n in children) {
                if (n.Name == "modifier") {
                    try {
                        c.AddModifier(NPCController.mAvailableModifiers[n.InnerText]);
                    }
                    catch (Exception e) { /* Catch potentially failing modifiers and duplicates */
                        Debug.Log("Error adding modifier for context: " + c.Name);
                    }
                }
            }
            if (!NPCController.mAvailableContexts.ContainsKey(c.Name)) {
                NPCController.mAvailableContexts.Add(c.Name, c);
            }
        }
        catch (Exception e) {
            Debug.Log("Error instantiating XML-defined Context" + pNode["name"]);
            return;
        }
    }
    #endregion
    /* TODO - do not forget to change deliberate for matching, in general */

    
}

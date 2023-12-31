﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Subtegral.DialogueSystem.Editor
{
    public class NodeSearchWindow : ScriptableObject,ISearchWindowProvider
    {
        private EditorWindow window;
        private StoryGraphView graphView;

        private Texture2D indentationIcon;
        public void Configure(EditorWindow window,StoryGraphView graphView)
        {
            this.window = window;
            this.graphView = graphView; 
            //Transparent 1px indentation icon as a hack
            indentationIcon = new Texture2D(1,1);
            indentationIcon.SetPixel(0,0,new Color(0,0,0,0));
            indentationIcon.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
                new SearchTreeGroupEntry(new GUIContent("Dialogue"), 1),
                new SearchTreeEntry(new GUIContent("Dialogue Node", indentationIcon))
                {
                    level = 2, userData = new DialogueNode()
                },
                new SearchTreeEntry(new GUIContent("Speech Node", indentationIcon))
                {
                    level = 2, userData = new SpeechNode()
                },
                new SearchTreeEntry(new GUIContent("Event Node", indentationIcon))
                {
                    level = 2, userData = new EventNode()
                },
                //Add other types of nodes here
            };

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            //Editor window-based mouse position
            Vector2 mousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent,
                context.screenMousePosition - window.position.position);
            Vector2 graphMousePosition = graphView.contentViewContainer.WorldToLocal(mousePosition);
            switch (SearchTreeEntry.userData)
            {
                case DialogueNode dialogueNode:
                    graphView.CreateNewDialogueNode("Dialogue Node", graphMousePosition);
                    return true;
                case SpeechNode speechNode:
                    graphView.CreateSpeechNode("Speech Node", graphMousePosition);
                    return true;
                case EventNode eventNode:
                    graphView.CreateEventNode("Event Node", graphMousePosition);
                    return true;

                    // and here 
            }
            return false;
        }
    }
}
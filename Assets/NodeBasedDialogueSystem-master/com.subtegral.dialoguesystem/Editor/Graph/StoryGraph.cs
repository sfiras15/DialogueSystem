using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Subtegral.DialogueSystem.DataContainers;

namespace Subtegral.DialogueSystem.Editor
{
    public class StoryGraph : EditorWindow
    {
        private string fileName = "New Narrative";

        private StoryGraphView graphView;
        [MenuItem("Graph/Narrative Graph")]
        public static void CreateGraphViewWindow()
        {
            var window = GetWindow<StoryGraph>();
            window.titleContent = new GUIContent("Narrative Graph");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
        }

        private void ConstructGraphView()
        {
            graphView = new StoryGraphView(this)
            {
                name = "Narrative Graph",
            };
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();
            var fileNameTextField = new TextField("File Name:");

            fileNameTextField.SetValueWithoutNotify(fileName);
            // To update the visual of the textField with the value
            fileNameTextField.MarkDirtyRepaint();
            // Calls the lambda expression when the value of the text changes 
            fileNameTextField.RegisterValueChangedCallback(evt => fileName = evt.newValue);

            toolbar.Add(fileNameTextField);
            // RequestDataOperation is the function that will be called if we click on the Button
            toolbar.Add(new Button(() => RequestDataOperation(true)) {text = "Save Data"});
            toolbar.Add(new Button(() => RequestDataOperation(false)) {text = "Load Data"});
            rootVisualElement.Add(toolbar);
        }

        private void RequestDataOperation(bool save)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var saveUtility = GraphSaveUtility.GetInstance(graphView);
                if (save)
                    saveUtility.SaveGraph(fileName);
                else
                    saveUtility.LoadNarrative(fileName);
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid File name", "Please Enter a valid filename", "OK");
            }
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }
    }
}
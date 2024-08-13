using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Bap.State_Machine
{
    public class StateCreator : VisualElement
    {
        #region To expose in UI Builder

        public new class Uxmlfactory : UxmlFactory<StateCreator, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }

        #endregion

        private TextField name => this.Q<TextField>("TextField--Name");
        private Toggle isRoot => this.Q<Toggle>("Toogle--IsRoot");
        private Button createBtn => this.Q<Button>("Button--Create");

        private string path;

        private string tempateContent = "using UnityEngine;\n\nnamespace Nguyen.State_Machine\n{{\n    " +
                                        "public class {0} : BaseState\n    {{\n        " +
                                        "public {0}(StateMachine ctx, StateFactory factory) : base(ctx, factory)\n        {{\n            _isRoot = {1};\n        }}\n\n        " +
                                        "public override void Enter()\n        {{\n            \n        }}\n\n        " +
                                        "public override void UpdateState()\n        {{\n            " +
                                        "CheckSwitchState();\n        }}\n\n        " +
                                        "public override void Exit()\n        {{\n            \n        }}\n\n        " +
                                        "protected override void CheckSwitchState()\n        {{\n            \n        }}\n\n        " +
                                        "public override void InitializeSubState()\n        {{\n            \n        }}\n    }}\n}}";

        public StateCreator()
        {
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Resources/Uxml/CreateNewState.uxml");
            visualTree.CloneTree(this);

            createBtn.clicked += HandleCreateScript;
        }

        private void HandleCreateScript()
        {
            string correctedName = name.value.Replace(" ", "");

            if (string.IsNullOrEmpty(name.value))
            {
                LogWarningBox("Name cannot be empty");
                return;
            }

            if (path == string.Empty)
            {
                path = EditorUtility.SaveFilePanelInProject("Create new State Script", correctedName, "cs",
                    "Save State Script");
            }
            else
            {
                path = EditorUtility.SaveFilePanel("Create new State Script", path, correctedName, "cs");
            }

            string content = string.Format(tempateContent, correctedName, isRoot.value.ToString().ToLower());
            File.WriteAllText(path, content);
            name.value = string.Empty;
            isRoot.value = false;
            AssetDatabase.Refresh();
        }

        private void LogWarningBox(string message)
        {
            EditorUtility.DisplayDialog("Warning", message, "OK");
        }
    }
}
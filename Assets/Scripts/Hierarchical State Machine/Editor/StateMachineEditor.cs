using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Bap.State_Machine;

namespace Bap.State_Machine
{
    [CustomEditor(typeof(HierarchicalStateMachine))]

    public class StateMachineEditor : Editor
    {
        [SerializeField] private VisualTreeAsset m_visualTreeAsset;

        private VisualElement root;

        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement();
            m_visualTreeAsset.CloneTree(root);

            StateCreator stateCreator = new StateCreator();

            root.Add(stateCreator);

            return root;
        }
    }
}
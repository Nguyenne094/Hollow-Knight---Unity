using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(ParallaxEffect)), CanEditMultipleObjects]
public class ParallaxEffectEditor : Editor
{
    private TextField usageTextField;
    
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        
        InspectorElement.FillDefaultInspector(root, serializedObject, this);
        
        usageTextField = new TextField("Usage");
        usageTextField.value = "The parallax coefficient is determined from the z-axis value.";
        usageTextField.isReadOnly = true;
        usageTextField.focusable = false;
        usageTextField.style.opacity = 0.7f;
        root.Add(usageTextField);
        
        return root;
    }
}
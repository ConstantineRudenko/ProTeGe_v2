using UnityEngine;
using System.Collections;
using ProTeGe.TextureProcessors;

public class PropertyDisplayController : MonoBehaviour
{
    public UnityEngine.UI.Slider valueSlider;
    public UnityEngine.UI.InputField maxInputField;

    [HideInInspector]
    public NodeController connectedNode;
    [HideInInspector]
    public string connectedProperty;

    protected void Start(){
		if (maxInputField != null) {
			maxInputField.onValidateInput = delegate (string text, int index, char c) {
				if(c == '-')
					return '\0';

				return c;
			};
		}
	}

	public bool boolValue {
		get { 
			if (connectedNode == null)
				return false;
			return connectedNode.processor [connectedProperty] != 0;
		}
		set { 
			if (connectedNode != null)
				connectedNode.processor [connectedProperty] = value ? 1 : 0;
		}
	}

	public float floatValue {
		set {
			if (connectedNode != null)
				connectedNode.processor [connectedProperty] = value;
		}
		get {
			if (connectedNode == null)
				return 0;
			return connectedNode.processor [connectedProperty];
		}
	}

	public int intValue {
		set {
			if (connectedNode != null)
				connectedNode.processor [connectedProperty] = value;
		}
		get {
			if (connectedNode == null)
				return 0;
			return Mathf.RoundToInt(connectedNode.processor [connectedProperty]);
		}
	}

	public string floatSavedLimit {
		set {
			float x;
			float.TryParse (value, out x);

			valueSlider.maxValue = x;

			if (connectedNode != null) 
				((ProcessorProperty_float)connectedNode.processor.GetPropertyByName(connectedProperty)).savedLimit = valueSlider.maxValue;
		}
	}

	public string stringValue {
		set { 
			int x;
			if (int.TryParse ((value), out x))
				floatValue = x;
		}
	}
}

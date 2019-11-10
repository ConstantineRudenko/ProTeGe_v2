using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProTeGe.TextureProcessors;

public class NodeGuiController : MonoBehaviour
{
    public GameObject floatPrefab;
    public GameObject dropDownPrefab;
    public GameObject fixedPrefab;
    public GameObject boolPrefab;
    public GameObject intPrefab;
    public GameObject buttonPrefab;

    public NodeController selectedNode {
		get{ return _selectedNode; }
		set {
			NodeController oldSelection = _selectedNode;
			_selectedNode = value;

			if (selectedNode != oldSelection) {
				if(selectedNode != null)
					selectedNode.processor.OnWake ();

				if (oldSelection != null) {
					oldSelection.processor.OnSnooze ();
				}
				
				UpdateGuiPanel ();	
			}
		}
	}

	protected void Awake ()
	{
		propertyDisplayList = new List<PropertyDisplayController> ();
		// SnoozeQueue = new Dictionary<NodeController, float> ();
	}

	public void UpdateGuiPanel ()
	{
		foreach (PropertyDisplayController ctrl in propertyDisplayList) {
			GameObject.Destroy (ctrl.gameObject);
		}
		propertyDisplayList.Clear ();
		if (selectedNode != null) {
			GameObject propertyDisplay;

			foreach (ProcessorProperty property in selectedNode.processor.properties) {
				if (property.hidden == true)
					continue;
				
				switch (property.type) {
				case ProcessorProperty.ProcessorPropertyType.Float:
					propertyDisplay = GameObject.Instantiate (floatPrefab);

					UnityEngine.UI.Slider s = propertyDisplay.transform.Find ("Panel").Find ("Slider").GetComponent<UnityEngine.UI.Slider> ();
					s.maxValue = ((ProcessorProperty_float)property).savedLimit;

					propertyDisplay.transform.Find("max value panel").Find ("max value").GetComponent<UnityEngine.UI.InputField> ().text = s.maxValue.ToString ();

					s.value = selectedNode.processor [property.name];

					if (s.value > s.maxValue)
						throw new System.Exception ("value exceeds saved limit");

					break;
				case ProcessorProperty.ProcessorPropertyType.Dropdown:
					propertyDisplay = GameObject.Instantiate (dropDownPrefab);
					UnityEngine.UI.Dropdown d = propertyDisplay.transform.Find ("Dropdown").GetComponent<UnityEngine.UI.Dropdown> ();
					d.options.Clear ();
					foreach (string optionName in ((ProcessorProperty_dropdown)property).options)
						d.options.Add (new UnityEngine.UI.Dropdown.OptionData (optionName));

					d.value = Mathf.RoundToInt (selectedNode.processor [property.name]);
					d.captionText.text = ((ProcessorProperty_dropdown)property).options [d.value];	// nice bug unity
					break;
				case ProcessorProperty.ProcessorPropertyType.Fixed:
					propertyDisplay = GameObject.Instantiate (fixedPrefab);
					propertyDisplay.transform.Find ("Slider").GetComponent<UnityEngine.UI.Slider> ().value = selectedNode.processor [property.name];
					break;
				case ProcessorProperty.ProcessorPropertyType.Bool:
					propertyDisplay = GameObject.Instantiate (boolPrefab);
					propertyDisplay.transform.Find ("Toggle").GetComponent<UnityEngine.UI.Toggle> ().isOn = (selectedNode.processor [property.name] != 0.0f);
					break;
				case ProcessorProperty.ProcessorPropertyType.Integer:
					propertyDisplay = GameObject.Instantiate (intPrefab);
					propertyDisplay.transform.Find ("InputField").GetComponent<UnityEngine.UI.InputField> ().text = ((int)selectedNode.processor [property.name]).ToString ();
					break;
				case ProcessorProperty.ProcessorPropertyType.Button:
					propertyDisplay = GameObject.Instantiate (buttonPrefab);
					propertyDisplay.transform.Find ("Button").Find ("Text").GetComponent<UnityEngine.UI.Text> ().text = property.name;
					break;
				default:
					throw new System.NotImplementedException ("unknown property type");
				}
				
				propertyDisplay.transform.SetParent (transform, false);
				PropertyDisplayController ctrl = propertyDisplay.GetComponent<PropertyDisplayController> ();
				ctrl.connectedNode = selectedNode;
				ctrl.connectedProperty = property.name;
				propertyDisplayList.Add (ctrl);
				Transform nameTransform = propertyDisplay.transform.Find ("Name");
				if (nameTransform == null) {
					Transform panelTransform = propertyDisplay.transform.Find ("Panel");
					if (panelTransform)
						nameTransform = panelTransform.Find ("Name");
				}
				if (nameTransform)
					nameTransform.GetComponent<UnityEngine.UI.Text> ().text = property.name.ToLower ();
				propertyDisplay.SetActive (true);
			}
		}
	}

	protected void Update ()
	{
		foreach (PropertyDisplayController p in propertyDisplayList)
			p.gameObject.SetActive (p.connectedNode.processor.CheckPropertyEnabled (p.connectedProperty));
	}

	private List<PropertyDisplayController> propertyDisplayList;
	private Dictionary<NodeController, float> SnoozeQueue;
	private NodeController _selectedNode = null;

}

using UnityEngine;
using System.Collections;

using  ProTeGe.MenuLib;

public class MenuController : MonoBehaviour {

	public GameObject L1;
	public GameObject L2;
	public GameObject L3;
	public GameObject L4;
	public GameObject buttonTemplate;
	public UnityEngine.RectTransform backgroung;

	public MenuItem root;

	public void HoverButton (string name, int level)
	{
		while (currentLevel > level)
			DecreseLevel ();
		IncreaseLevel (name);
	}

	public void ClickMenuButton (string name, int level)
	{
		HoverButton (name, level);
		if (currentItem.action != null) {
			MenuItem.MenuAction action = currentItem.action;
			while (currentLevel > 0)
				DecreseLevel ();
			action.Invoke ();
			currentItem = root;
		}
	}

	public void OpenMenu ()
	{
		if (currentLevel == 0) {
			currentLevel = 1;

			UnityEngine.UI.LayoutElement el = currentPanelObject.GetComponent<UnityEngine.UI.LayoutElement> ();
			Rect rect = ((RectTransform)buttonTemplate.transform).rect;
			float btnHeight = rect.height;

			el.minHeight = btnHeight * root.MaxHeight;
			fillCurrentPanel ();
		}
	}

	protected void Start ()
	{
		currentItem = root;
	}

	protected void Update ()
	{
		int displayLevel = currentLevel;

		if (currentItem.action != null) //fucking bug
			displayLevel--;
		
		L1.SetActive (displayLevel > 0);
		L2.SetActive (displayLevel > 1);
		L3.SetActive (displayLevel > 2);
		L4.SetActive (displayLevel > 3);
		backgroung.gameObject.SetActive (currentLevel > 0);

		if (isMouseOver == false && Input.GetMouseButton (0))
			CloseMenu ();
	}

	private int currentLevel = 0;
	private MenuItem currentItem;
	private MenuItem _root;

	private GameObject currentPanelObject {
		get {
			switch (currentLevel) {
			case 0:
				return null;
			case 1:
				return L1;
			case 2:
				return L2;
			case 3:
				return L3;
			case 4:
				return L4;
			default:
				throw(new System.ArgumentException ("this panel does not excist: " + currentLevel.ToString ()));
			}
		}
	}

	private static Rect RectTransformToScreenSpace (RectTransform transform)
	{
		Vector2 size = Vector2.Scale (transform.rect.size, transform.lossyScale);
		return new Rect ((Vector2)transform.position - (size * 0.5f), size);
	}

	private void CloseMenu ()
	{
		while (currentLevel > 0)
			DecreseLevel ();
	}

	private void DecreseLevel ()
	{
		currentLevel--;
		currentItem = currentItem.parent;
		if (currentItem == null)
			currentItem = root;
	}

	private void IncreaseLevel (string name)
	{
		MenuItem nextItem = currentItem [name];

		if (nextItem == null) {
			// mouseover was called after click
			// the menu is already closed
			// current item is root
			// -> do not try to increase level

			return;
		}
		currentLevel++;
		currentItem = currentItem [name];
		fillCurrentPanel ();
	}

	private void fillCurrentPanel ()
	{
		foreach (Transform button in currentPanelObject.transform)
			GameObject.Destroy (button.gameObject);
		foreach (MenuItem x in currentItem.GetChildren()) {
			GameObject newButton = GameObject.Instantiate (buttonTemplate, buttonTemplate.transform.position, Quaternion.identity) as GameObject;
			RectTransform rectTransform = newButton.GetComponent<RectTransform> ();
			Vector3 pos = rectTransform.position;
			pos.z = 0;
			rectTransform.position = pos;
			newButton.transform.SetParent (currentPanelObject.transform, false);
			MenuButtonController ctrl = newButton.GetComponent<MenuButtonController> ();
			ctrl.itemName = x.name;
			ctrl.level = currentLevel;
			newButton.SetActive (true);
		}
	}

	private bool isMouseOver {
		get {
			Vector3[] corners = new Vector3[4];
			backgroung.GetWorldCorners (corners);
			Rect rect = new Rect (corners [0], corners [2] - corners [0]);
			return rect.Contains (Input.mousePosition);
		}
	}
}

using UnityEngine;
using System.Collections;

public abstract class A_MenuButtonController : MonoBehaviour {
	public UnityEngine.UI.Text textField;
	public MenuController menuCtrl;
	public int level;

	public abstract string itemName { get; set; }

	public abstract void OnMouseOver ();
	public abstract void OnClick();
}

public class MenuButtonController : A_MenuButtonController {
	public override string itemName{
		get{ 
			return textField.text;
		}
		set{
			textField.text = value;
		}
	}

	public override void OnMouseOver(){
		menuCtrl.HoverButton(itemName, level);
	}
	public override void OnClick(){
		menuCtrl.ClickMenuButton(itemName, level);
	}
}

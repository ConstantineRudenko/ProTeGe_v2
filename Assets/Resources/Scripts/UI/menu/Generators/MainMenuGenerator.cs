using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace MenuLib{
		namespace Generators{
			public class MainMenuGenerator : MenuGenerator {
				private void ShowMenu(GameObject menu){
					menu.SetActive (true);
					Globals.instance.clickBlockerActive = true;
				}

				public MainMenuGenerator(){
					root = new MenuItem("root", null);

					root.AddChild (new MenuItem ("file", null));

					root.AddChild(new MenuItem("options", delegate { Globals.instance.components.dialogManager.ShowDialog(DialogManager.Dialog.Options); }));
					root.AddChild(new MenuItem("help", delegate { Globals.instance.components.dialogManager.ShowDialog(DialogManager.Dialog.Help); }));
					root.AddChild(new MenuItem("about", delegate { Globals.instance.components.dialogManager.ShowDialog(DialogManager.Dialog.About); }));

					root["file"].AddChild(new MenuItem("new project", delegate {  ProjectManager.Reset(); } ));
					root["file"].AddChild(new MenuItem("open project", delegate { Globals.instance.components.dialogManager.ShowDialog(DialogManager.Dialog.Load); }));
					root["file"].AddChild(new MenuItem("save project", delegate { Globals.instance.components.fileDialogController.SaveToLastLocation(); } ));
					root["file"].AddChild(new MenuItem("save project as", delegate { Globals.instance.components.dialogManager.ShowDialog(DialogManager.Dialog.Save); }));
					root["file"].AddChild(new MenuItem("export textures", delegate { Globals.instance.components.dialogManager.ShowDialog(DialogManager.Dialog.Export); }));
				}
			}
		}
	}
}
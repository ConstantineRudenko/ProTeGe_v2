using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ProTeGe;

[ExecuteInEditMode]
public class FileDialogController : MonoBehaviour {

	public InputField pathTextInputField;
	public GameObject overwriteWarning;
	public Text labelOverwriteWarning;
	public GameObject panelError;
	public Text labelError;
	public RectTransform folderView;
	public RectTransform folderPrefab;
	public RectTransform filePrefab;
	public RectTransform parentFolderPrefab;
	public GameObject diskShortcutPrefab;
	public OutputNodeController outputNodeController;
	public Dropdown resolutionSelector;
	public InputField inputFieldFileName;
	public GameObject SubdialogCreateFolder;

	[HideInInspector]
	public string lastFileName = "";

	private List<GameObject> diskShortcutsList = new List<GameObject>();
	public FileDialogModeController modeCtrl;

	public void SaveExportPreviewResolution(){
		Globals.instance.resolutionIndex_export = resolutionSelector.value;
	}

	public string curPath {
		set{
			string path = System.Environment.ExpandEnvironmentVariables (value);

			if (Directory.Exists (path) == false) {
				pathTextInputField.text = curPath;
				return;
			}

			DirectoryInfo dirInfo = new DirectoryInfo (path);
			path = dirInfo.FullName;

			_curPath = path;
			pathTextInputField.text = path;

			UpdateView ();
		}
		get { return _curPath; }
	}

	private string _curPath;

	public void Reset(){
		OnDisable ();
		OnEnable ();
	}

	public void SaveToLastLocation(){
		Globals.instance.components.dialogManager.ShowDialog (DialogManager.Dialog.Save);

		if (lastFileName == "")
			return;

		inputFieldFileName.text = lastFileName;
		SaveProject (true);
	}

	void OnEnable () {
		resolutionSelector.value = Globals.instance.resolutionIndex_export;

		List<Transform> kids = new List<Transform> ();
		foreach (Transform k in folderView)
			if (k.gameObject.activeSelf)
				kids.Add (k);


		for (int i = 0; i < kids.Count; i++)
			DestroyImmediate (kids [i].gameObject);

		_curPath = pathTextInputField.text;

		UpdateView ();

		foreach (var drive in Directory.GetLogicalDrives()) {
			GameObject btn = GameObject.Instantiate (diskShortcutPrefab);
			var helper = btn.GetComponent<DiskShortcutButtonController> ();
			helper.path = drive;
			helper.buttonText.text = helper.path;
			btn.SetActive (true);
			btn.transform.SetParent(diskShortcutPrefab.transform.parent);
			diskShortcutsList.Add (btn);
		}
	}

	void Start(){
		char[] invalidChars = Path.GetInvalidFileNameChars();
		inputFieldFileName.onValidateInput += delegate(string input, int charIndex, char c){ 
			foreach(char invalid in invalidChars)
				if(invalid == c)
					return '\0';
			return c;
		};
	}

	void OnDisable () {
		inputFieldFileName.text = "";

		foreach (GameObject x in diskShortcutsList)
			GameObject.DestroyImmediate (x);
		diskShortcutsList.Clear ();

		List<Transform> kids = new List<Transform> ();
		foreach (Transform k in folderView)
			if (k.gameObject.activeSelf)
				kids.Add (k);


		for (int i = 0; i < kids.Count; i++)
			DestroyImmediate (kids [i].gameObject);

		overwriteWarning.SetActive (false);
		panelError.SetActive (false);

		SubdialogCreateFolder.SetActive (false);
	}

	private void UpdateView() {
		List<Transform> kids = new List<Transform> ();
		foreach (Transform k in folderView)
			if (k.gameObject.activeSelf)
				kids.Add (k);


		for (int i = 0; i < kids.Count; i++)
			DestroyImmediate (kids [i].gameObject);

		DirectoryInfo dirInfo = new DirectoryInfo (curPath);
		if (dirInfo.Parent != null) {
			GameObject folder = (GameObject)GameObject.Instantiate (parentFolderPrefab.gameObject, parentFolderPrefab.parent);
			folder.SetActive (true);
		}

		foreach (string folderPath in Directory.GetDirectories(curPath)) {
			DirectoryInfo info = new DirectoryInfo (folderPath);
			if ((info.Attributes & FileAttributes.Hidden) != 0)
				continue;

			GameObject folder = (GameObject)GameObject.Instantiate (folderPrefab.gameObject, folderPrefab.parent);
			folder.SetActive (true);
			folder.GetComponent<FolderButtonController> ().folderPath = folderPath;
			string[] folderPathSplit = folderPath.Split ('\\');
			folder.transform.Find ("Text").GetComponent<Text> ().text = folderPathSplit [folderPathSplit.Length - 1];
		}

		foreach (string filePath in Directory.GetFiles(curPath)) {
			FileInfo info = new FileInfo (filePath);
			if ((info.Attributes & FileAttributes.Hidden) != 0)
				continue;

			GameObject file = (GameObject)GameObject.Instantiate (filePrefab.gameObject, filePrefab.parent);
			file.SetActive (true);
			file.GetComponent<FileButtonController> ().filePath = filePath;

			if (modeCtrl.mode == FileDialogModeController.FileDialogMode.Export) {
				file.GetComponent<Button>().enabled = false;
				file.GetComponent<Image> ().enabled = false;
			}

			string[] filePathSplit = filePath.Split ('\\');
			file.transform.Find ("Text").GetComponent<Text> ().text = filePathSplit [filePathSplit.Length - 1];
		}
	}

	public void OnButtonClick(){
		ButtonAction (false);	
	}

	public void OnOverwriteButtonClick(){
		ButtonAction (true);
	}

	public void SelectFile(string filePath){
		switch (modeCtrl.mode) {
		case FileDialogModeController.FileDialogMode.Load:
			LoadFile (filePath);
			break;
		case FileDialogModeController.FileDialogMode.Save:
			FileInfo fileInfo = new FileInfo (filePath);
			inputFieldFileName.text = fileInfo.Name;
			SaveProject (false);
			break;
		case FileDialogModeController.FileDialogMode.Import:
			throw new System.NotImplementedException ();
		case FileDialogModeController.FileDialogMode.Export:
			throw new System.InvalidOperationException ("file buttons should be disabled");
		}
	}

	private void LoadFile(string path){
		ProTeGe.ProjectSaveInfo saveInfo;
		byte[] bytes;

		try{
			bytes = File.ReadAllBytes(path);
		} catch{
			labelError.text = "Could not open file:\n\n" + path;
			panelError.SetActive (true);
			return;
		}

		try{
			var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(ProTeGe.ProjectSaveInfo));
			saveInfo = (ProTeGe.ProjectSaveInfo)serializer.ReadObject(new MemoryStream(bytes));
		}
		catch{
			labelError.text = "File has wrong format:\n\n" + path;
			panelError.SetActive (true);
			return;
		}

		ProTeGe.ProjectManager.Load(saveInfo);

		Globals.instance.components.dialogManager.CloseDialog ();
	}

	private bool CheckDirectoryExists(){
		if (Directory.Exists (curPath) == false) {
			labelError.text = "selected directory does not exist\n" + curPath;
			panelError.SetActive (true);
			Reset ();
			return false;
		}
		return true;
	}

	private void ShowErrorFileSave(){
		labelError.text = "error while saving files to\n" + curPath;
		panelError.SetActive (true);
	}

	private void ButtonAction(bool force){
		switch (modeCtrl.mode) {
		case FileDialogModeController.FileDialogMode.Export:
			ExportTextures (force);
			break;
		case FileDialogModeController.FileDialogMode.Import:
			break;
		case FileDialogModeController.FileDialogMode.Save:
			SaveProject (force);
			break;
		case FileDialogModeController.FileDialogMode.Load:
			throw new System.InvalidOperationException ("file open action must be called through OpenFile function");
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
	}

	private void SaveProject(bool force){
		var saveData = ProTeGe.ProjectManager.Save ();
		var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer (typeof(ProTeGe.ProjectSaveInfo));

		if (CheckDirectoryExists () == false)
			return;

		if (inputFieldFileName.text == "") {
			labelError.text = "file name can not be empty";
			panelError.SetActive (true);
			return;
		}
		if (inputFieldFileName.text [0] == '.') {
			labelError.text = "file name can not start with a dot";
			panelError.SetActive (true);
			return;
		}

		var filePath = curPath + @"\" + inputFieldFileName.text;
		if (filePath.Length >= 4) {
			if (filePath.Substring (filePath.Length - 4, 4) != ".ptg") {
				if (filePath.Substring (filePath.Length - 1) != ".")
					filePath += ".";
				filePath += "ptg";
			}
		} else {
			if (filePath.Substring (filePath.Length - 1) != ".")
				filePath += ".";
			filePath += "ptg";
		}
		FileInfo fileInfo;
		try{
			fileInfo = new FileInfo (filePath);
		} catch{
			labelError.text = "file with this name could not be created";
			panelError.SetActive (true);
			return;
		}

		if (force == false) {
			if (fileInfo.Exists) {
				labelOverwriteWarning.text = "file will be overwritten:\n\n" + fileInfo.FullName;
				overwriteWarning.SetActive (true);
				return;
			}
		}
		try{
			using(FileStream stream = new FileStream (fileInfo.FullName, FileMode.Create)){
				serializer.WriteObject (stream, saveData);
			}
		}catch{
			ShowErrorFileSave ();
		}

		lastFileName = inputFieldFileName.text;
		Globals.instance.components.dialogManager.CloseDialog ();
	}

	private void ExportTextures(bool force){
		bool hasOneOutputConnected = false;
		hasOneOutputConnected |= outputNodeController.processor_albedo.inputs[0].isConnected;
		hasOneOutputConnected |= outputNodeController.processor_normalMap.inputs[0].isConnected;
		if (hasOneOutputConnected == false) {
			labelError.text = "no textures were exported\n" + "try connecting something to output node";
			panelError.SetActive (true);
			Reset ();
			return;
		}

		if (hasOneOutputConnected == false) {

		}

		if (CheckDirectoryExists () == false)
			return;

		string albedoPath = curPath + "\\albedo.png";
		string normalPath = curPath + "\\normal.png";
		string smoothnessPath = curPath + "\\smoothness.png";
		string metallicPath = curPath + "\\metallicity.png";
		string occlusionPath = curPath + "\\occlusion.png";
		string parallaxPath = curPath + "\\parallax.png";
		string emissionPath = curPath + "\\emission.png";
		bool albedoConnected = outputNodeController.processor_albedo.inputs[0].isConnected;
		bool normalsConnected = outputNodeController.processor_normalMap.inputs[0].isConnected;
		bool smoothnessConnected = outputNodeController.processor_smoothness.inputs[0].isConnected;
		bool metallicConnected = outputNodeController.processor_metallic.inputs[0].isConnected;
		bool parallaxConnected = outputNodeController.processor_parallax.inputs[0].isConnected;
		bool occlusionConnected = outputNodeController.processor_occlusion.inputs[0].isConnected;
		bool emissionConnected = outputNodeController.processor_emission.inputs[0].isConnected;
		string filesToOverwrite = "";
		List<string> listFilesToOverwrite = new List<string>();

		if (force == false) {
			bool fileExists = false;

			fileExists |= File.Exists (albedoPath);
			fileExists |= File.Exists (normalPath);
			fileExists |= File.Exists (smoothnessPath);
			fileExists |= File.Exists (metallicPath);
			fileExists |= File.Exists (occlusionPath);
			fileExists |= File.Exists (parallaxPath);
			fileExists |= File.Exists (emissionPath);

			if (fileExists) {
				if (File.Exists (albedoPath))
					listFilesToOverwrite.Add ("albedo.png");
				if (File.Exists (normalPath))
					listFilesToOverwrite.Add ("normal.png");
				if (File.Exists (smoothnessPath))
					listFilesToOverwrite.Add ("smoothness.png");
				if (File.Exists (metallicPath))
					listFilesToOverwrite.Add ("metallicity.png");
				if (File.Exists (occlusionPath))
					listFilesToOverwrite.Add ("occlusion.png");
				if (File.Exists (parallaxPath))
					listFilesToOverwrite.Add ("parallax.png");
				if (File.Exists (emissionPath))
					listFilesToOverwrite.Add ("emission.png");

				for (int i = 0; i < listFilesToOverwrite.Count - 1; i++)
					filesToOverwrite += listFilesToOverwrite [i] + "\n";
				filesToOverwrite += listFilesToOverwrite [listFilesToOverwrite.Count - 1];

				labelOverwriteWarning.text = "files will be overwritten in ";
				labelOverwriteWarning.text += curPath + "\n\n";
				labelOverwriteWarning.text += filesToOverwrite;
				overwriteWarning.SetActive (true);
				return;
			}
		}

		bool success = false;
		try{		
			byte[] bytes;
			ProTeGe_Texture texture;

			if(albedoConnected){
				texture = outputNodeController.processor_albedo.Generate (Globals.instance.textureSize_export);
				bytes = Globals.instance.RenderTextureToTexture2D(texture.renderTexture).EncodeToPNG();
				texture.Release ();
				File.WriteAllBytes (albedoPath, bytes);
			}

			if(normalsConnected){
				texture = outputNodeController.processor_normalMap.Generate (Globals.instance.textureSize_export);
				bytes = Globals.instance.RenderTextureToTexture2D(texture.renderTexture).EncodeToPNG();
				texture.Release ();
				File.WriteAllBytes (normalPath, bytes);
			}

			if(smoothnessConnected){
				texture = outputNodeController.processor_smoothness.Generate (Globals.instance.textureSize_export);
				bytes = Globals.instance.RenderTextureToTexture2D(texture.renderTexture).EncodeToPNG();
				texture.Release ();
				File.WriteAllBytes (smoothnessPath, bytes);
			}

			if(metallicConnected){
				texture = outputNodeController.processor_metallic.Generate (Globals.instance.textureSize_export);
				bytes = Globals.instance.RenderTextureToTexture2D(texture.renderTexture).EncodeToPNG();
				texture.Release ();
				File.WriteAllBytes (metallicPath, bytes);
			}

			if(occlusionConnected){
				texture = outputNodeController.processor_occlusion.Generate (Globals.instance.textureSize_export);
				bytes = Globals.instance.RenderTextureToTexture2D(texture.renderTexture).EncodeToPNG();
				texture.Release ();
				File.WriteAllBytes (occlusionPath, bytes);
			}

			if(parallaxConnected){
				texture = outputNodeController.processor_parallax.Generate (Globals.instance.textureSize_export);
				bytes = Globals.instance.RenderTextureToTexture2D(texture.renderTexture).EncodeToPNG();
				texture.Release ();
				File.WriteAllBytes (parallaxPath, bytes);
			}

			if(emissionConnected){
				texture = outputNodeController.processor_emission.Generate (Globals.instance.textureSize_export);
				bytes = Globals.instance.RenderTextureToTexture2D(texture.renderTexture).EncodeToPNG();
				texture.Release ();
				File.WriteAllBytes (emissionPath, bytes);
			}

			success = true;
		} catch{
			ShowErrorFileSave ();
		}
		if(success)
			Globals.instance.components.dialogManager.CloseDialog();
	}
}

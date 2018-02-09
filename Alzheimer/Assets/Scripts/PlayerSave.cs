using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_WEBGL && !UNITY_EDITOR
	using System.Runtime.InteropServices;
#endif

[System.Serializable]
public class PlayerSave	{

#if UNITY_WEBGL && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void SyncFiles();
#endif

	private static PlayerSave m_instance;
	public static PlayerSave Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = new PlayerSave();
			}
			return m_instance;
		}
	}

	public class Save
	{
		public string fileName;
		public string name;
		public Dictionary<string, int> quizzHightScore;
		public Dictionary<string, string> cloth;

		public Save (string fileName, string name)
		{
			this.fileName = fileName;
			this.name = name;
		}
	}


	public enum LastGameType
	{
		NoMiniGame,
		Quizz,
		Action,
		Reflexion
	}

	private string			m_saveFolder;
	private string			m_current_filePath;
	private List<Save>		m_saves;
	private int				m_LastId = 0;
	private Save			m_currentSave = null;

	private PlayerDatas		m_datas;

	public void Update(){
		if(Input.GetKeyDown(KeyCode.R)){
			ResetDatas();
		}
	}

	#region getter and setter

	public List<Save> SaveList { get { return m_saves; } }

	public LastGameType LastGame { get { return m_datas.lastGame; } set { m_datas.lastGame = value; } }	



	public void genderSet(string newGender){
		m_datas.gender = newGender;
	}

	public string genderGet(){
		return m_datas.gender;
	}

	public void skinColorSet(float r, float g, float b){
		m_datas.skinColor [0] = r;
		m_datas.skinColor [1] = g;
		m_datas.skinColor [2] = b;
	}

	public float[] skinColorGet(){
		return m_datas.skinColor;
	}

	public void hairColorSet(float r, float g, float b){
		m_datas.hairColor [0] = r;
		m_datas.hairColor [1] = g;
		m_datas.hairColor [2] = b;
	}

	public float[] hairColorGet(){
		return m_datas.hairColor;
	}

	public void eyeColorSet(float r, float g, float b){
		m_datas.eyeColor [0] = r;
		m_datas.eyeColor [1] = g;
		m_datas.eyeColor [2] = b;
	}

	public float[] eyeColorGet(){
		return m_datas.eyeColor;
	}

	public void lastEnvSet(string newEnv){
		m_datas.lastEnv = newEnv;
	}

	public string lastEnvGet(){
		return m_datas.lastEnv;
	}


	public void nameSet(string newName){

		m_datas.name = newName;
		if (m_currentSave != null)
			m_currentSave.name = newName;
	}

	public string nameGet(){
		return m_datas.name;
	}

	public void tutoStepAdd(){
		m_datas.tutoDone++;
	}

	public int tutoStepGet(){
		return (int)m_datas.tutoDone;
	}

	public void diamondsAdd(string area){
		m_datas.diamonds [area] = true;
		int zonesCompleted = diamondsGet();
	}

	public bool diamondsGet(string area)
	{
		return m_datas.diamonds[area];
	}

	public int diamondsGet(){
		int x = 0;
		foreach (string a in m_datas.diamonds.Keys) {
			if (m_datas.diamonds [a] == true) {
				x++;
			}
		}
		return x;
	}

	public void LastScoreSet(int f){
		m_datas.lastScore = f;
	}

	public int LastScoreGet()	{
		return m_datas.lastScore;
	}

	#endregion

	private PlayerSave()
    {
#if UNITY_EDITOR
		m_datas = new PlayerDatas();
        m_current_filePath = Application.persistentDataPath + "/MyPlayerDatas.sav";
#endif

#if UNITY_ANDROID || UNITY_IOS
		m_current_filePath = Application.persistentDataPath +"/MyPlayerDatas.sav";
		LoadDatas();
		
#else
		m_saves = new List<Save>();
		m_saveFolder = Application.persistentDataPath + "/saves/";
		if (Directory.Exists(m_saveFolder)) {
			string[] saves = Directory.GetFiles(m_saveFolder);
			for (int i = 0; i < saves.Length; i++)
			{
				string saveName = Path.GetFileName(saves[i]);
				m_current_filePath = m_saveFolder + saveName;
				LoadDatas();
				Save newSave = new Save(saveName, m_datas.name);
				m_saves.Add(newSave);
			}
		}
		else {
			Directory.CreateDirectory(m_saveFolder);
		}
		if (m_saves.Count > 0) {
			m_LastId = int.Parse(m_saves[m_saves.Count - 1].fileName.Split('.')[0]);
		}
#endif
	}

	public void NewSave() {

		m_LastId++;
		string newFileName = m_LastId + ".sav";
		m_current_filePath = m_saveFolder + newFileName;
		m_datas = new PlayerDatas();
		m_currentSave = new Save(newFileName, m_datas.name);
		m_saves.Add(m_currentSave);
		SaveDatas();
	}

	public void LoadSave (Save save) {
		m_currentSave = save;
		m_current_filePath = m_saveFolder + save.fileName;
		LoadDatas();
	}

	public void DeleteSave(Save save)
	{
		m_currentSave = null;
		m_saves.Remove(save);
		m_current_filePath = m_saveFolder + save.fileName;
		File.Delete(m_current_filePath);
		LoadDatas();
	}

	public void UnlockAllDiamonds() {
		m_datas.diamonds = new Dictionary<string, bool>(){ {"jungle",true},{"sahara", true},{"mediterranean", true} , {"ruins", true}, {"mountain",true},{"savane", true},{"mangrove", true},{"fleuve", true},{"village", true},{"urban", true},{"ocean",true}};
	}


	public void ResetDatas()
	{
		m_datas = new PlayerDatas ();
		SaveDatas ();
		m_datas = new PlayerDatas ();
	}


	public void SaveDatas (){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(m_current_filePath);
		bf.Serialize(file, m_datas);
		file.Close();
#if UNITY_WEBGL && !UNITY_EDITOR
		SyncFiles();
#endif
	}

	private void LoadDatas()
	{
		if (File.Exists(m_current_filePath))
		{
			try
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(m_current_filePath, FileMode.Open);
				m_datas = bf.Deserialize(file) as PlayerDatas;
				file.Close();
			}
			catch
			{
				m_datas = new PlayerDatas();
			}
		}
		else
			m_datas = new PlayerDatas();
	}


	[System.Serializable]
	public class PlayerDatas{

	
		public float[] skinColor = new float[]{96,61,44};

		public float[] hairColor = new float[]{126,56,56};

		public float[] eyeColor = new float[]{180,132,16};

		public LastGameType lastGame = LastGameType.NoMiniGame;

		public int lastScore = 0;

		public float tutoDone = 0;

		public string gender = "woman";

		public string name = "Avatar";

		public string lastEnv = "SceneMapPrincipale";

		public Dictionary<string, bool> diamonds = new Dictionary<string, bool>(){ {"jungle",false},{"sahara", false},{"mediterranean", false} , {"ruins", false}, {"mountain",false},{"savane", false},{"mangrove", false},{"fleuve", false},{"village", false},{"urban", false},{"ocean",false}};
	}

}	
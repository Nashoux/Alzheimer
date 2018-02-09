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
	
		public Save (string fileName)
		{
			this.fileName = fileName;
		}
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





	public void ZoneSet(int result, string zoneName){
		m_datas.actualZoneCompleted[zoneName] = result;
	}

	public int ZoneGet(string zoneName){
		return m_datas.actualZoneCompleted[zoneName];
	}

	public Dictionary<string,int> ZoneAllGet(){
		return m_datas.actualZoneCompleted;
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
				Save newSave = new Save(saveName);
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
		m_currentSave = new Save(newFileName);
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

			public  Dictionary<string, int> actualZoneCompleted = new Dictionary<string, int>(){ {"zone1",0}, {"zone2",0},{"zone3",0},{"zone4",0},{"zone5",0} };
			public  Dictionary<string, int> LastZoneCompleted = new Dictionary<string, int>(){ {"zone1",0}, {"zone2",0},{"zone3",0},{"zone4",0},{"zone5",0} };
			
		}

}	
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour {

	[SerializeField] int fadeTimer = 45;

	[SerializeField] Color myColor;
	[SerializeField] Image myFade;

	[SerializeField] GameObject buttonNewGame;
	[SerializeField] GameObject buttonContinue;
	[SerializeField] GameObject buttonDiary;

	public void NewGame(){
	StartCoroutine(FadeOutNewGame());
	}
	public void ContinueGame(){
	StartCoroutine(FadeOutContinue());
	}
	public void Diary(){
	StartCoroutine(FadeOutDiary());	
	}
	public void Credits(){
	StartCoroutine(FadeOutCredits());	
	}
#region Fades
	IEnumerator FadeIn(){
	myFade.raycastTarget = true;
	for(int i =0; i<fadeTimer;i++){
		myFade.color = new Color(myColor.r,myColor.g,myColor.b,myFade.color.a- (1f / (float)fadeTimer));
		yield return new WaitForEndOfFrame();
	}
	myFade.color = new Color(myColor.r,myColor.g,myColor.b,0);
	myFade.raycastTarget = false;
		yield return null;
	}

	IEnumerator FadeOutNewGame(){
	myFade.raycastTarget = true;
	for(int i =0; i<fadeTimer;i++){
		myFade.color = new Color(myColor.r,myColor.g,myColor.b,((float)i/(float)fadeTimer));
		yield return new WaitForEndOfFrame();
	}
	SceneManager.LoadScene("test");
		yield return null;
	}

	IEnumerator FadeOutDiary(){
	myFade.raycastTarget = true;
	for(int i =0; i<fadeTimer;i++){
		myFade.color = new Color(myColor.r,myColor.g,myColor.b,((float)i/(float)fadeTimer));
		yield return new WaitForEndOfFrame();
	}
	SceneManager.LoadScene("Diary");
	}

	IEnumerator FadeOutCredits(){
	myFade.raycastTarget = true;
	for(int i =0; i<fadeTimer;i++){
		myFade.color = new Color(myColor.r,myColor.g,myColor.b,((float)i/(float)fadeTimer));
		yield return new WaitForEndOfFrame();
	}
	SceneManager.LoadScene("Credits");
	}

	IEnumerator FadeOutContinue(){
	myFade.raycastTarget = true;
	for(int i =0; i<fadeTimer;i++){
		myFade.color = new Color(myColor.r,myColor.g,myColor.b,((float)i/(float)fadeTimer));
		yield return new WaitForEndOfFrame();
	}
	for(int y = 0; y<PlayerSave.Instance.ZoneAllGet().Count;y++){
		if(PlayerSave.Instance.ZoneGet("zone"+y)==0){
			SceneManager.LoadScene("zone"+y);
		}
	}
		yield return null;
	}
	#endregion

	void Start () {
		myFade.gameObject.SetActive(true);
		StartCoroutine(FadeIn());
		if(PlayerSave.Instance.ZoneGet("zone1") == 0){
			buttonNewGame.SetActive(true);
			buttonContinue.SetActive(false);
			buttonDiary.SetActive(false);
		}else{
			buttonNewGame.SetActive(false);
			buttonContinue.SetActive(true);
			buttonDiary.SetActive(true);
		}
	}
}

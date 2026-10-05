using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;  

public class GameGoScript : MonoBehaviour {

	//globals
    public KMBombInfo bomb;
    public KMAudio Audio;

	//Selectables
	public KMSelectable customer; 
	public KMSelectable arrow; 
	public KMSelectable[] stocks; 

	//Objects
	public GameObject speechDisp; 
	public GameObject gameDisp; 
	public GameObject desk; 

	//Renderers
	public SpriteRenderer coverDisp; 
	public SpriteRenderer caseDisp; 
	public SpriteRenderer customerDisp; 
	public MeshRenderer background; 
	public TextMesh need;
	public TextMesh like;
	public TextMesh dislike; 
	public SpriteRenderer[] stockDisps; 
	public TextMesh[] priceLabels; 

	//Sprites
	public Sprite[] covers; 
	public Sprite[] cases; 
	public Sprite[] customers; 
	public Sprite dexter; 

	//Materials
	public Material[] backgrounds; 

	//Variables
	private bool speechView = false; 
	private bool shelfView = false; 
	private int coverInd; 
	private int caseInd;
	private int custInd; 
	private List<int> usedCov = new List<int>(2); 
	private List<int> usedCust = new List<int>(2);
	private string[] primaries = {"Card","Dungeon Crawler","Fighting","Horror","Platformer","Puzzle","RPG","Sandbox","Shooter","Sports","Strategy"}; 
	private string[] secondaries = {"Difficult","Fantasy","Retro","Sci-Fi","Survival","-"};
	private string[] audiences = {"Kids","Everyone","Adults"}; 
	private string[] styles = {"Solo","Co-Op","Versus"};
	private string[] days = {"Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"}; 
	private string[] consoles = {"BS5", "Smoke", "Swap", "YBox", "Yuu"}; 
	private string[] gameNames; 
	private string day; 
	private int dayInd; 
	private int custCount = 0; 
	private int tradeValue; 
	private int[] stockCovInds = new int[9]; 
	private int[] stockConInds = new int[9]; 
	private List<int> chosenGames = new List<int>(); 
	private string[] mes; 
	private string[] mes2;
	private string[] mes3; 
	private int[] prices = new int[9]; 
	private List<int> chosenPrices = new List<int>(); 
	private int[] points; 
	private int correctInd; 
	private int prevCovInd;
	private int prevCaseInd; 
	private Vector3 topLeft = new Vector3(-0.05f, 0.0093f, 0.181f);
	private Vector3 right = new Vector3(0.175f, 0.0093f, -0.011f); 
	private Vector3 topLeftSc = new Vector3(0f, 180f, 0f); 
	private Vector3 rightSc = new Vector3(0f, 270f, 0f); 
	bool jumping = false; 
	bool fah = false; 
	
	//Preferences
	private bool[] prefTypes; //Cost, PrimGen, SecGen, TargAud, PlaySty, Console 
	private int[] prefInds; 
	private int costPref; 
	private int costType; 
	private string[] costStatements = {"more", "less"}; 
	private int primPref;
	private int secPref;
	private int targAud;
	private int playSty; 
	private int console; 

	//Tables
	private int[][] table1 = {
		new int[]{55,20,75,10,35,60,25},
		new int[]{40,15,70,30,80,45,5},
		new int[]{65,50,10,75,20,35,55},
		new int[]{25,60,45,5,80,30,70},   
    	new int[]{15,55,40,20,75,10,65},   
    	new int[]{30,80,25,50,5,45,70},   
    	new int[]{60,35,15,55,20,75,10},   
    	new int[]{45,5,80,30,70,25,50},   
    	new int[]{10,65,55,40,20,75,15},     
    	new int[]{35,15,55,20,75,10,65},   
    	new int[]{50,70,25,60,45,5,80}  
	};

	private int[][] table2 = {
		new int[]{10,-5,0,-20,15,-10,5},
		new int[]{0, -15, -20, 20, -10, 5, 10},
		new int[]{-15,20,-5,10,0,-20,15},
		new int[]{5,-10,15,0,-5,20,-15},
		new int[]{-20,10,-15,5,20,0,-5}
	}; 

	//Game Details
	string[][] details = {
    	new string[] { "1 Night at Balloon Boy's", "Horror", "Survival", "Everyone", "Solo" },
    	new string[] { "14 Days", "Shooter", "Survival", "Everyone", "Versus" },
		new string[] { "Ablation Corporation", "Strategy", "Difficult", "Everyone", "Solo" },
    	new string[] { "Alley Dueler", "Fighting", "Retro", "Everyone", "Versus" },
    	new string[] { "Average Mario Bros", "Platformer", "Retro", "Kids", "Solo" },
    	new string[] { "Barony Ambush", "Strategy", "Fantasy", "Kids", "Solo" },
    	new string[] { "Besooge", "Sandbox", "Puzzle", "Everyone", "Solo" },
    	new string[] { "Block Stack", "Puzzle", "Retro", "Everyone", "Solo" },
    	new string[] { "Boulder's Gate 3", "RPG", "Fantasy", "Adults", "Co-Op" },
    	new string[] { "Boblox", "Sandbox", "-", "Kids", "Versus" },
    	new string[] { "Bright Spirits 3", "RPG", "Difficult", "Adults", "Solo" },
    	new string[] { "Brinecraft", "Sandbox", "Survival", "Kids", "Co-Op" },
    	new string[] { "Busnautica", "Horror", "Survival", "Everyone", "Solo" },
    	new string[] { "Car Soccer", "Sports", "-", "Everyone", "Versus" },
    	new string[] { "Car Theft 5", "Sandbox", "Shooter", "Adults", "Versus" },
    	new string[] { "Carving", "Card", "Dungeon Crawler", "Adults", "Solo" },
    	new string[] { "Coming Together Magically", "Card", "Fantasy", "Everyone", "Versus" },
    	new string[] { "Copper Tee", "Sports", "Retro", "Everyone", "Versus" },
    	new string[] { "Cooked by Commencement", "Horror", "Survival", "Adults", "Versus" },
    	new string[] { "Crops vs Undead", "Strategy", "-", "Kids", "Solo" },
    	new string[] { "Diós 2", "Dungeon Crawler", "Fantasy", "Adults", "Co-Op" },
    	new string[] { "Earthia", "Sandbox", "Fantasy", "Everyone", "Co-Op" },
    	new string[] { "Extraterrestrial Companionship", "Horror", "Sci-Fi", "Adults", "Solo" },
    	new string[] { "Feesfa 18", "Sports", "-", "Everyone", "Versus" },
    	new string[] { "Fungal Seed", "RPG", "Sci-Fi", "Everyone", "Solo" },
    	new string[] { "Gateway 2", "Puzzle", "Platform", "Everyone", "Solo" },
    	new string[] { "Goose Hunt", "Shooter", "Retro", "Kids", "Solo" },
    	new string[] { "Goodbye Resident", "Horror", "Platformer", "Everyone", "Solo" },
    	new string[] { "Groundrim", "RPG", "Fantasy", "Adults", "Solo" },
    	new string[] { "Harmless Crew", "Horror", "Sci-Fi", "Everyone", "Co-Op" },
    	new string[] { "Hope", "Shooter", "Retro", "Adults", "Solo" },
    	new string[] { "Horns 3", "Shooter", "Sci-Fi", "Adults", "Versus" },
    	new string[] { "I am Keke", "Puzzle", "Difficult", "Everyone", "Solo" },
    	new string[] { "Immortal Kombat 2", "Fighting", "Fantasy", "Adults", "Versus" },
    	new string[] { "Jabminions", "RPG", "Retro", "Kids", "Solo" },
    	new string[] { "Jake Paul's Punch-Out!", "Fighting", "Retro", "Everyone", "Solo" },
    	new string[] { "Jerry's Mod", "Sandbox", "Shooter", "Adults", "Co-Op" },
    	new string[] { "Kill the Tower", "Card", "Dungeon Crawler", "Everyone", "Solo" },
    	new string[] { "Killer's Belief", "RPG", "Fantasy", "Adults", "Solo" },
    	new string[] { "League of Losers", "Fighting", "Strategy", "Everyone", "Versus" },
    	new string[] { "Leave the Gungeon", "Dungeon Crawler", "Shooter", "Everyone", "Co-Op" },
    	new string[] { "Lunitaire", "Card", "-", "Everyone", "Solo" },
    	new string[] { "Mega Sibling Crusher Bedlam", "Fighting", "Platformer", "Kids", "Versus" },
    	new string[] { "Moderately Dark Dungeon", "Dungeon Crawler", "Difficult", "Everyone", "Solo" },
    	new string[] { "Mono Bridge", "Puzzle", "Sandbox", "Everyone", "Solo" },
    	new string[] { "Inflatable Monkey Attack", "Strategy", "Difficult", "Kids", "Co-Op" },
    	new string[] { "Oxygen Included", "Sandbox", "Sci-Fi", "Everyone", "Solo" },
    	new string[] { "Phasmophilia", "Horror", "Puzzle", "Adults", "Co-Op" },
    	new string[] { "Pluto", "Dungeon Crawler", "RPG", "Everyone", "Solo" },
    	new string[] { "Pythagorean Jog", "Platformer", "Difficult", "Everyone", "Solo" },
    	new string[] { "Resident of Good Moral Character 4", "Horror", "Survival", "Adults", "Solo" },
    	new string[] { "Spooro the Magic Lizard", "RPG", "Platformer", "Kids", "Co-Op" },
    	new string[] { "Stop Talking and Explode", "Puzzle", "Difficult", "Everyone", "Co-Op" },
    	new string[] { "Stuffed Knight", "RPG", "Platformer", "Everyone", "Solo" },
    	new string[] { "Talabro", "Card", "Dungeon Crawler", "Everyone", "Solo" },
    	new string[] { "Terminal Delusion VII", "RPG", "Sci-Fi", "Adults", "Solo" },
    	new string[] { "The Freeing of Isaac", "Dungeon Crawler", "Difficult", "Adults", "Solo" },
    	new string[] { "Threeza Horizon 5", "Sports", "–", "Everyone", "Versus" },
    	new string[] { "Tilapia BO2", "Shooter", "-", "Adults", "Versus" },
    	new string[] { "Ultra Woman", "Platformer", "Retro", "Kids", "Solo" },
    	new string[] { "Undersite", "Shooter", "Fighting", "Everyone", "Versus" },
    	new string[] { "Waluigi's Crack Den", "Horror", "-", "Kids", "Co-Op" },
    	new string[] { "World of Pacifists", "RPG", "Fantasy", "Everyone", "Versus" },
    	new string[] { "Yuu Sports", "Sports", "-", "Everyone", "Versus" }
	};

	//Logging
	private static int moduleIdCounter = 1;
    private int moduleId;
	private bool moduleSolved = false; 

	void Awake () {
		moduleId = moduleIdCounter++;
		customer.OnInteract += delegate () {CustomerPress(); return false;};
		arrow.OnInteract += delegate () {ArrowPress(); return false;};
		for (int i = 0; i < stocks.Length; i++){
			int j = i; 
			stocks[i].OnInteract += delegate(){StockPress(j); return false;}; 
			stocks[i].OnHighlight += delegate(){Audio.PlaySoundAtTransform("select",transform);};
		}
		day = DateTime.Now.DayOfWeek.ToString();
		dayInd = Array.IndexOf(days, day);
		mes = new string[9]; 
		mes2 = new string[9];
		mes3 = new string[9]; 
		gameNames = new string[covers.Length];
		for (int i = 0; i < covers.Length; i++){
			gameNames[i] = details[i][0]; 
		}
	}

	void Start () {
		Audio.PlaySoundAtTransform("NPC", transform); 
		costPref = -1; 
		primPref = -1;
		secPref = -1;
		targAud = -1;
		playSty = -1; 
		console = -1;
		tradeValue = 0; 
		prefInds = new int[]{-1,-1,-1};
		prefTypes = new bool[6];
		points = new int[9]; 
		shelfView = true; 
		speechView = false; 
		ArrowPress(); 
		speechView = false; 
		RandomizeCust(); 
		if (custCount < 2){
			Randomize();
		}
		else {
			Replace(); 
		}
		FindValue(); 
		GenPreferences(); 
		FindStockValue(); 
	}

	void CustomerPress(){
		if (jumping || fah){return;}
		Audio.PlaySoundAtTransform("crashing",transform); 
		StartCoroutine(Jump()); 
		speechView = !speechView; 
		if (speechView){
			gameDisp.SetActive(false);
			speechDisp.SetActive(true);
		}
		else{
			gameDisp.SetActive(true); 
			speechDisp.SetActive(false); 
		}
	}

	IEnumerator Jump(){
		jumping = true; 
		float elapsed = 0f;
		float duration = 0.1f; 
		float duration2 = 0.15f; 
		Vector3 start = customer.gameObject.transform.localPosition; 
		Vector3 end1 = start + new Vector3(0f, 0f, 0.01f); 
		Vector3 end2 = start + new Vector3(0f, 0f, 0.003f); 
		while (elapsed < duration){
			yield return null; 
			customer.gameObject.transform.localPosition = Vector3.Lerp(start, end1, elapsed/duration); 
			elapsed += Time.deltaTime; 
		}
		elapsed = 0f; 
		while (elapsed < duration2){
			yield return null; 
			customer.gameObject.transform.localPosition = Vector3.Lerp(end1, start, elapsed/duration); 
			elapsed += Time.deltaTime; 
		}
		elapsed = 0f; 
		while (elapsed < duration2){
			yield return null; 
			customer.gameObject.transform.localPosition = Vector3.Lerp(start, end2, elapsed/duration); 
			elapsed += Time.deltaTime; 
		}
		elapsed = 0f; 
		while (elapsed < duration2){
			yield return null; 
			customer.gameObject.transform.localPosition = Vector3.Lerp(end2, start, elapsed/duration); 
			elapsed += Time.deltaTime; 
		}
		jumping = false; 
		yield break; 
	}

	void ArrowPress(){
		if (jumping || fah){return;}
		shelfView = !shelfView; 
		if (shelfView){
			background.material = backgrounds[1]; 
			desk.gameObject.SetActive(false); 
			customerDisp.gameObject.SetActive(false); 
			gameDisp.SetActive(false);
			speechDisp.SetActive(false); 
			for (int i = 0; i<stocks.Length; i++){
				stocks[i].gameObject.SetActive(true); 
			}
			arrow.gameObject.transform.localEulerAngles = rightSc;
			arrow.gameObject.transform.localPosition = right;  
		} 
		else{
			background.material = backgrounds[0];
			desk.gameObject.SetActive(true);
			if (!moduleSolved){
				customerDisp.gameObject.SetActive(true); 
				if (speechView){
					gameDisp.SetActive(false);
					speechDisp.SetActive(true);
				}
				else{
					gameDisp.SetActive(true); 
					speechDisp.SetActive(false);
				}
			}
			for (int i = 0; i<stocks.Length; i++){
				stocks[i].gameObject.SetActive(false); 
			}
			arrow.gameObject.transform.localEulerAngles = topLeftSc;
			arrow.gameObject.transform.localPosition = topLeft; 
		}
	}

	void StockPress(int x){
		if (moduleSolved || fah){return;}
		if (correctInd == x){
			if (custCount < 2){
				Audio.PlaySoundAtTransform("cash", transform); 
				Start(); 
			}
			else{
				Audio.PlaySoundAtTransform("child celebration", transform); 
				moduleSolved = true; 
				ArrowPress(); 
				customerDisp.gameObject.SetActive(false);
				GetComponent<KMBombModule>().HandlePass();
			}
		}
		else{
			Debug.LogFormat("[GameGo #{0}] You incorrectly pressed {1} which costs {2}.", moduleId, mes[x], priceLabels[x].text);
			StartCoroutine(Fah(x)); 
		}
	}

	IEnumerator Fah(int i){
		fah = true; 
		Audio.PlaySoundAtTransform("FAAHH", transform); 
		GetComponent<KMBombModule>().HandleStrike();
		stockDisps[i].sprite = dexter; 
		float duration = 0.15f;
		float elapsed = 0f; 
		for (int j = 0; j < prices.Length; j++){
			stockDisps[j].sprite = dexter; 
		}
		while (elapsed < duration){
			yield return null; 
			elapsed += Time.deltaTime; 
		}
		elapsed = 0f; 
		for (int l = 0; l < prices.Length; l++){
			stockDisps[l].sprite = covers[chosenGames[l]];
		}
		while (elapsed < duration){
			yield return null; 
			elapsed += Time.deltaTime; 
		}
		elapsed = 0f; 
		for (int j = 0; j < prices.Length; j++){
			stockDisps[j].sprite = dexter; 
		}
		while (elapsed < duration){
			yield return null; 
			elapsed += Time.deltaTime; 
		}
		for (int l = 0; l < prices.Length; l++){
			stockDisps[l].sprite = covers[chosenGames[l]];
		}
		elapsed = 0f; 
		for (int j = 0; j < prices.Length; j++){
			stockDisps[j].sprite = dexter; 
		}
		duration = 0.5f;
		while (elapsed < duration){
			yield return null; 
			elapsed += Time.deltaTime; 
		}
		for (int l = 0; l < prices.Length; l++){
			stockDisps[l].sprite = covers[chosenGames[l]];
		}

		fah = false; 
		yield break;
	}

	void RandomizeCust(){
		custCount++; 
		coverInd = UnityEngine.Random.Range(0, covers.Length);
		while (usedCov.Contains(coverInd)){
			coverInd = UnityEngine.Random.Range(0, covers.Length);
		}
		caseInd = UnityEngine.Random.Range(0, cases.Length); 
		custInd = UnityEngine.Random.Range(0, customers.Length); 
		while (usedCust.Contains(custInd)){
			custInd = UnityEngine.Random.Range(0, customers.Length);
		}
		coverDisp.sprite = covers[coverInd]; 
		usedCov.Add(coverInd); 
		caseDisp.sprite = cases[caseInd]; 
		customerDisp.sprite = customers[custInd]; 
		usedCust.Add(custInd); 
		if (custInd == 11){
			Debug.LogFormat("[GameGo #{0}] Adam Sandler has brought in {1} for the {2}.", moduleId, details[coverInd][0], consoles[caseInd]); 
		}
		else{
			Debug.LogFormat("[GameGo #{0}] Customer {1} has brought in {2} for the {3}.", moduleId, custCount, details[coverInd][0], consoles[caseInd]); 
		}
		Debug.LogFormat("[GameGo #{0}] The primary genre is {1}, the secondary genre is {2}, the target audience is {3}, and the play style is {4}.", moduleId, details[coverInd][1], details[coverInd][2], details[coverInd][3],details[coverInd][4]);
		if (custInd < 11){Audio.PlaySoundAtTransform("man hello2", transform);}
		else if (custInd == 11){Audio.PlaySoundAtTransform("adam sandler", transform);}
		else{Audio.PlaySoundAtTransform("woman hello", transform);}
	}

	void Replace(){
		string temp = mes[correctInd]; 
		chosenGames[correctInd] = prevCovInd; 
		int z = UnityEngine.Random.Range(1, 17);
		while (chosenPrices.Contains(5*z)){
			z = UnityEngine.Random.Range(1, 17);
		}
		chosenPrices[correctInd] = 5*z; 
		prices[correctInd] = 5*z; 
		priceLabels[correctInd].text = "$" + prices[correctInd].ToString(); 
		mes[correctInd] = details[prevCovInd][0]; 
		mes2[correctInd] = consoles[prevCaseInd]; 
		stockDisps[correctInd].sprite = covers[prevCovInd]; 
		stocks[correctInd].GetComponent<SpriteRenderer>().sprite = cases[prevCaseInd];
		Debug.LogFormat("[GameGo #{0}] Replaced {1} with {2}. The new Retail Price is {3}.", moduleId, temp, mes[correctInd], priceLabels[correctInd].text);
	}

	void Randomize(){
		for (int i = 0; i < stocks.Length; i++){
			int x = UnityEngine.Random.Range(0, covers.Length); 
			int y = UnityEngine.Random.Range(0, consoles.Length); 
			int z = UnityEngine.Random.Range(1, 17);
			while (chosenPrices.Contains(5*z)){
				z = UnityEngine.Random.Range(1, 17);
			}
			chosenGames.Add(x); 
			chosenPrices.Add(5*z); //
			stockDisps[i].sprite = covers[x]; //
			stockCovInds[i] = x; //
			stocks[i].GetComponent<SpriteRenderer>().sprite = cases[y]; //
			stockConInds[i] = y; //
			prices[i] = 5*z; 
			mes[i] = details[x][0]; 
			mes2[i] = consoles[y]; 
			mes3[i] = prices[i].ToString(); 
			priceLabels[i].text = "$" + prices[i].ToString(); 
		}
		string _mes = string.Join(", ", mes); 
		string _mes2 = string.Join(", ", mes2); 
		string _mes3 = string.Join(", ", mes3); 
		Debug.LogFormat("[GameGo #{0}] The games in stock in reading order are: {1}.", moduleId, _mes);
		Debug.LogFormat("[GameGo #{0}] The games in stock are for the following consoles in reading order: {1}.", moduleId, _mes2);
		Debug.LogFormat("[GameGo #{0}] The Retail Value of games in stock in reading order are: {1}.", moduleId, _mes3);
	}

	void FindValue(){
		int rowInd = Array.IndexOf(primaries, details[coverInd][1]);
		int colInd; 
		if (Array.IndexOf(secondaries, details[coverInd][2]) != -1){
			colInd = Array.IndexOf(secondaries, details[coverInd][2]); 
		}
		else{
			colInd = 6; 
		}
		int baseValue = table1[rowInd][dayInd]; 
		int modValue = table2[caseInd][colInd]; 
		tradeValue = baseValue + modValue; 
		if (tradeValue < 5){
			tradeValue = 5; 
		}
		Debug.LogFormat("[GameGo #{0}] The Base Value is {1}, and the Value Modifier is {2}, so the Trade-In Value is {3}.", moduleId, baseValue, modValue, tradeValue); 
	}

	void GenPreferences(){
		int prefInd; 
		for (int i = 0; i < 3; i++){
			prefInd = UnityEngine.Random.Range(0, prefTypes.Length);
			while (i > 0 && (prefInds[0]==prefInd || prefInds[1]==prefInd)){
				prefInd = UnityEngine.Random.Range(0, prefTypes.Length);
			}
			prefInds[i] = prefInd; 
		}
		switch (prefInds[0]){ //Cost, PrimGen, SecGen, TargAud, PlaySty, Console 
			case 0:
				costPref = 5*UnityEngine.Random.Range(4, 14); 
				costType = UnityEngine.Random.Range(0,2); 
				need.text = "$" + costPref + " or " + costStatements[costType];
			break;
			case 1:
				primPref = UnityEngine.Random.Range(0, primaries.Length); 
				need.text = primaries[primPref] + " games";
			break;
			case 2:
				secPref = UnityEngine.Random.Range(0, secondaries.Length-1);
				need.text = secondaries[secPref] + " games";
			break;
			case 3:
				targAud = UnityEngine.Random.Range(0, audiences.Length);
				need.text = "for " + audiences[targAud]; 
			break;
			case 4:
				playSty = UnityEngine.Random.Range(0, styles.Length); 
				need.text = styles[playSty] + " games"; 
			break;
			case 5:
				console = UnityEngine.Random.Range(0, cases.Length); 
				need.text = "for the " + consoles[console].ToString();
			break;
		}
		switch (prefInds[1]){ //Cost, PrimGen, SecGen, TargAud, PlaySty, Console 
			case 0:
				costPref = 5*UnityEngine.Random.Range(4, 14); 
				costType = UnityEngine.Random.Range(0,2); 
				like.text = "$" + costPref + " or " + costStatements[costType];
			break;
			case 1:
				primPref = UnityEngine.Random.Range(0, primaries.Length); 
				like.text = primaries[primPref] + " games";
			break;
			case 2:
				secPref = UnityEngine.Random.Range(0, secondaries.Length-1);
				like.text = secondaries[secPref] + " games";
			break;
			case 3:
				targAud = UnityEngine.Random.Range(0, audiences.Length);
				like.text = "for " + audiences[targAud]; 
			break;
			case 4:
				playSty = UnityEngine.Random.Range(0, styles.Length); 
				like.text = styles[playSty] + " games"; 
			break;
			case 5:
				console = UnityEngine.Random.Range(0, cases.Length); 
				like.text = "for the " + consoles[console].ToString();
			break;
		}
		switch (prefInds[2]){ //Cost, PrimGen, SecGen, TargAud, PlaySty, Console 
			case 0:
				costPref = 5*UnityEngine.Random.Range(4, 14); 
				costType = UnityEngine.Random.Range(0,2); 
				dislike.text = "$" + costPref + " or " + costStatements[costType];
			break;
			case 1:
				primPref = UnityEngine.Random.Range(0, primaries.Length); 
				dislike.text = primaries[primPref] + " games";
			break;
			case 2:
				secPref = UnityEngine.Random.Range(0, secondaries.Length-1);
				dislike.text = secondaries[secPref] + " games";
			break;
			case 3:
				targAud = UnityEngine.Random.Range(0, audiences.Length);
				dislike.text = "for " + audiences[targAud]; 
			break;
			case 4:
				playSty = UnityEngine.Random.Range(0, styles.Length); 
				dislike.text = styles[playSty] + " games"; 
			break;
			case 5:
				console = UnityEngine.Random.Range(0, cases.Length); 
				dislike.text = "for the " + consoles[console].ToString();
			break;
		}
		if (custInd == 11){
			Debug.LogFormat("[GameGo #{0}] Adam Sandler's need is: {1}.", moduleId, need.text.ToString());
			Debug.LogFormat("[GameGo #{0}] Adam Sandler's like is: {1}.", moduleId, like.text.ToString());
			Debug.LogFormat("[GameGo #{0}] Adam Sandler's dislike is: {1}.", moduleId, dislike.text.ToString());
		}
		else{
			Debug.LogFormat("[GameGo #{0}] Customer {1}'s need is: {2}.", moduleId, custCount, need.text.ToString());
			Debug.LogFormat("[GameGo #{0}] Customer {1}'s like is: {2}.", moduleId, custCount, like.text.ToString());
			Debug.LogFormat("[GameGo #{0}] Customer {1}'s dislike is: {2}.", moduleId, custCount, dislike.text.ToString());
		}
		
	}

	void FindStockValue(){
		string[] __mes4 = new string[9]; 
		for (int i = 0; i < priceLabels.Length; i++){
			int dif = Math.Abs(prices[i] - tradeValue); 
			int _dif = 6 - (int)Math.Floor((double)(dif/5));
			if (_dif < 0){_dif = 0;}
			if (_dif > 5){_dif = 5;}
			points[i] += _dif; 
			if (coverInd == chosenGames[i]){points[i] -= 2;} 
			bool needB = false;
			bool likeB = false;
			bool dislikeB = false; 
			switch (prefInds[0]){ //Cost (more, less), PrimGen, SecGen, TargAud, PlaySty, Console 
				case 0:
					if (costType == 0 && prices[i] >= costPref){needB = true;}
					else if (costType == 1 && prices[i] <= costPref){needB = true;}
				break;
				case 1:
					if (primaries[primPref] == details[chosenGames[i]][1]){needB = true;}
				break;
				case 2:
					if (secondaries[secPref] == details[chosenGames[i]][2]){needB = true;}
				break;
				case 3:
					if (audiences[targAud] == details[chosenGames[i]][3]){needB = true;}
				break;
				case 4:
					if (styles[playSty] == details[chosenGames[i]][4]){needB = true;}
				break;
				case 5:
					if (consoles[console] == mes2[i]){needB = true;}
				break;
			}	
			switch (prefInds[1]){ //Cost, PrimGen, SecGen, TargAud, PlaySty, Console 
				case 0:
					if (costType == 0 && prices[i] >= costPref){likeB = true;}
					else if (costType == 1 && prices[i] <= costPref){likeB = true;}
				break;
				case 1:
					if (primaries[primPref] == details[chosenGames[i]][1]){likeB = true;}
				break;
				case 2:
					if (secondaries[secPref] == details[chosenGames[i]][2]){likeB = true;}
				break;
				case 3:
					if (audiences[targAud] == details[chosenGames[i]][3]){likeB = true;}
				break;
				case 4:
					if (styles[playSty] == details[chosenGames[i]][4]){likeB = true;}
				break;
				case 5:
					if (consoles[console] == mes2[i]){likeB = true;}
				break;
			}	
			switch (prefInds[2]){ //Cost, PrimGen, SecGen, TargAud, PlaySty, Console 
				case 0:
					if (costType == 0 && prices[i] >= costPref){dislikeB = true;}
					else if (costType == 1 && prices[i] <= costPref){dislikeB = true;}
				break;
				case 1:
					if (primaries[primPref] == details[chosenGames[i]][1]){dislikeB = true;}
				break;
				case 2:
					if (secondaries[secPref] == details[chosenGames[i]][2]){dislikeB = true;}
				break;
				case 3:
					if (audiences[targAud] == details[chosenGames[i]][3]){dislikeB = true;}
				break;
				case 4:
					if (styles[playSty] == details[chosenGames[i]][4]){dislikeB = true;}
				break;
				case 5:
					if (consoles[console] == mes2[i]){dislikeB = true;}
				break;
			}	
			if (needB){points[i]+=3;}
			if (likeB){points[i]+=1;}
			if (dislikeB){points[i]-=1;}
			if (points[i] < 0){points[i] = 0;}
			__mes4[i] = points[i].ToString(); 
		}	
		string _mes4 = string.Join(", ", __mes4);
		Debug.LogFormat("[GameGo #{0}] The Points of games in stock in reading order are: {1}.", moduleId, _mes4);
		if (points.Count(x => x == points.Max()) > 1){
			List<int> indices = new List<int>(); 
			List<int> indexCosts = new List<int>(); 
			int count = 0; 
			for (int i = 0; i < points.Length; i++){
				if (points[i] == points.Max()){
					indices.Add(i); 
					indexCosts.Add(prices[i]); 
					count++; 
				}
			}
			int[] _indices = indices.ToArray(); 
			int[] _indexCosts = indexCosts.ToArray(); 
			correctInd = _indices[Array.IndexOf(_indexCosts, _indexCosts.Min())]; 
		}
		else {
			correctInd = Array.IndexOf(points, points.Max()); 
		}
		Debug.LogFormat("[GameGo #{0}] The correct game is {1} which costs ${2}.", moduleId, mes[correctInd], prices[correctInd]);
		prevCovInd = coverInd; 
		prevCaseInd = caseInd; 
	}
}

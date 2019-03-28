﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace TDTK {
	
	public class PerkEditorWindow : TDEditorWindow {
		
		[MenuItem ("Tools/TDTK/PerkEditor", false, 10)]
		static void OpenPerkEditor () { Init(); }
		
		private static PerkEditorWindow window;
		
		public static void Init (int prefabID=-1) {
			window = (PerkEditorWindow)EditorWindow.GetWindow(typeof (PerkEditorWindow), false, "PerkEditor");
			window.minSize=new Vector2(420, 300);
			
			TDE.Init();
			
			InitLabel();
			
			//if(prefabID>=0) window.selectID=PerkDB.GetPrefabIDIndex(prefabID);
		}
		
		
		private static string[] perkTypeLabel;
		private static string[] perkTypeTooltip;
		
		private static string[] effTypeLabel;
		private static string[] effTypeTooltip;
		
		private static void InitLabel(){
			int enumLength = Enum.GetValues(typeof(_PerkType)).Length;
			perkTypeLabel=new string[enumLength];
			perkTypeTooltip=new string[enumLength];
			for(int i=0; i<enumLength; i++){
				perkTypeLabel[i]=((_PerkType)i).ToString();
				
				if((_PerkType)i==_PerkType.NewTower) 	perkTypeTooltip[i]="Add a new tower to the game";
				if((_PerkType)i==_PerkType.NewAbility) 	perkTypeTooltip[i]="Add a new ability to the game";
				
				if((_PerkType)i==_PerkType.ModifyTower) 		perkTypeTooltip[i]="Modify the attribute of a single/multiple/all tower(s)";
				if((_PerkType)i==_PerkType.ModifyAbility) 		perkTypeTooltip[i]="Modify the attribute of a single/multiple/all ability(s)";
				if((_PerkType)i==_PerkType.ModifyEffect) 		perkTypeTooltip[i]="Modify the attribute of a single/multiple/all effect(s)";
				if((_PerkType)i==_PerkType.ModifyPerkCost) 	perkTypeTooltip[i]="Modify the cost of a single/multiple/all perk(s)";
				
				if((_PerkType)i==_PerkType.GainLife) 		perkTypeTooltip[i]="Give player life";
				if((_PerkType)i==_PerkType.LifeCap) 		perkTypeTooltip[i]="Modify player life's capacity";
				if((_PerkType)i==_PerkType.LifeRegen) 		perkTypeTooltip[i]="Modify player life's regeneration rate";
				if((_PerkType)i==_PerkType.LifeGainWaveCleared) 		perkTypeTooltip[i]="Add a modifier/multiplier to life gain value when a wave is cleared";
				
				if((_PerkType)i==_PerkType.GainRsc) 		perkTypeTooltip[i]="Give player resource";
				if((_PerkType)i==_PerkType.RscRegen) 		perkTypeTooltip[i]="Modify player resource regeneration rate";
				
				if((_PerkType)i==_PerkType.RscGain) 					perkTypeTooltip[i]="Add a modifier/multiplier to resource gain value whenever player gain resource";
				if((_PerkType)i==_PerkType.RscGainCreepDestroyed)perkTypeTooltip[i]="Add a modifier/multiplier to resource gain value when a creep is destroyed";
				if((_PerkType)i==_PerkType.RscGainWaveCleared) 	perkTypeTooltip[i]="Add a modifier/multiplier to resource gain value when a wave is cleared";
				if((_PerkType)i==_PerkType.RscGainResourceTower) perkTypeTooltip[i]="Add a modifier/multiplier to resource gain value when gaining resource from resource tower";
				
				if((_PerkType)i==_PerkType.AbilityRscCap) 	perkTypeTooltip[i]="Modify AbilityManager's resource capacity";
				if((_PerkType)i==_PerkType.AbilityRscRegen) 	perkTypeTooltip[i]="Modify AbilityManager's resource regeneration rate";
				if((_PerkType)i==_PerkType.AbilityRscGainWaveCleared) 	perkTypeTooltip[i]="Add a modifier/multiplier to ability resource gain value when a wave is cleared";
			}
			
			enumLength = Enum.GetValues(typeof(Perk._EffType)).Length;
			effTypeLabel=new string[enumLength];
			effTypeTooltip=new string[enumLength];
			for(int i=0; i<enumLength; i++){
				effTypeLabel[i]=((Perk._EffType)i).ToString();
				if((Perk._EffType)i==Perk._EffType.Modifier) effTypeTooltip[i]="The value in the effect will be directly added to the target unit";
				if((Perk._EffType)i==Perk._EffType.Multiplier) effTypeTooltip[i]="The value in the effect will be be used to multiply the target unit's base value";
			}
		}
		
		
		
		public void OnGUI(){
			TDE.InitGUIStyle();
			
			if(!CheckIsPlaying()) return;
			if(window==null) Init();
			
			
			List<Perk> perkList=PerkDB.GetList();
			
			Undo.RecordObject(this, "window");
			Undo.RecordObject(PerkDB.GetDB(), "abilityDB");
			
			
			if(GUI.Button(new Rect(Math.Max(260, window.position.width-120), 5, 100, 25), "Save")) TDE.SetDirty();
			
			
			if(GUI.Button(new Rect(5, 5, 120, 25), "Create New")) Select(NewItem());
			if(perkList.Count>0 && GUI.Button(new Rect(130, 5, 100, 25), "Clone Selected")) Select(NewItem(selectID));
			
			
			float startX=5;	float startY=55;
			
			if(minimiseList){ if(GUI.Button(new Rect(startX, startY-20, 30, 18), ">>")) minimiseList=false; }
			else{ if(GUI.Button(new Rect(startX, startY-20, 30, 18), "<<")) minimiseList=true; }
			
			Vector2 v2=DrawPerkList(startX, startY, perkList);
			startX=v2.x+25;
			
			if(perkList.Count==0) return;
			
			
			Rect visibleRect=new Rect(startX, startY, window.position.width-startX, window.position.height-startY);
			Rect contentRect=new Rect(startX, startY, contentWidth, contentHeight);
			
			scrollPos = GUI.BeginScrollView(visibleRect, scrollPos, contentRect);
			
				v2=DrawPerkConfigurator(startX, startY, perkList[selectID]);
				contentWidth=v2.x-startX;
				contentHeight=v2.y-55;
			
			GUI.EndScrollView();
			
			
			if(GUI.changed) TDE.SetDirty();
		}
		
		
		private bool showTypeDesp=true;
		Vector2 DrawPerkConfigurator(float startX, float startY, Perk item){
			float maxX=startX;
			
			startY=TDE.DrawBasicInfo(startX, startY, item);
			
			
			int type=(int)item.type;		contL=TDE.SetupContL(perkTypeLabel, perkTypeTooltip);
			TDE.Label(startX, startY+=spaceY+5, width, height, "Perk Type:", "Specify what the perk do");
			type = EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), new GUIContent(""), type, contL);
			item.type=(_PerkType)type;
			
			showTypeDesp=EditorGUI.ToggleLeft(new Rect(startX+spaceX+width+2, startY, width, 20), "Show Description", showTypeDesp);
			if(showTypeDesp){
				EditorGUI.HelpBox(new Rect(startX, startY+=spaceY, width+spaceX, 40), perkTypeTooltip[(int)item.type], MessageType.Info);
				startY+=45-height;
			}
			
			startY+=10;
			
			
			startY=DrawBasicSetting(startX, startY, item)+10;
			
			
			startY=DrawEffectSetting(startX, startY, item);
			
			
			startY+=spaceY;
			
				GUIStyle style=new GUIStyle("TextArea");	style.wordWrap=true;
				cont=new GUIContent("Perk description (for runtime and editor): ", "");
				EditorGUI.LabelField(new Rect(startX, startY, 400, 20), cont);
				item.desp=EditorGUI.TextArea(new Rect(startX, startY+spaceY-3, 270, 150), item.desp, style);
			
			return new Vector2(maxX, startY+170);
		}
		
		
		
		private bool foldBasicSetting=true;
		protected float DrawBasicSetting(float startX, float startY, Perk item){
			//TDE.Label(startX, startY+=spaceY, width, height, "General Setting", "", TDE.headerS);
			string textF="General Setting ";//+(!foldBasicSetting ? "(show)" : "(hide)");
			foldBasicSetting=EditorGUI.Foldout(new Rect(startX, startY+=spaceY, spaceX, height), foldBasicSetting, textF, TDE.foldoutS);
			if(!foldBasicSetting) return startY;
			
			startX+=12;
			
			//~ TDE.Label(startX, startY+=spaceY, width, height, "Gained on Wave:", "");	CheckColor(item.autoUnlockOnWave, -1);
			//~ item.autoUnlockOnWave=EditorGUI.IntField(new Rect(startX+spaceX, startY, widthS, height), item.autoUnlockOnWave);	ResetColor();
			
			//~ startY+=10;
			
			TDE.Label(startX, startY+=spaceY, width, height, "Cost:", "The cost of PerkManager resource required for the perk\nUsed when 'Use RscManager For Cost' is disabled in PerkManager");
			item.cost=EditorGUI.IntField(new Rect(startX+spaceX, startY, widthS, height), item.cost);
			
			TDE.Label(startX, startY+=spaceY, width, height, "Cost (Rsc):", "The cost of RscManager resource required for the perk\nUsed when 'Use RscManager For Cost' is enabled in PerkManager");
			//~ while(item.costRsc.Count<RscDB.GetCount()) item.costRsc.Add(0);
			//~ while(item.costRsc.Count>RscDB.GetCount()) item.costRsc.RemoveAt(item.costRsc.Count-1);
			
			RscManager.MatchRscList(item.costRsc, 0);
			
			float cachedX=startX;
			for(int i=0; i<RscDB.GetCount(); i++){
				if(i>0 && i%3==0){ startX=cachedX; startY+=spaceY; }	if(i>0) startX+=widthS+2;
				TDE.DrawSprite(new Rect(startX+spaceX, startY, height, height), RscDB.GetIcon(i), RscDB.GetName(i));
				item.costRsc[i]=EditorGUI.FloatField(new Rect(startX+spaceX+height, startY, widthS-height, height), item.costRsc[i]);
			}
			startX=cachedX;
			
			//TDE.Label(startX, startY+=spaceY, width, height, "Repeatabe:", "");	
			//item.repeatable=EditorGUI.Toggle(new Rect(startX+spaceX, startY, widthS, height), item.repeatable);
			startY+=10;
			
			TDE.Label(startX, startY+=spaceY, width, height, "AutoUnlockOnWave:", "If given a value, the perk will automatically be purchased for the player upon completing the specified wave");	CheckColor(item.autoUnlockOnWave, 0);
			item.autoUnlockOnWave=EditorGUI.IntField(new Rect(startX+spaceX, startY, widthS, height), item.autoUnlockOnWave);	ResetColor();
			
			startY+=10;
			
			TDE.Label(startX, startY+=spaceY, width, height, "Min Level:", "The minimum level required before the perk becomes available\n\nThis is value of 'Level ID' in GameControl");	CheckColor(item.minLevel, 0);
			item.minLevel=EditorGUI.IntField(new Rect(startX+spaceX, startY, widthS, height), item.minLevel);	ResetColor();
			
			TDE.Label(startX, startY+=spaceY, width, height, "Min Wave:", "The minimum wave required before the perk becomes available");	CheckColor(item.minWave, 0);
			item.minWave=EditorGUI.IntField(new Rect(startX+spaceX, startY, widthS, height), item.minWave);	ResetColor();
			
			TDE.Label(startX, startY+=spaceY, width, height, "Min Perk Count:", "The minimum number of perk purchased required before the perk becomes available");	CheckColor(item.minPerkCount, 0);
			item.minPerkCount=EditorGUI.IntField(new Rect(startX+spaceX, startY, widthS, height), item.minPerkCount);	ResetColor();
			
			
			TDE.Label(startX, startY+=spaceY, width, height, "Prereq Perk:", "Perk(s) required to be purchased before the perk becomes available");
			for(int i=0; i<item.prereq.Count; i++){
				TDE.Label(startX+spaceX-20, startY, widthS, height, "-");
				
				int index=PerkDB.GetPrefabIndex(item.prereq[i]);
				if(index<0){ item.prereq.RemoveAt(i); i-=1; continue; }
				
				index=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), index, PerkDB.label);
				
				int prefabID=PerkDB.GetItem(index).prefabID;
				if(prefabID!=item.prefabID && !item.prereq.Contains(prefabID)) item.prereq[i]=prefabID;
				
				if(GUI.Button(new Rect(startX+spaceX+width+10, startY, height, height), "-")){ item.prereq.RemoveAt(i); i-=1; }
				
				startY+=spaceY;
			}
			
			int newID=-1;		CheckColor(item.prereq.Count, 0);
			newID=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), newID, PerkDB.label);
			if(newID>=0 && !item.prereq.Contains(newID)) item.prereq.Add(newID);
			startY+=10;	
			ResetColor();
			
			return startY;
		}
		
		
		public float DrawEffectTypeSetting(float startX, float startY, Perk item){
			if(!item.SupportModNMul()) return startY;
			
			int type=(int)item.effType;		contL=TDE.SetupContL(effTypeLabel, effTypeTooltip);
			TDE.Label(startX, startY, width, height, "Effect Type:", "", TDE.headerS);
			type = EditorGUI.Popup(new Rect(startX+spaceX, startY, 2*widthS+3, height), new GUIContent(""), type, contL);
			item.effType=(Perk._EffType)type;
			item.effect.effType=(Effect._EffType)type;
			
			
			if(GUI.Button(new Rect(startX+spaceX+2*widthS+5, startY, widthS*2-12, height), "Reset")){
				if(item.effType==Perk._EffType.Modifier){
					item.gain=0;
					for(int i=0; i<item.gainList.Count; i++) item.gainList[i]=0;
				
					if(item.type==_PerkType.ModifyAbility) item.costMul=0;
					if(item.type==_PerkType.ModifyEffect) item.effect.duration=0; 
				}
				if(item.effType==Perk._EffType.Multiplier){
					item.gain=1;
					for(int i=0; i<item.gainList.Count; i++) item.gainList[i]=1;
					
					if(item.type==_PerkType.ModifyAbility)item.costMul=1;
					if(item.type==_PerkType.ModifyEffect)item.effect.duration=1;
				}
				
				item.effect.Reset();
			}
			
			
			return startY+spaceY;
		}
		
		
		private bool foldStats=true;
		protected float DrawEffectSetting(float startX, float startY, Perk item){
			//TDE.Label(startX, startY, spaceX*2, height, "Perk Effect Attribute", "", TDE.headerS);	startY+=spaceY;
			string text="Perk Effect Attribute ";//+ (!foldStats ? "(show)" : "(hide)");
			foldStats=EditorGUI.Foldout(new Rect(startX, startY+=spaceY, spaceX, height), foldStats, text, TDE.foldoutS);
			if(!foldStats) return startY+spaceY;
			
			startY+=spaceY;	startX+=12;	
			
			if(item.type==_PerkType.NewTower){
				TDE.Label(startX, startY, width, height, "New Tower:", "The new tower to be added to game");
				item.newTowerPID=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), item.newTowerPID, TowerDB.label);
				
				TDE.Label(startX, startY+=spaceY, width, height, " - Replacing:", "OPTIONAL - exiting tower that will be replaced");
				item.replaceTowerPID=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), item.replaceTowerPID, TowerDB.label);
			}
			
			else if(item.type==_PerkType.NewAbility){
				TDE.Label(startX, startY, width, height, "New Ability:", "The new ability to be added to game");
				item.newAbilityPID=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), item.newAbilityPID, AbilityDB.label);
				
				TDE.Label(startX, startY+=spaceY, width, height, " - Replacing:", "OPTIONAL - exiting ability that will be replaced");
				item.replaceAbilityPID=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), item.replaceAbilityPID, AbilityDB.label);
			}
			
			else if(item.UseGainValue() || item.UseGainList()){
				startY=DrawEffectTypeSetting(startX, startY, item);
				
				string txtType=item.IsMultiplier() ? "Multiplier:" : "Modifier:" ;
				if(!item.SupportModNMul()) txtType="Gain:";
				
				if(item.UseGainValue()){
					string txt=item.UseGainList() ? "Global " : "" ;
					
					TDE.Label(startX, startY, width, height, txt+txtType);//"Gain Value:", "");
					item.gain=EditorGUI.FloatField(new Rect(startX+spaceX, startY, widthS, height), item.gain);
					startY+=spaceY;
				}
			
				if(item.UseGainList()){
					if(item.gainList.Count<RscDB.GetCount()) item.gainList.Add(0);
					if(item.gainList.Count>RscDB.GetCount()) item.gainList.Remove(item.gainList.Count-1);
					
					for(int i=0; i<item.gainList.Count; i++){
						TDE.DrawSprite(new Rect(startX, startY, height, height), RscDB.GetIcon(i));
						TDE.Label(startX+height, startY, width-height, height, " - "+RscDB.GetName(i));	//" - "+txtType, "");
						item.gainList[i]=EditorGUI.FloatField(new Rect(startX+spaceX, startY, widthS, height), item.gainList[i]);
						if(i<item.gainList.Count-1) startY+=spaceY;
					}
				}
				else startY-=spaceY;
			}
			
			else if(item.UseStats()){
				string textItem="";
				if(item.type==_PerkType.ModifyTower) textItem="towers";
				if(item.type==_PerkType.ModifyAbility) textItem="abilities";
				if(item.type==_PerkType.ModifyEffect) textItem="effects";
				
				TDE.Label(startX, startY, width, height, "Apply To All:", "Check to apply to all "+textItem);	
				item.applyToAll=EditorGUI.Toggle(new Rect(startX+spaceX, startY, widthS, height), item.applyToAll);
				
				if(!item.applyToAll){
					startY+=spaceY;
					if(item.type==_PerkType.ModifyTower){
						TDE.Label(startX, startY, width, height, "Target Tower:", "The target towers which this perk should be applied to");
						for(int i=0; i<item.towerPIDList.Count; i++){
							if(item.towerPIDList[i]<0){ item.towerPIDList.RemoveAt(i); i-=1; continue; } 
							
							int index=TowerDB.GetPrefabIndex(item.towerPIDList[i]);
							index=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), index, TowerDB.label);
							int prefabID=TowerDB.GetItem(index).prefabID;
							if(prefabID!=item.prefabID && !item.towerPIDList.Contains(prefabID)) item.towerPIDList[i]=prefabID;
							
							if(GUI.Button(new Rect(startX+spaceX+width+10, startY, height, height), "-")){ item.towerPIDList.RemoveAt(i); i-=1; }
							
							startY+=spaceY;
						}
						
						int newIdx=-1;
						newIdx=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), newIdx, TowerDB.label);
						if(newIdx>=0 && !item.towerPIDList.Contains(TowerDB.GetItem(newIdx).prefabID)){
							item.towerPIDList.Add(TowerDB.GetItem(newIdx).prefabID);
						}
					}
					if(item.type==_PerkType.ModifyAbility){
						TDE.Label(startX, startY, width, height, "Target Ability:", "The target abilities which this perk should be applied to");
						for(int i=0; i<item.abilityPIDList.Count; i++){
							int index=AbilityDB.GetPrefabIndex(item.abilityPIDList[i]);
							index=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), index, AbilityDB.label);
							int prefabID=AbilityDB.GetItem(index).prefabID;
							if(prefabID!=item.prefabID && !item.abilityPIDList.Contains(prefabID)) item.abilityPIDList[i]=prefabID;
							
							if(GUI.Button(new Rect(startX+spaceX+width+10, startY, height, height), "-")){ item.abilityPIDList.RemoveAt(i); i-=1; }
							
							startY+=spaceY;
						}
						
						int newIdx=-1;
						newIdx=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), newIdx, AbilityDB.label);
						if(newIdx>=0 && !item.abilityPIDList.Contains(AbilityDB.GetItem(newIdx).prefabID)){
							item.abilityPIDList.Add(AbilityDB.GetItem(newIdx).prefabID);
						}
					}
					if(item.type==_PerkType.ModifyEffect){
						TDE.Label(startX, startY, width, height, "Target Effect:", "The target effects which this perk should be applied to");
						for(int i=0; i<item.effectPIDList.Count; i++){
							int index=EffectDB.GetPrefabIndex(item.effectPIDList[i]);
							index=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), index, EffectDB.label);
							int prefabID=EffectDB.GetItem(index).prefabID;
							
							if(prefabID!=item.prefabID && !item.effectPIDList.Contains(prefabID)) item.effectPIDList[i]=prefabID;
							
							if(GUI.Button(new Rect(startX+spaceX+width+10, startY, height, height), "-")){ item.effectPIDList.RemoveAt(i); i-=1; }
							
							startY+=spaceY;
						}
						
						int newIdx=-1;
						newIdx=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), newIdx, EffectDB.label);
						if(newIdx>=0 && !item.effectPIDList.Contains(EffectDB.GetItem(newIdx).prefabID)){
							item.effectPIDList.Add(EffectDB.GetItem(newIdx).prefabID);
						}
					}
				}
				
				startY+=spaceY+10;
				
				startY=DrawEffectTypeSetting(startX, startY, item)-spaceY;
				
				startY+=spaceY;
				
				_EType eType=_EType.PerkT;
				
				if(item.type==_PerkType.ModifyAbility){
					eType=_EType.PerkA;
					
					TDE.Label(startX, startY, width, height, "Use Limit:", "Modify the use limit of the ability");
					if(item.effType==Perk._EffType.Multiplier) TDE.Label(startX+spaceX, startY, widthS, height, "-");
					else item.gain=EditorGUI.FloatField(new Rect(startX+spaceX, startY, widthS, height), item.gain);
					
					TDE.Label(startX, startY+=spaceY, width, height, "Cost:", "Modify/Multiply the activation cost of the ability");
					item.costMul=EditorGUI.FloatField(new Rect(startX+spaceX, startY, widthS, height), item.costMul);
					startY+=spaceY;
				}
				else if(item.type==_PerkType.ModifyEffect){
					eType=_EType.PerkE;
					
					TDE.Label(startX, startY, width, height, "Duration:", "Modify the duration of the effect");
					item.effect.duration=EditorGUI.FloatField(new Rect(startX+spaceX, startY, widthS, height), item.effect.duration);
					
					TDE.Label(startX, startY+=spaceY+5, width, height, "Stun:", "Check to enable the effec to stun. This will only override the default value if it's set to true");
					item.effect.stun=EditorGUI.Toggle(new Rect(startX+spaceX, startY, height, height), item.effect.stun);
					startY+=spaceY;
				}
				
				startY=DrawStats(startX-12, startY, item.effect.stats, eType)-spaceY;
			}
			
			else if(item.IsForPerk()){
				TDE.Label(startX, startY, width, height, "Apply To All:", "Check to apply to all perk");	
				item.applyToAll=EditorGUI.Toggle(new Rect(startX+spaceX, startY, widthS, height), item.applyToAll);
				
				if(!item.applyToAll){
					TDE.Label(startX, startY+=spaceY, width, height, "Target Perk:", "The target perk which this perk affect should be applied to");
					for(int i=0; i<item.perkPIDList.Count; i++){
						int index=PerkDB.GetPrefabIndex(item.perkPIDList[i]);
						index=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), index, PerkDB.label);
						int prefabID=PerkDB.GetItem(index).prefabID;
						if(prefabID!=item.prefabID && !item.perkPIDList.Contains(prefabID)) item.perkPIDList[i]=prefabID;
						
						if(GUI.Button(new Rect(startX+spaceX+width+10, startY, height, height), "-")){ item.perkPIDList.RemoveAt(i); i-=1; }
						
						startY+=spaceY;
					}
					
					int newID=-1;
					newID=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), newID, PerkDB.label);
					if(newID>=0 && !item.perkPIDList.Contains(newID)) item.perkPIDList.Add(newID);
					startY+=spaceY+10;
				}
				
				TDE.Label(startX, startY, width, height, "Perk Rsc Multiplier:", "Modify/Multiply the purchase cost of the ability");
				item.costMul=EditorGUI.FloatField(new Rect(startX+spaceX+25, startY, widthS, height), item.costMul);
				
				if(item.gainList.Count<RscDB.GetCount()) item.gainList.Add(0);
				if(item.gainList.Count>RscDB.GetCount()) item.gainList.Remove(item.gainList.Count-1);
				
				for(int i=0; i<item.gainList.Count; i++){
					TDE.DrawSprite(new Rect(startX, startY+=spaceY, height, height), RscDB.GetIcon(i));
					TDE.Label(startX+height, startY, width-height, height, " - "+RscDB.GetName(i)+":", "");
					item.gainList[i]=EditorGUI.FloatField(new Rect(startX+spaceX+25, startY, widthS, height), item.gainList[i]);
				}
			}
			
			return startY+spaceY;
		}
		
		
		
		
		
		protected Vector2 DrawPerkList(float startX, float startY, List<Perk> perkList){
			List<EItem> list=new List<EItem>();
			for(int i=0; i<perkList.Count; i++){
				EItem item=new EItem(perkList[i].prefabID, perkList[i].name, perkList[i].icon);
				list.Add(item);
			}
			return DrawList(startX, startY, window.position.width, window.position.height, list);
		}
		
		
		
		public static int NewItem(int idx=-1){ return window._NewItem(idx); }
		private int _NewItem(int idx=-1){
			Perk item=null;
			if(idx<0){ item=new Perk(); item.effect.Reset(); }
			if(idx>=0) item=PerkDB.GetList()[idx].Clone();
			
			item.prefabID=TDE.GenerateNewID(PerkDB.GetPrefabIDList());
			
			PerkDB.GetList().Add(item);
			PerkDB.UpdateLabel();
			
			return PerkDB.GetList().Count-1;
		}
		
		protected override void DeleteItem(){
			PerkDB.GetList().RemoveAt(deleteID);
			PerkDB.UpdateLabel();
		}
		
		protected override void SelectItem(){ SelectItem(selectID); }
		private void SelectItem(int newID){ 
			selectID=newID;
			if(PerkDB.GetList().Count<=0) return;
			selectID=Mathf.Clamp(selectID, 0, PerkDB.GetList().Count-1);
		}
		
		protected override void ShiftItemUp(){ 	if(selectID>0) ShiftItem(-1); }
		protected override void ShiftItemDown(){ if(selectID<PerkDB.GetList().Count-1) ShiftItem(1); }
		private void ShiftItem(int dir){
			Perk item=PerkDB.GetList()[selectID];
			PerkDB.GetList()[selectID]=PerkDB.GetList()[selectID+dir];
			PerkDB.GetList()[selectID+dir]=item;
			selectID+=dir;
		}
		
		
		
	}
	
}
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Verse.Sound;

namespace Verse
{
    // Token: 0x02000F65 RID: 3941
    public static class OGFind
	{
		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06005EB4 RID: 24244 RVA: 0x002A98EC File Offset: 0x002A7CEC
		public static Root Root
		{
			get
			{
				return Current.Root;
			}
		}

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06005EB5 RID: 24245 RVA: 0x002A98F3 File Offset: 0x002A7CF3
		public static SoundRoot SoundRoot
		{
			get
			{
				return Current.Root.soundRoot;
			}
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x06005EB6 RID: 24246 RVA: 0x002A98FF File Offset: 0x002A7CFF
		public static UIRoot UIRoot
		{
			get
			{
				return (!(Current.Root != null)) ? null : Current.Root.uiRoot;
			}
		}

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x06005EB7 RID: 24247 RVA: 0x002A9921 File Offset: 0x002A7D21
		public static MusicManagerEntry MusicManagerEntry
		{
			get
			{
				return ((Root_Entry)Current.Root).musicManagerEntry;
			}
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x06005EB8 RID: 24248 RVA: 0x002A9932 File Offset: 0x002A7D32
		public static MusicManagerPlay MusicManagerPlay
		{
			get
			{
				return ((Root_Play)Current.Root).musicManagerPlay;
			}
		}

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x06005EB9 RID: 24249 RVA: 0x002A9943 File Offset: 0x002A7D43
		public static LanguageWorker ActiveLanguageWorker
		{
			get
			{
				return LanguageDatabase.activeLanguage.Worker;
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06005EBA RID: 24250 RVA: 0x002A994F File Offset: 0x002A7D4F
		public static Camera Camera
		{
			get
			{
				return Current.Camera;
			}
		}

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x06005EBB RID: 24251 RVA: 0x002A9956 File Offset: 0x002A7D56
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.CameraDriver;
			}
		}

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x06005EBC RID: 24252 RVA: 0x002A995D File Offset: 0x002A7D5D
		public static ColorCorrectionCurves CameraColor
		{
			get
			{
				return Current.ColorCorrectionCurves;
			}
		}

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x06005EBD RID: 24253 RVA: 0x002A9964 File Offset: 0x002A7D64
		public static Camera PortraitCamera
		{
			get
			{
				return PortraitCameraManager.PortraitCamera;
			}
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x06005EBE RID: 24254 RVA: 0x002A996B File Offset: 0x002A7D6B
		public static PortraitRenderer PortraitRenderer
		{
			get
			{
				return PortraitCameraManager.PortraitRenderer;
			}
		}

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06005EBF RID: 24255 RVA: 0x002A9972 File Offset: 0x002A7D72
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.WorldCamera;
			}
		}

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x06005EC0 RID: 24256 RVA: 0x002A9979 File Offset: 0x002A7D79
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.WorldCameraDriver;
			}
		}

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x06005EC1 RID: 24257 RVA: 0x002A9980 File Offset: 0x002A7D80
		public static WindowStack WindowStack
		{
			get
			{
				return (Find.UIRoot == null) ? null : Find.UIRoot.windows;
			}
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06005EC2 RID: 24258 RVA: 0x002A999C File Offset: 0x002A7D9C
		public static ScreenshotModeHandler ScreenshotModeHandler
		{
			get
			{
				return Find.UIRoot.screenshotMode;
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06005EC3 RID: 24259 RVA: 0x002A99A8 File Offset: 0x002A7DA8
		public static MainButtonsRoot MainButtonsRoot
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mainButtonsRoot;
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06005EC4 RID: 24260 RVA: 0x002A99B9 File Offset: 0x002A7DB9
		public static MainTabsRoot MainTabsRoot
		{
			get
			{
				return Find.MainButtonsRoot.tabs;
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06005EC5 RID: 24261 RVA: 0x002A99C5 File Offset: 0x002A7DC5
		public static MapInterface MapUI
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mapUI;
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06005EC6 RID: 24262 RVA: 0x002A99D6 File Offset: 0x002A7DD6
		public static Selector Selector
		{
			get
			{
				return Find.MapUI.selector;
			}
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06005EC7 RID: 24263 RVA: 0x002A99E2 File Offset: 0x002A7DE2
		public static Targeter Targeter
		{
			get
			{
				return Find.MapUI.targeter;
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06005EC8 RID: 24264 RVA: 0x002A99EE File Offset: 0x002A7DEE
		public static ColonistBar ColonistBar
		{
			get
			{
				return Find.MapUI.colonistBar;
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06005EC9 RID: 24265 RVA: 0x002A99FA File Offset: 0x002A7DFA
		public static DesignatorManager DesignatorManager
		{
			get
			{
				return Find.MapUI.designatorManager;
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x06005ECA RID: 24266 RVA: 0x002A9A06 File Offset: 0x002A7E06
		public static ReverseDesignatorDatabase ReverseDesignatorDatabase
		{
			get
			{
				return Find.MapUI.reverseDesignatorDatabase;
			}
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06005ECB RID: 24267 RVA: 0x002A9A12 File Offset: 0x002A7E12
		public static GameInitData GameInitData
		{
			get
			{
				return (Current.Game == null) ? null : Current.Game.InitData;
			}
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06005ECC RID: 24268 RVA: 0x002A9A2E File Offset: 0x002A7E2E
		public static GameInfo GameInfo
		{
			get
			{
				return Current.Game.Info;
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06005ECD RID: 24269 RVA: 0x002A9A3C File Offset: 0x002A7E3C
		public static Scenario Scenario
		{
			get
			{
				if (Current.Game != null && Current.Game.Scenario != null)
				{
					return Current.Game.Scenario;
				}
				if (ScenarioMaker.GeneratingScenario != null)
				{
					return ScenarioMaker.GeneratingScenario;
				}
				if (Find.UIRoot != null)
				{
					Page_ScenarioEditor page_ScenarioEditor = Find.WindowStack.WindowOfType<Page_ScenarioEditor>();
					if (page_ScenarioEditor != null)
					{
						return page_ScenarioEditor.EditingScenario;
					}
				}
				return null;
			}
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06005ECE RID: 24270 RVA: 0x002A9AA0 File Offset: 0x002A7EA0
		public static World World
		{
			get
			{
				return (Current.Game == null || Current.Game.World == null) ? Current.CreatingWorld : Current.Game.World;
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06005ECF RID: 24271 RVA: 0x002A9ACF File Offset: 0x002A7ECF
		public static List<Map> Maps
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.Maps;
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06005ED0 RID: 24272 RVA: 0x002A9AE7 File Offset: 0x002A7EE7
		public static Map CurrentMap
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.CurrentMap;
			}
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06005ED1 RID: 24273 RVA: 0x002A9AFF File Offset: 0x002A7EFF
		public static Map AnyPlayerHomeMap
		{
			get
			{
				return Current.Game.AnyPlayerHomeMap;
			}
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06005ED2 RID: 24274 RVA: 0x002A9B0B File Offset: 0x002A7F0B
		public static StoryWatcher StoryWatcher
		{
			get
			{
				return Current.Game.storyWatcher;
			}
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x06005ED3 RID: 24275 RVA: 0x002A9B17 File Offset: 0x002A7F17
		public static ResearchManager ResearchManager
		{
			get
			{
				return Current.Game.researchManager;
			}
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06005ED4 RID: 24276 RVA: 0x002A9B23 File Offset: 0x002A7F23
		public static Storyteller Storyteller
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.storyteller;
			}
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06005ED5 RID: 24277 RVA: 0x002A9B3B File Offset: 0x002A7F3B
		public static GameEnder GameEnder
		{
			get
			{
				return Current.Game.gameEnder;
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x06005ED6 RID: 24278 RVA: 0x002A9B47 File Offset: 0x002A7F47
		public static LetterStack LetterStack
		{
			get
			{
				return Current.Game.letterStack;
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x06005ED7 RID: 24279 RVA: 0x002A9B53 File Offset: 0x002A7F53
		public static Archive Archive
		{
			get
			{
				return (Find.History == null) ? null : Find.History.archive;
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x06005ED8 RID: 24280 RVA: 0x002A9B6F File Offset: 0x002A7F6F
		public static PlaySettings PlaySettings
		{
			get
			{
				return Current.Game.playSettings;
			}
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x06005ED9 RID: 24281 RVA: 0x002A9B7B File Offset: 0x002A7F7B
		public static History History
		{
			get
			{
				return (Current.Game == null) ? null : Current.Game.history;
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x06005EDA RID: 24282 RVA: 0x002A9B97 File Offset: 0x002A7F97
		public static TaleManager TaleManager
		{
			get
			{
				return Current.Game.taleManager;
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06005EDB RID: 24283 RVA: 0x002A9BA3 File Offset: 0x002A7FA3
		public static PlayLog PlayLog
		{
			get
			{
				return Current.Game.playLog;
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x06005EDC RID: 24284 RVA: 0x002A9BAF File Offset: 0x002A7FAF
		public static BattleLog BattleLog
		{
			get
			{
				return Current.Game.battleLog;
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x06005EDD RID: 24285 RVA: 0x002A9BBB File Offset: 0x002A7FBB
		public static TickManager TickManager
		{
			get
			{
				return Current.Game.tickManager;
			}
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06005EDE RID: 24286 RVA: 0x002A9BC7 File Offset: 0x002A7FC7
		public static Tutor Tutor
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.tutor;
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06005EDF RID: 24287 RVA: 0x002A9BDF File Offset: 0x002A7FDF
		public static TutorialState TutorialState
		{
			get
			{
				return Current.Game.tutor.tutorialState;
			}
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x06005EE0 RID: 24288 RVA: 0x002A9BF0 File Offset: 0x002A7FF0
		public static ActiveLessonHandler ActiveLesson
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.tutor.activeLesson;
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06005EE1 RID: 24289 RVA: 0x002A9C0D File Offset: 0x002A800D
		public static Autosaver Autosaver
		{
			get
			{
				return Current.Game.autosaver;
			}
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x06005EE2 RID: 24290 RVA: 0x002A9C19 File Offset: 0x002A8019
		public static DateNotifier DateNotifier
		{
			get
			{
				return Current.Game.dateNotifier;
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x06005EE3 RID: 24291 RVA: 0x002A9C25 File Offset: 0x002A8025
		public static SignalManager SignalManager
		{
			get
			{
				return Current.Game.signalManager;
			}
		}

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x06005EE4 RID: 24292 RVA: 0x002A9C31 File Offset: 0x002A8031
		public static UniqueIDsManager UniqueIDsManager
		{
			get
			{
				return (Current.Game == null) ? null : Current.Game.uniqueIDsManager;
			}
		}

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x06005EE5 RID: 24293 RVA: 0x002A9C4D File Offset: 0x002A804D
		public static OGNFactionManager OGNFactionManager
		{
			get
			{
				return (OGNFactionManager)OGFind.World.factionManager;
			}
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x06005EE6 RID: 24294 RVA: 0x002A9C59 File Offset: 0x002A8059
		public static WorldPawns WorldPawns
		{
			get
			{
				return Find.World.worldPawns;
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x06005EE7 RID: 24295 RVA: 0x002A9C65 File Offset: 0x002A8065
		public static WorldObjectsHolder WorldObjects
		{
			get
			{
				return Find.World.worldObjects;
			}
		}

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x06005EE8 RID: 24296 RVA: 0x002A9C71 File Offset: 0x002A8071
		public static WorldGrid WorldGrid
		{
			get
			{
				return Find.World.grid;
			}
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x06005EE9 RID: 24297 RVA: 0x002A9C7D File Offset: 0x002A807D
		public static WorldDebugDrawer WorldDebugDrawer
		{
			get
			{
				return Find.World.debugDrawer;
			}
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x06005EEA RID: 24298 RVA: 0x002A9C89 File Offset: 0x002A8089
		public static WorldPathGrid WorldPathGrid
		{
			get
			{
				return Find.World.pathGrid;
			}
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x06005EEB RID: 24299 RVA: 0x002A9C95 File Offset: 0x002A8095
		public static WorldDynamicDrawManager WorldDynamicDrawManager
		{
			get
			{
				return Find.World.dynamicDrawManager;
			}
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06005EEC RID: 24300 RVA: 0x002A9CA1 File Offset: 0x002A80A1
		public static WorldPathFinder WorldPathFinder
		{
			get
			{
				return Find.World.pathFinder;
			}
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06005EED RID: 24301 RVA: 0x002A9CAD File Offset: 0x002A80AD
		public static WorldPathPool WorldPathPool
		{
			get
			{
				return Find.World.pathPool;
			}
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06005EEE RID: 24302 RVA: 0x002A9CB9 File Offset: 0x002A80B9
		public static WorldReachability WorldReachability
		{
			get
			{
				return Find.World.reachability;
			}
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06005EEF RID: 24303 RVA: 0x002A9CC5 File Offset: 0x002A80C5
		public static WorldFloodFiller WorldFloodFiller
		{
			get
			{
				return Find.World.floodFiller;
			}
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06005EF0 RID: 24304 RVA: 0x002A9CD1 File Offset: 0x002A80D1
		public static WorldFeatures WorldFeatures
		{
			get
			{
				return Find.World.features;
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06005EF1 RID: 24305 RVA: 0x002A9CDD File Offset: 0x002A80DD
		public static WorldInterface WorldInterface
		{
			get
			{
				return Find.World.UI;
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x06005EF2 RID: 24306 RVA: 0x002A9CE9 File Offset: 0x002A80E9
		public static WorldSelector WorldSelector
		{
			get
			{
				return Find.WorldInterface.selector;
			}
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x06005EF3 RID: 24307 RVA: 0x002A9CF5 File Offset: 0x002A80F5
		public static WorldTargeter WorldTargeter
		{
			get
			{
				return Find.WorldInterface.targeter;
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06005EF4 RID: 24308 RVA: 0x002A9D01 File Offset: 0x002A8101
		public static WorldRoutePlanner WorldRoutePlanner
		{
			get
			{
				return Find.WorldInterface.routePlanner;
			}
		}
	}
}

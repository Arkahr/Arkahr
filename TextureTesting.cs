using System.Linq;
using Turbo.Plugins.Default;
using SharpDX;
using System.Collections.Generic;


namespace Turbo.Plugins.Arkahr
{
    public class TextureTesting : BasePlugin, IInGameTopPainter
	{        
        private List<string> texNames;

		public TextureTesting()
		{
            Enabled = false;
		}

        public override void Load(IController hud)
        {
            base.Load(hud);                
            texNames = new List<string>();
            // texNames.Add("BattleNetCinematics_ListItemOver");
            // texNames.Add("ButtleNetStore_TabButtonUp_482x87");
            // texNames.Add("ButtleNetStore_TabButtonDown_482x87");
            // texNames.Add("ButtleNetStore_TabButtonSelected_482x87");
            // texNames.Add("ButtleNetStore_TabButtonDeluxeSelected_482x87");
            // texNames.Add("Artisan_Mystic_Enchant_Selection");
            // texNames.Add("Skills_Active_Frame_Selected");
            // texNames.Add("BattleNetAuctionHouse_MenuListItemHeader");
            // texNames.Add("Season_PortraitBannerBG");
            // texNames.Add("PVP_KillLog_EventEnemy");
            // texNames.Add("BattleNetStore_PlatinumBalance_Flash");
            // texNames.Add("KanaisCube_AcceptBlinker_P2");
            // texNames.Add("Stash_Btn_Purchase_Inactive");
            // texNames.Add("MailMessageListBoxesHighlight");
            // texNames.Add("MailMessageListBoxes");
            // texNames.Add("BattleNetNotifications_AddFriendEmailMessageBox");
            // texNames.Add("BattleNetFriendsList_FriendOfFriendSelected");
            // texNames.Add("Bounties_Header");
            // texNames.Add("TraitList_Button_Inactive");
            // texNames.Add("BattleNetAuctionHouse_MenuListItemHeader");
            // texNames.Add("DevilsHand_ListItemSelection");
            // texNames.Add("Artisan_List_Select");
            // texNames.Add("BattleNetButton_ClearOverBlinker_397x66");
            // texNames.Add("Tooltip_Equipped_Frame_Title_Primal");
            // texNames.Add("Tooltip_Frame_Rank");
            // texNames.Add("Tooltip_Equipped_Frame_Title");
            // texNames.Add("Tooltip_Frame_Top");
            // texNames.Add("Tooltip_Equipped_Frame_Title_Ancient");
            // texNames.Add("Tooltip_Frame_Cost");
            // texNames.Add("BattleNetStore_PlatinumBalance_Button_Over");
            // texNames.Add("BattleNetStore_PlatinumBalance_Button_Frame");
            // texNames.Add("BattleNetStore_PlatinumBalance_Button_Default");
            // texNames.Add("EquipmentManager_SkillsTitleBg");
            // texNames.Add("EnemyHPbar03_Base");
            // texNames.Add("DevilsHand_ListItemBackgroundAllMats");
            // texNames.Add("DevilsHand_ListItemBackground");    

texNames.Add("Name");
// texNames.Add("BattleNetButton_ClearDisabled_46x46");
// texNames.Add("BattleNetButton_ClearDown_46x46");
// texNames.Add("BattleNetButton_ClearOver_46x46");
// texNames.Add("BattleNetButton_ClearUp_46x46");
// texNames.Add("BattleNetButton_ClearSelected_74x66");
// texNames.Add("BattleNetButton_ClearDisabled_74x66");
// texNames.Add("BattleNetButton_ClearDown_74x66");
// texNames.Add("BattleNetButton_ClearOver_74x66");
// texNames.Add("BattleNetButton_ClearUp_74x66");
// texNames.Add("BattleNetButton_ClearDisabled_245x66");
// texNames.Add("BattleNetButton_ClearDown_245x66");
// texNames.Add("BattleNetButton_ClearOver_245x66");
// texNames.Add("BattleNetButton_ClearUp_245x66");
// texNames.Add("BattleNetButton_ClearDisabled_260x50");
// texNames.Add("BattleNetButton_ClearDown_260x50");
// texNames.Add("BattleNetButton_ClearOver_260x50");
// texNames.Add("BattleNetButton_ClearUp_260x50");
// texNames.Add("BattleNetButton_ClearSelected_397x66");
// texNames.Add("BattleNetButton_ClearDisabled_397x66");
// texNames.Add("BattleNetButton_ClearDown_397x66");
// texNames.Add("BattleNetButton_ClearUp_397x66");
// texNames.Add("BattleNetButton_ClearOver_397x66");
// texNames.Add("BattleNetButton_ClearDisabled_64x64");
// texNames.Add("BattleNetButton_ClearDown_64x64");
// texNames.Add("BattleNetButton_ClearUp_64x64");
// texNames.Add("BattleNetButton_ClearOver_64x64");
// texNames.Add("BattleNetButton_RedDisabled_46x46");
// texNames.Add("BattleNetButton_RedDown_46x46");
// texNames.Add("BattleNetButton_RedOver_46x46");
// texNames.Add("BattleNetButton_RedUp_46x46");
// texNames.Add("BattleNetButton_RedDisabled_219x67");
// texNames.Add("BattleNetButton_RedDown_219x67");
// texNames.Add("BattleNetButton_RedOver_219x67");
// texNames.Add("BattleNetButton_RedUp_219x67");
// texNames.Add("BattleNetButton_RedDisabled_245x66");
// texNames.Add("BattleNetButton_RedDown_245x66");
// texNames.Add("BattleNetButton_RedOver_245x66");
// texNames.Add("BattleNetButton_RedUp_245x66");
// texNames.Add("BattleNetButton_RedDisabled_262x50");
// texNames.Add("BattleNetButton_RedDown_262x50");
// texNames.Add("BattleNetButton_RedOver_262x50");
// texNames.Add("BattleNetButton_RedUp_262x50");
// texNames.Add("BattleNetButton_RedDisabled_341x62");
// texNames.Add("BattleNetButton_RedDown_341x62");
// texNames.Add("BattleNetButton_RedOver_341x62");
// texNames.Add("BattleNetButton_RedSelected_341x62");
// texNames.Add("BattleNetButton_RedUp_341x62");
// texNames.Add("BattleNetButton_RedDisabled_342x74");
// texNames.Add("BattleNetButton_RedDown_342x74");
// texNames.Add("BattleNetButton_RedOver_342x74");
// texNames.Add("BattleNetButton_RedUp_342x74");
// texNames.Add("BattleNetButton_RedDisabled_397x66");
// texNames.Add("BattleNetButton_RedDown_397x66");
// texNames.Add("BattleNetButton_RedOver_397x66");
// texNames.Add("BattleNetButton_RedUp_397x66");
// texNames.Add("BattleNetButton_RedDisabled_398x82");
// texNames.Add("BattleNetButton_RedDown_398x82");
// texNames.Add("BattleNetButton_RedOver_398x82");
// texNames.Add("BattleNetButton_RedUp_398x82");
// texNames.Add("BattleNetButton_RedSelected_484x62");
// texNames.Add("BattleNetButton_RedDisabled_484x62");
// texNames.Add("BattleNetButton_RedDown_484x62");
// texNames.Add("BattleNetButton_RedOver_484x62");
// texNames.Add("BattleNetButton_RedUp_484x62");
// texNames.Add("BattleNetButton_RedDisabled_373x67");
// texNames.Add("BattleNetButton_RedDown_373x67");
// texNames.Add("BattleNetButton_RedOver_373x67");
// texNames.Add("BattleNetButton_RedUp_373x67");
// texNames.Add("BattleNetButton_YellowSelected_484x62");
// texNames.Add("BattleNetButton_YellowOver_484x62");
// texNames.Add("BattleNetButton_YellowUp_484x62");
// texNames.Add("BattleNetButton_BlueSelected_484x62");
// texNames.Add("BattleNetButton_BlueDisabled_484x62");
// texNames.Add("BattleNetButton_BlueDown_484x62");
// texNames.Add("BattleNetButton_BlueOver_484x62");
// texNames.Add("BattleNetButton_BlueUp_484x62");
// texNames.Add("BattleNetButton_RedDisabled_684x82");
// texNames.Add("BattleNetButton_RedDown_684x82");
// texNames.Add("BattleNetButton_RedOver_684x82");
 texNames.Add("BattleNetButtonRedUp");
 //texNames.Add("-------------------");
// texNames.Add("BattleNetButton_MainMenu_Base");
// texNames.Add("BattleNetButton_MainMenu_BaseEmpty");
// texNames.Add("BattleNetButton_MainMenu_ClearDisabled");
// texNames.Add("BattleNetButton_MainMenu_ClearDown");
// texNames.Add("BattleNetButton_MainMenu_ClearOver");
// texNames.Add("BattleNetButton_MainMenu_ClearUp");
// texNames.Add("BattleNetButton_MainMenu_RedDisabled");
// texNames.Add("BattleNetButton_MainMenu_RedDown");
// texNames.Add("BattleNetButton_MainMenu_RedOver");
// texNames.Add("BattleNetButton_MainMenu_RedUp");
// texNames.Add("BattleNetButton_MainMenu_Selected");
// texNames.Add("BattleNetButton_RedArrowLeftDisabled_75x67");
// texNames.Add("BattleNetButton_RedArrowLeftDown_75x67");
// texNames.Add("BattleNetButton_RedArrowLeftOver_75x67");
// texNames.Add("BattleNetButton_RedArrowLeftUp_75x67");
// texNames.Add("BattleNetButton_RedArrowRightDisabled_75x67");
// texNames.Add("BattleNetButton_RedArrowRightDown_75x67");
// texNames.Add("BattleNetButton_RedArrowRightOver_75x67");
// texNames.Add("BattleNetButton_RedArrowRightUp_75x67");
// texNames.Add("BattleNetButton_RedArrowLeftDisabled_170x66");
// texNames.Add("BattleNetButton_RedArrowLeftDown_170x66");
// texNames.Add("BattleNetButton_RedArrowLeftOver_170x66");
// texNames.Add("BattleNetButton_RedArrowLeftUp_170x66");
// texNames.Add("BattleNetButton_RedArrowRightDisabled_170x66");
// texNames.Add("BattleNetButton_RedArrowRightDown_170x66");
// texNames.Add("BattleNetButton_RedArrowRightOver_170x66");
// texNames.Add("BattleNetButton_RedArrowRightUp_170x66");
// texNames.Add("BattleNetButton_RedUp_74x66");
// texNames.Add("BattleNetButton_RedOver_74x66");
// texNames.Add("BattleNetButton_RedSelected_74x66");
// texNames.Add("BattleNetButton_RedDisabled_74x66");
// texNames.Add("BattleNetButton_RedUp_294x62");
// texNames.Add("BattleNetButton_RedOver_294x62");
// texNames.Add("BattleNetButton_RedSelected_294x62");
// texNames.Add("BattleNetButton_RedDisabled_294x62");
// texNames.Add("BattleNetButton_ClearOverBlinker_397x66");
// texNames.Add("BattleNetButton_BlueDisabled_46x46");
// texNames.Add("BattleNetButton_BlueDown_46x46");
// texNames.Add("BattleNetButton_BlueOver_46x46");
// texNames.Add("BattleNetButton_BlueUp_46x46");
// texNames.Add("BattleNetButton_BlueDisabled_341x62");
// texNames.Add("BattleNetButton_BlueDown_341x62");
// texNames.Add("BattleNetButton_BlueOver_341x62");
// texNames.Add("BattleNetButton_BlueSelected_341x62");
// texNames.Add("BattleNetButton_BlueUp_341x62");
// texNames.Add("BattleNetButton_BlueDisabled_262x50");
// texNames.Add("BattleNetButton_BluedDown_262x50");
// texNames.Add("BattleNetButton_BlueOver_262x50");
// texNames.Add("BattleNetButton_BlueUp_262x50");
// texNames.Add("BattleNetButton_BlueDisabled_219x67");
// texNames.Add("BattleNetButton_BlueDown_219x67");
// texNames.Add("BattleNetButton_BlueOver_219x67");
// texNames.Add("BattleNetButton_BlueUp_219x67");
// texNames.Add("BattleNetButton_BlueDisabled_397x66");
// texNames.Add("BattleNetButton_BlueDown_397x66");
// texNames.Add("BattleNetButton_BlueOver_397x66");
// texNames.Add("BattleNetButton_BlueUp_397x66");
// texNames.Add("PlusIcon_26x25");            
texNames.Add("icon_shape_BC2013_rgb_01");
texNames.Add("icon_shape_cathedral001_rgb_001");
texNames.Add("icon_shape_celtic001_rgb_001");
texNames.Add("icon_shape_chevron001_rgb_01");
texNames.Add("icon_shape_chevron002_rgb_01");
texNames.Add("icon_shape_chevron003_rgb_01");
texNames.Add("icon_shape_fire001_rgb_001");
texNames.Add("icon_shape_goth001_rgb_001");
texNames.Add("icon_shape_goth002_rgb_001");
texNames.Add("icon_shape_goth003_rgb_001");
texNames.Add("icon_shape_knight001_rgb_001");
texNames.Add("icon_shape_knight002_rgb_001");
texNames.Add("icon_shape_knight003_rgb_001");
texNames.Add("icon_shape_point001_rgb_01");
texNames.Add("icon_shape_point002_rgb_01");
texNames.Add("icon_shape_point003_rgb_01");
texNames.Add("icon_shape_point004_rgb_01");
texNames.Add("icon_shape_point005_rgb_001");
texNames.Add("icon_shape_pvp_01_rgb_001");
texNames.Add("icon_shape_pvp_02_rgb_001");
texNames.Add("icon_shape_pvp_03_rgb_001");
texNames.Add("icon_shape_pvp_04_rgb_001");
texNames.Add("icon_shape_pvp_05_rgb_001");
        }
		
        public void PaintTopInGame(ClipState clipState)
        {
            if (clipState != ClipState.BeforeClip) return;
            //if (!Hud.Game.IsInTown) return;
            float y = 0;
            float scale = 0.3f;
            foreach (var texName in texNames)
            {
                var tex = Hud.Texture.GetTexture(texName);
                
                if (tex!=null) 
                {
                    var x =Hud.Window.Size.Width/2-tex.Width/2;
                    //var y =
                    var w =  tex.Width * scale;
                    var h = tex.Height * scale;

                    tex.Draw(new RectangleF(x, y, w, h ));
                    y += h + 10f;
                }
                else
                {
                    var x = Hud.Window.Size.Width/2- 100;
                    //var h = ""
    
                    Hud.Debug(texName );
                    Hud.Render.CreateFont("tahoma",7f,255,255,255,255,false,false,true).DrawText(texName+"\n",x,y);
                    //Hud.Render.CreateBrush(255, 0, 0, 0, 0).DrawRectangle(new RectangleF(Hud.Window.Size.Width/2 -100, y, 200, 40));
                    y += 20f + 5f;
                }                    
                
            }
            
        }
    }
}
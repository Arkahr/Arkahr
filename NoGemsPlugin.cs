using System;
using System.Linq;
using System.Collections.Generic;

using Turbo.Plugins;
using Turbo.Plugins.Default;
using Turbo.Plugins.Jack.Extensions;

namespace Turbo.Plugins.Arkahr
{
    public class NoGemsPlugin : BasePlugin, IInGameTopPainter
    {
        private Func<IPlayer, int> playerMissingLegendaryGemsPowersCount;
        private Func<IPlayer, string> messageFormat;

        public TopLabelWithTitleDecorator LabelDecorator;

        public NoGemsPlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            LabelDecorator = new TopLabelWithTitleDecorator(Hud)             
            {
                BorderBrush = Hud.Render.CreateBrush(255, 230, 30, 30, -1),
                BackgroundBrush = Hud.Render.CreateBrush(190, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 8, 255,  230, 30, 30, false, false, false),
                TitleFont = Hud.Render.CreateFont("tahoma", 10, 255, 255, 0, 0, true, false, false),
            };

            playerMissingLegendaryGemsPowersCount = (p) => 3 - p.Powers.UsedLegendaryGems.AllGemPrimaryBuffs().Count(b => b.Active && b.SnoPower.Sno != 403459 && b.SnoPower.Sno != 454736);
            messageFormat = (p) => string.Format("{0} ({1}) -> {2} missing\n", p.BattleTagAbovePortrait, p.HeroClassDefinition.HeroClass, playerMissingLegendaryGemsPowersCount(p));
        }

        public void PaintTopInGame(ClipState clipState)
        {
            // DO NOT SHOW POPUP IF:
            if (clipState != ClipState.BeforeClip) return; //clipped by window
            if (!Hud.Game.IsInTown) return; //you are not in town

            var players = GetPlayersWithMissingGemPowers();
            if (!players.Any()) return; //there are no players missing gem powers

            var text = string.Empty;
            var longestLineInText = 0;          
            
            foreach (var player in players)
            {
                var line = messageFormat(player);

                if (longestLineInText < line.Length)
                    longestLineInText = line.Length;

                text += line;              
            }

            var popUpWidth = longestLineInText * 8f; //make popup stretch with longer lines
            var popUpHeight = 110f;
            LabelDecorator.Paint(Hud.Window.Size.Width / 2f - popUpWidth/2, 0, popUpWidth, popUpHeight, text, "No Gems!");
        }

        /// <summary>
        /// Gets the level 70 players with missing gem powers.
        /// </summary>
        /// <returns></returns>
        private List<IPlayer> GetPlayersWithMissingGemPowers()
        {
            return Hud.Game
                .Players
                .Where(p => p.CurrentLevelNormal >= 70)
                .Where(p => p.Powers.UsedLegendaryGems.AllGemPrimaryBuffs().Count(b => b.Active && b.SnoPower.Sno != 403459 && b.SnoPower.Sno != 454736) < 3)
                .ToList();
        }
    }
}
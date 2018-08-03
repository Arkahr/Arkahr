using System.Linq;
using Turbo.Plugins.Default;
using System.Collections.Generic;
using System;

namespace Turbo.Plugins.Arkahr
{

    // public class mylab : MapLabelDecorator , IWorldDecorator
    // {

    //     public mylab(IController hud) : base(hud)
    //     {
         
    //     }

    //     public new void Paint(IActor actor, IWorldCoordinate coord, string text)
    //     {
    //         if (!Enabled) return;
    //         if (LabelFont == null) return;
    //         if (string.IsNullOrEmpty(text)) return;

    //         float mapx, mapy;
    //         Hud.Render.GetMinimapCoordinates(coord.X, coord.Y, out mapx, out mapy);

    //         var layout = LabelFont.GetTextLayout(text);
    //         if (!Up)
    //         {
    //             LabelFont.DrawText(layout, mapx - layout.Metrics.Width / 2, mapy + RadiusOffset);
    //         }
    //         else
    //         {
    //             LabelFont.DrawText(layout, mapx - layout.Metrics.Width / 2, mapy - RadiusOffset - layout.Metrics.Height);
    //         }
    //     }
    // }
    
    public class TownPoI : BasePlugin, IInGameWorldPainter
	{        
        public WorldDecoratorCollection KadalaDecorator { get; set; }
        public WorldDecoratorCollection MyriamDecorator { get; set; }
        public WorldDecoratorCollection EnchantressDecorator { get; set; }
        public bool KadalaEnabled { get; set; }
        public bool MyriamEnabled { get; set; } //enchanting npc
        public bool FollowersEnabled { get; set; }

       // private BrushFromHex brushFromHex;

        
		public TownPoI()
		{
            Enabled = true;
            KadalaEnabled = true;
            MyriamEnabled = false;
            FollowersEnabled = false;
		}

        public override void Load(IController hud)
        {
            base.Load(hud);  
            //brushFromHex = new BrushFromHex(hud);
            
            KadalaDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(255, 0, 0, 0, 0),
                    BorderBrush = Hud.Render.CreateBrush(200, 255, 100, 100, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 8f, 255, 255, 100, 100, true, false, 128, 0, 0, 0, true),
                }
                );      

            MyriamDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(255, 0, 0, 0, 0),
                    BorderBrush = Hud.Render.CreateBrush(200, 255, 216, 0, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 8f, 255, 255, 216, 0, true, false, 128, 0, 0, 0, true),
                }
                );
            EnchantressDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    //BackgroundTexture1 = Hud.Texture.GetTexture("Portrait_Follower_Enchantress"),   
                    //BackgroundTexture2 = Hud.Texture.GetTexture("Portrait_Follower_Enchantress"),   
                    
                    BorderBrush = Hud.Render.CreateBrush(255, 223, 73, 223, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 8f, 255, 223, 73, 223, true, false, 128, 0, 0, 0, true),

                }
            );  
        }
		
		public void PaintWorld(WorldLayer layer)
		{
            if (!Hud.Game.IsInTown) return;

            // Kadala Label toggle on/off 
            if (KadalaEnabled) {
                var Kadala = Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.SnoActor.NameEnglish == "Kadala");
                foreach (var actor in Kadala)
                {
                    KadalaDecorator.Paint(layer, actor, actor.FloorCoordinate, actor.SnoActor.NameLocalized + " " + Hud.Game.Me.Materials.BloodShard);
                }
            }
            

           // Myriam Label toggle on/off
           if (MyriamEnabled) {               
                var Myriam = Hud.Game.Actors.Where(x => x.DisplayOnOverlay && !x.IsOnScreen && x.SnoActor.NameEnglish == "Enchant");

                foreach (var actor in Myriam)
                {
                    MyriamDecorator.Paint(layer, actor, actor.FloorCoordinate, actor.SnoActor.NameLocalized);
                }
            }

            //Hud.Texture.
            if (FollowersEnabled) {
                //dodaj obsluge catch try! kiedys musisz zaczac :>
                List<Tuple<IEnumerable<IActor>, ITexture>> list = new List<Tuple<IEnumerable<IActor>, ITexture>>();
                list.Add( 
                    new Tuple<IEnumerable<IActor>, ITexture>( 
                        Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.SnoActor.Sno == 4062),
                        Hud.Texture.GetTexture("Portrait_Follower_Enchantress")
                        )
                    );
                list.Add( 
                    new Tuple<IEnumerable<IActor>, ITexture>( 
                        Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.SnoActor.Sno == 4538),
                        Hud.Texture.GetTexture("Portrait_TemplarNPC_x1")
                        )
                    );
                list.Add( 
                    new Tuple<IEnumerable<IActor>, ITexture>( 
                        Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.SnoActor.Sno == 4644),
                        Hud.Texture.GetTexture("Portrait_Follower_Scoundrel")
                        )
                    );

                //list = (texture:Hud.Texture.GetTexture("Portrait_Follower_Enchantress"), actor: Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.SnoActor.Sno == 4062).First());
                //List<ITexture, IActor> followers = new List<ITexture, IActor>();
                //ITexture[] followersTexture = new ITexture[3]
                // followersTexture.Add(Hud.Texture.GetTexture("Portrait_Follower_Enchantress"));
                // followersTexture.Add(Hud.Texture.GetTexture("Portrait_TemplarNPC_x1"));
                // followersTexture.Add(Hud.Texture.GetTexture("Portrait_Follower_Scoundrel"));
                // followersTexture[0] = Hud.Texture.GetTexture("Portrait_Follower_Enchantress");
                // followersTexture[1] = Hud.Texture.GetTexture("Portrait_TemplarNPC_x1");
                // followersTexture[2] = Hud.Texture.GetTexture("Portrait_Follower_Scoundrel");                
                
                // List<IActor> followerActors = new List<IActor>();
                // followerActors.Add(Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.SnoActor.Sno == 4062).First()); //enchantress
                // followerActors.Add(Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.SnoActor.Sno == 52693).First()); //templar
                // followerActors.Add(Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.SnoActor.Sno == 4644).First()); //scoundrell
                


                float texX, texY;
                
                // TODO offset to make texure triangle 
                //   1 
                // 3   2
                var texSize = 22f;
                 var offsetList = new List<Tuple<float, float>> {
                     new Tuple<float, float>(0,0),
                     new Tuple<float, float>(-22f,22f),
                     new Tuple<float, float>(22f,22f),
                 };
                 var texArray = offsetList.ToArray();
                // first     x + 5
                // second    x + 10
                var i=0;
                foreach (var item in list)
                {

                    if (item.Item1.Any()) {
                        IActor follower = item.Item1.First();
                        Hud.Render.GetMinimapCoordinates(follower.FloorCoordinate.X, follower.FloorCoordinate.Y, out texX, out texY);
                        // var newTexCoordX = texX;
                        // var newTexCoordY = texY;
                        // texX += texArray[i].Item1;
                        // texY += texArray[i].Item2;
                        //if (item.Item1.First().)
                        //TODO offset each on condition in which configuration are they standing from each other ?
                        // but god damnit they are located ... i have to know which has to be offest in what direction
                        //need to state their cords with...

                        item.Item2.Draw(texX + texArray[i].Item1, texY + texArray[i].Item1, texSize, texSize, 1f);
                        //item.Item2.Draw(texX - texSize/2, texY - texSize/2, texSize, texSize, 1f);
                        i++;
                    }

                }

            }
		}
    }
}
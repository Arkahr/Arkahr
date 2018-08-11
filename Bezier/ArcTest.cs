using Turbo.Plugins.Default;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System;

namespace Turbo.Plugins.Arkahr
{
    public enum Orientation
    {
        Left,
        Right        
    }

    public class ArcTest : BasePlugin, IInGameTopPainter
	{        
		public ArcTest()
		{
            Enabled = true;
		}

        public override void Load(IController hud)
        {
            base.Load(hud);                
        }

        public void drawArc(float x, float y, float width, float height,  float arcWidth, IBrush brush, Orientation o)
        {
            RawVector2 startPoint = o == Orientation.Left ? new RawVector2(x + width - arcWidth, y) : new RawVector2(x, y);
            using (var arc = Hud.Render.CreateGeometry()) 
            {
                using (var sink = arc.Open())
                {
                    //begin drawing from top start point of curve
                    sink.BeginFigure(startPoint, FigureBegin.Filled);
                    QuadraticBezierSegment qbs_1 = new QuadraticBezierSegment();
                    //add point for curve control point
                    qbs_1.Point1 = o == Orientation.Left ? new RawVector2(x, y + height/2) : new RawVector2(x + width - arcWidth, y + height/2); //watch out for  fractions
                    var bezEndPoint = qbs_1.Point2 = o == Orientation.Left ? new RawVector2(x + width - arcWidth, y + height) : new RawVector2(x, y + height); 
                    sink.AddQuadraticBezier(qbs_1);

                    //draw line between low end point of curve and low start point of 2nd curve
                    sink.AddLine(new RawVector2(bezEndPoint.X + arcWidth, bezEndPoint.Y));

                    QuadraticBezierSegment qbs_2 = new QuadraticBezierSegment();
                    //add point for 2nd curve control point
                    qbs_2.Point1 = o == Orientation.Left ? new RawVector2(x + arcWidth, y +  height/2) : new RawVector2(x + width, y + height/2); //watch out for  fractions
                    var bez2EndPoint = qbs_2.Point2 =  o == Orientation.Left ? new RawVector2(x + width, y) : new RawVector2(x + arcWidth, y); //its on top!
                    sink.AddQuadraticBezier(qbs_2);
                    
                    //draw line between top end point of 2nd curve and top start point of 1st curve
                    sink.AddLine(bez2EndPoint); //?

                    sink.EndFigure(FigureEnd.Closed);
                    sink.Close();
                }
                brush.DrawGeometry(arc);
            }

        }

        public void PaintTopInGame(ClipState clipState)
        {
            if (clipState != ClipState.BeforeClip) return;
            //DrawRect();
            //DrawBezier();

            var hudWindow = Hud.Window.Size;
            var screenCenterX = hudWindow.Width/2;
            var screenCenterY = hudWindow.Height/2;

            var arcWidth = 70;
            var arcHeight = 413 * 0.7f;
            var arcFat = 10;
            var arcSpacing = 130;
            
            var leftArcX = screenCenterX - arcWidth*2 - arcSpacing;
            var leftArcY = hudWindow.Height * 0.41f - arcHeight/2;

            var rightArcX = screenCenterX + arcSpacing;
            var rightArcY = leftArcY;

            var green = Hud.Render.CreateBrush(255,0,230,20,0);
            var blue = Hud.Render.CreateBrush(255, 0, 168, 255, 0);
            var orange = Hud.Render.CreateBrush(255, 255, 132, 0, 0);
            var red = Hud.Render.CreateBrush(255, 255, 39, 3, 0);
            var cream = Hud.Render.CreateBrush(255, 215, 201, 164, 0);
            
            var redT = Hud.Render.CreateBrush(70, 255, 39, 3, 0);
            //var creamT = Hud.Render.CreateBrush(70, 255, 204, 0, 0);
            var creamT = Hud.Render.CreateBrush(90, 215, 201, 164, 0);
            
            // drawArc(300, 300, 200, 500, 50, green, Orientation.Left);
            // drawArc(800, 300, 200, 500, 50, orange, Orientation.Right);            
            drawArc(leftArcX, leftArcY, arcWidth*2, arcHeight, arcFat, redT, Orientation.Left);
            drawArc(rightArcX, rightArcY, arcWidth*2, arcHeight, arcFat, creamT, Orientation.Right);

        }    

        public void DrawQuadraticBezier()
        {

           var geometryBrush = Hud.Render.CreateBrush(255, 0, 168, 255, 0);
            using (var bezier = Hud.Render.CreateGeometry())
            {
                
                using (var bz = bezier.Open())
                {
                    RawVector2 begin = new RawVector2(400,100);
                    bz.BeginFigure(begin, FigureBegin.Filled);
                    

                    QuadraticBezierSegment bs = new QuadraticBezierSegment();
                    bs.Point1 = new RawVector2(350,450); 
                    bs.Point2 = new RawVector2(400,800); 
                    bz.AddQuadraticBezier(bs);
                    
                    bz.AddLine(new RawVector2(450,800));

                    QuadraticBezierSegment bs2 = new QuadraticBezierSegment();
                    bs2.Point1 = new RawVector2(400,450);
                    bs2.Point2 = new RawVector2(450,100);

                    bz.AddQuadraticBezier(bs2);
                    bz.AddLine(new RawVector2(450,100));

                    bz.EndFigure(FigureEnd.Closed);
                    
                    bz.Close();
                }
                geometryBrush.DrawGeometry(bezier);
            }            
        }


        public void DrawBezier()
        {
            var geometryBrush = Hud.Render.CreateBrush(220, 0, 0, 0, 0);
            using (var bezier = Hud.Render.CreateGeometry())
            {
                
                using (var bz = bezier.Open())
                {
                    // ArcSegment seg = new ArcSegment();                        
                    // seg.Point = new RawVector2(50,100);
                    // seg.Size = new Size2F(10,500);
                    // seg.RotationAngle = 30;
                    // seg.SweepDirection = SweepDirection.Clockwise;
                    // seg.ArcSize = ArcSize.Large;
                    //bz.AddArc(seg);

                    RawVector2 begin = new RawVector2(100,100); //curve start point
                    bz.BeginFigure(begin, FigureBegin.Filled);
                    

                    BezierSegment bs = new BezierSegment();
                    bs.Point1 = new RawVector2(50,500); // 1st curve control point 
                    bs.Point2 = new RawVector2(50,500); // 2nd curve control point
                    bs.Point3 = new RawVector2(100,800); // end point
                    bz.AddBezier(bs);
                    
                    bz.AddLine(new RawVector2(150,800));

                    BezierSegment bs2 = new BezierSegment();
                    bs2.Point1 = new RawVector2(100,500);
                    bs2.Point2 = new RawVector2(100,500);
                    bs2.Point3 = new RawVector2(150,100);
                    
                    bz.AddBezier(bs2);
                    bz.AddLine(new RawVector2(100,100));

                    bz.EndFigure(FigureEnd.Closed);
                    bz.Close();
                }
                geometryBrush.DrawGeometry(bezier);
            }

        }

        public void DrawRect() 
        {
           var geometryBrush = Hud.Render.CreateBrush(255, 255, 132, 0, 0);
            using (var rectangle = Hud.Render.CreateGeometry())
            {
                
                using (var re = rectangle.Open())
                {
                    re.BeginFigure(new RawVector2(100,100), FigureBegin.Filled);

                    RawVector2[] points = new RawVector2[6];
                    points[0] = new RawVector2(300,100);
                    points[1] = new RawVector2(300,300);
                    points[2] = new RawVector2(100,300);
                    points[3] = new RawVector2(100,100);
                    re.AddLines(points);

                    re.EndFigure(FigureEnd.Closed);
                    re.Close();
                }
                geometryBrush.DrawGeometry(rectangle);
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SharpPong
{
    public interface IDisplay
    {
        void Draw(Rectangle WorldArea, List<Movable> listMovable, int Player1Score, int Player2Score, Game.GameResultEnum result);
        void ReleaseDisplayDevice();
    }
    public class PictureBoxDisplay : IDisplay
    {
        private PictureBox picturebox = null;
        public PictureBoxDisplay(PictureBox pPicturebox)
        {
            picturebox = pPicturebox;
            picturebox.Paint -= Picturebox_Paint;
            picturebox.Paint += Picturebox_Paint;
        }
        private List<Movable> listMovable = null;
        private int Player1Score = 0;
        private int Player2Score = 0;
        private Rectangle WorldArea;
        private Game.GameResultEnum result = Game.GameResultEnum.NotDecideyet;
        private void Picturebox_Paint(object sender, PaintEventArgs e)
        {

            if (listMovable == null)
            {
                return;
            }
            e.Graphics.Clear(Color.Black);
            foreach (Movable mov in listMovable)
            {

                using (Brush BasicWhiteBrush = new SolidBrush(Color.White))
                {

                    if (mov.IsCircle)
                    {
                        e.Graphics.FillEllipse(BasicWhiteBrush, mov.Rectan);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(BasicWhiteBrush, mov.Rectan);
                    }
                }

            }
            using (Pen pen = new Pen(Color.White, 6))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                Point fromPoint = new Point(300, 0);
                Point toPoint = new Point(300, 600);
                e.Graphics.DrawLine(pen, fromPoint, toPoint);
            }

            Font SegoeFont = new Font("Segoe UI", 28, FontStyle.Bold);

            float OneForthWorldWidth = WorldArea.Width / 4;
            float OneSixthWorldHeight = WorldArea.Height / 6;
            float OneThirdWorldWidth = WorldArea.Width / 3;
            PointF Player1PointF = new PointF(OneForthWorldWidth, OneSixthWorldHeight);
            PointF Player2PointF = new PointF(OneForthWorldWidth * 3, OneSixthWorldHeight);

            using (Brush BasicWhiteBrush = new SolidBrush(Color.White))
            {
                e.Graphics.DrawString(Player1Score.ToString(), SegoeFont, BasicWhiteBrush, Player1PointF.X, Player1PointF.Y);
                e.Graphics.DrawString(Player2Score.ToString(), SegoeFont, BasicWhiteBrush, Player2PointF.X, Player2PointF.Y);
            }

            if(result != Game.GameResultEnum.NotDecideyet)
            {
                String ResultWord = "Player Won";
                if(result == Game.GameResultEnum.Player2Won)
                {
                    ResultWord = "Bot Won.";
                }

              
                PointF GameOverPointF = new PointF(OneThirdWorldWidth, OneSixthWorldHeight * 4);

                using (Brush BasicWhiteBrush = new SolidBrush(Color.White))
                {
                    e.Graphics.DrawString(ResultWord, SegoeFont, BasicWhiteBrush, GameOverPointF.X, GameOverPointF.Y);                 
                }
            }

        }

        public void Draw(Rectangle WorldArea, List<Movable> listMovable, int Player1Score, int Player2Score, Game.GameResultEnum result)
        {
            //   throw new NotImplementedException();
            this.WorldArea = WorldArea;

            this.listMovable = listMovable;
            this.Player1Score = Player1Score;
            this.Player2Score = Player2Score;
            this.result = result;
            if (this.picturebox == null)
            {
                return;
            }
            this.picturebox.Invalidate();
        }

        public void ReleaseDisplayDevice()
        {
            picturebox.Paint -= Picturebox_Paint;
            picturebox = null;
        }
    }
}

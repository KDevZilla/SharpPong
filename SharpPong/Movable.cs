using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SharpPong
{
    public class Movable
    {
        public Rectangle WorldBorder { get; private set; }
        public Rectangle Rectan;
        public int DeltaX { get; set; } = 1;
        public int DeltaY { get; set; } = 1;
        public Boolean CanMoveExceedTopSide = false;
        public Boolean CanMoveExceedBottomSide = false;
        public Boolean IsCircle = false;
        public Movable (Rectangle pRectangle, Rectangle pWordBorder)
        {
            Rectan = new Rectangle(pRectangle.X,
                pRectangle.Y,
                pRectangle.Width,
                pRectangle.Height);

            WorldBorder = pWordBorder;

        }

        public event EventHandler HasPassTheWorldAtRightSide;
        public event EventHandler HasPassTheWorldAtLeftSide;
        public Boolean HasReachTheWorldAtLeftSide = false;
        public Boolean HasReachTheWorldAtRightSide = false ;
        public Boolean HasReachTheWorldAtTopSide = false;
        public Boolean HasReachTheWorldAtBottomSide = false;
        private void ClearHashReachTheWorld()
        {
            HasReachTheWorldAtLeftSide = false;
            HasReachTheWorldAtRightSide = false;
            HasReachTheWorldAtTopSide = false;
            HasReachTheWorldAtBottomSide = false;
        }
        public void Move()
        {
            this.Rectan.X += DeltaX;
            this.Rectan.Y += DeltaY;

            if (!CanMoveExceedBottomSide)
            {

            }
            
            ClearHashReachTheWorld();
            if(this.Rectan.X + this.Rectan.Width > WorldBorder.Width)
            {
                HasReachTheWorldAtRightSide = true;
                //HasPassTheWorldAtRightSide?.Invoke(this, new EventArgs());
            }
            if(this.Rectan.X <= -this.Rectan.Width)
            {
                HasReachTheWorldAtLeftSide = true;
                //HasPassTheWorldAtLeftSide?.Invoke(this, new EventArgs());
            }
            if(this.Rectan.Y <= 0)
            {
                HasReachTheWorldAtTopSide = true;
                if (!CanMoveExceedTopSide)
                {
                    this.Rectan.Y = 0;
                }
            }
            if(this.Rectan.Y + this.Rectan.Height  >= this.WorldBorder.Height)
            {
                HasReachTheWorldAtBottomSide = true;
                if (!CanMoveExceedBottomSide)
                {
                    this.Rectan.Y = this.WorldBorder.Height - this.Rectan.Height;
                }
            }


        }
    }
}

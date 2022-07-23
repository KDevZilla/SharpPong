using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SharpPong
{
    public class Game
    {
        public Rectangle World = new Rectangle(0, 0, 600, 600);
        public List<Movable> listMovable = null;
        Movable Player = null;
        Movable Enemy = null;
        Movable Ball = null;
        private System.Timers.Timer timerTrigger = null;
        IDisplay display = null;
        IInputController Inputcontroller = null;
        public void ReleaseDevice()
        {
            if (this.Inputcontroller != null)
            {
                this.Inputcontroller.KeyDownPush -= Inputcontroller_KeyDownPush;
                this.Inputcontroller.KeyUpPush -= Inputcontroller_KeyUpPush;
                this.Inputcontroller.KeyDownRelease -= Inputcontroller_KeyDownRelease;
                this.Inputcontroller.KeyUpRelease -= Inputcontroller_KeyUpRelease;
            }
            if (this.display != null)
            {
                display.ReleaseDisplayDevice();
            }

        }
        public Game(IDisplay display, IInputController Inputcontroller)
        {
            this.display = display;
            this.Inputcontroller = Inputcontroller;
            this.Initial();
        }
        public void Start()
        {
            timerTrigger.Enabled = true;
        }
        public enum GameResultEnum
        {
            NotDecideyet,
            Player1Won,
            Player2Won
        }
        public enum GameStateEnum
        {
            Playing,
            BeforeRoundBegin,
            Pause,
            FinishedGame
        }
        public int Player1Score { get; private set; } = 0;
        public int Player2Score { get; private set; } = 0;
        public GameStateEnum State { get; private set; } = GameStateEnum.FinishedGame;
        public GameResultEnum Result { get; private set; } = GameResultEnum.NotDecideyet;
        private int PaddleWidth = 16;
        private int PaddleHeight = 100;
        private int SpaceBetweenBorderAndPaddle = 30;
        private int BallSize = 20;
        private int PlayerBaseSpeed = 7;
        private int EnemyBaseSpeed = 7;
        private int ScoreWon = 9;

        int NumberofGameLoop = 0;

        private int BallSpeed
        {
            get
            {
                if (NumberofGameLoop < 500)
                {
                    return 5;
                }
                if (NumberofGameLoop < 1000)
                {
                    return 6;
                }
                if (NumberofGameLoop < 2000)
                {
                    return 7;
                }
                return 8;
            }
        }

        private void Initial()
        {
            this.State = GameStateEnum.BeforeRoundBegin;
            this.Result = GameResultEnum.NotDecideyet;

            Rectangle PlayerRectangle = new Rectangle(SpaceBetweenBorderAndPaddle, 62, PaddleWidth, PaddleHeight);
            Rectangle EnemyRectangle = new Rectangle(World.Width - PaddleWidth - SpaceBetweenBorderAndPaddle, 62, PaddleWidth, PaddleHeight);
            Rectangle BallRectangle = new Rectangle(World.Width / 2, 62, BallSize, BallSize);

            Player = new Movable(PlayerRectangle, this.World);
            Ball = new Movable(BallRectangle, this.World);
            Enemy = new Movable(EnemyRectangle, this.World);

            Player.CanMoveExceedBottomSide = false;
            Player.CanMoveExceedTopSide = false;
            Player.DeltaX = 0;
            Player.DeltaY = 4;
            Enemy.CanMoveExceedBottomSide = false;
            Enemy.CanMoveExceedTopSide = false;
            Enemy.DeltaX = 0;
            Enemy.DeltaY = 4;

            Ball.CanMoveExceedBottomSide = false;
            Ball.CanMoveExceedTopSide = false;
            Ball.IsCircle = true;
            Ball.DeltaX = 5;
            Ball.DeltaY = 5;

            MoveBalltoCenter();
            SetBallInitialDirection();
            listMovable = new List<Movable>();

            listMovable.Add(Player);
            listMovable.Add(Enemy);
            listMovable.Add(Ball);

            if (timerTrigger != null)
            {
                timerTrigger.Enabled = false;
                timerTrigger.Elapsed -= TimerTrigger_Elapsed;
            }
            NumberofGameLoop = 0;
            timerTrigger = new System.Timers.Timer();

            timerTrigger.Interval = 10;
            timerTrigger.Elapsed -= TimerTrigger_Elapsed;
            timerTrigger.Elapsed += TimerTrigger_Elapsed;
            timerTrigger.Enabled = false;


            this.Inputcontroller.KeyDownPush -= Inputcontroller_KeyDownPush;
            this.Inputcontroller.KeyUpPush -= Inputcontroller_KeyUpPush;
            this.Inputcontroller.KeyDownRelease -= Inputcontroller_KeyDownRelease;
            this.Inputcontroller.KeyUpRelease -= Inputcontroller_KeyUpRelease;

            this.Inputcontroller.KeyDownPush += Inputcontroller_KeyDownPush;
            this.Inputcontroller.KeyUpPush += Inputcontroller_KeyUpPush;
            this.Inputcontroller.KeyDownRelease += Inputcontroller_KeyDownRelease;
            this.Inputcontroller.KeyUpRelease += Inputcontroller_KeyUpRelease;

        }
        private void StartRoundIfItHasnotStartYet()
        {
            if (this.State != GameStateEnum.BeforeRoundBegin)
            {
                return;
            }
            this.State = GameStateEnum.Playing;
        }
        private void Inputcontroller_KeyUpRelease(object sender, EventArgs e)
        {
            
            IsPlayerGoUp = false;
        }

        private void Inputcontroller_KeyDownRelease(object sender, EventArgs e)
        {

            IsPlayerGoDown = false;
        }

        private void PlayerGoUp()
        {
            if (this.Player.DeltaY > 0)
            {
                this.Player.DeltaY *= -1;
            }
        }
        private void PlayerGoDown()
        {
            if (this.Player.DeltaY < 0)
            {
                this.Player.DeltaY *= -1;
            }
        }
        private bool IsPlayerGoUp = false;
        private bool IsPlayerGoDown = false;

        private void Inputcontroller_KeyUpPush(object sender, EventArgs e)
        {

            IsPlayerGoUp = true;
            StartRoundIfItHasnotStartYet();

        }

        private void Inputcontroller_KeyDownPush(object sender, EventArgs e)
        {

            IsPlayerGoDown = true;
            StartRoundIfItHasnotStartYet();
        }

        private void CalculateWinner()
        {

            if (Player1Score >= ScoreWon)
            {
                this.State = GameStateEnum.FinishedGame;
                this.Result = GameResultEnum.Player1Won;
                return;
            }

            if (Player2Score >= ScoreWon )
            {
                this.State = GameStateEnum.FinishedGame;
                this.Result = GameResultEnum.Player2Won;
                return;
            }

        }
        private Boolean IsRectangleCollisionOnLeftSide(Rectangle rectangle1, Rectangle rectangle2)
        {
            if (rectangle1.Y + rectangle1.Height < rectangle2.Y)
            {
                return false;
            }
            if (rectangle1.Y > rectangle2.Y + rectangle2.Height)
            {
                return false;
            }
            if (rectangle1.X + rectangle1.Width >= rectangle2.X &&
                rectangle1.X + rectangle1.Width <= rectangle2.X + rectangle2.Width)
            {

                return true;
            }
            return false;
        }

        private Boolean IsRectangleCollisionOnRightSide(Rectangle rectangle1, Rectangle rectangle2)
        {
            if (rectangle1.Y + rectangle1.Height < rectangle2.Y)
            {
                return false;
            }
            if (rectangle1.Y > rectangle2.Y + rectangle2.Height)
            {
                return false;
            }
            if (rectangle1.X <= rectangle2.X + rectangle2.Width &&
                rectangle1.X >= rectangle2.X - rectangle2.Width)
            {

                return true;
            }
            return false;
        }
        private static Random random;
        private static int GetRandom(int Min, int Max)
        {
            if (random == null)
            {
                random = new Random();
            }
            int Next = random.Next(Min, Max);
            return Next;
        }
        private void MoveBalltoCenter()
        {
            Ball.Rectan.X = (World.Width - Ball.Rectan.Width) / 2;
            Ball.Rectan.Y = (World.Height - Ball.Rectan.Height) / 2;


        }
        private void SetBallInitialDirection()
        {
            bool IsGotoPlayer = true;
            if (GetRandom(1, 4) == 2)
            {
                IsGotoPlayer = false;
            }
            Ball.DeltaX = GetRandom(2, BallSpeed);
            Ball.DeltaY = GetRandom(2, BallSpeed);
            if (IsGotoPlayer)
            {
                if (Ball.DeltaX > 0)
                {
                    Ball.DeltaX *= -1;
                }
            }
            else
            {
                if (Ball.DeltaX < 0)
                {
                    Ball.DeltaX *= -1;
                }
            }
            if (Ball.DeltaY > 0)
            {
                if (GetRandom(1, 3) == 2)
                {
                    Ball.DeltaY *= -1;
                }
            }
        }
        private void Run()
        {
            NumberofGameLoop++;
            Player.DeltaY = 0;
            if (IsPlayerGoUp)
            {
                Player.DeltaY = -PlayerBaseSpeed;
            }
            if (IsPlayerGoDown)
            {
                Player.DeltaY = PlayerBaseSpeed;
            }
            int RandomNumber = GetRandom(1, 6);
            Boolean IsEnemyDoingNothing = false;
            if (RandomNumber == 3)
            {
                IsEnemyDoingNothing = true;
            }

            Enemy.DeltaY = 0;
            if (!IsEnemyDoingNothing)
            {
                if (Ball.Rectan.Y > Enemy.Rectan.Y)
                {
                    Enemy.DeltaY = EnemyBaseSpeed;
                }
                if (Ball.Rectan.Y < Enemy.Rectan.Y)
                {
                    Enemy.DeltaY = -EnemyBaseSpeed;
                }
            }
            foreach (Movable movObj in listMovable)
            {
                movObj.Move();
            }
            if (Ball.HasReachTheWorldAtBottomSide)
            {
                Ball.DeltaY = -BallSpeed;
            }
            if (Ball.HasReachTheWorldAtTopSide)
            {
                Ball.DeltaY = BallSpeed;
            }

            if (IsRectangleCollisionOnLeftSide(Ball.Rectan, Enemy.Rectan))
            {
                Ball.DeltaX = -BallSpeed;
            }
            if (IsRectangleCollisionOnRightSide(Ball.Rectan, Player.Rectan))
            {
                Ball.DeltaX = BallSpeed;
            }
            if (Ball.HasReachTheWorldAtLeftSide ||
                Ball.HasReachTheWorldAtRightSide)
            {
                if (Ball.HasReachTheWorldAtLeftSide)
                {
                    Player2Score++;
                    this.State = GameStateEnum.BeforeRoundBegin;
                    MoveBalltoCenter();
                    SetBallInitialDirection();
                }
                if (Ball.HasReachTheWorldAtRightSide)
                {
                    Player1Score++;
                    this.State = GameStateEnum.BeforeRoundBegin;
                    MoveBalltoCenter();
                    SetBallInitialDirection();
                }

                CalculateWinner();

            }
          
        }
        private void TimerTrigger_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.State == GameStateEnum.FinishedGame ||
                this.State == GameStateEnum.Pause)
            {
                this.timerTrigger.Enabled = false;
                return;
            }

            if (this.State == GameStateEnum.BeforeRoundBegin)
            {
                display.Draw(this.World, listMovable, this.Player1Score, this.Player2Score, this.Result );
                return;
            }

            Run();
            display.Draw(this.World, listMovable, this.Player1Score, this.Player2Score, this.Result );

            //throw new NotImplementedException();
        }
    }
}

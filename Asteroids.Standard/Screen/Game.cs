using System.Drawing;
using System.Linq;
using Asteroids.Standard.Base;
using Asteroids.Standard.Components;
using Asteroids.Standard.Enums;
using static Asteroids.Standard.Sounds.ActionSounds;

namespace Asteroids.Standard.Screen
{
    /// <summary>
    /// Summary description for CGame.
    /// </summary>
    public class Game : CommonOps
    {
        private Ship ship;
        private Bullet[] shipBullets;
        private AsteroidBelt asteroids;
        private Explosions explosions;
        private bool inProcess;
        private int iLevel;
        private bool paused;
        const int PAUSE_INTERVAL = (int)FPS;
        int iPauseTimer;

        private readonly Score _score;
        private readonly TextDraw _textDraw;

        public Game(Score score, TextDraw textDraw, ScreenCanvas canvas) : base(canvas)
        {
            _score = score;
            _textDraw = textDraw;

            iLevel = 4; // start with 4 asteroids
            inProcess = true;
            ship = new Ship(true, canvas); // new game - we know ship is alive
            shipBullets = new Bullet[4];
            for (int i = 0; i < 4; i++)
                shipBullets[i] = new Bullet(canvas);
            asteroids = new AsteroidBelt(iLevel, canvas);
            explosions = new Explosions(canvas);
            paused = false;
            iPauseTimer = PAUSE_INTERVAL;
        }

        public bool Done()
        {
            return !inProcess;
        }

        public void Thrust(bool bThrustOn)
        {
            if (!paused && ship.IsAlive())
            {
                ship.DecayThrust();

                if (bThrustOn)
                    ship.Thrust();
            }
        }

        public void Left()
        {
            if (!paused && ship.IsAlive())
                ship.RotateLeft();
        }

        public void Right()
        {
            if (!paused && ship.IsAlive())
                ship.RotateRight();
        }

        public void Hyperspace()
        {
            if (!paused && ship.IsAlive())
                if (!ship.Hyperspace())
                    ExplodeShip();
        }

        public void Shoot()
        {
            if (paused)
                return;

            if (ship.IsAlive())
            {
                var bullets = shipBullets.ToList();
                foreach (Bullet bullet in bullets)
                {
                    if (bullet.Available())
                    {
                        bullet.Shoot(ship.GetCurrLoc(), ship.GetRadians(), ship.GetVelocityX(), ship.GetVelocityY());
                        PlaySound(this, ActionSound.Fire);
                        return;
                    }
                }
            }
            else if (explosions.Count() == 0 && _score.HasReserveShips())
            {
                _score.GetNewShip();
                ship = new Ship(true, Canvas);
            }
        }

        public void Pause()
        {
            iPauseTimer = PAUSE_INTERVAL;
            paused = !paused;
        }

        private bool CheckPointInAsteroid(Point ptCheck)
        {
            int pointValue = asteroids.CheckPointCollisions(ptCheck);
            if (pointValue > 0)
            {
                _score.AddScore(pointValue);
                return true;
            }
            return false;
        }

        private void ExplodeShip()
        {
            Point ptCheck = new Point(0);

            PlaySound(this, ActionSound.Explode1);
            PlaySound(this, ActionSound.Explode2);
            PlaySound(this, ActionSound.Explode3);

            var points = ship.GetPoints();
            foreach (var ptExp in points)
            {
                ship.Explode();
                ptCheck.X = ptExp.X + ship.GetCurrLoc().X;
                ptCheck.Y = ptExp.Y + ship.GetCurrLoc().Y;
                explosions.AddExplosion(ptCheck, 2);
            }
        }

        public void DrawScreen()
        {
            Point ptCheck = new Point(0);
            var bullets = shipBullets.ToList();

            if (paused)
            {
                // Pause flashes on and off
                if (iPauseTimer > PAUSE_INTERVAL / 2)
                {
                    _textDraw.DrawText("PAUSE", TextDraw.Justify.CENTER,
                       iMaxY / 3, 200, 400);
                }
                if (--iPauseTimer < 0)
                    iPauseTimer = PAUSE_INTERVAL;
            }
            else // Do all game processing if game is not paused
            {
                // If no ship displaying, after explosions are done
                // get a new one - or end the game
                if (!ship.IsAlive() &&
                   (explosions.Count() == 0))
                {
                    if (!_score.HasReserveShips())
                    {
                        // Game over
                        inProcess = false;
                    }
                    else
                    {
                        if (asteroids.IsCenterSafe())
                        {
                            _score.GetNewShip();
                            ship = new Ship(true, Canvas);
                        }
                    }
                }

                // Create a new asteroid belt if 
                // no explosions and no asteroids
                if ((explosions.Count() == 0) &&
                   (asteroids.Count() == 0))
                {
                    asteroids = new AsteroidBelt(++iLevel, Canvas);
                }

                // Move all objects
                ship.Move();

                foreach (Bullet bullet in bullets)
                {
                    bullet.Move();
                }
                asteroids.Move();
                explosions.Move();

                // Check bullets for collisions        
                foreach (Bullet bullet in bullets)
                {
                    if (bullet.AcquireLoc(ref ptCheck) &&
                       CheckPointInAsteroid(ptCheck))
                    {
                        explosions.AddExplosion(ptCheck);
                        bullet.Disable();
                    }
                }

                // Check ship for collisions
                if (ship.IsAlive())
                {
                    var points = ship.GetPoints();
                    foreach (Point ptInShip in points)
                    {
                        ptCheck.X = ptInShip.X + ship.GetCurrLoc().X;
                        ptCheck.Y = ptInShip.Y + ship.GetCurrLoc().Y;

                        if (CheckPointInAsteroid(ptCheck))
                        {
                            ExplodeShip();
                            break;
                        }
                    }
                }
            }

            // Draw all objects
            ship.Draw();
            foreach (Bullet bullet in bullets)
                bullet.Draw();

            asteroids.Draw();
            explosions.Draw();

            // Draw the score
            _score.Draw();
        }
    }
}

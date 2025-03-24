using System;
using System.Collections.Generic;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

// main algorithm : greedy by survive, tailing on one enemy, but the turret shoots everywhere
// if it hits the bot, do shoot once its biggest bullet, but then it scans again
// if the bot followed is dead, it scans again 
// if it hit a wall, then it scans rescan
// if the energy is low enough, then it go to the nearest wall and then run along the wall and sweep shoots along its way

public class Ormas : Bot
{
    static int targetID = -1;
    bool isHit = false;

    // Constructor: Load the config file
    public Ormas() : base(BotInfo.FromFile("Ormas.json")){}

    // Main method: Start the bot
    public static void Main(string[] args)
    {
        new Ormas().Start();
    }

    // Called when a new round starts
    public override void Run()
    {
        BodyColor = Color.FromArgb(204, 0, 0);     // Blood Red (Power, Militancy, Fear, Aggression)
        TurretColor = Color.FromArgb(255, 140, 0); // Fiery Orange (Influence, Loud Protests, Boldness)
        GunColor = Color.FromArgb(0, 0, 0);        // Deep Black (Secrecy, Shadows, Underground Movements)
        BulletColor = Color.FromArgb(255, 255, 0); // Bright Yellow (Public Presence, Provocation, Populism)
        RadarColor = Color.FromArgb(128, 128, 128); // Neutral Gray (Uncertainty, Hidden Intentions, Duality)
        ScanColor = Color.FromArgb(50, 205, 50);   // Bright Green (Solidarity, Brotherhood, Internal Structure)


        while (IsRunning)
        {
            if (Energy >= 20)
            {
                TurnRadarLeft(40); 
            } else {
                EvadeMode();
            }
        }
        targetID = -1;
    }

    // When we see another bot -> store it & fire!
    public override void OnScannedBot(ScannedBotEvent evt)
    {
        if (targetID == -1)
        {
            if (DistanceTo(evt.X, evt.Y) > 100)
            {
                targetID = evt.ScannedBotId;  
            }
        }        
        ChaseTarget(evt);
    }

    // if the bot is shot at, then it will move 90 degrees of its current position and then keep its target as long its target don't die
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // Console.WriteLine($"Hit by a bullet! Power: {e.BulletDamage}, From Bot ID: {e.BulletOwnerId}");
        
        // Example: Move away from the direction the bullet came from
         if (isHit)
        {
            TurnLeft(BearingTo(e.Bullet.X * 2, e.Bullet.Y));
            isHit = true;
        } else {
            TurnRight(BearingTo(e.Bullet.X * 3, e.Bullet.Y));
            isHit = false;
        }
        Forward(50);
        targetID = -1;
    }

    // if the bot is rammed, it will shoot at that troublemaker and keep going at the initial target. If the energy below 200, then it will go 180 deg back
    public override void OnHitBot(HitBotEvent botHitBotEvent)
    {
        if (Energy >= 20)
        {
            TurnToFaceTarget(botHitBotEvent.X, botHitBotEvent.Y);
            Fire(2);
        } else { // evade mode
            TurnLeft(180);
        }  
    }

    // if the bot we followed died, we scan again to go to new bot
    public override void OnBotDeath(BotDeathEvent e)
    {
        if (e.VictimId == targetID)
        {
            targetID = -1; // Reset target
            Rescan();
        }
    }

    // if energy is below 20, then go to the wall and shoot from there
    private void EvadeMode(){
        TurnLeft(BearingTo(0,Y)); // go to the left hand side border
    }

    private void ChaseTarget(ScannedBotEvent target)
    {
        TurnToFaceTarget(target.X, target.Y);
        double distance = DistanceTo(target.X, target.Y);
        double iter = 12;
        double step = distance / iter;
        // int degrees = 30;
        while (iter > 0) {
            TurnGunLeft(30);
            Fire(0.1);
            if (iter == 1)
            {
                Forward(step + 5);
            } else {
                Forward(step);
            }
            iter--;
        }
    }

    // face the target to go forward
    private void TurnToFaceTarget(double x, double y)
    {
        var bearing = BearingTo(x, y);
        var bearingGun = GunBearingTo(x, y);
        var bearingRadar = RadarBearingTo(x, y);
        TurnLeft(bearing);
        TurnGunLeft(bearingGun);
        TurnRadarLeft(bearingRadar);
    }

    // if the bot hit wall, then it scan again 
    public override void OnHitWall(HitWallEvent botHitWallEvent)
    {
        // when energy still enough
        if (Energy >= 20)
        {
            targetID = -1;
            Rescan();   
        } else { // when the energy is low
            double distance = DistanceTo(0, 0);
            double iter = 12;
            double step = distance / iter;
            double degStep = 180 / iter;
            TurnLeft(90);
            if (X == 0 && Y == ArenaHeight) //  top left corner
            {
                if (GunDirection != 270)
                {
                    TurnGunLeft(BearingTo(X, 0)); 
                }
                while (iter > 0) {
                    TurnGunLeft(degStep);
                    Fire(0.1);
                    if (iter == 1)
                    {
                        Forward(step + 5);
                    } else {
                        Forward(step);
                    }
                    iter--;
                } 
                Forward(ArenaHeight);
            } else if (X == ArenaWidth && Y == 0) { // bottom right corner
                if (GunDirection != 90)
                {
                    TurnGunLeft(BearingTo(X, ArenaHeight)); 
                }
                while (iter > 0) {
                    TurnGunLeft(degStep);
                    Fire(0.1);
                    if (iter == 1)
                    {
                        Forward(step + 5);
                    } else {
                        Forward(step);
                    }
                    iter--;
                }
                Forward(ArenaHeight);
            } else if (X == ArenaWidth && Y == ArenaHeight) { // top right 
                if (GunDirection != 180)
                {
                    TurnGunLeft(BearingTo(X, 0)); 
                }
                while (iter > 0) {
                    TurnGunLeft(degStep);
                    Fire(0.1);
                    if (iter == 1)
                    {
                        Forward(step + 5);
                    } else {
                        Forward(step);
                    }
                    iter--;
                }
                Forward(ArenaWidth);
            } else { // bottom left corner
                if (GunDirection != 0)
                {
                    TurnGunLeft(BearingTo(ArenaWidth, 0)); 
                }
                while (iter > 0) {
                    TurnGunLeft(degStep);
                    Fire(0.1);
                    if (iter == 1)
                    {
                        Forward(step + 5);
                    } else {
                        Forward(step);
                    }
                    iter--;
                }
                Forward(ArenaWidth);
            }
        }
    }


    // unused functions
    // private bool IsOnWallSides(double x, double y){
    //     // double margin = 50; // Margin to define "close to corner"
    //     // double maxX = ArenaWidth;
    //     // double maxY = ArenaHeight;

    //     return (0 < X && X < ArenaWidth && Y == 0) ||           // bottom side
    //            (0 < X && X < ArenaWidth && Y == ArenaHeight) ||    // top side
    //            (0 < Y && Y < ArenaHeight && X == ArenaWidth) ||    // right hand side
    //            (0 < Y && Y < ArenaHeight && X == 0) ; // left hand side
    // }
    
    // Get the closest enemy
    // private ScannedBotEvent EnemyEventSmallestDistance(List<ScannedBotEvent> scannedBots)
    // {
    //     if (scannedBots == null || scannedBots.Count == 0) return null;

    //     ScannedBotEvent closestBot = null;
    //     double minDistance = double.MaxValue;

    //     foreach (var bot in scannedBots)
    //     {
    //         double distance = Math.Sqrt(Math.Pow(bot.X - X, 2) + Math.Pow(bot.Y - Y, 2));
    //         if (bot != null && distance < minDistance) // ✅ Fix: Use `Distance` property
    //         {
    //             minDistance = distance;
    //             closestBot = bot;
    //         }
    //     }

    //     return closestBot;
    // }


    // private bool IsInCorner(double X, double Y)
    // {
    //     double margin = 0; // Margin to define "close to corner"
    //     double maxX = ArenaWidth;
    //     double maxY = ArenaHeight;

    //     return (X < margin && Y < margin) ||           // Bottom-left
    //            (X < margin && Y > maxY - margin) ||    // Top-left
    //            (X > maxX - margin && Y < margin) ||    // Bottom-right
    //            (X > maxX - margin && Y > maxY - margin); // Top-right
    // }


            // Keep scanning for enemies by continuously turning the radar
            // if (Energy > 20)
            // {
                // Get the closest enemy
                // ScannedBotEvent target = EnemyEventSmallestDistance(enemyArray);
                
                // if (target != null)
                // {
                //     ChaseTarget(target);
                // }

                // If it goes to the spot and ram the bot, it will go to its highest speed to ram and fire the highest bullet damage
                // if it hits a wall, then it will scan the arena again and try to find the smallest distance enemy
            // }
            // else
            // {
            //     // if energy < 20, go to walls and chase bots with lower energy
            //     if (!IsInCorner(X, Y) && !IsOnWallSides(X, Y))
            //     {
            //         var bearing = BearingTo(0, Y); // always make the bot to go to the right hand wall side
            //         if (bearing >= 0)
            //         {
            //             TurnLeft(bearing);
            //         } else {
            //             TurnLeft(-bearing);
            //         }
            //     } else
            //     { // evade mode
            //         // if on the horizontal walls
            //         if ((Y == 0 || Y == ArenaHeight) && Direction != 0)
            //         {
            //             TurnRight(BearingTo(ArenaWidth, 0)); // make it to go to the right always
            //             Forward(ArenaWidth);
            //         } else if ((X == 0 || X == ArenaWidth) && Direction != 180) { // if on the vertical walls
            //             TurnRight(BearingTo(0, 90)); // make it to go upward always
            //             Forward(ArenaHeight);
            //         }
            //     }
            // }
}

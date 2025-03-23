using System;
using System.Drawing;
using System.Xml.Linq;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class ChillPips : Bot
{
    // double moveAmount;
    // main : starting our bot
    static void Main(string[] args){
        new ChillPips().Start();
    }

    // constructor : load the config file
    ChillPips() : base(BotInfo.FromFile("ChillPips.json")){}

    private Random random = new Random();
    private const double jarakAmanMusuh = 200;
    private const double jarakAmanWall = 5.0;
    private bool movingForward = true;


    // Called when a new round is started -> initialize and do movement
    public override void Run()
    {
        BodyColor = Color.FromArgb(141, 186, 183);
        TurretColor = Color.FromArgb(66, 82, 37);
        GunColor = Color.FromArgb(175, 126, 67);
        BulletColor = Color.FromArgb(135, 143, 119);
        RadarColor = Color.FromArgb(135, 143, 119);
        ScanColor = Color.FromArgb(135, 143, 119);

        // moveAmount = Math.Max(ArenaHeight - Y, ArenaWidth - X) - jarakAmanWall; // jarak aman terjauh dari posisi skrg
        MaxSpeed = 8;

        TurnLeft(DirectionToWall(X, Y));
        Forward(DistanceToWall(X, Y));
        TurnRight(90);
        // Forward(SafeMoveAmount(Direction));

        while (IsRunning)
        {   
            MaxSpeed = random.Next(5, 9);
            SetTurnGunRight(double.PositiveInfinity);

            if (movingForward == true) {
                Forward(SafeMoveAmountForward(Direction));
                TurnRight(90);
                
            }
            else {
                Back(SafeMoveAmountBackward(Direction));
                TurnLeft(90);
            }
            
            
        }
        
    }

    private double SafeMoveAmountForward(double direction) 
    {
        if (direction == 0) {
            return Math.Abs(ArenaWidth - X - jarakAmanWall);
        }
        else if (direction == 90) {
            return Math.Abs(ArenaHeight - Y - jarakAmanWall);
        }
        else if (direction == 180) {
            return (X - jarakAmanWall);
        }
        else {
            return (Y - jarakAmanWall);
        }
    }

    private double SafeMoveAmountBackward(double direction) 
    {
        if (direction == 0) {
            return (X - jarakAmanWall);
        }
        else if (direction == 90) {
            return (Y - jarakAmanWall);
        }
        else if (direction == 180) {
            return Math.Abs(ArenaWidth - X - jarakAmanWall);
        }
        else {
            return Math.Abs(ArenaHeight - Y - jarakAmanWall);
        }
    }

    private double DistanceToWall(double x, double y) 
    {
        double distanceToLeftWall = DistanceTo(0, y) - jarakAmanWall;
        double distanceToRightWall = DistanceTo(ArenaWidth, y) - jarakAmanWall;
        double distanceToUpperWall = DistanceTo(x, ArenaHeight) - jarakAmanWall;
        double distanceToBottomWall = DistanceTo(x, 0) - jarakAmanWall;

        double nearestDistance = Math.Min(Math.Min(distanceToLeftWall, distanceToRightWall), 
                                  Math.Min(distanceToUpperWall, distanceToBottomWall));
        
        return nearestDistance;
    }

    private double DirectionToWall(double x, double y)
    {
        double nearestDistance = DistanceToWall(x, y);
        if (DistanceTo(0, y) - jarakAmanWall == nearestDistance) return CalcBearing(180);
        if (DistanceTo(ArenaWidth, y) - jarakAmanWall == nearestDistance) return CalcBearing(0);
        if (DistanceTo(x, ArenaHeight) - jarakAmanWall == nearestDistance) return CalcBearing(90);
        if (DistanceTo(x, 0) - jarakAmanWall == nearestDistance) return CalcBearing(270);

        return 0;
    }


    public override void OnScannedBot(ScannedBotEvent e)
    {   
        if (DistanceTo(e.X, e.Y) <= jarakAmanMusuh){
            Fire(3);
        } else {
            Fire(1);
        }
    }

    public override void OnHitBot(HitBotEvent e)
    {
        MaxSpeed = 8;
        movingForward = !movingForward;
    }

}

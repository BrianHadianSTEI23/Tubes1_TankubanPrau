using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class BotMeriang : Bot
{
    private bool sudahDiPusat = false; // Cek apakah bot sudah sampai ke tengah

    static void Main(string[] args)
    {
        new BotMeriang().Start();
    }

    BotMeriang() : base(BotInfo.FromFile("BotMeriang.json")) { }

    public override void Run()
    {
        BodyColor = Color.FromArgb(141, 186, 183);
        TurretColor = Color.FromArgb(66, 82, 37);
        GunColor = Color.FromArgb(175, 126, 67);
        BulletColor = Color.FromArgb(135, 143, 119);
        RadarColor = Color.FromArgb(135, 143, 119);
        ScanColor = Color.FromArgb(135, 143, 119);

        GunTurnRate = 15;

        while (IsRunning)
        {
            if (!sudahDiPusat)  
            {
                MovePoint(); // Menuju pusat
            }
            else
            {
                Muter(10); // Jika sudah di pusat, mulai berputar
            }
            TargetSpeed = 8;
            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        double jarak = DistanceTo(e.X, e.Y);
        SmartFire(jarak);
    }

    private void SmartFire(double distance)
    {
        double firePower = (distance < 100) ? 3 : (distance < 300) ? 2 : 1;
        Fire(firePower);
    }

    public override void OnHitBot(HitBotEvent e)
    {
        Back(50); 
        TurnRight(15); 
        TargetSpeed = 6;
        Go();
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Console.WriteLine("Menabrak dinding! Menghindar...");
        Back(30);
        TurnRight(90);
        TargetSpeed = 6;
        Go();
    }

    private void MovePoint()
    {
        double centerX = ArenaWidth / 2;
        double centerY = ArenaHeight / 2;

        double bearingToCenter = CalcBearing(centerX, centerY);
        TurnRight(bearingToCenter);
        TargetSpeed = 8;
        
        double jarak = DistanceTo(centerX, centerY);
        
        if (jarak > 10) // Jika masih jauh dari pusat, lanjutkan
        {
            Forward(jarak);
        }
        else
        {
            sudahDiPusat = true; // Jika sudah di pusat, mulai muter
        }
    }

    private void Muter(double radius)
    {
        double turnAngle = 10;
        double speed = Math.Min(8, radius / 10);
        TargetSpeed = speed;
        TurnRate = turnAngle;
        Go();
    }

    private double CalcBearing(double targetX, double targetY)
    {
        double angle = Math.Atan2(targetY - Y, targetX - X) * (180 / Math.PI);
        return NormalizeBearing(angle - Direction);
    }

    private double NormalizeBearing(double angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}

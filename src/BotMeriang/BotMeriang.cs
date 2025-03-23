using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class BotMeriang : Bot
{
    private bool sudahDiPusat = false; // Cek apakah bot sudah sampai ke tengah
    private const double radiusOrbit = 100; // Radius orbit di sekitar pusat

    static void Main(string[] args)
    {
        new BotMeriang().Start();
    }

    BotMeriang() : base(BotInfo.FromFile("BotMeriang.json")) { }

    public override void Run()
    {
        // Set warna bot
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
                MovePoint(); // Menuju pusat arena
            }
            else
            {
                OrbitCenter(); // Jika sudah di pusat, mulai mengelilingi pusat
            }
            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        if(Energy>20)
        {
                double jarak = DistanceTo(e.X, e.Y);
                SmartFire(jarak);
        }
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
        TargetSpeed = 8;
        Go();
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Console.WriteLine("Menabrak dinding! Menghindar...");
        Back(30);
        TurnRight(90);
        TargetSpeed = 8;
        Go();
    }

    private void MovePoint()
    {
        double centerX = ArenaWidth / 2;
        double centerY = ArenaHeight / 2;
        double jarak = DistanceTo(centerX, centerY);

        if (jarak > 10) // Jika masih jauh dari pusat
        {
            double bearingToCenter = CalcBearing(centerX, centerY);
            TurnRate = NormalizeBearing(bearingToCenter); // Belok sambil bergerak
            TargetSpeed = 8; 
            Forward(Math.Min(jarak, 50)); // Bergerak dalam langkah kecil menuju pusat
        }
        else
        {
            Console.WriteLine("Bot sudah di pusat, mulai mengelilingi!");
            sudahDiPusat = true;
        }
    }

    private void OrbitCenter()
    {
        double centerX = ArenaWidth / 2;
        double centerY = ArenaHeight / 2;

        // Hitung sudut yang diperlukan untuk tetap berada dalam orbit
        double bearingToOrbit = CalcBearing(centerX, centerY) + 90; // Tambah 90° untuk gerak melingkar
        TurnRate = NormalizeBearing(bearingToOrbit);
        
        TargetSpeed = 8; // Kecepatan konstan untuk mengelilingi pusat
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

using System;
// using System.Collections.Generic;
// using Robocode;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

// main algorithm : scans all bot first, then it stores the data inside an array.
// it then picks 3 tanks that is among the lowest energy of all, then go to the middle of 
// the coordinate of those tanks. Then, it circularly shoot random. NOTE : it will
// not hit the wall because the any coordinate will always indicate that it's going
// to valid space. If there's an bot death event, do rescan. 
// IF THE TANKS ARE LESS THAN 3, THEN IT WILL PICK THE HIGHEST NUMBER NEAR 3

// IF the bot is rammed or hit by a bullet, it will evade at 90 deg of that ramming bot
// IF the bot hit the wall or another bot, it stop and rescans


public class KPR : Bot
{
    static int MAX_TANK_COUNT = 10;
    bool isHit = false;

    int clusterLength;

    // static int targetID = -1;
    int enemyCount = 0;
    static ScannedBotEvent[] enemyArray = new ScannedBotEvent[10]; // Max 10 enemies

    // Constructor: Load the config file and initiate enemyArray
    public KPR() : base(BotInfo.FromFile("KPR.json")){
        for (int i = 0; i < MAX_TANK_COUNT; i++)
        {
            enemyArray[i] = null;
        }
    }

    // Main method: Start the bot
    public static void Main(string[] args)
    {
        new KPR().Start();
    }

    // Called when a new round starts
    public override void Run()
    {
        while (IsRunning)
        {
            if (IsEmptyEnemyArray(enemyArray))
            {
                TurnRadarLeft(360);
                Console.WriteLine("Dor!");
            } else { // enemy Array is not empty
                TurnGunLeft(45);
                Fire(1);
                // Forward(50);
            }
        }
    }

    // When we see another bot -> store it & fire!
    public override void OnScannedBot(ScannedBotEvent evt)
    {
        if (enemyCount < 10)
        {
            enemyArray[enemyCount] = evt;
            enemyCount++;
        }
        // get the list of the desired bots
        ScannedBotEvent[] cluster = EnemyEventsOfSmallestDistance(enemyArray);

        // chase cluster if scannedbot not empty
        ChaseCluster(cluster);

    }

    // if the bot is shot at, then it will move 90 degrees of its current position
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        if (isHit)
        {
            TurnLeft(90);
            isHit = true;
        } else {
            TurnRight(90);
            isHit = false;
        }
        Forward(50);
    }

    // arrange scannedBots from lowest to the highest based on energy and get cluster enemy with the lowest energy
    private ScannedBotEvent[] EnemyEventsOfSmallestDistance(ScannedBotEvent[] scannedBots)
    {
        // selection sorting
        int i;
        for (i = 0; i < enemyCount; i++)
        {
            int swapIdx = i;
            int j = i;
            ScannedBotEvent currBot = scannedBots[i];
            for (j = i; j < enemyCount; j++)
            {
                if (currBot.Energy < scannedBots[j].Energy)
                {
                    swapIdx = j;
                }
                // found the smallest distance, then swap
            }
            scannedBots[i] = scannedBots[swapIdx];
            scannedBots[j] = currBot;
        }


        // get array containing three of the most lowest energy 
        clusterLength = Math.Min(3, enemyCount); 
        ScannedBotEvent[] cluster = new ScannedBotEvent[clusterLength];
        for (int k = 0; k < clusterLength; k++)
        {
            cluster[k] = scannedBots[k];
        }
        return cluster;
    }

    // if the bot is rammed, it will move 90 degrees of its current position (evade)
    public override void OnHitBot(HitBotEvent botHitBotEvent)
    {
        if (isHit)
        {
            TurnLeft(90);
            isHit = true;
        } else {
            TurnRight(90);
            isHit = false;
        }
        Forward(50);
    }

    // if the bot we followed died, we scan again to get new bot cluster
    public override void OnBotDeath(BotDeathEvent e)
    {
        // emptying the enemyArray
        for (int i = 0; i < enemyCount; i++)
        {
            enemyArray[i] = null;
        }
        // scan again
        TurnRadarLeft(360);
    }

    // if the bot hit wall, then it scan again 
    public override void OnHitWall(HitWallEvent botHitWallEvent)
    {
        // emptying the enemyArray
        for (int i = 0; i < enemyCount; i++)
        {
            enemyArray[i] = null;
        }
        // scan again
        TurnRadarLeft(360);
    }

    // func : get length of the cluster
    private int GetClusterLength(ScannedBotEvent[] cluster){
        int i = 0;
        while (cluster[i] != null){
            i++;
        }
        return i;
    }

    // chase the cluster
    private void ChaseCluster(ScannedBotEvent[] cluster)
    {
        // find the mean of the positions of cluster
        double XCluster = 0;
        double YCluster = 0;
        for (int i = 0; i < clusterLength; i++)
        {
            XCluster += cluster[i].X;
            YCluster += cluster[i].Y;
        }
        XCluster /= clusterLength;
        YCluster /= clusterLength;

        // go to the target
        TurnToFaceTarget(XCluster, YCluster);
        double distance = DistanceTo(XCluster, YCluster);
        double iter = 12;
        double step = distance / iter;
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

    // func : check whether the enemyArray is empty or not
    private bool IsEmptyEnemyArray(ScannedBotEvent[] scannedBots) {
        int i = 0;
        bool empty = true;
        while (i < MAX_TANK_COUNT && empty){
            if (scannedBots[i] != null) {
                return false;
            }
            i++;
        }
        return true;
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
    
}

using UnityEngine;

public static class PlayerStats
{
    public static int Health { get; set; } = 6; // Начальное здоровье
    public static int Coins { get; set; } = 0;
    public static bool IsInitialized { get; set; } = false; // Сделали сеттер публичным

    public static void SaveStats(Ment player)
    {
        Health = player.GetHealth(); // Используем публичный метод вместо прямого доступа
        Coins = player.GetCoins();
        IsInitialized = true;
    }
}
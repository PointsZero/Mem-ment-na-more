using UnityEngine;

public static class PlayerStats
{
    public static int Health { get; set; } = 6; // ��������� ��������
    public static int Coins { get; set; } = 0;
    public static bool IsInitialized { get; set; } = false; // ������� ������ ���������

    public static void SaveStats(Ment player)
    {
        Health = player.GetHealth(); // ���������� ��������� ����� ������ ������� �������
        Coins = player.GetCoins();
        IsInitialized = true;
    }
}
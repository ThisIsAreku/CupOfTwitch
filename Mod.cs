using System.Diagnostics;
using System.IO;

namespace CupOfTwitch
{
    static class Mod
    {
        [Conditional("DEBUG")]
        private static void Log(string text)
        {
#if DEBUG
            File.AppendAllText("CupOfTwitch.txt", $"{text}\r\n");
#else
#endif
        }

        public static void Main()
        {
            Log("Init start -----");
            PlayerManager.OnPlayerJoinedEvent += PlayerManager_OnPlayerJoinedEvent;
            SceneLoader.OnFadeOutEndEvent += SceneLoader_OnFadeOutEndEvent;
        }

        private static void SceneLoader_OnFadeOutEndEvent()
        {
            Log("SceneLoader_OnFadeOutEndEvent");
            AbstractPlayerController p = PlayerManager.GetFirst();
            if (p == null)
            {
                Log("SceneLoader_OnFadeOutEndEvent p null");
                return;
            }

            Log($"SceneLoader_OnFadeOutEndEvent p is {p.id}");
            p.stats.OnPlayerDeathEvent += Stats_OnPlayerDeathEvent;
        }

        private static void PlayerManager_OnPlayerJoinedEvent(PlayerId playerId)
        {
            Log($"PlayerManager_OnPlayerJoinedEvent for {playerId}");
            AbstractPlayerController p = PlayerManager.GetPlayer(playerId);
            if (p == null)
            {
                Log($"p null for {playerId}");
                return;
            }

            Log($"p not null for {playerId}");
            p.stats.OnPlayerDeathEvent += Stats_OnPlayerDeathEvent;
        }

        private static void Stats_OnPlayerDeathEvent(PlayerId playerId)
        {
            Log($"Stats_OnPlayerDeathEvent for {playerId}");
            int deaths = PlayerData.Data.DeathCount(playerId);

            File.WriteAllText($"CupOfTwitch_{playerId}_Stats_Deaths.txt", $"{deaths}");
        }
    }
}

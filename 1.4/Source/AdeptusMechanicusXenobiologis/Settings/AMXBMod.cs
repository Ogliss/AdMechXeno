using AdeptusMechanicus.settings;
using Verse;

namespace AdeptusMechanicus
{
    public class AM_Xenbiologis : Mod
    {
        public static ModContentPack pack;
        public AM_Xenbiologis(ModContentPack content) : base(content)
        {
            pack = content;
            LongEventHandler.ExecuteWhenFinished(GetFactionSettings);
        }
        public void GetFactionSettings()
        {
            AMAMod.settings.GenerateFactionSettings(pack);
        }
    }
}

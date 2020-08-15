using System;
using AdeptusMechanicus;
using RimWorld;

namespace Verse
{
    public class CompProperties_GlowerNecron : CompProperties_Glower
    {
        // Token: 0x06000AB1 RID: 2737 RVA: 0x0005598C File Offset: 0x00053D8C
        public CompProperties_GlowerNecron()
        {
            this.compClass = typeof(CompGlowerNecron);
        }
    }
    
    public class CompGlowerNecron : CompGlower
    {
        public new CompProperties_GlowerNecron Props
        {
            get
            {
                return (CompProperties_GlowerNecron)this.props;
            }
        }
        
        private bool ShouldBeLitNow
        {
            get
            {
                if (!this.parent.Spawned)
                {
                    return false;
                }
                if (this.parent.TryGetComp<Comp_NecronOG>()!=null && this.parent.TryGetComp<Comp_NecronOG>() is Comp_NecronOG _NecronOG)
                {
                    return _NecronOG.reviveFlag;
                    if (_NecronOG.reviveFlag)
                    {
                        
                    }
                }
                return false;
            }
        }
        
        public override void PostExposeData()
        {
            Scribe_Values.Look<bool>(ref this.glowOnInt, "glowOn", false, false);
        }
        
        private bool glowOnInt;
    }
}

using System;
using System.Text;
using Verse;

namespace RimWorld
{
    // Token: 0x020006C1 RID: 1729
    public class Building_Monolith : Building_CrashedShipPart
    {
        // Token: 0x060024D8 RID: 9432 RVA: 0x00117E46 File Offset: 0x00116246
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.age, "age", 0, false);
        }

        // Token: 0x060024D9 RID: 9433 RVA: 0x00117E60 File Offset: 0x00116260
        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            if (stringBuilder.Length != 0)
            {
                stringBuilder.AppendLine();
            }
            stringBuilder.Append("AwokeDaysAgo".Translate(new object[]
            {
                this.age.TicksToDays().ToString("F1")
            }));
            return stringBuilder.ToString();
        }

        // Token: 0x060024DA RID: 9434 RVA: 0x00117ECA File Offset: 0x001162CA
        public override void Tick()
        {
            base.Tick();
            this.age++;
        }

        // Token: 0x040014CA RID: 5322
        protected int age;

        internal void SetFaction(object ofNecrons, object p)
        {
            throw new NotImplementedException();
        }
    }
}

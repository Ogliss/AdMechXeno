using System;

namespace Verse.AI
{
    public class ThinkNode_OGAMXBChancePerHour_Constant : ThinkNode_OGAMXBChancePerHour
    {
        private float mtbHours = 1f;

        public override ThinkNode DeepCopy(bool resolve = true)
        {
            ThinkNode_OGAMXBChancePerHour_Constant thinkNode_OGAMXBChancePerHour_Constant = (ThinkNode_OGAMXBChancePerHour_Constant)base.DeepCopy(resolve);
            thinkNode_OGAMXBChancePerHour_Constant.mtbHours = this.mtbHours;
            return thinkNode_OGAMXBChancePerHour_Constant;
        }

        protected override float MtbHours(Pawn Pawn)
        {
            return this.mtbHours;
        }
    }
}

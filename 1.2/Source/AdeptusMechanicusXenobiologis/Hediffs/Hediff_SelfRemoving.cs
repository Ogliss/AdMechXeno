using Verse;

namespace AdeptusMechanicus
{
    public class Hediff_SelfRemoving : Hediff
    {
        public override bool ShouldRemove
        {
            get
            {
                return true;
            }
        }

        public Hediff_SelfRemoving()
        {
        }
    }
}

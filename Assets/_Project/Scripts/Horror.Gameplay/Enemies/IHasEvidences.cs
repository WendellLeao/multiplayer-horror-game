using Horror.Gameplay.Evidences;

namespace Horror.Gameplay.Enemies
{
    public interface IHasEvidences
    {
        EvidenceData[] Evidences { get; }
    }
}
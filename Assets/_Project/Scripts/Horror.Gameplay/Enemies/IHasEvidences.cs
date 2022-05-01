using Horror.Gameplay.Evidences;

namespace Horror.Gameplay.Enemies
{
    public interface IHasEvidences
    {
        EvidenceType[] Evidences { get; }
    }
}
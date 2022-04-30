using Horror.Gameplay.Items;

namespace Horror.Gameplay.Scenary
{
    public interface IParanormalObject
    {
        bool IsEvidence { get; }
        float EvidenceDuration { get; }
        EmfScore EmfScore { get; } 
        void Interact();
    }
}
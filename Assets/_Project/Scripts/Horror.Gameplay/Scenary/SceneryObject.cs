using Horror.Gameplay.Items;
using UnityEngine;

namespace Horror.Gameplay.Scenary
{
    public abstract class SceneryObject : Entity, IParanormalObject
    {
        [SerializeField] private float _evidenceDuration;

        public bool IsEvidence { get; protected set; }
        public float EvidenceDuration => _evidenceDuration;

        public EmfScore EmfScore => EmfScore.Evidence;//TODO: THE EMF CHANGES DEPENDING ON THE LAST INTERACTION

        public abstract void Interact();
    }
}
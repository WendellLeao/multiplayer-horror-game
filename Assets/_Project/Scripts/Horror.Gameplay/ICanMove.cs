namespace Horror.Gameplay
{
    public interface ICanMove
    {
        void SetVelocityMultiplier(float multiplier);
        bool IsMoving();
    }
}

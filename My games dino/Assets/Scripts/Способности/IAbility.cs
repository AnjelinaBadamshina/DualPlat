public interface IAbility
{
    void Activate();
}

public interface IAbilityWithDuration : IAbility
{
    float Duration { get; }
}
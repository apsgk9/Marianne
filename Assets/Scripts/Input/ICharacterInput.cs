namespace CharacterInput
{
    public interface ICharacterInput
    {
        float MovementHorizontal();
        float MovementVertical();
        bool IsRunning();
        bool IsThereMovement();
        bool IsJump();
    }
}
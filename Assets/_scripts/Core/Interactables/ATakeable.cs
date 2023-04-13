namespace Core
{
    public abstract class ATakeable : AInteractableItem
    {
        public string TakeEffectID;
        public abstract ETakeableType TakeableType { get; }
        
    }
}

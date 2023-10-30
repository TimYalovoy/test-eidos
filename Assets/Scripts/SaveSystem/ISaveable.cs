namespace SaveSystem
{
    public interface ISaveable
    {
        public void SaveIsLoaded(ISaver saver);
        public void SetData(ISaver saver);
    }
}

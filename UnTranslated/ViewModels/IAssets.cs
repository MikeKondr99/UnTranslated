namespace UnTranslated.ViewModels
{
    internal interface IAssets
    {
        public string Name { get; init; }
        public string Path { get; init; }
        public bool IsOriginal { get; init; }
        public bool IsNotOriginal => !IsOriginal;
    }
}
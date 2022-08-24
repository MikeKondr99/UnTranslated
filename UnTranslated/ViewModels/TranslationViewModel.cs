namespace UnTranslated.ViewModels
{
    internal partial class Translation : IAssets
    {
        public Translation(string name, string path, bool isReadOnly = false)
        {
            Name = name;
            Path = path;
        }

        public Translation(IAssets copy, bool isReadOnly = false)
        {
            Name = copy.Name;
            Path = copy.Path;
        }

        public string Path { get; init; }
        public string Name { get; init; }
        public bool IsReadOnly { get; init; }

        public override string ToString()
        {
            return Name;
        }
    }



}
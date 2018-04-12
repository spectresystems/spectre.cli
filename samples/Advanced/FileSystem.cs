namespace Sample
{
    public interface IFileSystem
    {
        bool FileExist(string path);
    }

    public sealed class FileSystem : IFileSystem
    {
        public bool FileExist(string path)
        {
            return true;
        }
    }
}

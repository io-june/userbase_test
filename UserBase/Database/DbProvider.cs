namespace UserBase.Database
{
    public enum DbProvider { Access, Sqlite, SqlServer }

    public static class DatabaseFileTypes
    {
        public sealed record FileType(string Extension, DbProvider Provider, string Label);

        public static readonly FileType[] All =
        {
        new(".accdb", DbProvider.Access, "Access Database"),
        new(".mdb", DbProvider.Access, "Legacy Access Database"),
        new(".db", DbProvider.Sqlite, "SQLite Database"),
        new(".sqlite", DbProvider.Sqlite, "SQLite Database"),
        new(".sqlite3", DbProvider.Sqlite, "SQLite Database"),
    };

        public static DbProvider? FromExtension(string path) =>
            All.FirstOrDefault(t =>
                t.Extension.Equals(Path.GetExtension(path), StringComparison.OrdinalIgnoreCase))
               ?.Provider;

        public static DbProvider? FromFilterIndex(int index)
        {
            var groups = All.GroupBy(t => t.Provider).ToArray();
            return index >= 1 && index <= groups.Length
                ? groups[index - 1].Key
                : null;
        }

        public static string BuildFilter() => string.Join("|",
            All.GroupBy(t => t.Provider)
               .Select(g => $"{g.First().Label} ({string.Join(";", g.Select(t => "*" + t.Extension))})" +
                            $"|{string.Join(";", g.Select(t => "*" + t.Extension))}"));
    }
}

namespace Laboranforderung.Daten.Interfaces
{
    public interface ILaborwert
    {
        string Name { get; set; }
        string Bezeichnung { get; set; }
        string Synonyme { get; set; }
        string Findehilfe { get; set; }
        string Roehrchenart { get; set; }
        int Index { get; set; }
    }
}
namespace LibraryManagementSystem.WinUI.Entities.Models.Library;

public class Monograph
{
    public int MonographId { get; set; }
    public string? Classification { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Tutor { get; set; }
    public DateTime PresentationDate { get; set; }
    public byte[]? Image { get; set; }
    public int CareerId { get; set; }
    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }
}
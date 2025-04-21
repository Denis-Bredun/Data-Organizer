namespace Data_Organizer.MVVM.Models
{
    public class NoteHeader
    {
        public string UserId { get; set; }
        public string NoteBodyReferenceId { get; set; }
        public string Title { get; set; }
        public string PreviewText { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

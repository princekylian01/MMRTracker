namespace mmrcalc.Models
{
    public class MmrSession
    {
        public int StartMMR { get; set; }
        public int CurrentMMR { get; set; }
        public int Gained => CurrentMMR - StartMMR;
    }
}

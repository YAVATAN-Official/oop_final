using gis_final.Models;

namespace gis_final.ViewModels
{
    public class ScheduleViewModel
    {
        public Schedule Schedule { get; set; }
        public User Teacher { get; set; }
        public User Student { get; set; }
        public Field Field { get; set; }
        public Course Course { get; set; }
        public YearTerm YearTerm { get; set; }
        public decimal Score { get; set; }
        public EnumScoreStatus ScoreStatus { get; set; }
    }
}

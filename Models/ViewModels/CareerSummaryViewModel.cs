namespace CareerSim.Models.ViewModels
{
    public class CareerSummaryViewModel
    {
        public string CareerTitle { get; set; } = string.Empty;
        public List<CareerSummaryItemViewModel> Items { get; set; } = new();
        public string SummaryText { get; set; } = string.Empty;
    }

}

namespace CareerSim.Models.Career
{
    public class Career
    {
        public Career()
        {
            Tasks = new List<CareerTask>();
        }
        public int Id { get; set; }
        public string Title { get; set; }  // Örn: Yazılım Geliştirici
        public string Description { get; set; }  // Tanım
        public ICollection<CareerTask> Tasks { get; set; }
    }

}

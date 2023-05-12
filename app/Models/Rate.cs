using System.ComponentModel.DataAnnotations;

namespace ActuarialApplications.Models
{
    public class Rate
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime ValueDate { get; set; }
        public string ParameterName { get; set; }
        public int Duration { get; set; }
        public double? Value { get; set; }
    }
}
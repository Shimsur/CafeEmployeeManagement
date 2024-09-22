using System.ComponentModel.DataAnnotations;
namespace CafeEmployeeManagement.Models

{
    public class Cafe
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Logo { get; set; }
        public string Location { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}

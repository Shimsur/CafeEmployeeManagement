using System.ComponentModel.DataAnnotations;

namespace CafeEmployeeManagement.Models
{
    public class Employee
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(); // UIXXXXXXX format
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime StartDate { get; set; }

        // Foreign key to Cafe
        public Guid? CafeId { get; set; }  // Nullable if an employee can exist without a café
        public Cafe Cafe { get; set; } // Navigation property
    }
}

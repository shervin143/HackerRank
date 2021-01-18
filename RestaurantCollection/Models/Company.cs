using System.ComponentModel.DataAnnotations;

namespace RestaurantCollection.WebApi.Models
{
    public class Company
    {
        /// <summary>
        /// Company Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Company Name
        /// </summary>
        public string companyName { get; set; }

        /// <summary>
        /// Number of employees
        /// </summary>
        public int NumberOfEmployees { get; set; }

        /// <summary>
        /// Average Salary
        /// </summary>
        public decimal AverageSalary { get; set; }
    }
}

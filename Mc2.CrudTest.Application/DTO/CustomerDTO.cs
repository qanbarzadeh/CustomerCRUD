using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Mc2.CrudTest.Application.DTO
{
    public class CustomerDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        [DefaultValue("Alireza")]
        public string FirstName { get; set; }
        [DefaultValue("Qanbarzadeh")]
        public string LastName { get; set; }
        [DefaultValue("1985-01-01")]
        public DateTime DateOfBirth { get; set; }
        [DefaultValue("+60173771596")]
        public string PhoneNumber { get; set; }
        [DefaultValue("+ghxalireza@gmail.com")]
        public string Email { get; set; }                
        [DefaultValue("GB29NWBK60161331926819")]
        public string BankAccountNumber { get; set; }
    }
}

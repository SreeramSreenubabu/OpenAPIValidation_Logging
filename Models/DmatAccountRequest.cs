namespace DmatAccountApi.Models
{
    public class DmatAccountRequest
    {
        public string FullName { get; set; }          // Mandatory
        public string Email { get; set; }             // Mandatory
        public string MobileNumber { get; set; }      // Mandatory
        public string PanNumber { get; set; }         // Mandatory
        public string AadharNumber { get; set; }      // Mandatory
        public string DateOfBirth { get; set; }       // Mandatory (ISO 8601 format)
        public string Address { get; set; }           // Mandatory
        public string Occupation { get; set; }        // Optional
        public decimal? AnnualIncome { get; set; }    // Optional
        public string NomineeName { get; set; }       // Optional
    }
}

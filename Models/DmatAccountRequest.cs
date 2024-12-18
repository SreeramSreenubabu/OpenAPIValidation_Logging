using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DmatAccountApi.Models
{
    public class DmatAccountRequest
    {
        [Required(ErrorMessage = "SecCode is required.")]
        // [StringLength(4, ErrorMessage = "SecCode must be exactly 4 characters long.")]
        //[MaxLength(4, ErrorMessage = "The Name field cannot be longer than 100 characters.")]
        public string SecCode { get; set; }


        [Required(ErrorMessage = "RowCount is required.")]
        //[Range(1, 99999, ErrorMessage = "RowCount must be between 1 and 99999.")]
        //[Range(1, int.MaxValue, ErrorMessage = "RowCount must be between 1 and the maximum value of an integer.")]
        public int RowCount { get; set; }


        [DefaultValue(true)]
        public bool BLivePriceData { get; set; }

        [Required(ErrorMessage = "PageIndex is required.")]
        [DefaultValue(0)]
        //  [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be greater than 0.")]
        public int PageIndex { get; set; }

        public DateTime? DtDate { get; set; }
    }
}

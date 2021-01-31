using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        //[JsonIgnore]
        //[Required(ErrorMessage = "First Name is required")]
        //[StringLength(10, ErrorMessage = "Name can't be longer than 10 characters")]
        public string FirstName { get; set; }
        //[JsonIgnore]
        //[Required(ErrorMessage = "Last Name is required")]
        //[StringLength(10, ErrorMessage = "Name can't be longer than 10 characters")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Email Address is required")]
        //[StringLength(100, ErrorMessage = "Email Address can not be loner then 100 characters")]
        public string Email { get; set; }
        public bool IsVarified { get; set; }
    }
}

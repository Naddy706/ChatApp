using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

       
        public string Messages { get; set; }

        public string ImagePath { get; set; }

        [NotMapped]
        public List<HttpPostedFileBase> ImageFiles { get; set; }


        [Required]
        public string recevier_Id { get; set; }

        [Required]
        public string sender_Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

    }
}
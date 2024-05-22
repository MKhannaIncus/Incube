using System;

namespace API.DTOs
{
    public class DealDTO
    {
        public string DealName { get; set; }

        public string Fund {  get; set; }
        public DateTime Signingdate { get; set; }
        public string BorrowerOne { get; set; }

    }
}

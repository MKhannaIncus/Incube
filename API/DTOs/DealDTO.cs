using System;

namespace API.DTOs
{
    public class DealDTO
    {
        public string Alias { get; set; }

        public string Fund {  get; set; }
        public DateTime Signingdate { get; set; }
        public string BorrowerOne { get; set; }

    }
}

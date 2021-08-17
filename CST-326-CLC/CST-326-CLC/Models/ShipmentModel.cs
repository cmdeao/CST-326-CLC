using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CST_326_CLC.Models
{
    public class ShipmentModel
    {
        private int ShipmentId { get; set; }
        // Shall be used for user lookup options and delivery status updates
        private string Status { get; set; }
        // Shall be used for standard package sizes if Packaging type is STANDARD (Small, Medium, Large)
        private string PackageSize { get; set; }
        [Required(ErrorMessage = "The Weight field is required.")]
        private int Weight { get; set; }
        [Required(ErrorMessage = "The Height field is required.")]
        private int Height { get; set; }
        [Required(ErrorMessage = "The Width field is required.")]
        private int Width { get; set; }
        [Required(ErrorMessage = "The Length field is required.")]
        private int Length { get; set; }
        [Required(ErrorMessage = "The Zip field is required.")]
        private int Zip { get; set; }
        // The Packaging type will determine wether or not the user needs to input dimensions
        [Required(ErrorMessage = "The Packaging Type field is required.")]
        private string PackagingType { get; set; }
        // Delivery Option shall be three choices (Ground 4-5 days, Next Day, Standard 1-3 days)
        [Required(ErrorMessage = "The Delivery Option field is required.")]
        private string DeliveryOption { get; set; }
        // Business prices should be higher
        private bool IsResidential { get; set; }

        
        public float CalculateCost(int zip, int length, int width, int height, int weight, string packagingType,
            string deliveryOption, bool isResidential)
        {
            // Logic for zip cost here

            // Logic for package dimensions cost variable here -> will check the packaging type for STANDARD sizes

            // Logic for Delivery options cost variable here

            // Logic for is residential cost variable here

            // Place holder -> needs to be replace with cost variables sum
            return (float)width + height;
        }

        // Method to determine the Packaging type
        public void PackageTypeSelection(string packageType)
        {
            if (packageType == null) return;

            packageType.ToUpper();
            PackagingType = packageType;
            if (PackagingType == "STANDARD")
            {
                // Logic to choose form that only allows user to select package size (Small, Medium, Large)
            }
            else
            {
                // Logic to choose form that allows user to enter custom dimensions for package
            }
        }
    }
}